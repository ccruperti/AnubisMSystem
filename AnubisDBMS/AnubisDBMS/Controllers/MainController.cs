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


        #endregion

        #region ACT_DESACT Servicios
         public void Activar_Serivcio ()
        {
            if(db.Servicio.Any(x=>x.Activo))
            {

            }
            else
            {
            new Servicio
            {
                Activo = true,
                FechaRegistro = DateTime.Now,
                UsuarioRegistro = User.Identity.Name,
                EstadoServicio = true

            };

            }
        }
        public void Desactivar_Serivcio ()
        {

        }
        #endregion
    }
}