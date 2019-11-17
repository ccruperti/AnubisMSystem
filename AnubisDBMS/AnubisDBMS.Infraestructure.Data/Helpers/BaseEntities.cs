using System;

namespace AnubisDBMS.Infraestructure.Data.Helpers
{
    public class CrudEntities
    {
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        //public string SesionRegistro { get; set; }
        //public string SesionModificacion { get; set; }
        //public string SesionEliminacion { get; set; }
        //public string IpRegistro { get; set; }
        //public string IpModificacion { get; set; }
        //public string IpEliminacion { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioEliminacion { get; set; }
    }

    public class CatalogEntities : CrudEntities
    {
        public bool Sistema { get; set; }
        public bool Defecto { get; set; }
        public string ClaseCss { get; set; }
        public string IconoCss { get; set; }
    }

    public class ApiCatalogEntities : CatalogEntities
    {
        public bool TransmisibleApi { get; set; }
    }

    public class TransactionEntities : CrudEntities
    {
        public long Transaccion { get; set; }
    }

    public class ActivationEntities : CrudEntities
    {
        public bool Disponible { get; set; }
        public DateTime FechaActivacion { get; set; }
        public DateTime FechaDesactivacion { get; set; }
        public string UsuarioActivacion { get; set; }
        public string UsuarioDesactivacion { get; set; }
    }
}