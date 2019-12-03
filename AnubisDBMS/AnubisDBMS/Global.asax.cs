using AnubisDBMS.Data;
using AnubisDBMS.Data.Entities;
using AnubisDBMS.Infraestructura.Data;
using AnubisDBMS.Infraestructure.Data.Security.Entities;
using AnubisDBMS.Infraestructure.Filters.WebFilters;
using AnubisDBMS.Infraestructure.Security;
using AnubisDBMS.Infraestructure.Security.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AnubisDBMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        Random rnd = new Random();
        Random random = new Random();
        protected void Application_Start()
        {
         
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //AreaRegistration.RegisterAllAreas();

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var context = new AnubisDBMSDbContext();
            var db = new AnubisDbContext();
            var applicationRoles = new List<AnubisDBMSUserRole>
            {
                new AnubisDBMSUserRole("Developers", "Developers", "Usuarios Developers", true),
            };

            Infraestructure.Security.StartupData.DefaultRoles(new Infraestructure.Security.Managers.AnubisDBMSRoleManager(new AnubisDBMSRoleStore(context)), applicationRoles);
            StartupData.DefaultUsers(new Infraestructure.Security.Managers.AnubisDBMSUserManager(new Infraestructure.Security.Stores.AnubisDBMSUserStore(context)));

        
            if (db.EquipoSensor.Any(x => x.Activo))
            {
                var countdatos = db.EquipoSensor.Count(c => c.Activo);

                
                for (int i = 0; i < 500; i++)
                    {

                    var ideq = rnd.Next(1, 2);
                    var eqsen = db.EquipoSensor.FirstOrDefault(x => x.Activo && x.IdEquipoSensor == ideq);
                        var data = new DataSensores();
                        data.Activo = true;
                        data.FechaRegistro = DateTime.Now;
                        data.UsuarioRegistro = "System";
                        data.Medida = GetRandomNumber(0.1, 4.9);
                        data.UnidadMedida = eqsen.Sensores.TipoSensor.UnidadSensor;
                        //data.IdEquipoSensor = eqsen.IdEquipoSensor;
                        db.DataSensores.Add(data);
                    db.SaveChanges();

                }


            }

            context.Dispose();
        }
        public double GetRandomNumber(double minimum, double maximum)
        {
            
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
        private Random gen = new Random();
        public DateTime RandomDay()
        {
            DateTime start = new DateTime(2019, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }

    }
}
