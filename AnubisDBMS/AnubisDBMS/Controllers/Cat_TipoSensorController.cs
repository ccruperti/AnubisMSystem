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
    public class Cat_TipoSensorController : MainController
    {
        
        // GET: INDEX
        public ActionResult Index()
        {
            var lista = db.TipoSensor.Where(x => x.Activo).OrderBy(x => x.IdTipoSensor).ToList();
            var model = new Catalogos_viewModels.TipoSensorVM
            {
                Lista = lista
            };
            return View(model);
        }

        //CREATE GET
        public ActionResult Create()
        {
            var model = new Catalogos_viewModels.TipoSensorVM
            {

            };
            return View(model);
        }
        //CREATE POST
        [HttpPost]
        public ActionResult Create(Catalogos_viewModels.TipoSensorVM model)
        {
            var bdd = db.TipoSensor.FirstOrDefault(x => x.NombreTipoSensor == model.NombreTipoSensor.Trim().ToUpper());
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
                    var nuevo = new TipoSensor
                    {
                        Activo = true,
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = User.Identity.Name,
                        //Audit end
                      NombreTipoSensor=model.NombreTipoSensor,
                      UnidadSensor=model.UnidadSensor
                    };

                    db.TipoSensor.Add(nuevo);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            return View(model);
        }

        //EDIT GET
        public ActionResult Edit(int id)
        {
            var bdd = db.TipoSensor.Find(id);
            var model = new Catalogos_viewModels.TipoSensorVM
            {
                IdTipoSensor = id,
                NombreTipoSensor = bdd.NombreTipoSensor.Trim().ToUpper(),
                UnidadSensor = bdd.UnidadSensor.Trim()

            };
            return View(model);
        }
        //EDIT POST
        [HttpPost]
        public ActionResult Edit(Catalogos_viewModels.TipoSensorVM model)
        {
            var bdd = db.TipoSensor.Find(model.IdTipoSensor);
            bdd.IdTipoSensor = model.IdTipoSensor;
            bdd.NombreTipoSensor = model.NombreTipoSensor;
            bdd.UnidadSensor = model.UnidadSensor;
            //AUDIT
            bdd.UsuarioModificacion = User.Identity.Name;
            bdd.FechaModificacion = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //LOGICAL DELETE
        public ActionResult Delete(long id)
        {
            var area = db.TipoSensor.Find(id);
            area.Activo = false;
            area.UsuarioEliminacion = User.Identity.Name;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
