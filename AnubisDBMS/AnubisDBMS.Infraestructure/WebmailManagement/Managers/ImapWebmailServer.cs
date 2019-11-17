using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnubisDBMS.Infraestructure.Data.WebmailManagement.Dtos;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;

namespace AnubisDBMS.Infraestructure.WebmailManagement.Managers
{
    public class ImapWebmailServer
    {
        private ImapClient client = new ImapClient();

        public ImapWebmailServer()
        {
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            client.Connect("", 143, false);
            client.AuthenticationMechanisms.Remove("");
            client.Authenticate("", "");
        }

        public List<string> GetFoldersNames()
        {
            return client.GetFolders(client.PersonalNamespaces[0]).Select(c => c.FullName).ToList();
        }

        public void RegisterFolder(string folderName)
        {
            var rootFolder = client.GetFolder(client.PersonalNamespaces[0]);
            var existingFolders = rootFolder.GetSubfolders();
            if (!existingFolders.Any(c => c.Name == folderName))
            {
                rootFolder.Create(folderName, true);
            }
        }

        public void RegisterSubFolder(string folderName, string subFolderName)
        {
            var rootFolder = client.GetFolder(client.PersonalNamespaces[0]);
            RegisterFolder(folderName);
            var parentFolder = rootFolder.GetSubfolder(folderName);
            var parentSubfolders = parentFolder.GetSubfolders();
            if (!parentSubfolders.Any(c => c.Name == subFolderName))
            {
                parentFolder.Create(subFolderName, true);
            }
        }

        public void DeleteFolder(string folderName)
        {
            var rootFolder = client.GetFolder(client.PersonalNamespaces[0]);
            var existingFolders = rootFolder.GetSubfolders();
            if (existingFolders.Any(c => c.Name == folderName))
            {
                rootFolder.GetSubfolder(folderName).Delete();
            }
        }

        public List<string> GetInboxMessages()
        {
            List<MimeMessage> inboxMessages = new List<MimeMessage>();
            var inbox = client.Inbox;
            for (int i = 0; i < inbox.Count; i++)
            {
                inboxMessages.Add(inbox.GetMessage(i));
            }
            return inboxMessages.Select(c => c.Subject).ToList();
        }

        public List<EmailMessageEnvelope> GetInboxMessagesWithAttachment(DateTime? minDate = null, DateTime? maxDate = null)
        {
            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadWrite);
            List<EmailMessageEnvelope> inboxMessages = new List<EmailMessageEnvelope>();
            var query = SearchQuery.All;
            if (minDate != null && maxDate != null)
            {
                query = SearchQuery.DeliveredAfter(minDate.Value);
            }
            var order = new[] { OrderBy.ReverseArrival };
            var uids = client.Inbox.Sort(query, order);
            foreach (var uid in uids)
            {
                var message = inbox.GetMessage(uid);
                if (message.Attachments != null && message.Attachments.Any())
                {
                    var messageEnvelope = new EmailMessageEnvelope
                    {
                        SenderName = message.From.First().Name,
                        Subject = message.Subject,
                        ReceptionDate = message.Date.DateTime,
                        MessageKey = message.MessageId
                    };
                    foreach (var attachment in message.Attachments.Where(c => c.IsAttachment))
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        if (attachment is MessagePart)
                        {
                            var part = (MessagePart)attachment;
                            part.Message.WriteTo(memoryStream);
                        }
                        else
                        {
                            var part = (MimePart)attachment;
                            part.Content.DecodeTo(memoryStream);
                        }
                        memoryStream.Position = 0;
                        messageEnvelope.Attachments.Add(new EmailMessageAttachmentEnvelope
                        {
                            FileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType?.Name,
                            FileStream = memoryStream
                        });
                    }
                    inboxMessages.Add(messageEnvelope);
                }
            }
            return inboxMessages;
        }

        public void Dispose()
        {
            client.Disconnect(true);
        }
    }
}
