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
using AnubisDBMS.Infraestructure.Extensions;
using AnubisDBMS.Reportes;
using System.Data.Entity;

namespace AnubisDBMS.Controllers
{
    
    public class GestionEquiposController : MainController
    {
        [CustomAuthorization]
        public ActionResult MonitoreoEquipos(DateTime? Desde, DateTime? Hasta)
        {

            var model = new ListaEquipos();
            List<Equipo> equipos = db.Equipos.Where(x => x.Activo && x.IdEmpresa == IdEmpresa).ToList();
            List<EquipoSensor> EquipoSensor = new List<EquipoSensor>();
            List<Mantenimiento> Mant = new List<Mantenimiento>();
            foreach (var eq in equipos) 
            {
                EquipoSensor = db.EquipoSensor.Where(x => x.IdEquipo == eq.IdEquipo && x.Activo && x.IdEmpresa == IdEmpresa).ToList();
                foreach (var es in EquipoSensor)
                {
                    Mant = db.Mantenimiento.Where(x => x.Activo && x.IdEquipo == es.IdEquipo && x.Estados.NombreEstado== "Pendiente" && x.IdEmpresa == IdEmpresa).ToList();
                
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
            var EquiposSensores = db.EquipoSensor.Where(x => x.IdEquipo == IdEquipo && x.Activo && x.IdEmpresa == IdEmpresa).OrderBy(x=>x.NumeroPuerto).ToList();
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
                    var val = db.EquipoSensor.Any(x => x.IdSensor == model.IdSensor && x.IdEquipo == model.IdEquipo && x.NumeroPuerto == model.NumPuerto && x.Activo == false && x.IdEmpresa == IdEmpresa);
                    if (val)
                    {
                        var activar = db.EquipoSensor.FirstOrDefault(x => x.IdSensor == model.IdSensor 
                        && x.IdEquipo == model.IdEquipo
                        && x.NumeroPuerto == model.NumPuerto 
                        && x.Activo == false && x.IdEmpresa == IdEmpresa);
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
                            IdEmpresa = IdEmpresa,
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
            var es = db.EquipoSensor.FirstOrDefault(x => x.IdEquipoSensor == IdEquipoSensor && x.IdEmpresa == IdEmpresa);
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
            var equiposSensores = db.EquipoSensor.Where(x => x.Activo && x.IdEquipo == equipo.IdEquipo && x.IdEmpresa == IdEmpresa).ToList();
            foreach(var es in equiposSensores)
            {
                var sensor = db.Sensores.FirstOrDefault(x => x.IdSensor == es.IdSensor);

                 var lectura =  db.DataSensores.OrderByDescending(x=>x.FechaRegistro).FirstOrDefault(x=>x.TipoSensor==sensor.TipoSensor.NombreTipoSensor && x.IdEmpresa == IdEmpresa);
                if(lectura != null)
                {
                    model.DatosSensores.Add(new DataSensoresVM
                    {
                        
                        TipoSensor = sensor?.TipoSensor?.NombreTipoSensor,
                        UnidadMedida = lectura?.UnidadMedida,
                        Lectura = lectura.Medida, 
                        LecMin = sensor.TipoSensor.Min_TipoSensor,
                        LecMax = sensor.TipoSensor.Max_TipoSensor,
                    });
                }
               
                
            }
            model.Desde = DateTime.Now.AddDays(-7);
            model.Hasta=DateTime.Now; 
            return View(model);
        }
        [HttpPost]
        public ActionResult LecturaMedidoresEquipo(MonitoreoSensoresVM model)
        {
            var equipo = db.Equipos.FirstOrDefault(x => x.IdEquipo == model.IdEquipo && x.IdEmpresa == IdEmpresa);
        
            var equiposSensores = db.EquipoSensor.Where(x => x.Activo && x.IdEquipo == equipo.IdEquipo && x.IdEmpresa == IdEmpresa).ToList();
            foreach (var es in equiposSensores)
            {
                var sensor = db.Sensores.FirstOrDefault(x => x.IdSensor == es.IdSensor && x.IdEmpresa == IdEmpresa);

                var lectura = db.DataSensores.OrderByDescending(x => x.FechaRegistro).FirstOrDefault(x => x.TipoSensor == sensor.TipoSensor.NombreTipoSensor && x.IdEmpresa == IdEmpresa);
                if (lectura != null)
                {
                    model.DatosSensores.Add(new DataSensoresVM
                    { 
                        TipoSensor = sensor?.TipoSensor?.NombreTipoSensor,
                        UnidadMedida = lectura?.UnidadMedida,
                        Lectura = lectura.Medida, 
                        LecMin = sensor.TipoSensor.Min_TipoSensor,
                        LecMax = sensor.TipoSensor.Max_TipoSensor,
                        
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
        public ActionResult GraficosLecturasMedidores(string TipoSensor, DateTime? Desde, DateTime? Hasta)
        {
            if(Desde == null && Hasta == null)
            {

                Desde = DateTime.Now.GetWeekStartDate();
                Hasta = DateTime.Now.GetWeekEndDate();
            }
            var EquipoSensor = db.EquipoSensor.Where(x => x.Sensores.TipoSensor.NombreTipoSensor == TipoSensor && x.FechaRegistro >= Desde && x.FechaRegistro <= Hasta && x.IdEmpresa == IdEmpresa).Select(c => c.Sensores.TipoSensor.NombreTipoSensor).ToList();
            
            var lecturas = db.DataSensores.Where(c => EquipoSensor.Contains(c.TipoSensor) && c.IdEmpresa == IdEmpresa).Select(x => new
            {
                lec = x.Medida, 
                lecmin = db.TipoSensor.FirstOrDefault(y => y.NombreTipoSensor == x.TipoSensor && y.IdEmpresa == IdEmpresa).Min_TipoSensor,
                lecmax = db.TipoSensor.FirstOrDefault(y => y.NombreTipoSensor == x.TipoSensor && y.IdEmpresa == IdEmpresa).Max_TipoSensor,
                sensor = x.TipoSensor,                
                Dia= x.FechaRegistro.Day,
                Mes = x.FechaRegistro.Month,
                Anio = x.FechaRegistro.Year,
                Hora = x.FechaRegistro.Hour,
                Minuto = x.FechaRegistro.Minute
            }).ToList();
            return Json(lecturas.Where(x => x.sensor == TipoSensor ).OrderBy(x=>x.Anio).ThenBy(x=>x.Mes).ThenBy(x=>x.Dia).ThenBy(x => x.Hora).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AccesoBloqueado()
        {
            return View();
        }
        
        public ActionResult GenerarExcel(string TipoSensor, DateTime? Desde, DateTime? Hasta)
        {
            var fecha = ViewBag.Desde;
            if(Desde == null && Hasta == null)
            {
                Desde = DateTime.Now.GetWeekStartDate();
                Hasta = DateTime.Now.GetWeekEndDate();
            }
            GenerarExcelConsultas Gen = new GenerarExcelConsultas();
            byte[] fileStream = Gen.GenerarDocumentoLecturasEquipos(TipoSensor, Desde, Hasta, IdEmpresa);
            string fileName = string.Format("Lecturas.xlsx");
            return File(fileStream.ToArray(), "application/octet-stream", fileName);
        }

        public ActionResult MedicionesSensor(string TipoSensor, DateTime? Desde, DateTime? Hasta)
        {
            var model = new ConsultasMedicionesViewModel();
            var desde = ViewBag.Desde;
            model.Lecturas = db.DataSensores.Where(x => x.TipoSensor == TipoSensor
            && (DbFunctions.TruncateTime(x.FechaRegistro) >= DbFunctions.TruncateTime(Desde)
            && DbFunctions.TruncateTime(x.FechaRegistro) <= DbFunctions.TruncateTime(Hasta)) && x.IdEmpresa == IdEmpresa).ToList();
            return View(model);
        }
    }
}