using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace AnubisDBMS.Infraestructure.Security.Services
{
    public class AnubisDBMSSecurityEmailService : IIdentityMessageService
    {
        private IRazorEngineService _service;

        private TemplateServiceConfiguration _serviceConfiguration = new TemplateServiceConfiguration();

        public AnubisDBMSSecurityEmailService(string templateDirectory)
        {
            _serviceConfiguration.BaseTemplateType = typeof(HtmlTemplateBase<>);
            _service = RazorEngineService.Create(_serviceConfiguration);
            var creacionUsuarioTemplateFile = Path.Combine(templateDirectory, "CreacionUsuario.html");
            var creacionUsuarioTemplate = new LoadedTemplateSource("creacionUsuario", creacionUsuarioTemplateFile);
            //_service.AddTemplate("key", );
        }

        public Task SendAsync(IdentityMessage message)
        {
            return Task.CompletedTask;
        }
    }
}
