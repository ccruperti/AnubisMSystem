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
        Random rstring = new Random();
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

        
            
                var countdatos = db.EquipoSensor.Count(c => c.Activo);
            
                string[] numserie = new string[4];
                numserie[0] = "HAU3R7J3IDF72K3";
                numserie[1] = "WIEFW74JF3U3JK3";
                numserie[2] = "AD23D28UDI22FED";
                numserie[3] = "W4F3O3F83HOI33";

                for (int i = 0; i < 500; i++)
                    {

                  
                        var data = new DataSensores();
                        data.Activo = true;
                        data.FechaRegistro = DateTime.Now;
                        data.UsuarioRegistro = "System";
                        data.Medida = GetRandomNumber(0.1, 4.9);
                        data.UnidadMedida = "C";
                    data.SerieSensor = numserie[rstring.Next(numserie.Length)];
                        //data.IdEquipoSensor = eqsen.IdEquipoSensor;
                        db.DataSensores.Add(data);
                        db.SaveChanges();

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
