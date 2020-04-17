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
        public string SerieSensor { get; set; }
        public string Medicion { get; set; }
        public string EncimaDebajo { get; set; }
        public string MedidaSensor { get; set; } 
        public string Logo { get; set; }
        public string rounderup { get; set; }
        public string divider { get; set; }
        public string rounderdwn { get; set; } 

    }
}