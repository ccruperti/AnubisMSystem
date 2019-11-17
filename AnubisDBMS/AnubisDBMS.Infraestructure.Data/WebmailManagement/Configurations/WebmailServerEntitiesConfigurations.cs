using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnubisDBMS.Infraestructure.Data.Helpers;
using AnubisDBMS.Infraestructure.Data.WebmailManagement.Entities;

namespace AnubisDBMS.Infraestructure.Data.WebmailManagement.Configurations
{
    public class ServidorWebmailConfiguration : EntityTypeConfiguration<ServidorWebmail>
    {
        public ServidorWebmailConfiguration()
        {
            ToTable("WebmailServidores", Schemas.General);

            HasKey(c => c.IdServidorWebmail);
        }
    }

    public class MensajeEmailConfiguration : EntityTypeConfiguration<MensajeEmail>
    {
        public MensajeEmailConfiguration()
        {
            ToTable("WebmailMensajes", Schemas.General);

            HasKey(c => c.IdMensajeEmail);

            /*HasRequired(c => c.ServidorWebmail)
                .WithMany(c => c.MensajesEmail)
                .HasForeignKey(c => c.IdServidorWebmail);*/
        }
    }
}
