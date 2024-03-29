﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AnubisDBMS.WebmailManagement.Managers;
using System.Net.Mail;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace AnubisDBMS.Infraestructure.WebmailManagement.Services
{
    public class AnubisEmailService
    {
//        public class MailingRepository
//        {
//            private SmtpClient mailClient = new SmtpClient
//            {
//                Credentials = new NetworkCredential("anubisolutions@gmail.com", "ANUBISmonitoreo2020"),
//EnableSsl = true,
//Host = "smtp.gmail.com",
//DeliveryMethod = SmtpDeliveryMethod.Network,
//Port = 465
//            };

//            public async Task<MailingRepositoryResponse> SendEmailAsync(MailMessage emailMessage)
//            {
//                emailMessage.Sender = new MailAddress("notificaciones@apptelink.net", "Apptelink Mail Service");
//                try
//                {
//                    await mailClient.SendMailAsync(emailMessage);
//                    return new MailingRepositoryResponse { Succesful = true, Message = "Correo enviado correctamente" };
//                }
//                catch (Exception e)
//                {
//                    return new MailingRepositoryResponse { Succesful = false, Message = e.Message };
//                }
//            }

//            public MailingRepositoryResponse SendEmail(MailMessage emailMessage)
//            {
//                emailMessage.Sender = new MailAddress("notificaciones@apptelink.net", "Apptelink Mail Service");
//                try
//                {
//                    mailClient.Send(emailMessage);
//                    return new MailingRepositoryResponse { Succesful = true, Message = "Correo enviado correctamente" };
//                }
//                catch (Exception e)
//                {
//                    return new MailingRepositoryResponse { Succesful = false, Message = e.Message };
//                }
//            }

//            public string RenderViewToString(Controller controller, string viewName, string viewRoute, object model)
//            {
//                controller.ControllerContext = new ControllerContext();
//                controller.ViewData.Model = model;
//                try
//                {
//                    using (var sw = new StringWriter())
//                    {


//                        var context = new HttpContextWrapper(HttpContext.Current);
//                        var routeData = new RouteData();
//                        var controllerContext = new ControllerContext(new RequestContext(context, routeData), controller);
//                        var razor = new RazorView(controllerContext, viewRoute, null, false, null);

//                        var viewContext = new ViewContext(controller.ControllerContext, razor, controller.ViewData, controller.TempData, sw);
//                        razor.Render(new ViewContext(controllerContext, razor, new ViewDataDictionary(model), new TempDataDictionary(), sw), sw);
//                        return sw.ToString();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    throw ex;
//                }
//            }
//        }

    }
    public class MailingRepositoryResponse
    {
        public bool Succesful { get; set; }
        public string Message { get; set; }
    }
}
