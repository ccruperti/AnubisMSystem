using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnubisDBMS.Controllers
{
    public class MailerController : MainController
    {
        // GET: Mailer
        public ActionResult Index()
        {
            return View();
        }
    }
    public class NotificacionCorreo
    {
        public string Usuario { get; set; }
        public string CodigoSensor { get; set; }
        public string Alerta { get; set; }

    }
}