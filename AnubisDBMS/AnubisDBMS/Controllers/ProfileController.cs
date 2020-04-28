using AnubisDBMS.Data.ViewModels;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using System;

namespace AnubisDBMS.Controllers
{
    public class ProfileController : MainController
    { 
        public ActionResult PerfilUsuario()
        {
            
            var empresa =db.Empresas.FirstOrDefault(c => c.IdEmpresa == IdEmpresa);
           
            var model = new Catalogos_viewModels.PerfilVM
            {
                Ruc=empresa.RUC,
                RazonSocial=empresa.RazonSocial,                
                correo = empresa.EmailNotificacion,
                PrimeraNotificacion = empresa.PrimeraNotificacion,
                SegundaNotificacion = empresa.SegundaNotificacion,
                TerceraNotificacion = empresa.TerceraNotificacion
            };
            return View(model);
        }
        public ActionResult EditarPerfilUsuario()
        {
            var empresa = db.Empresas.FirstOrDefault(c => c.IdEmpresa == IdEmpresa);

            var model = new Catalogos_viewModels.PerfilVM
            {
                Ruc = empresa.RUC,
                RazonSocial = empresa.RazonSocial,
                correo = empresa.EmailNotificacion,
                PrimeraNotificacion= empresa.PrimeraNotificacion,
                SegundaNotificacion= empresa.SegundaNotificacion,
                TerceraNotificacion= empresa.TerceraNotificacion
                
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult GuardarPerfilUsuario(Catalogos_viewModels.PerfilVM model)
        {
            var empresa = db.Empresas.FirstOrDefault(c => c.IdEmpresa == IdEmpresa);
          
            empresa.RUC = model.Ruc;
            empresa.EmailNotificacion = model.correo;
            empresa.RazonSocial = model.RazonSocial; 
            empresa.PrimeraNotificacion = model.PrimeraNotificacion;
            empresa.SegundaNotificacion = model.SegundaNotificacion;
            empresa.TerceraNotificacion = model.TerceraNotificacion;
            empresa.FechaModificacion = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("PerfilUsuario");
        }

 

    }
}