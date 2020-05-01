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
using AnubisDBMS.Data.Localization.Entities;

namespace AnubisDBMS.Controllers
{
    [CustomAuthorization]
    public class Cat_EmpresasController : MainController
    {


        // GET: INDEX

        public ActionResult Index()
        {

            var lista = db.Empresas.Where(x => x.Activo).OrderBy(x => x.IdEmpresa).ToList();
            var model = new Catalogos_viewModels.EmpresaVM
            {
                Lista = lista
            };
            return View(model);
        }

        //CREATE GET
        public ActionResult Create()
        {
            var model = new Catalogos_viewModels.EmpresaVM
            {

            };
            return View(model);
        }
        //CREATE POST
        [HttpPost]
        public ActionResult Create(Catalogos_viewModels.EmpresaVM model)
        {
            var bdd = db.Empresas.FirstOrDefault(x => x.Nombre == model.Nombre.Trim().ToUpper());
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
                    var nuevo = new Empresa
                    {
                        Activo = true,
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = User.Identity.Name,
                        //Audit end
                        Nombre = model.Nombre.Trim().ToUpper(),
                        RazonSocial = model.RazonSocial.Trim().ToUpper(),
                        RUC = model.RUC,
                        EmailNotificacion=model.EmailNotificacion,
                        PrimeraNotificacion=model.PrimeraNotificacion,
                        SegundaNotificacion=model.SegundaNotificacion,
                        TerceraNotificacion=model.TerceraNotificacion,
                        ServicioActivo=model.ServicioActivo, 
                        IdEmpresa = IdEmpresa
                    };

                    db.Empresas.Add(nuevo);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            return View(model);
        }

        //EDIT GET
        public ActionResult Edit(int id)
        {
            var bdd = db.Empresas.Find(id);
            var model = new Catalogos_viewModels.EmpresaVM
            {

                Nombre = bdd.Nombre.Trim().ToUpper(),
                RazonSocial = bdd.RazonSocial.Trim().ToUpper(),
                RUC = bdd.RUC,
                EmailNotificacion = bdd.EmailNotificacion,
                PrimeraNotificacion = bdd.PrimeraNotificacion,
                SegundaNotificacion = bdd.SegundaNotificacion,
                TerceraNotificacion = bdd.TerceraNotificacion,
                ServicioActivo = bdd.ServicioActivo,
                IdEmpresa = bdd.IdEmpresa
            };
            return View(model);
        }
        //EDIT POST
        [HttpPost]
        public ActionResult Edit(Catalogos_viewModels.EmpresaVM model)
        {
            var bdd = db.Empresas.Find(model.IdEmpresa);
            bdd.IdEmpresa = model.IdEmpresa;
            bdd.Nombre = model.Nombre.Trim().ToUpper();
            bdd.RazonSocial = model.RazonSocial.Trim().ToUpper();
            bdd.RUC = model.RUC;
            bdd.EmailNotificacion = model.EmailNotificacion;
            bdd.PrimeraNotificacion = model.PrimeraNotificacion;
            bdd.SegundaNotificacion = model.SegundaNotificacion;
            bdd.TerceraNotificacion = model.TerceraNotificacion;
            bdd.ServicioActivo = model.ServicioActivo; 
            //AUDIT
            bdd.UsuarioModificacion = User.Identity.Name;
            bdd.FechaModificacion = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //LOGICAL DELETE
        public ActionResult Delete(long id)
        {
            var a = db.Empresas.Find(id);
            a.Activo = false;
            a.UsuarioEliminacion = User.Identity.Name;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}