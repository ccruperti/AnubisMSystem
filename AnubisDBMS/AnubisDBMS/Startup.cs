using AnubisDBMS.Controllers;
using AnubisDBMS.Data.Entities;
using AnubisDBMS.Infraestructure.Helpers;
using Hangfire;
using Microsoft.Owin;
using Owin;
using System;
using System.Linq;
using GlobalConfiguration = Hangfire.GlobalConfiguration;

[assembly: OwinStartup(typeof(AnubisDBMS.Startup))]

namespace AnubisDBMS
{
    public partial class Startup: MainController
    {
        Random rnd = new Random();
        Random random = new Random();
        Random rstring = new Random();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration
            .UseSqlServerStorage("DefaultConnection");
            app.UseHangfireDashboard("/HangfireTrip", new DashboardOptions()
            {
                Authorization = new[] { new HangfireAuthorizatonFilter() }
            });
           // RecurringJob.AddOrUpdate(() => Check(), Cron.Hourly);
           // RecurringJob.AddOrUpdate(() => GenDataSensore(), Cron.MinuteInterval(5));
            app.UseHangfireServer();

        }

        public void Check()
        {
            var Sensores = db.Sensores.Where(x => x.Activo).ToList();
            foreach(var s in Sensores)
            {
                CheckMinMax(s.IdSensor, s.SerieSensor);
                var errores = db.DataSensores.Where(x => x.Error && x.Notificado == false).ToList();
                    foreach(var e in errores)
                    {
                        //MANDAR EMAIL
                        e.Notificado = true;
                        db.SaveChanges();
                    }
            }
        }

        public void GenDataSensore()
        {
           
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
                data.FechaRegistro = RandomDay();
                data.UsuarioRegistro = "System";
                data.Medida = GetRandomNumber(0.1, 30.9);
                data.UnidadMedida = "C";
                data.SerieSensor = numserie[rstring.Next(numserie.Length)];
                //data.IdEquipoSensor = eqsen.IdEquipoSensor;
                db.DataSensores.Add(data);

                db.SaveChanges();

            } 
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

