using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Infraestructure.Data.FileManagement.Entities
{
    /// <summary>
    /// Emite una respuesta del estado de la operacion delegada al proveedor de almacenamiento.
    /// </summary>
    public class FileStorageProviderResponse
    {
        public FileStorageProviderResponse()
        {
            Errors = new List<string>();
            Succesful = false;
        }
        /// <summary>
        /// Indica si la ultima operacion delegada fue exitosa.
        /// </summary>
        public bool Succesful { get; private set; }
        public string Message { get; set; }
        public List<string> Errors { get; }
        public Stream FileStream { get; set; }
        public string SystemFileName { get; set; }
        /// <summary>
        /// Agrega un error a la coleccion de errores de la respuesta del proveedor de almacenamiento.
        /// </summary>
        /// <param name="error">Mensaje del error a incluir</param>
        public void AddError(string error)
        {
            Succesful = false;
            Errors.Add(error);
        }
        /// <summary>
        /// Cambia el estado de respuesta a exitoso.
        /// </summary>
        public void IsSuccesful()
        {
            Succesful = true;
        }
    }
}
