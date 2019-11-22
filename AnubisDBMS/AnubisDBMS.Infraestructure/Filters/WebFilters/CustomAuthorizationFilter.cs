using AnubisDBMS.Data;
using AnubisDBMS.Infraestructura.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace AnubisDBMS.Infraestructure.Filters.WebFilters
{
    public class CustomAuthorizationAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (!IsAuthorized(filterContext))
            {
                filterContext.Result =
                    new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AccessDenied" , action =""}));
            }
        }

        private bool IsAuthorized(ActionExecutingContext filterContext)
        {
            var descriptor = filterContext.ActionDescriptor;
            var authorizeAttr = descriptor.GetCustomAttributes(typeof(AuthorizeAttribute), false).FirstOrDefault() as AuthorizeAttribute;
            var db = filterContext.HttpContext.GetRequiredService<AnubisDbContext>() ;
            if (authorizeAttr != null)
            {
                if (!authorizeAttr.Users.Contains(filterContext.HttpContext.User.ToString()))
                {
                    return false;
                }
                else
                {
                    if (db.Servicio.FirstOrDefault().EstadoServicio)
                    {
                        return true;
                    }
                    return false;
                }
                   
            }
           
                return true;
            


        }
    }
}
