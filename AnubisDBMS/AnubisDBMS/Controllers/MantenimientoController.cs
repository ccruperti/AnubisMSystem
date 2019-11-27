using AnubisDBMS.Data;
using AnubisDBMS.Data.Entities;
using AnubisDBMS.Data.ViewModels;
using AnubisDBMS.Infraestructure.Filters.WebFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static AnubisDBMS.Controllers.HomeController;

namespace AnubisDBMS.Controllers
{
    [CustomAuthorization]
    public class MantenimientoController : MainController
    {
          

        public ActionResult AgregarMantenimiento(long IdEquipo)
        {
            ViewBag.IdFrecuencia = SelectListFrecuencias();
            ViewBag.IdTecnico = SelectListTecnico();
            var equipo = db.Equipos.FirstOrDefault(x => x.IdEquipo == IdEquipo && x.Activo);
            var model = new MantenimientoVM{
                FechaMant = DateTime.Now,
                IdEquipo = equipo.IdEquipo,
                AliasEquipo=equipo.Alias,
                QR=equipo.CodigoQR,
                Descripcion="" 
            };
          
            return View(model);
        }
        [HttpPost]
        public ActionResult AgregarMantenimiento(MantenimientoVM model)
        {
            var transaction = db.Database.BeginTransaction();
            if (ModelState.IsValid)
            {
                try
                {
                    var mantenimiento = new Mantenimiento
                    {
                        IdTecnico = model.IdTecnico,
                        IdEquipo= model.IdEquipo,
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = User.Identity.Name,
                        Activo = true,
                        IdEstado = db.Estados.FirstOrDefault(c => c.Activo && c.TipoEstado == "Mantenimiento" && c.NombreEstado == "Pendiente").IdEstado,
                        IdFrecuencia = model.IdFrecuencia,
                        Descripcion = model.Descripcion,
                        FechaMantenimiento = model.FechaMant


                    };
                    db.Mantenimiento.Add(mantenimiento);
                    db.SaveChanges();
                    var eq = db.Equipos.Find(model.IdEquipo);
                    TempData["Mensaje"] = new MensajeViewModel(true, "Registro Exitoso!", "Se ingreso un mantenimiento para el equipo: " + eq.Alias);
                    transaction.Commit();
                    ViewBag.IdFrecuencia = SelectListFrecuencias(model.IdFrecuencia);
                    ViewBag.IdTecnico = SelectListTecnico(model.IdTecnico);
                    return View(model);
                }
                catch(Exception e)
                {
                    transaction.Rollback();
                    ViewBag.IdFrecuencia = SelectListFrecuencias(model.IdFrecuencia);
                    ViewBag.IdTecnico = SelectListTecnico(model.IdTecnico);
                    TempData["Mensaje"] = new MensajeViewModel(false, "Error de registro", "Ocurrio un erorr al guardar los datos.");
                    return View(model);
                }
            

            }
            TempData["Mensaje"] = new MensajeViewModel(false, "Error de registro", "Ocurrio un error al registrar el mantenimiento, revise que todos los campos esten ingresados correctamente.");

            return View(model);
        }
            public ActionResult Mantenimientos(long IdEquipo)
        {
            var Eq = db.Equipos.FirstOrDefault(x => x.IdEquipo == IdEquipo && x.Activo);
            var model = new MantenimientoVM
            {
                Lista = db.Mantenimiento.Where(c => c.Activo && c.IdEquipo == IdEquipo).ToList(),
                IdEquipo=Eq.IdEquipo
                
            };
            return View(model);
        }
    }
}