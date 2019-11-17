using System.IO;
using System.Threading.Tasks;
using AnubisDBMS.Infraestructure.Data.FileManagement.Entities;

namespace AnubisDBMS.Infraestructure.FileManagement.Interfaces
{
    public interface IFileStorageProvider
    {
        string ProviderName();
        bool RequireCredentials();
        Task<FileStorageProviderResponse> UploadFileAsync(Stream fileStream, string systemFileName);
        Task<FileStorageProviderResponse> DownloadFileAsync(string systemFileName);
        Task<FileStorageProviderResponse> DeleteFileAsync(string systemFileName);
    }
}