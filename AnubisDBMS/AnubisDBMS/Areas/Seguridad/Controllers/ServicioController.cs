using System.Web.Mvc;

namespace CruzRoja.Web.Areas.Seguridad.Controllers
{
    public class ServicioController : Controller
    {
        [AllowAnonymous]
        // GET: Seguridad/Servicio
        public ActionResult Index()
        {
            return View();
        }
      
    }
}