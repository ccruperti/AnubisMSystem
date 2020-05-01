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
using System.Threading.Tasks;
using AnubisDBMS.Views.Helpers;

namespace AnubisDBMS.Controllers
{

    public class MainController : Controller
    {
        public long IdEmpresa = new long();
        public bool AccesoGeneral = new bool();
        public List<long> idsEmpresas = new List<long>();
    
        #region DB_Users_Mail
        public AnubisDbContext db = new AnubisDbContext();
        public MailingRepository emailSvc = new MailingRepository(); 
        public AnubisDBMSUserManager _userManager;
        public AnubisDBMSRoleManager _roleManager;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

           
                if (User.Identity.Name != null)
                {
                    IdEmpresa = User.Identity.GetEmpresaId();

                    idsEmpresas.Add(IdEmpresa);

                }
            
            

        }

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
            foreach (var x in db.TipoSensor.Where(x=>x.Activo && x.IdEmpresa == IdEmpresa).ToList())
            {
                TipoSensor.Add(x);
            }
            return new SelectList(TipoSensor, "IdTipoSensor", "NombreTipoSensor");

        } 
        public SelectList SelectListEquipo(long selected = 0)
            {

                List<Equipo> Equipo = new List<Equipo>();
                foreach (var x in db.Equipos.Where(x => x.Activo && x.IdEmpresa == IdEmpresa).ToList())
                {
                Equipo.Add(x); 
                }
            Equipo.Add(new Equipo { IdEquipo = 0, Alias = "Seleccione Equipo" });
            return new SelectList(Equipo, "IdEquipo", "Alias");

            }
        public SelectList SelectListEquipoSensor(long? id = null)
        {

            List<SelectListItem> data = db.EquipoSensor.Where(x => x.Activo && x.IdEmpresa == IdEmpresa).Select(x => new SelectListItem{ 
             Text = x.Equipos.Alias + " - "+ x.Equipos.SerieEquipo,
             Value = x.IdEquipoSensor.ToString()
            }).ToList(); 
            return new SelectList(data, "Value", "Text", id);

        }
        public SelectList SelectListFrecuencias(long? id = null)
        {

            List<Frecuencia> data = db.Frecuencia.Where(c => c.Activo && c.IdEmpresa == IdEmpresa).ToList();
            data.Add(new Frecuencia { IdFrecuencia = 0 , NombreFrecuencia = "Seleccione frecuencia" });
            return new SelectList(data.OrderBy(c => c.IdFrecuencia), "IdFrecuencia", "NombreFrecuencia", id);

        }
        public SelectList SelectListSensores(long selected = 0)
        {
            List<long> sensoresIDs = db.EquipoSensor.Where(x => x.Activo && x.IdEmpresa == IdEmpresa).Select(i =>i.IdSensor??0).ToList();
            List<SelectListItem> sensores = db.Sensores.AsNoTracking()
                   .OrderBy(n => n.SerieSensor).Where(x=>x.Activo && x.IdEmpresa == IdEmpresa)
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
            var Equipos = db.EquipoSensor.Where(x => x.IdEquipo == idEquipo && x.Activo && x.NumeroPuerto!=0 && x.IdEmpresa == IdEmpresa).ToList();
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
            foreach (var x in db.Tecnicos.Where(x => x.Activo && x.IdEmpresa == IdEmpresa).ToList())
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

            var empresa = db.Empresas.FirstOrDefault(x => x.IdEmpresa == IdEmpresa);
            if (!empresa.ServicioActivo)
            {
                empresa.ServicioActivo=true;
                empresa.FechaModificacion = DateTime.Now;
                db.SaveChanges();
            } 
            return Redirect("Index");
        }
        public ActionResult Desactivar_Servicio ()
        {
            var empresa = db.Empresas.FirstOrDefault(x => x.IdEmpresa == IdEmpresa);
            if (empresa.ServicioActivo)
            {
                empresa.ServicioActivo = false;
                empresa.FechaModificacion = DateTime.Now;
                db.SaveChanges();
            }

            return Redirect("Index");
        }
        #endregion

      

         

    }
}