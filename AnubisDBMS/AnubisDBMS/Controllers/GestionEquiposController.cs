using AnubisDBMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static AnubisDBMS.Controllers.HomeController;
using AnubisDBMS.Infraestructure.Helpers;
using System.Drawing;
using System.ComponentModel;

namespace AnubisDBMS.Controllers
{
    public class GestionEquiposController : Controller
    {
        QRGenerator QR = new QRGenerator();
        public List<EquiposViewModels> ListaEquipos()
        {
            var lista = new List<EquiposViewModels>();
            var eq1 = new EquiposViewModels();
            var eq2 = new EquiposViewModels();
            var eq3 = new EquiposViewModels();

            eq1.AliasEquipo = "Equipo 1";
            eq1.NombreTecnico = "Tecnico 1";
            eq1.Sensores.Add(new SensoresEquipos {
                CodigoSensor = "Cod1",
                NombreSensor = "Sensor 1"
            });
            eq1.Sensores.Add(new SensoresEquipos
            {
                CodigoSensor = "Cod2",
                NombreSensor = "Sensor 2"
            });
            eq1.Sensores.Add(new SensoresEquipos
            {
                CodigoSensor = "Cod3",
                NombreSensor = "Sensor 3"
            });
            eq2.AliasEquipo = "Equipo 2";
            eq2.NombreTecnico = "Tecnico 2";
            eq2.Sensores.Add(new SensoresEquipos
            {
                CodigoSensor = "Cod1",
                NombreSensor = "Sensor 1"
            });
            eq2.Sensores.Add(new SensoresEquipos
            {
                CodigoSensor = "Cod2",
                NombreSensor = "Sensor 2"
            });
            eq2.Sensores.Add(new SensoresEquipos
            {
                CodigoSensor = "Cod3",
                NombreSensor = "Sensor 3"
            });

            eq3.AliasEquipo = "Equipo 3";
            eq3.NombreTecnico = "Tecnico 3";
            eq3.Sensores.Add(new SensoresEquipos
            {
                CodigoSensor = "Cod1",
                NombreSensor = "Sensor 1"
            });
            eq3.Sensores.Add(new SensoresEquipos
            {
                CodigoSensor = "Cod2",
                NombreSensor = "Sensor 2"
            });
            eq3.Sensores.Add(new SensoresEquipos
            {
                CodigoSensor = "Cod3",
                NombreSensor = "Sensor 3"
            });
            lista.Add(eq1);
            lista.Add(eq2);
            lista.Add(eq3);
            return lista;
        }
        // GET: GestionEquipos
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MonitoreoEquipos(DateTime? Desde, DateTime? Hasta)
        {
            var model =new  ListaEquipos();
            model.Equipos = ListaEquipos();
            return View(model);
        }
        public ActionResult RegistrarEquipo()
        {
            var model = new EquiposViewModels();
            return View(model);
        }

       
        public ActionResult SensoresEquipo()
        {
         
           string CodigoQR =  QR.GenerarQR("juandiegoaguilar.com"); 
            var model = new SensoresEquipos
            {
                QR = CodigoQR,
                CodigoSensor="225SUERWNWRU234",
                NombreSensor="TERMODINAMIZALIZADOR"
            };
            return View(model);
        }
        public ActionResult AgregarMantenimiento()
        {
            return View();
        }
        public ActionResult LecturaMedidoresEquipo()
        {
            return View();
        }
        public ActionResult Mantenimientos()
        {
            return View();
        }
        public ActionResult PerfilUsuario()
        {
            return View();
        }
     
    }
}