
using Hangfire;
using NLog;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Hosting;

using System.Collections.Generic;
using AnubisDBMS.Data;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using AnubisDBMS.Data.Entities;
using AnubisDBMS.Controllers;
using static AnubisDBMS.Resources.AnubisEmailService;
using System.Net.Mail;
using System.IO;
using System.Net.Mime;
using AnubisDBMS.Data.Localization.Entities;

namespace AnubisDBMS.Web.App_Start
{
    public class HangfireService : IRegisteredObject
    {
        public static readonly HangfireService Instance = new HangfireService();
        private readonly object _lockObject = new object();
        private bool _started;
        private BackgroundJobServer _backgroundJobServer;
        private AnubisDbContext db = new AnubisDbContext(); 
        public MailingRepository emailSvc = new MailingRepository();

        public HangfireService()
        {

        }

        public void Start()
        {
            lock (_lockObject)
            {
                if (_started) return;
                _started = true;


                HostingEnvironment.RegisterObject(this);

                GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");

                _backgroundJobServer = new BackgroundJobServer();

                RecurringJob.AddOrUpdate("Chequear Medidas", () => this.CheckAsync(), cronExpression: Cron.HourInterval(1));

            }
        }

        public void Stop()
        {
            lock (_lockObject)
            {
                _backgroundJobServer?.Dispose();

                HostingEnvironment.UnregisterObject(this);
            }
        }

        void IRegisteredObject.Stop(bool immediate)
        {
            Stop();
        }
   

        #region SensorMinMaxCheck
        public bool CheckMinMax(DataSensores dataSensor)
        {
            var  Sensor = db.Sensores.FirstOrDefault(x => x.TipoSensor.NombreTipoSensor == dataSensor.TipoSensor && x.Activo); 
          
            if(Sensor != null)
            {
                var Max = Sensor.Max != null ? Sensor.Max : 0;
                var Min = Sensor.Max != null ? Sensor.Min : 0;
                bool TodoBienMAX = true;
                bool TodoBienMIN = true;
                //SI MIN ES 0 REVISO MAX
                if (Min == 0)
                {
                    if (!CheckRange(null, Max, dataSensor.Medida))
                    { TodoBienMAX = false; }
                }
                //SI MAX ES 0 REVISO MIN
                if (Max == 0)
                {
                    if (!CheckRange(Min, null, dataSensor.Medida))
                    { TodoBienMIN = false; }
                }
                //SI NINGUNO ES 0 REVISO AMBOS
                if (Min != 0 && Max != 0)
                {
                    if (!CheckRange(null, Max, dataSensor.Medida))
                    { TodoBienMAX = false; }
                    if (!CheckRange(Min, null, dataSensor.Medida))
                    { TodoBienMIN = false; }
                }
                if (TodoBienMAX == false || TodoBienMIN == false)
                {
                    dataSensor.Chequeado = true;
                    dataSensor.Error = true;
                    dataSensor.EncimaNormal = TodoBienMIN;
                    dataSensor.DebajoNormal = TodoBienMAX;
                    db.SaveChanges();
                    return false;
                }
                dataSensor.Chequeado = true;
                db.SaveChanges();
                return true;
            }
            return false;
        }
         
        public bool CheckRange(double? Min, double? Max, double Lectura)
        {
            if (Min == null)
            {
                //SI MIN ES NULO REVISO MAX
                if (Lectura <= Max)
                    return true;
                else return false;
            }
            if (Max == null)
            {
                //SI MAX ES NULO REVISO MIN
                if (Lectura >= Min)
                    return true;
                else return false;
            }
            if (Min == null && Max == null)
            {
                return true;
            }
            return false;

        }

