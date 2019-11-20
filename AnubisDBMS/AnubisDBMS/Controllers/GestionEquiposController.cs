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
            var model =new  ListaEquipos();
            //model.Equipos = ListaEquiposVM();
            model.EquiposDb = db.Equipos.Where(c => c.Activo).ToList();
            return View(model);
        }
        public ActionResult RegistrarEquipo(long? IdEquipo)
        {
            ViewBag.IdEquipo = ListaEquipos(IdEquipo ?? 0);
            ViewBag.IdSensor = ListaSensores();
            var regtemporal = new EquipoSensor();
            var model = new GestionEquiposViewModels();
            if (!db.EquipoSensor.Any(c => c.IdEquipo == IdEquipo))
            {
                regtemporal = new EquipoSensor
                {
                    Activo = false,
                    FechaRegistro = DateTime.Now,
                    UsuarioRegistro = User.Identity.Name,

                };
                db.EquipoSensor.Add(regtemporal);
                db.SaveChanges();
            }
            else
            {
                regtemporal = db.EquipoSensor.FirstOrDefault(c => c.IdEquipo == IdEquipo && c.Activo == false);
            }
        
          
            model = new GestionEquiposViewModels {
                Sensores = IdEquipo != null ? db.EquipoSensor.Where(x => x.IdEquipo == IdEquipo).ToList() : new List<EquipoSensor>(),
                IdEquipoSensor = regtemporal?.IdEquipoSensor,
                IdEquipo = IdEquipo
            };
      
            return View(model);
        }
        [HttpPost]
         public ActionResult RegistrarEquipo(GestionEquiposViewModels model, string a)
        {
            var transaction = db.Database.BeginTransaction();
            if (ModelState.IsValid)
            {
                try
                {
                   
                    if(a == "gc" && model.IdEquipoSensor !=null)
                    {
                        var eqsen = db.EquipoSensor.Find(model.IdEquipoSensor);
                        eqsen.IdEquipo = model.IdEquipo;
                        db.SaveChanges();
                        transaction.Commit();
                        ViewBag.IdEquipo = ListaEquipos(model.IdEquipo);
                        ViewBag.IdSensor = ListaSensores();
                        return View(model);

                    }
                    else if(a == "gf")
                    {
                        var eqsen = db.EquipoSensor.Find(model.IdEquipoSensor);
                        eqsen.Activo = true;
                        eqsen.FechaModificacion = DateTime.Now;
                        db.SaveChanges();
                        transaction.Commit();
                        return RedirectToAction("MonitoreoEquipos");

                    }

                }
                catch(Exception e)
                {
                    transaction.Rollback();
                    ViewBag.IdEquipo = ListaEquipos(model.IdEquipo);
                    ViewBag.IdSensor = ListaSensores();
                    return View(model);
                }
            
            }
            ViewBag.IdEquipo = ListaEquipos(model.IdEquipo);
            ViewBag.IdSensor = ListaSensores();
            return View(model);
        }

        public ActionResult RegistrarSensor(long? IdSensor)
        {

        }
        public ActionResult LecturaMedidoresEquipo()
        {
            return View();
        }
       
      

    }
}