using AnubisDBMS.Data;
using AnubisDBMS.Data.ViewModels;
using AnubisDBMS.Infraestructure.Filters.WebFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnubisDBMS.Controllers
{
    [CustomAuthorization]
    public class MantenimientoController : MainController
    {
          

        public ActionResult AgregarMantenimiento(long? IdEquipo)
        {
            ViewBag.IdFrecuencia = SelectListFrecuencias();
            var model = new MantenimientoVM{
                FechaMant = DateTime.Now,
                IdEquipo = IdEquipo
             
            };
            return View();
        }

        public ActionResult Mantenimientos(long? IdEquipo)
        {
            var model = new MantenimientoVM
            {
                Lista = db.Mantenimiento.Where(c => c.Activo && c.EquiposSensor.IdEquipo == IdEquipo).ToList()
            };
            return View(model);
        }
    }
}