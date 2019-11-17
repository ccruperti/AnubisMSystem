using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApptelinkInfrastructure.Data.WebmailManagement.Entities
{
    public class SmtpClientOptions
    {
        public SmtpClientOptions(string host, int port = 143)
        {
            TemplateDefaultDirectory = "/EmailTemplates";
            SmtpHost = host;
            Port = port;
            RequiresAuth = false;
        }

        public void SetCredentials(string username, string password = null)
        {
            RequiresAuth = true;
            Username = username;
            Password = password;
        }

        public bool RequiresAuth { get; private set; }
        public string TemplateDefaultDirectory { get; set; }
        public string SmtpHost { get; }
        public int Port { get; }
        public string Username { get; private set; }
        public string Password { get; private set; }
    }
}
