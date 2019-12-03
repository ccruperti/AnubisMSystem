using AnubisDBMS.Data;
using AnubisDBMS.Data.Entities;
using AnubisDBMS.Data.ViewModels;
using AnubisDBMS.Infraestructure.Filters.WebFilters;
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
    
    public class GestionEquiposController : MainController
    {
        [CustomAuthorization]
        public ActionResult MonitoreoEquipos(DateTime? Desde, DateTime? Hasta)
        {

            var model = new ListaEquipos();
            List<Equipo> equipos = db.Equipos.Where(x => x.Activo).ToList();
            List<EquipoSensor> EquipoSensor = new List<EquipoSensor>();
            List<Mantenimiento> Mant = new List<Mantenimiento>();
            foreach (var eq in equipos) 
            {
                EquipoSensor = db.EquipoSensor.Where(x => x.IdEquipo == eq.IdEquipo && x.Activo).ToList();
                foreach (var es in EquipoSensor)
                {
                    Mant = db.Mantenimiento.Where(x => x.Activo && x.IdEquipo == es.IdEquipo && x.Estados.NombreEstado== "Pendiente").ToList();
                
                }
                model.EquiposSensor.Add(new EquipoSensorVM
                {
                    EquipoDb=eq,
                    Sensores = EquipoSensor.Count(),
                    Mantenimeintos= Mant.Count()
                });
            } 
            return View(model);
        }
        [CustomAuthorization]
        public ActionResult RegistrarEquipoSensor(long IdEquipo)
        {
            ViewBag.IdEquipo = SelectListEquipo();
            ViewBag.IdSensor = SelectListSensores();
            ViewBag.NumPuerto = SelectListPuertos(IdEquipo);
            #region RegistroTemporal
            //var model = new GestionEquiposViewModels();
            //var regtemporal = new EquipoSensor();
            //if (!db.EquipoSensor.Any(c => c.IdEquipo == IdEquipo))
            //{
            //    regtemporal = new EquipoSensor
            //    {
            //        Activo = false,
            //        FechaRegistro = DateTime.Now,
            //        UsuarioRegistro = User.Identity.Name,

            //    };
            //    db.EquipoSensor.Add(regtemporal);
            //    db.SaveChanges();
            //}
            //else
            //{
            //    regtemporal = db.EquipoSensor.FirstOrDefault(c => c.IdEquipo == IdEquipo && c.Activo == false);
            //}
            //var model = new GestionEquiposViewModels {
            //    Sensores = IdEquipo != null ? db.EquipoSensor.Where(x => x.IdEquipo == IdEquipo).ToList() : new List<EquipoSensor>(),
            //    IdEquipoSensor = regtemporal?.IdEquipoSensor,
            //    IdEquipo = IdEquipo
            //};
            #endregion
            var EquiposSensores = db.EquipoSensor.Where(x => x.IdEquipo == IdEquipo && x.Activo).OrderBy(x=>x.NumeroPuerto).ToList();
            var model = new GestionEquiposViewModels
            {
                EquiposSensores = EquiposSensores,
                IdEquipo = IdEquipo
            };
            return View(model);
        }
        [HttpPost]
         public ActionResult RegistrarEquipoSensor(GestionEquiposViewModels model)
        { 
            var transaction = db.Database.BeginTransaction();
            if (ModelState.IsValid)
            {
                try
                {
                    var val = db.EquipoSensor.Any(x => x.IdSensor == model.IdSensor && x.IdEquipo == model.IdEquipo && x.NumeroPuerto == model.NumPuerto && x.Activo == false);
                    if (val)
                    {
                        var activar = db.EquipoSensor.FirstOrDefault(x => x.IdSensor == model.IdSensor 
                        && x.IdEquipo == model.IdEquipo
                        && x.NumeroPuerto == model.NumPuerto 
                        && x.Activo == false);
                        activar.Activo = true;
                        activar.FechaModificacion = DateTime.Now;
                        activar.UsuarioModificacion = User.Identity.Name;
                        db.SaveChanges();
                        transaction.Commit();
                        ViewBag.IdEquipo = SelectListEquipo(model.IdEquipo);
                        ViewBag.IdSensor = SelectListSensores(model.IdSensor);
                        ViewBag.NumPuerto = SelectListPuertos(model.IdEquipo);
                        return RedirectToAction("RegistrarEquipoSensor", new { Idequipo = model.IdEquipo });
                    }
                    else
                    {
                        var EquipoSensor = new EquipoSensor
                        {
                            IdEquipo = model.IdEquipo,
                            IdSensor = model.IdSensor,
                            NumeroPuerto = model.NumPuerto,
                            UsuarioRegistro = User.Identity.Name,
                            FechaRegistro = DateTime.Now,
                            Activo = true

                        };
                        db.EquipoSensor.Add(EquipoSensor);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    ViewBag.IdEquipo = SelectListEquipo(model.IdEquipo);
                    ViewBag.IdSensor = SelectListSensores(model.IdSensor);
                    ViewBag.NumPuerto = SelectListPuertos(model.IdEquipo);
                    return RedirectToAction("RegistrarEquipoSensor", new { Idequipo = model.IdEquipo });
                } 
            }
            ViewBag.IdEquipo = SelectListEquipo(model.IdEquipo);
            ViewBag.IdSensor = SelectListSensores(model.IdSensor);
            ViewBag.NumPuerto = SelectListPuertos(model.IdEquipo);
            return RedirectToAction("RegistrarEquipoSensor", new { Idequipo = model.IdEquipo });
        }

        public ActionResult RegistrarSensor(long? IdSensor)
        {
            return View();
        }

        public ActionResult EliminarRelacionEquipoSensor(long IdEquipoSensor)
        {
            var es = db.EquipoSensor.FirstOrDefault(x => x.IdEquipoSensor == IdEquipoSensor);
            es.Activo = false;
            es.FechaModificacion = DateTime.Now;
            es.UsuarioModificacion = User.Identity.Name;
            db.SaveChanges();
            ViewBag.IdEquipo = SelectListEquipo();
            ViewBag.IdSensor = SelectListSensores();
            ViewBag.NumPuerto = SelectListPuertos(es.IdEquipo?? 0);
            return RedirectToAction("RegistrarEquipoSensor", new { Idequipo = es.IdEquipo });
        }
        [CustomAuthorization]
        public ActionResult LecturaMedidoresEquipo(long IdEquipo)
        {
            var equipo = db.Equipos.FirstOrDefault(x => x.IdEquipo == IdEquipo);
            var model = new MonitoreoSensoresVM
            {
                IdEquipo = equipo.IdEquipo,
                QR = equipo.CodigoQR,
                AliasEquipo=equipo.Alias

            }; 
            var equiposSensores = db.EquipoSensor.Where(x => x.Activo && x.IdEquipo == equipo.IdEquipo).ToList();
            foreach(var es in equiposSensores)
            {
                var sensor = db.Sensores.FirstOrDefault(x => x.IdSensor == es.IdSensor);

                 var lectura =  db.DataSensores.OrderByDescending(x=>x.FechaRegistro).FirstOrDefault(x=>x.IdEquipoSensor==es.IdEquipoSensor);
                if(lectura != null)
                {
                    model.DatosSensores.Add(new DataSensoresVM
                    {
                        SerieSensor = sensor?.SerieSensor,
                        TipoSensor = sensor?.TipoSensor?.NombreTipoSensor,
                        UnidadMedida = lectura?.UnidadMedida,
                        Lectura = lectura.lectura
                    });
                }
               
                
            }
            return View(model);
        }
        public ActionResult HabilitarMonitoreo(long IdEquipo, bool IsHab)
        {
            var transaction = db.Database.BeginTransaction();
            try
            {
                if (IsHab)
                {
                    var equipo = db.Equipos.Find(IdEquipo);
                    equipo.AplicaMonitoreo = true;
                    equipo.FechaModificacion = DateTime.Now;
                    equipo.UsuarioModificacion = User.Identity.Name;
                    db.SaveChanges();
                }
                else
                {
                    var equipo = db.Equipos.Find(IdEquipo);
                    equipo.AplicaMonitoreo = false;
                    equipo.FechaModificacion = DateTime.Now;
                    equipo.UsuarioModificacion = User.Identity.Name;
                    db.SaveChanges();
                }
                transaction.Commit();

            }
            catch (Exception e)
            {
                transaction.Rollback();
            }
            return RedirectToAction("MonitoreoEquipos");

        }
        public ActionResult GraficosLecturasMedidores(long IdEquipo)
        {
            var lecturas = db.DataSensores.Where(c => c.EquipoSensor.IdEquipo == IdEquipo).Select(x => new { 
            lec = x.lectura,
            sensor = x.EquipoSensor.Sensores.SerieSensor
            }).ToList();
            return Json(lecturas, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AccesoBloqueado()
        {
            return View();
        }

    }
}