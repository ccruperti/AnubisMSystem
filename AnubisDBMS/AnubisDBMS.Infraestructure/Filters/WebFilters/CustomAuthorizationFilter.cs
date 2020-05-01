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
            var valido = false;
            using (var db = new AnubisDbContext()) {
                var usuario = HttpContext.Current.User.Identity.Name;
                var empresa = db.Users.FirstOrDefault(x => x.UserName == usuario).IdEmpresa;
                var servicio = db.Empresas.FirstOrDefault(c => c.IdEmpresa== empresa); 
                if (servicio != null )
                {
                    if(servicio.ServicioActivo)
                    return valido = true;
                    else
                    return valido = false; 
                }
                else
                {
                    var user = HttpContext.Current.User.IsInRole("Developers");
                    if(user)
                    {

                        return valido = true;
                    }
                    return valido=false;
                }
                
            }
                

        }
    }
}
