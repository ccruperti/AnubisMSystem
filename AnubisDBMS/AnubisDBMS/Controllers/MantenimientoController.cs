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
          

        public ActionResult AgregarMantenimiento(long IdEquipo, bool Registro=false)
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
            if(Registro)
            { 
            TempData["Mensaje"] = new MensajeViewModel(true, "Registro Exitoso!", "Se ingreso un mantenimiento para el equipo: " + equipo.Alias);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult AgregarMantenimiento(MantenimientoVM model, string submitButton)
        {
            switch (submitButton)
            {
                case "SaveAndCont":
                    var transaction = db.Database.BeginTransaction();
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            var mantenimiento = new Mantenimiento
                            {
                                IdTecnico = model.IdTecnico,
                                IdEquipo = model.IdEquipo,
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
                            transaction.Commit();
                            ViewBag.IdFrecuencia = SelectListFrecuencias(model.IdFrecuencia);
                            ViewBag.IdTecnico = SelectListTecnico(model.IdTecnico);
                            return RedirectToAction("AgregarMantenimiento", new { model.IdEquipo, Registro = true });
                        }
                        catch (Exception e)
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
                case "SaveAndBack":
                    var transaction2 = db.Database.BeginTransaction();
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            var mantenimiento = new Mantenimiento
                            {
                                IdTecnico = model.IdTecnico,
                                IdEquipo = model.IdEquipo,
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
                            transaction2.Commit();
                            ViewBag.IdFrecuencia = SelectListFrecuencias(model.IdFrecuencia);
                            ViewBag.IdTecnico = SelectListTecnico(model.IdTecnico);
                            return RedirectToAction("Mantenimientos", new { model.IdEquipo, Registro=true});
                        }
                        catch (Exception e)
                        {
                            transaction2.Rollback();
                            ViewBag.IdFrecuencia = SelectListFrecuencias(model.IdFrecuencia);
                            ViewBag.IdTecnico = SelectListTecnico(model.IdTecnico);
                            TempData["Mensaje"] = new MensajeViewModel(false, "Error de registro", "Ocurrio un erorr al guardar los datos.");
                            return View(model);
                        } 
                    }
                    TempData["Mensaje"] = new MensajeViewModel(false, "Error de registro", "Ocurrio un error al registrar el mantenimiento, revise que todos los campos esten ingresados correctamente.");
                    return View(model); 
                default: return View();
            }
        }
            public ActionResult Mantenimientos(long IdEquipo, bool Registro=false)
        {
            var Eq = db.Equipos.FirstOrDefault(x => x.IdEquipo == IdEquipo && x.Activo);
            var model = new MantenimientoVM
            {
                Lista = db.Mantenimiento.Where(c => c.Activo && c.IdEquipo == IdEquipo).ToList(),
                IdEquipo=Eq.IdEquipo,
                AliasEquipo = Eq.Alias
                
            }; 
            if (Registro)
            {
                TempData["Mensaje"] = new MensajeViewModel(true, "Registro Exitoso!", "Se ingreso un mantenimiento para el equipo: " + Eq.Alias);
            }
            return View(model);
        }
    }
}