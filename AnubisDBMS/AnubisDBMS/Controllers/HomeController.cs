using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnubisDBMS.Infraestructure.Helpers;

namespace AnubisDBMS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            QRGenerator QR = new QRGenerator();
            QR.GenerarQR("juandiegoaguilar.com");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        [AllowAnonymous]
        public PartialViewResult Mensaje()
        {
            MensajeViewModel viewModel = null;
            if (TempData["Mensaje"] != null)
            {
                viewModel = (MensajeViewModel)TempData["Mensaje"];
            }

            return PartialView("_Mensaje", viewModel);
        }

        public class MensajeViewModel
        {
            public MensajeViewModel(bool positivo, string titulo, string mensaje = null, string[] detalle = null)
            {
                Positivo = positivo;
                Titulo = titulo;
                Mensaje = mensaje;
                Detalle = detalle?.ToList();
            }

            public bool Positivo { get; set; }
            public string Titulo { get; set; }
            public string Mensaje { get; set; }
            public List<string> Detalle { get; set; }


        }
    }
}