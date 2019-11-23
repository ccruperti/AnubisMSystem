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

namespace AnubisDBMS.Controllers
{

    public class MainController : Controller
    {

        #region DB&Users
        public AnubisDbContext db = new AnubisDbContext();

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
        public SelectList SelectListTipoSensor(string selected = null)
        {

            List<TipoSensor> TipoSensor = new List<TipoSensor>();
            foreach (var x in db.TipoSensor.ToList())
            {
                TipoSensor.Add(x);
            }
            return new SelectList(TipoSensor, "IdTipoSensor", "NombreTipoSensor");

        }

            public SelectList SelectListEquipo(string selected = null)
            {

                List<Equipo> Equipo = new List<Equipo>();
                foreach (var x in db.Equipos.ToList())
                {
                Equipo.Add(x); 
                }
                return new SelectList(Equipo, "IdEquipo", "SerieEquipo");

            }

        public SelectList SelectListSensores(string selected = null)
        {
            List<SelectListItem> sensores = db.Sensores.AsNoTracking()
                   .OrderBy(n => n.SerieSensor)
                       .Select(n =>
                       new SelectListItem
                       {
                           Value = n.IdSensor.ToString(),
                           Text = n.TipoSensor.NombreTipoSensor+ " - " + n.SerieSensor
                       }).ToList();
              
            return new SelectList(sensores, "Value", "Text"); 
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
                Actual.EstadoServicio = false;
                //db.Servicio.Add(new Servicio
                //{
                //    Activo = true,
                //    FechaRegistro = DateTime.Now,
                //    UsuarioRegistro = User.Identity.Name,
                //    EstadoServicio = true
                //});
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
                //db.Servicio.Add(new Servicio
                //{
                //    Activo = true,
                //    FechaRegistro = DateTime.Now,
                //    UsuarioRegistro = User.Identity.Name,
                //    EstadoServicio = false
                //});
                db.SaveChanges();
            }
           return Redirect("Index");
        }
        #endregion
    }
}