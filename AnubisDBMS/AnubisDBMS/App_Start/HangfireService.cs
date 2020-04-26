
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

namespace AnubisDBMS.Web.App_Start
{
    public class HangfireService : IRegisteredObject
    {
        public static readonly HangfireService Instance = new HangfireService();
        private readonly object _lockObject = new object();
        private bool _started;
        private BackgroundJobServer _backgroundJobServer;
        private AnubisDbContext db = new AnubisDbContext();
        private static Logger logCotizacion = LogManager.GetLogger("f");
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

                RecurringJob.AddOrUpdate("Sincronizar Ordenes", () => this.CheckAsync(), cronExpression: Cron.HourInterval(1));

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
        //public bool CheckMinMax(DataSensores dataSensor)
        //{
        //    var Max = db.Sensores.FirstOrDefault(x => x.SerieSensor == dataSensor.SerieSensor).Max ?? 0;
        //    var Min = db.Sensores.FirstOrDefault(x => x.SerieSensor == dataSensor.SerieSensor).Min ?? 0;
        //    bool TodoBienMAX = true;
        //    bool TodoBienMIN = true;
        //    //SI MIN ES 0 REVISO MAX
        //    if (Min == 0)
        //    {
        //        if (!CheckRange(null, Max, dataSensor.Medida))
        //        { TodoBienMAX = false; }
        //    }
        //    //SI MAX ES 0 REVISO MIN
        //    if (Max == 0)
        //    {
        //        if (!CheckRange(Min, null, dataSensor.Medida))
        //        { TodoBienMIN = false; }
        //    }
        //    //SI NINGUNO ES 0 REVISO AMBOS
        //    if (Min != 0 && Max != 0)
        //    {
        //        if (!CheckRange(null, Max, dataSensor.Medida))
        //        { TodoBienMAX = false; }
        //        if (!CheckRange(Min, null, dataSensor.Medida))
        //        { TodoBienMIN = false; }
        //    }
        //    if (TodoBienMAX == false || TodoBienMIN == false)
        //    {
        //        dataSensor.Chequeado = true;
        //        dataSensor.Error = true;
        //        dataSensor.EncimaNormal = TodoBienMAX;
        //        dataSensor.DebajoNormal = TodoBienMIN;
        //        db.SaveChanges();
        //        return false;
        //    }
        //    dataSensor.Chequeado = true;
        //    db.SaveChanges();
        //    return true;
        //}
        //public bool CheckRange(double? Min, double? Max, double Lectura)
        //{
        //    if (Min == null)
        //    {
        //        //SI MIN ES NULO REVISO MAX
        //        if (Lectura <= Max)
        //            return true;
        //        else return false;
        //    }
        //    if (Max == null)
        //    {
        //        //SI MAX ES NULO REVISO MIN
        //        if (Lectura >= Min)
        //            return true;
        //        else return false;
        //    }
        //    if (Min == null && Max == null)
        //    {
        //        return true;
        //    }
        //    return false;

        //}
        //public bool CheckMedidas()
        //{
        //    DateTime fech = DateTime.Today.AddDays(-8);
        //    var dataSensores = db.DataSensores.Where(x => DbFunctions.TruncateTime(x.FechaRegistro) == DbFunctions.TruncateTime(fech) && x.Activo && x.Chequeado == false).ToList();
        //    foreach (var medidas in dataSensores)
        //    {
        //        CheckMinMax(medidas);
        //    }
        //    return true;
        //}
        //public async Task<bool> NotificarAsync(string correoDestino)
        //{
        //    foreach (var error in db.DataSensores.Where(x => x.Error && x.Notificado == false).ToList())
        //    {
        //        string path = HostingEnvironment.MapPath("~\\");
        //        string logoImage = Path.Combine(path, "Content\\Images\\AnubisLogoEmail.jpeg");
        //        string rounderup = Path.Combine(path, "Content\\Images\\rounder-up.png");
        //        string divider = Path.Combine(path, "Content\\Images\\divider.png");
        //        string rounderdwn = Path.Combine(path, "Content\\Images\\rounder-dwn.png");
        //        Attachment logoImageAtt = new Attachment(logoImage);
        //        Attachment rounderupAtt = new Attachment(rounderup);
        //        Attachment dividerAtt = new Attachment(divider);
        //        Attachment rounderdwnAtt = new Attachment(rounderdwn);

        //        logoImageAtt.ContentDisposition.Inline = true;
        //        logoImageAtt.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
        //        string logoImgId = "headerimg1";
        //        logoImageAtt.ContentId = logoImgId;

        //        rounderupAtt.ContentDisposition.Inline = true;
        //        rounderupAtt.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
        //        string rounderupId = "headerimg2";
        //        rounderupAtt.ContentId = rounderupId;

