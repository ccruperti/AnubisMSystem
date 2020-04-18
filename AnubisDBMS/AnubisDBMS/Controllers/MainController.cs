using AnubisDBMS.Data;
using AnubisDBMS.Infraestructure.Helpers;
using AnubisDBMS.Infraestructure.Security.Managers;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnubisDBMS.Data.Entities;
using AnubisDBMS.Infraestructure.Filters.WebFilters;
using System.Data.Entity;
using System.IO;
using System.Net.Mail;
using static AnubisDBMS.Resources.AnubisEmailService;
using System.Web.Hosting;
using System.Net.Mime;

namespace AnubisDBMS.Controllers
{

    public class MainController : Controller
    { 
        #region DB_Users_Mail
        public AnubisDbContext db = new AnubisDbContext();
        public MailingRepository emailSvc = new MailingRepository(); 
        public AnubisDBMSUserManager _userManager;
        public AnubisDBMSRoleManager _roleManager;



        public MainController()
        {

        }

        public MainController(AnubisDBMSUserManager userManager, AnubisDBMSRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public AnubisDBMSUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<AnubisDBMSUserManager>();
            private set => _userManager = value;
        }

        public AnubisDBMSRoleManager RoleManager
        {
            get => _roleManager ?? HttpContext.GetOwinContext().GetUserManager<AnubisDBMSRoleManager>();
            private set => _roleManager = value;
        }
        #endregion

        #region QR
        public QRGenerator QR = new QRGenerator();
        #endregion

        #region SelectLists
        public SelectList SelectListTipoSensor(long selected = 0)
        {

            List<TipoSensor> TipoSensor = new List<TipoSensor>(); 
            foreach (var x in db.TipoSensor.Where(x=>x.Activo).ToList())
            {
                TipoSensor.Add(x);
            }
            return new SelectList(TipoSensor, "IdTipoSensor", "NombreTipoSensor");

        } 
        public SelectList SelectListEquipo(long selected = 0)
            {

                List<Equipo> Equipo = new List<Equipo>();
                foreach (var x in db.Equipos.Where(x => x.Activo).ToList())
                {
                Equipo.Add(x); 
                }
            Equipo.Add(new Equipo { IdEquipo = 0, Alias = "Seleccione Equipo" });
            return new SelectList(Equipo, "IdEquipo", "Alias");

            }
        public SelectList SelectListEquipoSensor(long? id = null)
        {

            List<SelectListItem> data = db.EquipoSensor.Where(x => x.Activo).Select(x => new SelectListItem{ 
             Text = x.Equipos.Alias + " - "+ x.Equipos.SerieEquipo,
             Value = x.IdEquipoSensor.ToString()
            }).ToList(); 
            return new SelectList(data, "Value", "Text", id);

        }
        public SelectList SelectListFrecuencias(long? id = null)
        {

            List<Frecuencia> data = db.Frecuencia.Where(c => c.Activo).ToList();
            data.Add(new Frecuencia { IdFrecuencia = 0 , NombreFrecuencia = "Seleccione frecuencia" });
            return new SelectList(data.OrderBy(c => c.IdFrecuencia), "IdFrecuencia", "NombreFrecuencia", id);

        }
        public SelectList SelectListSensores(long selected = 0)
        {
            List<long> sensoresIDs = db.EquipoSensor.Where(x => x.Activo).Select(i =>i.IdSensor??0).ToList();
            List<SelectListItem> sensores = db.Sensores.AsNoTracking()
                   .OrderBy(n => n.SerieSensor).Where(x=>x.Activo)
                       .Select(n =>
                       new SelectListItem
                       {
                           Value = n.IdSensor.ToString(),
                           Text = n.TipoSensor.NombreTipoSensor+ " - " + n.SerieSensor
                       }).ToList();

           
           
            var filtered = sensores
                              .Where(x => !sensoresIDs.Contains(Convert.ToInt64(x.Value))).ToList();
            var sensorNull = new SelectListItem()
            {
                Value = null,
                Text = "Seleccione Sensor"
            };
            filtered.Insert(0, sensorNull);
            return new SelectList(filtered, "Value", "Text"); 
        } 
        public SelectList SelectListPuertos(long idEquipo, int selected = 0)
        {
            List<int> puertosOcupados = new List<int>();
            List<int> puertosDisponibles = new List<int>();
            var Equipos = db.EquipoSensor.Where(x => x.IdEquipo == idEquipo && x.Activo && x.NumeroPuerto!=0).ToList();
            for(var x=1;x<=8;x++)
            {
                puertosDisponibles.Add(x);
            }
            foreach(var eq in Equipos)
            {
            puertosOcupados.Add(eq.NumeroPuerto);
            }
            List<int> Puertos = puertosDisponibles.Except(puertosOcupados).ToList(); 
            return new SelectList(Puertos);
        } 
        public SelectList SelectListTecnico(long? id = null)
        {

            List<Tecnicos> Tecnicos = new List<Tecnicos>();
            foreach (var x in db.Tecnicos.Where(x => x.Activo).ToList())
            {
                Tecnicos.Add(x);
            }
            Tecnicos.Add(new Tecnicos { IdTecnico = 0, NombreTecnico = "Seleccione Técnico" });
            return new SelectList(Tecnicos.OrderBy(c => c.IdTecnico), "IdTecnico", "NombreTecnico", id);

        }
        #endregion
         
