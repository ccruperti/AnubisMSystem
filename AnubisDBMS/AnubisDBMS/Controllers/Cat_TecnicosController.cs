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
    public class Cat_TecnicosController : MainController
    { 

        // GET: INDEX
        public ActionResult Index()
        {
            var lista = db.Tecnicos.Where(x => x.Activo).OrderBy(x => x.IdTecnico).ToList();
            var model = new Catalogos_viewModels.TecnicoVM
            {
                Lista = lista
            };
            return View(model);
        }

        //CREATE GET
        public ActionResult Create()
        {
            var model = new Catalogos_viewModels.TecnicoVM
            {

            };
            return View(model);
        }
        //CREATE POST
        [HttpPost]
        public ActionResult Create(Catalogos_viewModels.TecnicoVM model)
        {
            var bdd = db.Tecnicos.FirstOrDefault(x => x.NombreTecnico == model.NombreTecnico.Trim().ToUpper());
            if (bdd != null)
            {
                bdd.Activo = true;
                db.SaveChanges();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var nuevo = new Tecnicos
                    {
                        Activo = true,
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = User.Identity.Name,
                        //Audit end
                        NombreTecnico = model.NombreTecnico.Trim().ToUpper(),
                        Cedula = model.Cedula.Trim(),
                        CelularTecnico = model.CelularTecnico,
                        CorreoTecnico = model.CorreoTecnico,
                        };

                    db.Tecnicos.Add(nuevo);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            return View(model);
        }

        //EDIT GET
        public ActionResult Edit(int id)
        {
            var bdd = db.Tecnicos.Find(id);
            var model = new Catalogos_viewModels.TecnicoVM
            {
                IdTecnico = id,
                NombreTecnico = bdd.NombreTecnico,
                Cedula = bdd.Cedula.Trim(),
                CelularTecnico = bdd.CelularTecnico,
                CorreoTecnico = bdd.CorreoTecnico

            };
            return View(model);
        }
        //EDIT POST
        [HttpPost]
        public ActionResult Edit(Catalogos_viewModels.TecnicoVM model)
        {
            var bdd = db.Tecnicos.Find(model.IdTecnico);
            bdd.IdTecnico = model.IdTecnico;
            bdd.NombreTecnico = model.NombreTecnico;
            bdd.Cedula = model.Cedula;
            bdd.CelularTecnico = model.CelularTecnico;
            bdd.CorreoTecnico = model.CorreoTecnico;
            //AUDIT
            bdd.UsuarioModificacion = User.Identity.Name;
            bdd.FechaModificacion = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //LOGICAL DELETE
        public ActionResult Delete(long id)
        {
            var area = db.Tecnicos.Find(id);
            area.Activo = false;
            area.UsuarioEliminacion = User.Identity.Name;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

