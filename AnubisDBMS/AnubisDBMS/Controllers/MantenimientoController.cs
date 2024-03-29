﻿using AnubisDBMS.Data.Entities;
using AnubisDBMS.Data.ViewModels;
using AnubisDBMS.Infraestructure.Filters.WebFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static AnubisDBMS.Controllers.HomeController;

namespace AnubisDBMS.Controllers
{
    [CustomAuthorization]
    public class MantenimientoController : MainController
    {

        public ActionResult AgregarMantenimiento(long IdEquipo, bool Registro = false)
        {
            ViewBag.IdFrecuencia = SelectListFrecuencias();
            ViewBag.IdTecnico = SelectListTecnico();
            var equipo = db.Equipos.FirstOrDefault(x => x.IdEquipo == IdEquipo && x.Activo);
            var model = new MantenimientoVM {
                FechaMant = DateTime.Now,
                IdEquipo = equipo.IdEquipo,
                AliasEquipo = equipo.Alias,
                QR = equipo.CodigoQR,
                Descripcion = ""
            };
            if (Registro)
            {
                TempData["Mensaje"] = new MensajeViewModel(true, "Registro Exitoso!", "Se ingreso un mantenimiento para el equipo: " + equipo.Alias);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult AgregarMantenimiento(MantenimientoVM model, string submitButton)
        {
            if (ModelState.IsValid)
            {
                switch (submitButton)
                {
                    case "SaveAndCont":
                        var modelo = GuardarMantenimiento(model);
                        if (modelo != null)
                        {
                            return RedirectToAction("AgregarMantenimiento", new { modelo.IdEquipo, Registro = true });
                        }
                        else
                        {
                            return View(modelo);
                        }
                    case "SaveAndBack":
                        var modelo2 = GuardarMantenimiento(model);
                        if (modelo2 != null)
                        {

                            return RedirectToAction("Mantenimientos", new { modelo2.IdEquipo, Registro = true });
                        }
                        else
                        {
                            return View(modelo2);
                        }
                    default:
                        return View(model);
                }
            }
            else
                ViewBag.IdFrecuencia = SelectListFrecuencias();
            ViewBag.IdTecnico = SelectListTecnico();
            return View(model);
        }
        public ActionResult EditarMantenimiento(long id)
        {
            var mant = db.Mantenimiento.FirstOrDefault(x => x.IdManteniemiento == id);

            ViewBag.IdFrecuencia = SelectListFrecuencias(mant.IdFrecuencia);
            ViewBag.IdTecnico = SelectListTecnico(mant.IdTecnico);
            var model = new MantenimientoVM
            {
                FechaMant = DateTime.Now,
                IdEquipo = mant.IdEquipo ?? 0,
                AliasEquipo = mant.Equipo?.Alias,
                QR = mant.Equipo.CodigoQR,
                Descripcion = mant.Descripcion,
                Notificaciones=mant.Notificiaciones,
                IdManteniemiento=mant.IdManteniemiento
            };
            //if (Registro)
            //{
            //    TempData["Mensaje"] = new MensajeViewModel(true, "Registro Exitoso!", "Se ingreso un mantenimiento para el equipo: " + equipo.Alias);
            //}
            return View(model);
        }
        [HttpPost]
        public ActionResult EditarMantenimiento(MantenimientoVM model, string submitButton)
        {
            if (ModelState.IsValid)
            {
                switch (submitButton)
                {
                    case "SaveAndCont":
                        var modelo = EditarMantenimientoVM(model);
                        if (modelo != null)
                        {
                            return RedirectToAction("EditarMantenimiento", new { id = model.IdManteniemiento });
                        }
                        else
                        {
                            return View(modelo);
                        }
                    case "SaveAndBack":
                        var modelo2 = EditarMantenimientoVM(model);
                        if (modelo2 != null)
                        {

                            return RedirectToAction("Mantenimientos", new { modelo2.IdEquipo, Registro = true });
                        }
                        else
                        {
                            return View(modelo2);
                        }
                    default:
                        return View(model);
                }
            }
            else
                ViewBag.IdFrecuencia = SelectListFrecuencias();
            ViewBag.IdTecnico = SelectListTecnico();
            return View(model);
        }
        [HttpPost]
        public ActionResult CambiarEstadoMantenimiento(long? IdMant, string Desc)
        {
            if (IdMant != null)
            {
                var mant = db.Mantenimiento.Find(IdMant);
                mant.FechaModificacion = DateTime.Now;
                mant.FechaFinMantenimiento = DateTime.Now;
                mant.UsuarioModificacion = User.Identity.Name;
                mant.Notas = Desc;
                mant.IdEstado = db.Estados.FirstOrDefault(c => c.TipoEstado == "Mantenimiento" && c.NombreEstado == "Completado").IdEstado;
                db.SaveChanges();
                return View();
            }
            return View();

        }

        public MantenimientoVM GuardarMantenimiento(MantenimientoVM model)
        {
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
                        FechaMantenimiento = model.FechaMant,
                        Notificiaciones=model.Notificaciones,
                        IdEmpresa = IdEmpresa
                    };
                    db.Mantenimiento.Add(mantenimiento);
                    db.SaveChanges();

                    transaction.Commit();
                    ViewBag.IdFrecuencia = SelectListFrecuencias(model.IdFrecuencia);
                    ViewBag.IdTecnico = SelectListTecnico(model.IdTecnico);
                    return model;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    ViewBag.IdFrecuencia = SelectListFrecuencias(model.IdFrecuencia);
                    ViewBag.IdTecnico = SelectListTecnico(model.IdTecnico);
                    TempData["Mensaje"] = new MensajeViewModel(false, "Error de registro", "Ocurrio un error al guardar los datos.");
                    return null;
                }
            }
            TempData["Mensaje"] = new MensajeViewModel(false, "Error de registro", "Ocurrio un error al registrar el mantenimiento, revise que todos los campos esten ingresados correctamente.");
            return null;
        }
        public MantenimientoVM EditarMantenimientoVM(MantenimientoVM model)
        {
            var transaction = db.Database.BeginTransaction();
            if (ModelState.IsValid)
            {
                try
                { 
                    var mantenimiento = db.Mantenimiento.Find(model.IdManteniemiento);
                    mantenimiento.Notificiaciones = model.Notificaciones;
                    mantenimiento.IdTecnico = model.IdTecnico;
                    mantenimiento.IdEquipo = model.IdEquipo;
                    mantenimiento.FechaRegistro = DateTime.Now;
                    mantenimiento.UsuarioRegistro = User.Identity.Name;
                    mantenimiento.Activo = true;
                    mantenimiento.IdEstado = db.Estados.FirstOrDefault(c => c.Activo && c.TipoEstado == "Mantenimiento" && c.NombreEstado == "Pendiente").IdEstado;
                    mantenimiento.IdFrecuencia = model.IdFrecuencia;
                    mantenimiento.Descripcion = model.Descripcion;
                    mantenimiento.FechaMantenimiento = model.FechaMant;
                    mantenimiento.IdEmpresa = IdEmpresa; 
                                          
                    db.SaveChanges();

                    transaction.Commit();
                    ViewBag.IdFrecuencia = SelectListFrecuencias(model.IdFrecuencia);
                    ViewBag.IdTecnico = SelectListTecnico(model.IdTecnico);
                    return model;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    ViewBag.IdFrecuencia = SelectListFrecuencias(model.IdFrecuencia);
                    ViewBag.IdTecnico = SelectListTecnico(model.IdTecnico);
                    TempData["Mensaje"] = new MensajeViewModel(false, "Error de registro", "Ocurrio un error al guardar los datos.");
                    return null;
                }
            }
            TempData["Mensaje"] = new MensajeViewModel(false, "Error de registro", "Ocurrio un error al registrar el mantenimiento, revise que todos los campos esten ingresados correctamente.");
            return null;
        }

