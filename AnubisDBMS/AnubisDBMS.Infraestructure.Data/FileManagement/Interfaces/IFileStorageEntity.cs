namespace AnubisDBMS.Infraestructure.Data.FileManagement.Interfaces
{
    public interface IFileStorageProviderEntity<TKey>
    {
        TKey IdProveedorAlmacenamiento { get; set; }
        string Nombre { get; set; }
        string Descripcion { get; set; }
        bool Externo { get; set; }
        bool Interno { get; set; }
        bool Disponible { get; set; }
        bool Credenciales { get; set; }
    }
}