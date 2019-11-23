using System.Web.Mvc;

namespace CruzRoja.Web.Areas.Seguridad.Controllers
{
    public class ServicioController : Controller
    {
        // GET: Seguridad/Servicio
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult AccessoBloqueado()
        {
            return View();
        }
      
    }
}