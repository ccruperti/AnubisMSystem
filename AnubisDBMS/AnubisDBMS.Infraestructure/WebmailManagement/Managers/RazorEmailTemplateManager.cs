using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using System.Web;
using RazorEngine.Templating;

namespace AnubisDBMS.WebmailManagement.Managers
{
    //public class RazorEmailTemplateManager : ITemplateManager
    //{
    //    private static string _templatesDirectory = HttpContext.Current.Server.MapPath("~/EmailTemplates");

    //    public RazorEmailTemplateManager()
    //    {
            
    //    }

    //    public RazorEmailTemplateManager(string templatesDirectory)
    //    {
    //        _templatesDirectory = templatesDirectory;
    //    }

    //    public string GetTemplateHTMLBody(string templateKey)
    //    {
    //        var service = new RazorEngineService();
    //        service.
    //    }

    //    public static void RegisterDefaultTemplates()
    //    {
    //        var templateFiles = Directory.GetFiles(_templatesDirectory, "*.cshtml", SearchOption.TopDirectoryOnly);
    //        var manager = new RazorEmailTemplateManager();
    //        foreach (var templateFile in templateFiles)
    //        {
                
    //        }
    //    }

    //    public void AddDynamic(ITemplateKey key, ITemplateSource source)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public ITemplateKey GetKey(string name, ResolveType resolveType, ITemplateKey context)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public ITemplateSource Resolve(ITemplateKey key)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
