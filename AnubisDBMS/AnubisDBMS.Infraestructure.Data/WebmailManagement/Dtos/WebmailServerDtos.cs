using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Infraestructure.Data.WebmailManagement.Dtos
{
    public class EmailMessageEnvelope
    {
        public EmailMessageEnvelope()
        {
            Attachments = new List<EmailMessageAttachmentEnvelope>();
        }
        public string MessageKey { get; set; }
        public string Subject { get; set; }
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public string Body { get; set; }
        public DateTime ReceptionDate { get; set; }
        public List<EmailMessageAttachmentEnvelope> Attachments { get; set; }
    }

    public class EmailMessageAttachmentEnvelope
    {
        public Stream FileStream { get; set; }
        public string FileName { get; set; }
    }
}
