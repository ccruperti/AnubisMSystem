using AnubisDBMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnubisDBMS.Data.ViewModels;
using AnubisDBMS.Infraestructure.Security.Managers;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using AnubisDBMS.Data.Entities;
using AnubisDBMS.Infraestructure.Helpers;
using AnubisDBMS.Infraestructure.Filters.WebFilters;

namespace AnubisDBMS.Controllers
{
    [CustomAuthorization]
    public class Cat_SensoresController : MainController
    {  

        // GET: INDEX
        public ActionResult Index()
        {
            var lista = db.Sensores.Where(x => x.Activo).OrderBy(x => x.IdSensor).ToList() ;
            var VM = new Catalogos_viewModels.SensorVM();
            foreach (var x in lista)
            {
                var model = new Catalogos_viewModels.SensorVM
                {
                     IdSensor=x.IdSensor,
                     SerieSensor=x.SerieSensor,
                     IdTipoSensor=x.TipoSensor?.IdTipoSensor,
                     UnidadSensor=x.TipoSensor?.UnidadSensor,
                     NombreTipoSensor=x.TipoSensor?.NombreTipoSensor,
                     Activo=x.Activo, 
                }; 
                VM.Lista.Add(model);
            } 
            return View(VM);
        }

        //CREATE GET
        public ActionResult Create()
        {
            var model = new Catalogos_viewModels.SensorVM
            {

            };
            ViewBag.IdTipoSensor = SelectListTipoSensor();
            return View(model);
        }
        //CREATE POST
        [HttpPost]
        public ActionResult Create(Catalogos_viewModels.SensorVM model)
        {
            var bdd = db.Sensores.FirstOrDefault(x => x.SerieSensor == model.SerieSensor.Trim().ToUpper());
            if (bdd != null)
            {
                bdd.Activo = true;
                db.SaveChanges();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                    var userId = claim.Value;
                    var nuevo = new Sensor
                    {
                        Activo = true,
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = User.Identity.Name,
                        //Audit end
                        SerieSensor = model.SerieSensor.Trim().ToUpper(), 
                        IdTipoSensor=model.IdTipoSensor??0,
                    }; 
                    db.Sensores.Add(nuevo);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            ViewBag.IdTipoSensor = SelectListTipoSensor();
            return View(model);
        }

        //EDIT GET
        public ActionResult Edit(int id)
        {
            var bdd = db.Sensores.Find(id);
            var model = new Catalogos_viewModels.SensorVM
            {
                IdSensor = id, 
                SerieSensor = bdd.SerieSensor,
                IdTipoSensor=bdd.IdTipoSensor, 
            };
            ViewBag.IdTipoSensor = SelectListTipoSensor();
            return View(model);
        }
        //EDIT POST
        [HttpPost]
        public ActionResult Edit(Catalogos_viewModels.SensorVM model)
        {
            var bdd = db.Sensores.Find(model.IdSensor);
            bdd.IdSensor = model.IdSensor;
            bdd.IdTipoSensor = model.IdTipoSensor??0;
            bdd.SerieSensor = model.SerieSensor.Trim().ToUpper(); 
            //AUDIT
            bdd.UsuarioModificacion = User.Identity.Name;
            bdd.FechaModificacion = DateTime.Now;
            db.SaveChanges();
            ViewBag.IdTipoSensor = SelectListTipoSensor();
            return RedirectToAction("Index");
        }

        //LOGICAL DELETE
        public ActionResult Delete(long id)
        {
            var area = db.Sensores.Find(id);
            area.Activo = false;
            area.UsuarioEliminacion = User.Identity.Name;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