        public bool CheckMedidas(Empresa emp)
        { 
            var dataSensores = db.DataSensores.Where(x =>
            DbFunctions.TruncateTime(x.FechaRegistro) == DbFunctions.TruncateTime(DateTime.Today)
            && x.Activo 
            && x.Chequeado == false
            && x.IdEmpresa ==emp.IdEmpresa).ToList(); 
            foreach (var medidas in dataSensores)
            {
                CheckMinMax(medidas);
            }
            return true;
        }

        
        public async Task<bool> NotificarAsync(DataSensores error, string correoDestino)
        {

            string path = HostingEnvironment.MapPath("~\\");
            string logoImage = Path.Combine(path, "Content\\Images\\AnubisLogoEmail.jpeg");
            string rounderup = Path.Combine(path, "Content\\Images\\rounder-up.png");
            string divider = Path.Combine(path, "Content\\Images\\divider.png");
            string rounderdwn = Path.Combine(path, "Content\\Images\\rounder-dwn.png");
            Attachment logoImageAtt = new Attachment(logoImage);
            Attachment rounderupAtt = new Attachment(rounderup);
            Attachment dividerAtt = new Attachment(divider);
            Attachment rounderdwnAtt = new Attachment(rounderdwn);

            logoImageAtt.ContentDisposition.Inline = true;
            logoImageAtt.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            string logoImgId = "headerimg1";
            logoImageAtt.ContentId = logoImgId;

            rounderupAtt.ContentDisposition.Inline = true;
            rounderupAtt.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            string rounderupId = "headerimg2";
            rounderupAtt.ContentId = rounderupId;

            dividerAtt.ContentDisposition.Inline = true;
            dividerAtt.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            string dividerId = "headerimg3";
            dividerAtt.ContentId = dividerId;

            rounderdwnAtt.ContentDisposition.Inline = true;
            rounderdwnAtt.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            string rounderdwnId = "headerimg4";
            rounderdwnAtt.ContentId = rounderdwnId;


            var email = new MailMessage("anubisolutions@gmail.com", correoDestino);
            email.Attachments.Add(logoImageAtt);
            email.Attachments.Add(rounderupAtt);
            email.Attachments.Add(dividerAtt);
            email.Attachments.Add(rounderdwnAtt);

            email.Subject = "Nueva Alerta - Sensor" + " " + error.TipoSensor;
            email.IsBodyHtml = true;
            var not = new NotificacionCorreo
            {
                Usuario = "Adminsitrador",
                TipoSensor = error.TipoSensor,
                Medicion = error.UnidadMedida,
                

                Logo = logoImgId,
                divider = dividerId,
                rounderdwn = rounderdwnId,
                rounderup = rounderupId
            };
            if(error.Medida.ToString().Length>5)
            {
                not.MedidaSensor = error.Medida.ToString().Substring(0, 5);
            }
            else
            {
                not.MedidaSensor = error.Medida.ToString();
            }
            try
            {
                var bodyAprobadoProveedor = emailSvc.RenderViewToString(new MailerController(), "PlantillaAnubis", "~/Views/Mailer/PlantillaAnubis.cshtml", not);
                email.Body = bodyAprobadoProveedor;
                await emailSvc.SendEmailAsync(email);
                error.Notificado = true;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
        #endregion
        public async Task<bool> CalcularNotificacionesAsync(Empresa empresa)
        {
            int PrimeraNotif = empresa.PrimeraNotificacion;
            int SegundaNotif = empresa.SegundaNotificacion;
            int TerceraNotif = empresa.TerceraNotificacion;
            var Errores = db.DataSensores.Where(x => x.Error && x.Notificado == false
            && x.IdEmpresa == empresa.IdEmpresa).ToList();
            int Conteo = Errores.Count();
            DataSensores ErrorANotificar = Errores.FirstOrDefault();
            if(PrimeraNotif != 0 && SegundaNotif != 0 && TerceraNotif != 0)
            {  
            if (Conteo >= PrimeraNotif && Conteo < SegundaNotif)
            {
                try
                {
                    await NotificarAsync(ErrorANotificar, empresa.EmailNotificacion);
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            if (Conteo >= SegundaNotif && Conteo < TerceraNotif)
            {
                try
                {
                    await NotificarAsync(ErrorANotificar, empresa.EmailNotificacion);
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            if (Conteo >= TerceraNotif )
            {
                try
                {
                    await NotificarAsync(ErrorANotificar, empresa.EmailNotificacion);
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            }
            return true;

        }


        public async Task<bool> CheckAsync()
        {
                var empresa = db.Empresas.Where(c => c.Activo && c.ServicioActivo).ToList();
                foreach(var emp in empresa)
                {
                    if (CheckMedidas(emp))
                    {
                            if (await CalcularNotificacionesAsync(emp))
                            {
                                return true;
                            }
                            else
                            { return false; }
                        } 
            }
            return false;

        }

    }
}

