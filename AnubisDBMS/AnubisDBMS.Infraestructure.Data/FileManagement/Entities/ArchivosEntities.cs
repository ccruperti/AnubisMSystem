using AnubisDBMS.Infraestructure.Data.FileManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnubisDBMS.Infraestructure.Data.FileManagement.Entities
{
    //public class Archivo : IFileEntity<long>
    //{
    //    public long IdArchivo { get; set; }
    //    public string EnlacePublico { get; set; }
    //    public string EnlacePrivado { get; set; }
    //    public string NombreOriginal { get; set; }
    //    public string NombreSistema { get; set; }
    //    public bool Lectura { get; set; }
    //    public bool Escritura { get; set; }
    //    public bool Descarga { get; set; }
    //    public bool Sistema { get; set; }
    //    public bool Disponible { get; set; }
    //    public decimal TamanioKb { get; set; }
    //    public decimal TamanioMb { get; set; }
    //    public DateTime FechaCarga { get; set; }
    //    public DateTime? FechaModificacion { get; set; }
    //    public DateTime? FechaEliminacion { get; set; }
    //}

    //public class ProveedorAlmacenamiento : IFileStorageProviderEntity<int>
    //{
    //    public virtual ICollection<Archivo> Archivos { get; set; }
    //    public int IdProveedorAlmacenamiento { get; set; }
    //    public string Nombre { get; set; }
    //    public string Descripcion { get; set; }
    //    public bool Externo { get; set; }
    //    public bool Interno { get; set; }
    //    public bool Disponible { get; set; }
    //    public bool Credenciales { get; set; }
    //}

    //public class DescargaArchivo
    //{
    //    public long IdDescargaArchivo { get; set; }
    //    public long IdArchivo { get; set; }
    //    public bool Directa { get; set; }
    //    public bool Redireccionada { get; set; }
    //    public string UrlRedireccion { get; set; }
    //    public string Usuario { get; set; }
    //    public virtual Archivo Archivo { get; set; }
    //}
}