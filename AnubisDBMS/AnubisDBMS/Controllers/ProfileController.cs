using AnubisDBMS.Data.ViewModels;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnubisDBMS.Controllers
{
    public class ProfileController : MainController
    { 
        public ActionResult PerfilUsuario()
        {

            var user = UserManager.FindByName(User.Identity.Name);

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
        public async Task<ActionResult> GuardarPerfilUsuario(Catalogos_viewModels.PerfilVM model)
        {
            var user = UserManager.FindByNameAsync(User.Identity.Name);
            user.Result.Celular = model.telefono;
            user.Result.Email = model.correo;
            user.Result.PrimeraNotificacion = model.PrimeraNotificacion;
            user.Result.SegundaNotificacion = model.SegundaNotificacion;
            user.Result.TerceraNotificacion = model.TerceraNotificacion;
            await UserManager.UpdateAsync(user.Result);
            return RedirectToAction("PerfilUsuario");
        }
    }
}