        public ActionResult Mantenimientos(long IdEquipo, bool Registro = false)
        {
            var Eq = db.Equipos.FirstOrDefault(x => x.IdEquipo == IdEquipo && x.Activo && x.IdEmpresa == IdEmpresa);
            var model = new MantenimientoVM
            {
                Lista = db.Mantenimiento.Where(c => c.Activo && c.IdEquipo == IdEquipo).ToList(),
                IdEquipo = Eq.IdEquipo,
                AliasEquipo = Eq.Alias

            };
            if (Registro)
            {
                TempData["Mensaje"] = new MensajeViewModel(true, "Registro Exitoso!", "Se ingreso un mantenimiento para el equipo: " + Eq.Alias);
            }
            return View(model);
        }
        public ActionResult ListaErrosDataSensores(long? Id = null)
        {
            var listaErrores = new List<DataSensores>();
            if (User.IsInRole("Developers"))
            {
                listaErrores = db.DataSensores.Where(c => c.Activo && c.Error).ToList();
            }
            else
            {
                listaErrores = db.DataSensores.Where(c => c.Activo && c.Error && c.AlertaRecibida == false && c.IdEmpresa == IdEmpresa  ).ToList();
            }


            List<Alerta> model = new List<Alerta>();
            foreach (var error in listaErrores)
            {
                var equipoSensor = db.EquipoSensor.FirstOrDefault(x => x.Sensores.TipoSensor.NombreTipoSensor == error.TipoSensor);
                model.Add(new Alerta
                {
                    Min = equipoSensor.Sensores.Min ?? 0,
                    Max = equipoSensor.Sensores.Max ?? 0,
                    Equipo = equipoSensor.Equipos.SerieEquipo,
                    medida = error.Medida,
                    Sensor = equipoSensor.Sensores.TipoSensor.NombreTipoSensor,
                    UnidadMedida = error.UnidadMedida,
                    encimadebajo = error.EncimaNormal == true ? "Encima de lo normal" : "Debajo de lo normal"
                });

            }

            return View(model);
        }

        public ActionResult RevisarMedidas()
        {
            if (!User.IsInRole("Developers"))
            {
                var listaErrores = db.DataSensores.Where(c => c.Activo && c.Error && c.IdEmpresa == IdEmpresa).ToList();
             foreach(var l in listaErrores)
            {
                l.AlertaRecibida = true;
                db.SaveChanges();
            } 
            }
            return Redirect("ListaErrosDataSensores");
        }
         
        }
}