        //        dividerAtt.ContentDisposition.Inline = true;
        //        dividerAtt.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
        //        string dividerId = "headerimg3";
        //        dividerAtt.ContentId = dividerId;

        //        rounderdwnAtt.ContentDisposition.Inline = true;
        //        rounderdwnAtt.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
        //        string rounderdwnId = "headerimg4";
        //        rounderdwnAtt.ContentId = rounderdwnId;


        //        var email = new MailMessage("anubisolutions@gmail.com", correoDestino);
        //        email.Attachments.Add(logoImageAtt);
        //        email.Attachments.Add(rounderupAtt);
        //        email.Attachments.Add(dividerAtt);
        //        email.Attachments.Add(rounderdwnAtt);

        //        email.Subject = "Nueva Alerta - Sensor" + " " + error.SerieSensor;
        //        email.IsBodyHtml = true;
        //        var not = new NotificacionCorreo
        //        {
        //            Usuario = "FALTA EL USUARIO",
        //            SerieSensor = error.SerieSensor,
        //            Medicion = error.UnidadMedida,
        //            MedidaSensor = error.Medida.ToString(),

        //            Logo = logoImgId,
        //            divider = dividerId,
        //            rounderdwn = rounderdwnId,
        //            rounderup = rounderupId
        //        };
        //        //if (error.DebajoNormal == true)
        //        //{
        //        //    not.EncimaDebajo = "debajo";
        //        //}
        //        //if (error.EncimaNormal == true)
        //        //{
        //        //    not.EncimaDebajo = "encima";
        //        //}
        //        try
        //        {
        //            var bodyAprobadoProveedor = emailSvc.RenderViewToString(new MailerController(), "PlantillaAnubis", "~/Views/Mailer/PlantillaAnubis.cshtml", not);
        //            email.Body = bodyAprobadoProveedor;
        //            await emailSvc.SendEmailAsync(email);
        //            error.Notificado = true;
        //            db.SaveChanges();
        //        }
        //        catch (Exception e)
        //        {
        //            return false;
        //        }

        //    }
        //    return true;
        //}


        #region SensorMinMaxCheck
        public bool CheckMinMax(DataSensores dataSensor)
        {
            var Max = db.Sensores.FirstOrDefault(x => x.SerieSensor == dataSensor.SerieSensor).Max ?? 0;
            var Min = db.Sensores.FirstOrDefault(x => x.SerieSensor == dataSensor.SerieSensor).Min ?? 0;
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
                dataSensor.EncimaNormal = TodoBienMAX;
                dataSensor.DebajoNormal = TodoBienMIN;
                db.SaveChanges();
                return false;
            }
            dataSensor.Chequeado = true;
            db.SaveChanges();
            return true;
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

        public bool CheckMedidas()
        {
            var dataSensores = db.DataSensores.Where(x => DbFunctions.TruncateTime(x.FechaRegistro) == DbFunctions.TruncateTime(DateTime.Today) && x.Activo && x.Chequeado == false).ToList();
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

            email.Subject = "Nueva Alerta - Sensor" + " " + error.SerieSensor;
            email.IsBodyHtml = true;
            var not = new NotificacionCorreo
            {
                Usuario = "Adminsitrador",
                SerieSensor = error.SerieSensor,
                Medicion = error.UnidadMedida,
                MedidaSensor = error.Medida.ToString(),

                Logo = logoImgId,
                divider = dividerId,
                rounderdwn = rounderdwnId,
                rounderup = rounderupId
            };
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
        public async Task<bool> CalcularNotificacionesAsync(string correoDestino)
        {
            int PrimeraNotif = db.Users.FirstOrDefault(c => c.Email == correoDestino).PrimeraNotificacion;
            int SegundaNotif = db.Users.FirstOrDefault(c => c.Email == correoDestino).SegundaNotificacion;
            int TerceraNotif = db.Users.FirstOrDefault(c => c.Email == correoDestino).TerceraNotificacion;
             
            var Errores = db.DataSensores.Where(x => x.Error && x.Notificado == false).ToList();
            int Conteo = Errores.Count();
            DataSensores ErrorANotificar = Errores.FirstOrDefault();
            
            if (Conteo >= PrimeraNotif && Conteo < SegundaNotif)
            {
                try
                {
                    await NotificarAsync(ErrorANotificar, correoDestino);
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
                    await NotificarAsync(ErrorANotificar, correoDestino);
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
                    await NotificarAsync(ErrorANotificar, correoDestino);
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;

        }


        public async Task<bool> CheckAsync()
        {

            if (CheckMedidas())
            {

                if (await CalcularNotificacionesAsync("aguilar996@hotmail.com"))
                {
                    return true;
                }
                else
                        { return false; }
            }
            return false;

        }

    }
}

