using AnubisDBMS.Data;
using AnubisDBMS.Infraestructure.Helpers;
using AnubisDBMS.Infraestructure.Security.Managers;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnubisDBMS.Data.Entities;
using AnubisDBMS.Infraestructure.Filters.WebFilters;
using System.Data.Entity;

namespace AnubisDBMS.Controllers
{

    public class MainController : Controller
    { 
        #region DB&Users
        public AnubisDbContext db = new AnubisDbContext();

        public AnubisDBMSUserManager _userManager;
        public AnubisDBMSRoleManager _roleManager;



        public MainController()
        {

        }

        public MainController(AnubisDBMSUserManager userManager, AnubisDBMSRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public AnubisDBMSUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<AnubisDBMSUserManager>();
            private set => _userManager = value;
        }

        public AnubisDBMSRoleManager RoleManager
        {
            get => _roleManager ?? HttpContext.GetOwinContext().GetUserManager<AnubisDBMSRoleManager>();
            private set => _roleManager = value;
        }
        #endregion

        #region QR
        public QRGenerator QR = new QRGenerator();
        #endregion

        #region SelectLists
        public SelectList SelectListTipoSensor(long selected = 0)
        {

            List<TipoSensor> TipoSensor = new List<TipoSensor>(); 
            foreach (var x in db.TipoSensor.Where(x=>x.Activo).ToList())
            {
                TipoSensor.Add(x);
            }
            return new SelectList(TipoSensor, "IdTipoSensor", "NombreTipoSensor");

        } 
        public SelectList SelectListEquipo(long selected = 0)
            {

                List<Equipo> Equipo = new List<Equipo>();
                foreach (var x in db.Equipos.Where(x => x.Activo).ToList())
                {
                Equipo.Add(x); 
                }
            Equipo.Add(new Equipo { IdEquipo = 0, Alias = "Seleccione Equipo" });
            return new SelectList(Equipo, "IdEquipo", "Alias");

            }
        public SelectList SelectListEquipoSensor(long? id = null)
        {

            List<SelectListItem> data = db.EquipoSensor.Where(x => x.Activo).Select(x => new SelectListItem{ 
             Text = x.Equipos.Alias + " - "+ x.Equipos.SerieEquipo,
             Value = x.IdEquipoSensor.ToString()
            }).ToList(); 
            return new SelectList(data, "Value", "Text", id);

        }
        public SelectList SelectListFrecuencias(long? id = null)
        {

            List<Frecuencia> data = db.Frecuencia.Where(c => c.Activo).ToList();
            data.Add(new Frecuencia { IdFrecuencia = 0 , NombreFrecuencia = "Seleccione frecuencia" });
            return new SelectList(data.OrderBy(c => c.IdFrecuencia), "IdFrecuencia", "NombreFrecuencia", id);

        }
        public SelectList SelectListSensores(long selected = 0)
        {
            List<long> sensoresIDs = db.EquipoSensor.Where(x => x.Activo).Select(i =>i.IdSensor??0).ToList();
            List<SelectListItem> sensores = db.Sensores.AsNoTracking()
                   .OrderBy(n => n.SerieSensor).Where(x=>x.Activo)
                       .Select(n =>
                       new SelectListItem
                       {
                           Value = n.IdSensor.ToString(),
                           Text = n.TipoSensor.NombreTipoSensor+ " - " + n.SerieSensor
                       }).ToList();

           
           
            var filtered = sensores
                              .Where(x => !sensoresIDs.Contains(Convert.ToInt64(x.Value))).ToList();
            var sensorNull = new SelectListItem()
            {
                Value = null,
                Text = "Seleccione Sensor"
            };
            filtered.Insert(0, sensorNull);
            return new SelectList(filtered, "Value", "Text"); 
        } 
        public SelectList SelectListPuertos(long idEquipo, int selected = 0)
        {
            List<int> puertosOcupados = new List<int>();
            List<int> puertosDisponibles = new List<int>();
            var Equipos = db.EquipoSensor.Where(x => x.IdEquipo == idEquipo && x.Activo && x.NumeroPuerto!=0).ToList();
            for(var x=1;x<=8;x++)
            {
                puertosDisponibles.Add(x);
            }
            foreach(var eq in Equipos)
            {
            puertosOcupados.Add(eq.NumeroPuerto);
            }
            List<int> Puertos = puertosDisponibles.Except(puertosOcupados).ToList(); 
            return new SelectList(Puertos);
        } 
        public SelectList SelectListTecnico(long? id = null)
        {

            List<Tecnicos> Tecnicos = new List<Tecnicos>();
            foreach (var x in db.Tecnicos.Where(x => x.Activo).ToList())
            {
                Tecnicos.Add(x);
            }
            Tecnicos.Add(new Tecnicos { IdTecnico = 0, NombreTecnico = "Seleccione Técnico" });
            return new SelectList(Tecnicos.OrderBy(c => c.IdTecnico), "IdTecnico", "NombreTecnico", id);

        }
        #endregion
         
