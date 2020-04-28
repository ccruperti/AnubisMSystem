using AnubisDBMS.Infraestructure.Data.Helpers;
using System.Collections.Generic;

namespace AnubisDBMS.Data.Localization.Entities
{
    public class Pais
    {
        public int IdPais { get; set; }
        public int Codigo { get; set; }
        public string Alpha2 { get; set; }
        public string Alpha3 { get; set; }
        public string NombreCS { get; set; }
        public string NombreDE { get; set; }
        public string NombreEN { get; set; }
        public string NombreES { get; set; }
        public string NombreFR { get; set; }
        public string NombreIT { get; set; }
        public string NombreNL { get; set; }
        public string DenominacionEstado { get; set; }
        public string CodigoTelefonico { get; set; }
        public bool Defecto { get; set; }

        public bool Disponible { get; set; }

        // FKs
        public int? IdContinente { get; set; }

        public int? IdIdioma { get; set; }

        // Virtuals
        public virtual Continente Continente { get; set; }

        public virtual ICollection<Region> Regiones { get; set; }
        public virtual ICollection<Estado> Estados { get; set; }
        public virtual Idioma Idioma { get; set; }
    }
    public class Empresa : CrudEntities
    {
        public long IdEmpresa { get; set; }
        public string Nombre { get; set; }
        public string RUC { get; set; }
        public string RazonSocial { get; set; }
        public bool ServicioActivo { get; set; } 
        public string EmailNotificacion { get; set; }
        public int PrimeraNotificacion { get; set; }
        public int SegundaNotificacion { get; set; }
        public int TerceraNotificacion { get; set; }
    }
    public class Continente
    {
        public int IdContinente { get; set; }
        public string Codigo { get; set; }

        public string Nombre { get; set; }

        // Virtuals
        public virtual ICollection<Pais> Paises { get; set; }
    }

    public class Region
    {
        public int IdRegion { get; set; }
        public string Codigo { get; set; }

        public string Nombre { get; set; }

        // FKs
        public int IdPais { get; set; }

        // Virtuals
        public virtual Pais Pais { get; set; }

        public virtual ICollection<Estado> Estados { get; set; }
    }

    public class Estado
    {
        public int IdEstado { get; set; }
        public string Codigo { get; set; }

        public string Nombre { get; set; }

        // FKs
        public int IdPais { get; set; }

        public int? IdRegion { get; set; }

        // Virtuals
        public virtual Pais Pais { get; set; }

        public virtual Region Region { get; set; }
        public virtual ICollection<Ciudad> Ciudades { get; set; }
    }

    public class Ciudad
    {
        public int IdCiudad { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string CodigoPostal { get; set; }
        public decimal Latitud { get; set; }

        public decimal Longitud { get; set; }

        // FKs
        public int IdEstado { get; set; }

        // Virtual
        public virtual Estado Estado { get; set; }
    }
}