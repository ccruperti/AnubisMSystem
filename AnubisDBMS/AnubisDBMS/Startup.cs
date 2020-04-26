using AnubisDBMS.Controllers;
using AnubisDBMS.Data.Entities;
using AnubisDBMS.Infraestructure.Helpers;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Owin;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using GlobalConfiguration = Hangfire.GlobalConfiguration;

[assembly: OwinStartup(typeof(AnubisDBMS.Startup))]

namespace AnubisDBMS
{
    public partial class Startup : MainController
    {
      
        Random rnd = new Random();
        Random random = new Random();
        Random rstring = new Random(); 
      
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");

            app.UseHangfireServer();
            app.UseHangfireDashboard("/Hangfire", new DashboardOptions()
            {
                Authorization = new[] { new HangfireAuthorizatonFilter() }
            });

        }

        //public async Task<bool> CheckAsync()
        //{

        //    if (CheckMedidas())
        //    {

        //        if (await CalcularNotificacionesAsync("aguilar996@hotmail.com"))
        //        {
        //            return true;
        //        }
        //        else
        //        { return false; }
        //    }
        //    return false;

        //}


        //#region RandomGenData
        //public void GenDataSensore()
        //{

        //    var countdatos = db.EquipoSensor.Count(c => c.Activo);

        //    string[] numserie = new string[4];
        //    numserie[0] = "HAU3R7J3IDF72K3";
        //    numserie[1] = "WIEFW74JF3U3JK3";
        //    numserie[2] = "AD23D28UDI22FED";
        //    numserie[3] = "W4F3O3F83HOI33";

        //    for (int i = 0; i < 500; i++)
        //    {


        //        var data = new DataSensores();
        //        data.Activo = true;
        //        data.FechaRegistro = RandomDay();
        //        data.UsuarioRegistro = "System";
        //        data.Medida = GetRandomNumber(0.1, 30.9);
        //        data.UnidadMedida = "C";
        //        data.SerieSensor = numserie[rstring.Next(numserie.Length)];
        //        //data.IdEquipoSensor = eqsen.IdEquipoSensor;
        //        db.DataSensores.Add(data);

        //        db.SaveChanges();

        //    } 
        //}
        //public double GetRandomNumber(double minimum, double maximum)
        //{
        //    return random.NextDouble() * (maximum - minimum) + minimum;
        //}

        //private Random gen = new Random();
        //public DateTime RandomDay()
        //{
        //    DateTime start = new DateTime(2019, 1, 1);
        //    int range = (DateTime.Today - start).Days;
        //    return start.AddDays(gen.Next(range));
        //}
        //#endregion
    }
    public class AnubisDBMSHangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public string[] AllowedRoles { get; set; }

        /// <summary>
        /// Indica que Roles pueden ingresar al Dashboard de Hangfire
        /// </summary>
        /// <param name="allowedRoles">Nombres de los roles autorizados</param>
        public AnubisDBMSHangfireAuthorizationFilter(params string[] allowedRoles)
        {
            AllowedRoles = allowedRoles;
        }

        public bool Authorize(DashboardContext context)
        {
            var appContext = HttpContext.Current;
            if (appContext.User.Identity.IsAuthenticated)
            {
                foreach (var role in AllowedRoles)
                {
                    if (appContext.User.IsInRole(role))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

}

