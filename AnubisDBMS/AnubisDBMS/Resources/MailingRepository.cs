using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AnubisDBMS.Resources
{
    public class AnubisEmailService
    {
         
        public class MailingRepository
        {
            
            public SmtpClient mailClient = new SmtpClient
            {

                Host = "smtp.gmail.com",
                Port = 465,
                EnableSsl = true,
                Credentials = new NetworkCredential("anubisolutions@gmail.com", "ANUBISmonitoreo2030"),



        };
 
            public async Task<MailingRepositoryResponse> SendEmailAsync(MailMessage emailMessage)
            {
                emailMessage.Sender = new MailAddress("anubisolutions@gmail.com", "Anubis Mail Service");
                try
                {

                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                    smtpClient.Credentials = new System.Net.NetworkCredential()
                    {
                        UserName = "anubisolutions@gmail.com",
                        Password = "ANUBISmonitoreo2030"

                    };

                    smtpClient.EnableSsl = true;
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                            System.Security.Cryptography.X509Certificates.X509Chain chain,
                            System.Net.Security.SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };
                    await smtpClient.SendMailAsync(emailMessage);
                    return new MailingRepositoryResponse { Succesful = true, Message = "Correo enviado correctamente" };
                }
                catch (Exception e)
                {
                    return new MailingRepositoryResponse { Succesful = false, Message = e.Message };
                }
            }

            public MailingRepositoryResponse SendEmail(MailMessage emailMessage)
            {
                emailMessage.Sender = new MailAddress("anubisolutions@gmail.com", "Anubis Mail Service");
                try
                {
                    mailClient.Send(emailMessage);
                    return new MailingRepositoryResponse { Succesful = true, Message = "Correo enviado correctamente" };
                }
                catch (Exception e)
                {
                    return new MailingRepositoryResponse { Succesful = false, Message = e.Message };
                }
            }

            public string RenderViewToString(Controller controller, string viewName, string viewRoute, object model)
            {
                controller.ControllerContext = new ControllerContext();
                controller.ViewData.Model = model;
                try
                {
                    using (var sw = new StringWriter())
                    {
                        HttpContext.Current = new HttpContext(
                        new HttpRequest("", "http://localhost:54802", ""),
                        new HttpResponse(new StringWriter())
                        );
                        var context = new HttpContextWrapper(HttpContext.Current);
                        var routeData = new RouteData();
                        var controllerContext = new ControllerContext(new RequestContext(context, routeData), controller);
                        var razor = new RazorView(controllerContext, viewRoute, null, false, null);
                        var viewContext = new ViewContext(controller.ControllerContext, razor, controller.ViewData, controller.TempData, sw);
                        razor.Render(new ViewContext(controllerContext, razor, new ViewDataDictionary(model), new TempDataDictionary(), sw), sw);
                        return sw.ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }

    public class MailingRepositoryResponse
    {
        public bool Succesful { get; set; }
        public string Message { get; set; }
    }
}