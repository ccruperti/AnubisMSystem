using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnubisDBMS.Infraestructure.Filters.WebFilters
{
    public class XsrfInvalidTokenFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!(filterContext.Exception is HttpAntiForgeryException)) return;
            filterContext.Result = new RedirectResult("/Seguridad/Cuenta/SesionExpirada");
            filterContext.ExceptionHandled = true;
        }
    }
}
