using AnubisDBMS.Data;
using AnubisDBMS.Data.Entities;
using AnubisDBMS.Data.ViewModels;
using AnubisDBMS.Infraestructure.Security.Managers;
using AnubisDBMS.Infraestructure.Security.Stores;
using AnubisDBMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AnubisDBMS.Controllers
{
    public class GestionEquiposController : Controller
    {
        private AnubisDbContext _context = new AnubisDbContext();

        protected AnubisDBMSUserManager _userManager;
        protected AnubisDBMSRoleManager _roleManager;

        public GestionEquiposController()
        {

        }

        public GestionEquiposController(AnubisDBMSUserManager userManager, AnubisDBMSRoleManager roleManager)
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

        #region Helpers
        public SelectList ListaEquipos(long? id)
        {
            var data = _context.Equipos.Where(c => c.Activo).ToList();
            data.Add(new Equipo { IdEquipo = 0, Alias = "Seleccione un Equipo" });
            return new SelectList(data, "IdEquipo", "Alias", id);
        }
        public SelectList ListaSensores(long? id)
        {
            var data = _context.Sensores.Where(c => c.Activo).ToList();
            data.Add(new Sensor { IdSensor = 0, SerieSensor  = "Seleccione un Sensor" });
            return new SelectList(data, "IdSensor", "SerieSensor", id);
        }
        #endregion
        public List<EquiposViewModels> ListaEquipos()
        {
            var lista = new List<EquiposViewModels>();
            var eq1 = new EquiposViewModels();
            var eq2 = new EquiposViewModels();
            var eq3 = new EquiposViewModels();

            eq1.AliasEquipo = "Equipo 1";
            eq1.NombreTecnico = "Tecnico 1";
            eq1.Sensores.Add(new SensoresEquipos {
                CodigoSensor = "Cod1",
                NombreSensor = "Sensor 1"
            });
            eq1.Sensores.Add(new SensoresEquipos
            {
                CodigoSensor = "Cod2",
                NombreSensor = "Sensor 2"
            });
            eq1.Sensores.Add(new SensoresEquipos
            {
                CodigoSensor = "Cod3",
                NombreSensor = "Sensor 3"
            });
            eq2.AliasEquipo = "Equipo 2";
            eq2.NombreTecnico = "Tecnico 2";
            eq2.Sensores.Add(new SensoresEquipos
            {
                CodigoSensor = "Cod1",
                NombreSensor = "Sensor 1"
            });
            eq2.Sensores.Add(new SensoresEquipos
            {
                CodigoSensor = "Cod2",
                NombreSensor = "Sensor 2"
            });
            eq2.Sensores.Add(new SensoresEquipos
            {
                CodigoSensor = "Cod3",
                NombreSensor = "Sensor 3"
            });

            eq3.AliasEquipo = "Equipo 3";
            eq3.NombreTecnico = "Tecnico 3";
            eq3.Sensores.Add(new SensoresEquipos
            {
                CodigoSensor = "Cod1",
                NombreSensor = "Sensor 1"
            });
            eq3.Sensores.Add(new SensoresEquipos
            {
                CodigoSensor = "Cod2",
                NombreSensor = "Sensor 2"
            });
            eq3.Sensores.Add(new SensoresEquipos
            {
                CodigoSensor = "Cod3",
                NombreSensor = "Sensor 3"
            });
            lista.Add(eq1);
            lista.Add(eq2);
            lista.Add(eq3);
            return lista;
        }
        // GET: GestionEquipos
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MonitoreoEquipos(DateTime? Desde, DateTime? Hasta)
        {
            var model =new  ListaEquipos();
            model.Equipos = ListaEquipos();
            return View(model);
        }
        public ActionResult RegistrarEquipo()
        {
            var model = new EquiposViewModels();
            return View(model);
        }
        public ActionResult SensoresEquipo()
        { 
           
            var model = new SensoresEquipos
            { 
            };
            return View(model);
        }
        public ActionResult AgregarMantenimiento()
        {
            return View();
        }
        public ActionResult LecturaMedidoresEquipo()
        {
            return View();
        }
        public ActionResult Mantenimientos()
        {
            return View();
        }
        public ActionResult PerfilUsuario()
        {

            var user = UserManager.FindByName(User.Identity.Name);

            var model = new Catalogos_viewModels.PerfilVM
            {
                telefono = user.Celular,
                correo = user.Email
            };
            return View(model);
        }
        public ActionResult EditarPerfilUsuario()
        {
            var user = UserManager.FindByName(User.Identity.Name);

            var model = new Catalogos_viewModels.PerfilVM
            {
                telefono = user.Celular,
                correo = user.Email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GuardarPerfilUsuario(Catalogos_viewModels.PerfilVM model)
        {
            var user = UserManager.FindByNameAsync(User.Identity.Name);
            user.Result.Celular = model.telefono;
            user.Result.Email = model.correo;
            await UserManager.UpdateAsync(user.Result);
            return RedirectToAction("PerfilUsuario");
        }

    }
}