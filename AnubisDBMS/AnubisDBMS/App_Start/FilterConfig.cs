using AnubisDBMS.Infraestructure.Filters.WebFilters;
using System.Web;
using System.Web.Mvc;

namespace AnubisDBMS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomAuthorizationAttribute());
            filters.Add(new AuthorizeAttribute());

        }
    }
}
