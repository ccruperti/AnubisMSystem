using AnubisDBMS.Data.ViewModels;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnubisDBMS.Controllers
{
    public class ProfileController : MainController
    {

       

        // GET: Profile
        public ActionResult PerfilUsuario()
        {

            var user = UserManager.FindByName(User.Identity.Name);

            var model = new Catalogos_viewModels.PerfilVM
            {
                telefono = user.Celular,
                correo = user.Email
            };
            return View(model);
        }
        public ActionResult EditarPerfilUsuario()
        {
            var user = UserManager.FindByName(User.Identity.Name);

            var model = new Catalogos_viewModels.PerfilVM
            {
                telefono = user.Celular,
                correo = user.Email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GuardarPerfilUsuario(Catalogos_viewModels.PerfilVM model)
        {
            var user = UserManager.FindByNameAsync(User.Identity.Name);
            user.Result.Celular = model.telefono;
            user.Result.Email = model.correo;
            await UserManager.UpdateAsync(user.Result);
            return RedirectToAction("PerfilUsuario");
        }
    }
}