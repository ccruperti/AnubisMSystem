using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using AnubisDBMS.Infraestructure.Data.FileManagement.Entities;
using AnubisDBMS.Infraestructure.FileManagement.Interfaces;

namespace AnubisDBMS.Infraestructure.FileManagement.Providers
{
    /// <summary>
    ///     Proveedor de Almacenamiento de Amazon S3.
    /// </summary>
    public class S3FileStorageProvider : IFileStorageProvider
    {
        private readonly S3Bucket _bucket = new S3Bucket
        {
            BucketName = ""
        };

        private readonly IAmazonS3 _client = new AmazonS3Client();


        /// <summary>
        ///     Carga un archivo a Amazon S3, el parametro systemFileName se utiliza como el KEY para encontrar el archivo
        ///     posteriormente en el bucket.
        /// </summary>
        /// <param name="fileStream">Stream del archivo a cargar</param>
        /// <param name="systemFileName">Identificador que se utiliza como el KEY para identificar el archivo posteriormente.</param>
        /// <returns></returns>
        public async Task<FileStorageProviderResponse> UploadFileAsync(Stream fileStream, string systemFileName)
        {
            var providerResponse = new FileStorageProviderResponse();
            var requestObject = new PutObjectRequest
            {
                InputStream = fileStream,
                BucketName = _bucket.BucketName,
                CannedACL = S3CannedACL.PublicRead,
                Key = systemFileName
            };
            var response = await _client.PutObjectAsync(requestObject);
            switch (response.HttpStatusCode)
            {
                case HttpStatusCode.OK:
                    providerResponse.IsSuccesful();
                    break;
                case HttpStatusCode.Forbidden:
                    providerResponse.AddError("Verificar si las credenciales de Amazon estan correctas.");
                    break;
                default:
                    providerResponse.AddError("Error en carga del archivo.");
                    break;
            }
            return providerResponse;
        }

        public async Task<FileStorageProviderResponse> DownloadFileAsync(string systemFileName)
        {
            var requestObject = new GetObjectRequest
            {
                BucketName = _bucket.BucketName,
                Key = systemFileName
            };
            var response = await _client.GetObjectAsync(requestObject);
            
            return new FileStorageProviderResponse();
        }

        public async Task<FileStorageProviderResponse> DeleteFileAsync(string systemFileName)
        {
            var requestObject = new DeleteObjectRequest {
                BucketName = _bucket.BucketName,
                Key = systemFileName
            };
            var response = await _client.DeleteObjectAsync(requestObject);
            return new FileStorageProviderResponse();
        }

        string IFileStorageProvider.ProviderName()
        {
            return "Amazon S3 File Storage";
        }

        bool IFileStorageProvider.RequireCredentials()
        {
            return true;
        }
    }
}