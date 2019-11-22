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
    public class CustomAuthorizationAttribute : AuthorizeAttribute
    {

        private readonly AnubisDbContext dbContext;
        public CustomAuthorizationAttribute(AnubisDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public CustomAuthorizationAttribute()
        {
          
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //filterContext.Result = new HttpUnauthorizedResult(); // Try this but i'm not sure
            filterContext.Result =
                   new RedirectToRouteResult(new RouteValueDictionary(new { area="", controller = "GestionEquipos" , action ="AccesoBloqueado"}));
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (IsAuthorized(filterContext))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }
     
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{

        //    if (!IsAuthorized(filterContext))
        //    {
        //        filterContext.Result =
        //            new RedirectToRouteResult(new RouteValueDictionary(new { area="", controller = "GestionEquipos" , action ="AccesoBloqueado"}));
        //    }
        //}

        private bool IsAuthorized(AuthorizationContext filterContext)
        {

            var descriptor = filterContext.ActionDescriptor;
            var authorizeAttr = descriptor.GetCustomAttributes(typeof(AuthorizeAttribute), false).FirstOrDefault() as AuthorizeAttribute;
 
                    if (dbContext.Servicio.FirstOrDefault().EstadoServicio)
                    {
                        return true;
                    }
                    return false;

        }
    }
}
