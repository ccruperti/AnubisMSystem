using AnubisDBMS.Data.ViewModels;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using System;
using static AnubisDBMS.Data.ViewModels.Catalogos_viewModels;

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
                TerceraNotificacion = empresa.TerceraNotificacion,
                IdEmpresa=empresa.IdEmpresa
            };
            return View(model);
        }
        public ActionResult ListaPerfilesEmpresas()
        {
            var model = new ListaPerfilesEmpresas
            {
                ListaPerfiles = db.Empresas.Where(x => x.Activo).Select(x => new PerfilVM
                {
                    Ruc = x.RUC,
                    RazonSocial = x.RazonSocial,
                    correo = x.EmailNotificacion,
                    PrimeraNotificacion = x.PrimeraNotificacion,
                    SegundaNotificacion = x.SegundaNotificacion,
                    TerceraNotificacion = x.TerceraNotificacion,
                    IdEmpresa = x.IdEmpresa,
                    NombreEmpresa = x.Nombre,
                    IsServicioActivo = x.ServicioActivo

                }).ToList()
            };
            return View(model);
        }
        public ActionResult EditarPerfilUsuario(long IdEmpresa)
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
        public ActionResult FiltrarInformacionPorEmpresa(long Id)
        {
            var user = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            user.IdEmpresa = Id;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
 

    }
}