using AnubisDBMS.Controllers;
using AnubisDBMS.Data.Entities;
using AnubisDBMS.Infraestructure.Helpers;
using Hangfire;
using Microsoft.Owin;
using Owin;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.Hosting;
using static AnubisDBMS.Infraestructure.WebmailManagement.Services.AnubisEmailService;
using GlobalConfiguration = Hangfire.GlobalConfiguration;

[assembly: OwinStartup(typeof(AnubisDBMS.Startup))]

namespace AnubisDBMS
{
    public partial class Startup: MainController
    {
        public MailingRepository emailSvc = new MailingRepository();

        Random rnd = new Random();
        Random random = new Random();
        Random rstring = new Random(); 
        string path = HostingEnvironment.MapPath("~\\"); 
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration
            .UseSqlServerStorage("DefaultConnection");
            app.UseHangfireDashboard("/HangfireTrip", new DashboardOptions()
            {
                Authorization = new[] { new HangfireAuthorizatonFilter() }
            });
            RecurringJob.AddOrUpdate(() => CheckAsync(), Cron.Hourly);
            RecurringJob.AddOrUpdate(() => GenDataSensore(), Cron.MinuteInterval(1));
            app.UseHangfireServer();

        }

        public async System.Threading.Tasks.Task CheckAsync()
        {
            string logoImage = Path.Combine(path, "Content\\Images\\AnubisLogoEmail.jpeg");  
            string rounderup = Path.Combine(path, "Content\\Images\\rounder-up.png");
            string divider = Path.Combine(path, "Content\\Images\\divider.png");
            string rounderdwn = Path.Combine(path, "Content\\Images\\rounder-dwn.png");
            Attachment logoImageAtt = new Attachment(logoImage);
            Attachment rounderupAtt = new Attachment(rounderup);
            Attachment dividerAtt = new Attachment(divider);
            Attachment rounderdwnAtt = new Attachment(rounderdwn);
            logoImageAtt.ContentDisposition.Inline = true;
            logoImageAtt.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            string logoImgId = "headerimg1";
            logoImageAtt.ContentId = logoImgId;

            rounderupAtt.ContentDisposition.Inline = true;
            rounderupAtt.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            string rounderupId = "headerimg2";
            rounderupAtt.ContentId = rounderupId;

            dividerAtt.ContentDisposition.Inline = true;
            dividerAtt.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            string dividerId = "headerimg3";
            dividerAtt.ContentId = dividerId;

            rounderdwnAtt.ContentDisposition.Inline = true;
            rounderdwnAtt.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            string rounderdwnId = "headerimg4";
            rounderdwnAtt.ContentId = rounderdwnId;

            var Sensores = db.Sensores.Where(x => x.Activo).ToList();
            foreach(var s in Sensores)
            {
                CheckMinMax(s.IdSensor, s.SerieSensor);
                var errores = db.DataSensores.Where(x => x.Error && x.Notificado == false).ToList();
                    foreach(var e in errores)
                    {  
                    var email = new MailMessage("anubisolutions@gmail.com", "aguilar996@hotmail.com");
                    email.Attachments.Add(logoImageAtt);
                    email.Attachments.Add(rounderupAtt);
                    email.Attachments.Add(dividerAtt);
                    email.Attachments.Add(rounderdwnAtt);

                    email.Subject = "Nueva Alerta - Sensor" + " " + s.SerieSensor;
                    email.IsBodyHtml = true;
                    var eqs = db.EquipoSensor.FirstOrDefault(x => x.IdSensor == s.IdSensor);
                    var not = new NotificacionCorreo
                    {
                        Usuario = "test",
                        SerieSensor = s.SerieSensor,
                        Medicion = e.UnidadMedida,
                        MedidaSensor = e.Medida.ToString(),
                        Logo = logoImgId,
                        divider = dividerId,
                        rounderdwn = rounderdwnId,
                        rounderup = rounderupId
                    };
                    if(e.DebajoNormal==true)
                    {
                        not.EncimaDebajo = "debajo";
                    }
                    if (e.EncimaNormal == true)
                    {
                        not.EncimaDebajo = "encima";
                    }
                    var bodyAprobadoProveedor = emailSvc.RenderViewToString(new MailerController(), "PlantillaAnubis", "~/Views/Mailer/PlantillaAnubis.cshtml",not);
                    email.Body = bodyAprobadoProveedor;
                    await emailSvc.SendEmailAsync(email);
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

