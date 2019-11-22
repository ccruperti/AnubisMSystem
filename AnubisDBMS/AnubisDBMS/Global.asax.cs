using AnubisDBMS.Data;
using AnubisDBMS.Infraestructura.Data;
using AnubisDBMS.Infraestructure.Data.Security.Entities;
using AnubisDBMS.Infraestructure.Filters.WebFilters;
using AnubisDBMS.Infraestructure.Security;
using AnubisDBMS.Infraestructure.Security.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AnubisDBMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
         
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //AreaRegistration.RegisterAllAreas();

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var context = new AnubisDBMSDbContext();

            var applicationRoles = new List<AnubisDBMSUserRole>
            {
                new AnubisDBMSUserRole("Developers", "Developers", "Usuarios Developers", true),
            };

            Infraestructure.Security.StartupData.DefaultRoles(new Infraestructure.Security.Managers.AnubisDBMSRoleManager(new AnubisDBMSRoleStore(context)), applicationRoles);
            StartupData.DefaultUsers(new Infraestructure.Security.Managers.AnubisDBMSUserManager(new Infraestructure.Security.Stores.AnubisDBMSUserStore(context)));
            var dbcontext = new AnubisDbContext();

            GlobalFilters.Filters.Add(new CustomAuthorizationAttribute(dbcontext));
            context.Dispose();
        }
       
    }
}
