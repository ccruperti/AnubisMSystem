using AnubisDBMS.Data.ViewModels;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;

namespace AnubisDBMS.Controllers
{
    public class ProfileController : MainController
    { 
        public ActionResult PerfilUsuario()
        {

            var user =db.Users.FirstOrDefault(c => c.UserName == User.Identity.Name && c.IdEmpresa == IdEmpresa);

            var model = new Catalogos_viewModels.PerfilVM
            {
                telefono = user.Celular,
                correo = user.Email,
                PrimeraNotificacion = user.PrimeraNotificacion,
                SegundaNotificacion = user.SegundaNotificacion,
                TerceraNotificacion = user.TerceraNotificacion
            };
            return View(model);
        }
        public ActionResult EditarPerfilUsuario()
        {
            var user = UserManager.FindByName(User.Identity.Name);

            var model = new Catalogos_viewModels.PerfilVM
            {
                telefono = user.Celular,
                correo = user.Email,
                PrimeraNotificacion=user.PrimeraNotificacion,
                SegundaNotificacion=user.SegundaNotificacion,
                TerceraNotificacion=user.TerceraNotificacion
                
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult GuardarPerfilUsuario(Catalogos_viewModels.PerfilVM model)
        {
            var user = db.Users.FirstOrDefault(c => c.UserName == User.Identity.Name && c.IdEmpresa == IdEmpresa);
            user.Celular = model.telefono;
            user.Email = model.correo;
            user.PrimeraNotificacion = model.PrimeraNotificacion;
            user.SegundaNotificacion = model.SegundaNotificacion;
            user.TerceraNotificacion = model.TerceraNotificacion;
            db.SaveChanges();
            return RedirectToAction("PerfilUsuario");
        }

 

    }
}