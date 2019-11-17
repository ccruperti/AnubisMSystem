using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace AnubisDBMS.Infraestructure.Helpers
{
    public static class RazorHelpers
    {
        public static IHtmlString AssemblyVersion(this HtmlHelper helper)
        {
            var curAssembly = Assembly.GetCallingAssembly();
            var version = curAssembly.GetName().Version.ToString();
            return MvcHtmlString.Create(version);
        }
    }
}