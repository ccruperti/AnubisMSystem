using AnubisDBMS.Data;
using AnubisDBMS.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnubisDBMS.Controllers
{
    public class MantenimientoController : MainController
    {
          

        public ActionResult AgregarMantenimiento()
        {
            return View();
        }

        public ActionResult Mantenimientos()
        {
            return View();
        }
    }
}