using AnubisDBMS.Data;
using AnubisDBMS.Infraestructura.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AnubisDBMS.Infraestructure.Filters.WebFilters
{
    public class CustomAuthorizationAttribute : AuthorizeAttribute
    {

        
      
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //filterContext.Result = new HttpUnauthorizedResult(); // Try this but i'm not sure
          
            filterContext.Result =
                   new RedirectToRouteResult(new RouteValueDictionary(new { area="", controller = "GestionEquipos" , action = "AccesoBloqueado" }));
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

            using (var db = new AnubisDbContext()) {
                if (db.Servicio.FirstOrDefault(c => c.Activo).EstadoServicio)
                {
                    return true;
                }
                else
                {
                    var user = HttpContext.Current.User.IsInRole("Administrador Sistema" ?? "Administrador");
               if(user)
                    {

                        return true;
                    }
                    return false;
                }
                
            }
                

        }
    }
}
