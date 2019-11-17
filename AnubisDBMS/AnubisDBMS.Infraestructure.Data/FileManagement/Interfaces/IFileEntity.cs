using System;

namespace AnubisDBMS.Infraestructure.Data.FileManagement.Interfaces
{
    public interface IFileEntity<TFileEntityKey> where TFileEntityKey : struct
    {
        TFileEntityKey IdArchivo { get; set; }
        string EnlacePublico { get; set; }
        string EnlacePrivado { get; set; }
        string NombreOriginal { get; set; }
        string NombreSistema { get; set; }
        bool Lectura { get; set; }
        bool Escritura { get; set; }
        bool Descarga { get; set; }
        bool Sistema { get; set; }
        bool Disponible { get; set; }
        decimal TamanioKb { get; set; }
        decimal TamanioMb { get; set; }
        DateTime FechaCarga { get; set; }
        DateTime? FechaModificacion { get; set; }
        DateTime? FechaEliminacion { get; set; }
    }
}