        #region ACT_DESACT Servicios
        public ActionResult Activar_Servicio ()
        {
            if (!db.Servicio.Any(x => x.Activo))
            {

                db.Servicio.Add(new Servicio
                {
                    Activo = true,
                    FechaModificacion = DateTime.Now,
                    UsuarioModificacion = User.Identity.Name,
                    EstadoServicio = true
                });
                db.SaveChanges();
            }
            else
            {
                var Actual = db.Servicio.FirstOrDefault(x => x.Activo);
                Actual.FechaModificacion = DateTime.Now;
                Actual.UsuarioModificacion = User.Identity.Name;
                Actual.Activo = false;
                db.Servicio.Add(new Servicio
                {
                    Activo = true,
                    FechaRegistro = DateTime.Now,
                    UsuarioRegistro = User.Identity.Name,
                    EstadoServicio = true
                });
                db.SaveChanges();
            }
            return Redirect("Index");
        }
        public ActionResult Desactivar_Servicio ()
        {
            if (!db.Servicio.Any(x => x.Activo))
            {

                db.Servicio.Add(new Servicio
                {
                    Activo = true,
                    FechaModificacion = DateTime.Now,
                    UsuarioModificacion = User.Identity.Name,
                    EstadoServicio = false
                });
                db.SaveChanges();
            }
            else
            {
                var Actual = db.Servicio.FirstOrDefault(x => x.Activo);
                Actual.FechaModificacion = DateTime.Now;
                Actual.UsuarioModificacion = User.Identity.Name;
                Actual.EstadoServicio = false;
                Actual.Activo = false;
                db.Servicio.Add(new Servicio
                {
                    Activo = true,
                    FechaRegistro = DateTime.Now,
                    UsuarioRegistro = User.Identity.Name,
                    EstadoServicio = false
                });
                db.SaveChanges();
            }
           return Redirect("Index");
        }
        #endregion

        #region SensorMinMaxCheck
        public bool CheckMinMax (long IdSensor,string ModeloSensor)
        {
            var sensor = db.Sensores.FirstOrDefault(x => x.IdSensor == IdSensor);
            var data = db.DataSensores.Where(x => x.Activo &&
            x.SerieSensor == ModeloSensor &&
            x.Chequeado==false
            ).ToList();
            foreach (var lec in data)
            {
                var TodoBien = 1;
                if (sensor.Min == null)
                { //SI MIN ES NULO REVISO MAX
                    if (CheckRange(null, sensor.Max, lec.Medida))
                        TodoBien = 1;
                    else TodoBien=0;
                }
                if (sensor.Max == null)
                {//SI MAX ES NULO REVISO MIN
                    if (CheckRange(sensor.Min, null, lec.Medida))
                        TodoBien = 1;
                    else TodoBien = 0;
                }
                if (sensor.Min != null && sensor.Max != null)
                {//SI NINGUNO ES NULL REVISO AMBOS
                    if (CheckRange(null, sensor.Max, lec.Medida) &&
                    CheckRange(sensor.Min, null, lec.Medida))
                    {
                        TodoBien = 1;
                    }
                    else TodoBien = 0;  
                }
                if(TodoBien==0)
                {
                    lec.Chequeado = true;
                    lec.Error = true;
                    db.SaveChanges();
                }
                lec.Chequeado = true;
                db.SaveChanges();
            }
            return true;
        }

        public bool CheckRange( double? Min, double? Max, double? Lectura)
        {
            if(Min==null)
            {
                //SI MIN ES NULO REVISO MAX
                if (Lectura <= Max)
                    return true;
                else return false;
            }
            if (Max==null)
            {
                //SI MAX ES NULO REVISO MIN
                if (Lectura >= Min)
                    return true;
                else return false;
            }
            if(Min==null && Max==null)
            {
                return true;
            }
            return false;
       
        }
        #endregion
    }
}