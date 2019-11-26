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
          

        public ActionResult AgregarMantenimiento(long IdEquipo)
        {
            var equipo = db.Equipos.FirstOrDefault(x => x.IdEquipo == IdEquipo && x.Activo);
            var model = new MantenimientoVM{
                FechaMant = DateTime.Now,
                IdEquipo = equipo.IdEquipo,
                AliasEquipo=equipo.Alias,
                QR=equipo.CodigoQR,
                Descripcion="" 
            };
            ViewBag.IdFrecuencia = SelectListFrecuencias();
            ViewBag.IdTecnico = SelectListTecnico();
            return View(model);
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