        #region ACT_DESACT Servicios
        public ActionResult Activar_Servicio ()
        {
            if (!db.Servicio.Any(x => x.Activo))
            {

                db.Servicio.Add(new Servicio
                {
                    Activo = true,
                    FechaModificacion = DateTime.Now,
                    UsuarioModificacion = User.Identity.Name,
                    EstadoServicio = true
                });
                db.SaveChanges();
            }
            else
            {
                var Actual = db.Servicio.FirstOrDefault(x => x.Activo);
                Actual.FechaModificacion = DateTime.Now;
                Actual.UsuarioModificacion = User.Identity.Name;
                Actual.Activo = false;
                db.Servicio.Add(new Servicio
                {
                    Activo = true,
                    FechaRegistro = DateTime.Now,
                    UsuarioRegistro = User.Identity.Name,
                    EstadoServicio = true
                });
                db.SaveChanges();
            }
            return Redirect("Index");
        }
        public ActionResult Desactivar_Servicio ()
        {
            if (!db.Servicio.Any(x => x.Activo))
            {

                db.Servicio.Add(new Servicio
                {
                    Activo = true,
                    FechaModificacion = DateTime.Now,
                    UsuarioModificacion = User.Identity.Name,
                    EstadoServicio = false
                });
                db.SaveChanges();
            }
            else
            {
                var Actual = db.Servicio.FirstOrDefault(x => x.Activo);
                Actual.FechaModificacion = DateTime.Now;
                Actual.UsuarioModificacion = User.Identity.Name;
                Actual.EstadoServicio = false;
                Actual.Activo = false;
                db.Servicio.Add(new Servicio
                {
                    Activo = true,
                    FechaRegistro = DateTime.Now,
                    UsuarioRegistro = User.Identity.Name,
                    EstadoServicio = false
                });
                db.SaveChanges();
            }
           return Redirect("Index");
        }
        #endregion

        #region SensorMinMaxCheck
        public bool CheckMinMax (DataSensores dataSensor)
        {
            var Max = db.Sensores.FirstOrDefault(x => x.SerieSensor == dataSensor.SerieSensor).Max??0;
            var Min = db.Sensores.FirstOrDefault(x => x.SerieSensor == dataSensor.SerieSensor).Min??0;
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
                if (TodoBienMAX==false||TodoBienMIN==false)
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
          
        

        public bool CheckRange( double? Min, double? Max, double Lectura)
        {
            if(Min==null)
            {
                //SI MIN ES NULO REVISO MAX
                if (Lectura <= Max)
                    return true;
                else return false;
            }
            if (Max==null)
            {
                //SI MAX ES NULO REVISO MIN
                if (Lectura >= Min)
                    return true;
                else return false;
            }
            if(Min==null && Max==null)
            {
                return true;
            }
            return false;
       
        }
        #endregion

        public bool CheckMedidas()
        { 
            var dataSensores = db.DataSensores.Where(x => DbFunctions.TruncateTime(x.FechaRegistro) == DbFunctions.TruncateTime(DateTime.Today) && x.Activo && x.Chequeado == false).ToList();
            foreach (var medidas in dataSensores)
            {
                CheckMinMax(medidas);
            }
            return true; 
        }


        public async System.Threading.Tasks.Task<bool> NotificarAsync(string correoDestino)
        {
            foreach (var error in db.DataSensores.Where(x => x.Error && x.Notificado == false).ToList())
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
                    Usuario = "FALTA EL USUARIO",
                    SerieSensor = error.SerieSensor,
                    Medicion = error.UnidadMedida,
                    MedidaSensor = error.Medida.ToString(),

                    Logo = logoImgId,
                    divider = dividerId,
                    rounderdwn = rounderdwnId,
                    rounderup = rounderupId
                };
                if (error.DebajoNormal == true)
                {
                    not.EncimaDebajo = "debajo";
                }
                if (error.EncimaNormal == true)
                {
                    not.EncimaDebajo = "encima";
                }
                try
                { 
                var bodyAprobadoProveedor = emailSvc.RenderViewToString(new MailerController(), "PlantillaAnubis", "~/Views/Mailer/PlantillaAnubis.cshtml", not);
                email.Body = bodyAprobadoProveedor;
                await emailSvc.SendEmailAsync(email);
                error.Notificado = true;
                db.SaveChanges();
                }
                catch(Exception e)
                {
                    return false;
                }
                
            }
            return true;
        }

        
    }
}