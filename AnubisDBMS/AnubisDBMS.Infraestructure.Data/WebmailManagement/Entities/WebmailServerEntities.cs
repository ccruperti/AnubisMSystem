using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnubisDBMS.Infraestructure.Data.Helpers;

namespace AnubisDBMS.Infraestructure.Data.WebmailManagement.Entities
{
    public class ServidorWebmail : CrudEntities
    {
        public int IdServidorWebmail { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Lectura { get; set; }
        public bool Escritura { get; set; }
        public string Opciones { get; set; }
        public int? Buffer { get; set; }
        public int? Capacidad { get; set; }

        public virtual ICollection<MensajeEmail> MensajesEmail { get; set; }
    }

    public class MensajeEmail
    {
        public long IdMensajeEmail { get; set; }
        public string WebmailKey { get; set; }
        //public int IdServidorWebmail { get; set; }
        public string Asunto { get; set; }
        public string NombreEmisor { get; set; }
        public string EmailEmisor { get; set; }
        public bool Adjunto { get; set; }
        public bool Disponible { get; set; }
        public string Prioridad { get; set; }
        public string RutaOriginal { get; set; }
        public string RutaActual { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public DateTime FechaSincronizacion { get; set; }
        public string Adjuntos { get; set; }
        //public virtual ServidorWebmail ServidorWebmail { get; set; }
    }
}
