using AnubisDBMS.Controllers;
using AnubisDBMS.Models;
using Hangfire;
using Microsoft.Owin;
using Owin;
using System.Linq;

[assembly: OwinStartup(typeof(AnubisDBMS.Startup))]

namespace AnubisDBMS
{
    public partial class Startup: MainController
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration
            .UseSqlServerStorage("DefaultConnection");
            app.UseHangfireDashboard("/HangfireTrip", new DashboardOptions()
            {
                Authorization = new[] { new HangfireAuthorizatonFilter() }
            });
            //BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget!"));
            RecurringJob.AddOrUpdate(() => Check(),Cron.Hourly);
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


    }
}
