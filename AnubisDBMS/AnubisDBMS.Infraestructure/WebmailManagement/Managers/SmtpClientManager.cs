using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AnubisDBMS.Infraestructure.WebmailManagement.Managers
{
    public class SmtpClientManager : IDisposable
    {
        private SmtpClient _smtpClient;

        public SmtpClientManager()
        {
            _smtpClient = new SmtpClient();
            
        }

        /// <summary>
        /// Gestiona conexion al servidor SMTP de manera sincrona.
        /// </summary>
        internal void SmtpConnect()
        {
            _smtpClient.Connect("", 587, SecureSocketOptions.None);
            _smtpClient.Authenticate("", "");
        }

        /// <summary>
        /// Gestiona conexion al servidor SMTP de manera asincrona.
        /// </summary>
        internal void SmtpConnectAsync()
        {
            _smtpClient.Connect("", 587, SecureSocketOptions.None);
            _smtpClient.Authenticate("", "");
        }

        public void SendMultipleEmails(List<MimeMessage> emailMessages)
        {
            SmtpConnect();

            foreach (var emailMessage  in emailMessages)
            {
                _smtpClient.Send(emailMessage);
            }
        }

        public void SendEmail(MimeMessage emailMessage)
        {
            _smtpClient.Send(emailMessage);
        }

        public void Dispose()
        {
            _smtpClient.Disconnect(true);
            _smtpClient.Dispose();
        }
    }
}
