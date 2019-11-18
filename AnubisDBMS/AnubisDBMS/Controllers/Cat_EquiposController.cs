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

namespace AnubisDBMS.Controllers
{
    public class Cat_EquiposController : Controller
    {
        private AnubisDbContext db = new AnubisDbContext();
        QRGenerator QR = new QRGenerator(); 

        // GET: INDEX
        public ActionResult Index()
        {
            var lista = db.Equipos.Where(x => x.Activo).OrderBy(x => x.IdEquipo).ToList();
            var model = new Catalogos_viewModels.EquipoVM
            {
                Lista = lista
            };
            return View(model);
        }

        //CREATE GET
        public ActionResult Create()
        {
            var model = new Catalogos_viewModels.EquipoVM
            {

            };
            return View(model);
        }
        //CREATE POST
        [HttpPost]
        public ActionResult Create(Catalogos_viewModels.EquipoVM model)
        {
            var bdd = db.Equipos.FirstOrDefault(x => x.SerieEquipo == model.SerieEquipo.Trim().ToUpper());
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
                    var nuevo = new Equipo
                    {
                        Activo = true,
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = User.Identity.Name,
                        //Audit end
                        SerieEquipo = model.SerieEquipo.Trim().ToUpper(),
                        Alias = model.Alias.Trim().ToUpper(),
                        IdUsuario = userId,
                        CodigoQR = QR.GenerarQR(model.SerieEquipo)
                    };

                    db.Equipos.Add(nuevo);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            return View(model);
        }

        //EDIT GET
        public ActionResult Edit(int id)
        {
            var bdd = db.Equipos.Find(id);
            var model = new Catalogos_viewModels.EquipoVM
            {
                IdEquipo = id,
                CodigoQR = bdd.CodigoQR,
                SerieEquipo = bdd.SerieEquipo,
                Alias = bdd.Alias,
                IdUsuario = bdd.IdUsuario

            };
            return View(model);
        }
        //EDIT POST
        [HttpPost]
        public ActionResult Edit(Catalogos_viewModels.EquipoVM model)
        {
            var bdd = db.Equipos.Find(model.IdEquipo);
            bdd.IdEquipo = model.IdEquipo;
            bdd.IdUsuario = model.IdUsuario;
            bdd.SerieEquipo = model.SerieEquipo.Trim().ToUpper();
            bdd.Alias = model.Alias.Trim().ToUpper();
            bdd.CodigoQR = QR.GenerarQR(model.SerieEquipo);
            //AUDIT
            bdd.UsuarioModificacion = User.Identity.Name;
            bdd.FechaModificacion = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //LOGICAL DELETE
        public ActionResult Delete(long id)
        {
            var area = db.Equipos.Find(id);
            area.Activo = false;
            area.UsuarioEliminacion = User.Identity.Name;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

  