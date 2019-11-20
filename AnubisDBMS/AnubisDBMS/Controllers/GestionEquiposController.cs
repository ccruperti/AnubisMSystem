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
    public class GestionEquiposController : MainController
    {


        #region Helpers
        public SelectList ListaEquipos(long? id = null)
        {
            var data = db.Equipos.Where(c => c.Activo).ToList();
            data.Add(new Equipo { IdEquipo = 0, Alias = "Seleccione un Equipo" });
            return new SelectList(data.OrderByDescending(c => c.IdEquipo), "IdEquipo", "Alias", id);
        }
        public SelectList ListaSensores(long? id = null)
        {
            var data = db.Sensores.Where(c => c.Activo).ToList();
            data.Add(new Sensor { IdSensor = 0, SerieSensor = "Seleccione un Sensor" });
            return new SelectList(data.OrderByDescending(c => c.IdSensor), "IdSensor", "SerieSensor", id);
        }
        #endregion
        //public List<EquiposViewModels> ListaEquiposVM()
        //{
        //    var lista = new List<EquiposViewModels>();
        //    var eq1 = new EquiposViewModels();
        //    var eq2 = new EquiposViewModels();
        //    var eq3 = new EquiposViewModels();

        //    eq1.AliasEquipo = "Equipo 1";
        //    eq1.NombreTecnico = "Tecnico 1";
        //    eq1.Sensores.Add(new SensoresEquipos {
        //        CodigoSensor = "Cod1",
        //        NombreSensor = "Sensor 1"
        //    });
        //    eq1.Sensores.Add(new SensoresEquipos
        //    {
        //        CodigoSensor = "Cod2",
        //        NombreSensor = "Sensor 2"
        //    });
        //    eq1.Sensores.Add(new SensoresEquipos
        //    {
        //        CodigoSensor = "Cod3",
        //        NombreSensor = "Sensor 3"
        //    });
        //    eq2.AliasEquipo = "Equipo 2";
        //    eq2.NombreTecnico = "Tecnico 2";
        //    eq2.Sensores.Add(new SensoresEquipos
        //    {
        //        CodigoSensor = "Cod1",
        //        NombreSensor = "Sensor 1"
        //    });
        //    eq2.Sensores.Add(new SensoresEquipos
        //    {
        //        CodigoSensor = "Cod2",
        //        NombreSensor = "Sensor 2"
        //    });
        //    eq2.Sensores.Add(new SensoresEquipos
        //    {
        //        CodigoSensor = "Cod3",
        //        NombreSensor = "Sensor 3"
        //    });

        //    eq3.AliasEquipo = "Equipo 3";
        //    eq3.NombreTecnico = "Tecnico 3";
        //    eq3.Sensores.Add(new SensoresEquipos
        //    {
        //        CodigoSensor = "Cod1",
        //        NombreSensor = "Sensor 1"
        //    });
        //    eq3.Sensores.Add(new SensoresEquipos
        //    {
        //        CodigoSensor = "Cod2",
        //        NombreSensor = "Sensor 2"
        //    });
        //    eq3.Sensores.Add(new SensoresEquipos
        //    {
        //        CodigoSensor = "Cod3",
        //        NombreSensor = "Sensor 3"
        //    });
        //    lista.Add(eq1);
        //    lista.Add(eq2);
        //    lista.Add(eq3);
        //    return lista;
        //}
       
        public ActionResult MonitoreoEquipos(DateTime? Desde, DateTime? Hasta)
        {

            var model = new ListaEquipos();
            var equipos = db.Equipos.Where(x => x.Activo).ToList();
            foreach(var eq in equipos) 
            {
                model.EquiposSensor.Add(new EquipoSensorVM
                {
                    EquipoDb=eq,
                    Sensores = db.EquipoSensor.Count(x => x.IdEquipo == eq.IdEquipo && x.Activo)
                });
            } 
            return View(model);
        }
        public ActionResult RegistrarEquipoSensor(long IdEquipo)
        {
            ViewBag.IdEquipo = ListaEquipos(IdEquipo);
            ViewBag.IdSensor = ListaSensores();
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
            #region GuardadoTemporal
            //var transaction = db.Database.BeginTransaction();
            //if (ModelState.IsValid)
            //{
            //    try
            //    {

            //        if(a == "gc" && model.IdEquipoSensor !=null)
            //        {
            //            var eqsen = db.EquipoSensor.Find(model.IdEquipoSensor);
            //            eqsen.IdEquipo = model.IdEquipo;
            //            db.SaveChanges();
            //            transaction.Commit();
            //            ViewBag.IdEquipo = ListaEquipos(model.IdEquipo);
            //            ViewBag.IdSensor = ListaSensores();
            //            return View(model);

            //        }
            //        else if(a == "gf")
            //        {
            //            var eqsen = db.EquipoSensor.Find(model.IdEquipoSensor);
            //            eqsen.Activo = true;
            //            eqsen.FechaModificacion = DateTime.Now;
            //            db.SaveChanges();
            //            transaction.Commit();
            //            return RedirectToAction("MonitoreoEquipos");

            //        }

            //    }
            //    catch(Exception e)
            //    {
            //        transaction.Rollback();
            //        ViewBag.IdEquipo = ListaEquipos(model.IdEquipo);
            //        ViewBag.IdSensor = ListaSensores();
            //        return View(model);
            //    }

            //}
            #endregion
            if(ModelState.IsValid)
            {
                var val = db.EquipoSensor.Any(x => x.IdSensor == model.IdSensor && x.IdEquipo == model.IdEquipo && x.Activo==false);
                if(val)
                {
                    var activar = db.EquipoSensor.FirstOrDefault(x => x.IdSensor == model.IdSensor && x.IdEquipo == model.IdEquipo && x.Activo == false);
                    activar.Activo = true;
                    activar.FechaModificacion = DateTime.Now;
                    activar.UsuarioModificacion = User.Identity.Name;
                    db.SaveChanges();
                    ViewBag.IdEquipo = ListaEquipos(model.IdEquipo);
                    ViewBag.IdSensor = ListaSensores(model.IdSensor);
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
                }
                ViewBag.IdEquipo = ListaEquipos(model.IdEquipo);
                ViewBag.IdSensor = ListaSensores(model.IdSensor);
                ViewBag.NumPuerto = SelectListPuertos(model.IdEquipo);
                return RedirectToAction("RegistrarEquipoSensor", new { Idequipo = model.IdEquipo });
            }
            ViewBag.IdEquipo = ListaEquipos(model.IdEquipo);
            ViewBag.IdSensor = ListaSensores(model.IdSensor);
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
            ViewBag.IdEquipo = ListaEquipos();
            ViewBag.IdSensor = ListaSensores();
            ViewBag.NumPuerto = SelectListPuertos(es.IdEquipo?? 0);
            return RedirectToAction("RegistrarEquipoSensor", new { Idequipo = es.IdEquipo });
        }
        public ActionResult LecturaMedidoresEquipo(long IdEquipo)
        {
            var equipo = db.Equipos.FirstOrDefault(x => x.IdEquipo == IdEquipo);
            var model = new MonitoreoSensoresVM
            {
                IdEquipo = equipo.IdEquipo,
                QR = equipo.CodigoQR,
                AliasEquipo=equipo.Alias

            }; 
            var equiposSensores = db.EquipoSensor.Where(x => x.IdEquipo == equipo.IdEquipo && x.Activo).ToList();
            foreach(var es in equiposSensores)
            {
                var sensor = db.Sensores.FirstOrDefault(x => x.IdSensor == es.IdSensor);
                 //var lectura =  db.DataSensores.LastOrDefault(x=>x.SerieSensor==sensor.SerieSensor);
                    model.DatosSensores.Add(new DataSensoresVM
                    {
                        SerieSensor=sensor?.SerieSensor,
                        TipoSensor=sensor?.TipoSensor?.NombreTipoSensor,
                        UnidadMedida=sensor?.TipoSensor?.UnidadSensor,
                        Lectura=0
                    });
                
            }
            return View(model);
        }
       
      

    }
}