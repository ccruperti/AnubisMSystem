using AnubisDBMS.Data;
using AnubisDBMS.Infraestructura.Data;
using AnubisDBMS.Infraestructure.FileManagement.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AnubisDBMS.Infraestructure.FileManagement.Managers
{
    public class URPlataformFileManager
    {
        private readonly AnubisDBMSDbContext _context;

        private readonly IFileStorageProvider _fileStorageProvider;

        public URPlataformFileManager(AnubisDBMSDbContext context, IFileStorageProvider fileStorageProvider)
        {
            _context = context;
            _fileStorageProvider = fileStorageProvider;
        }


        //private int ObtenerCargasPrevias(string fileName)
        //{
        //    return _context.Archivos.Count(c => c.NombreOriginal == fileName);
        //}

        //private string ObtenerNombreSistema(string fileName)
        //{
        //    fileName = fileName.Replace(' ', '+');
        //    var cargasPrevias = ObtenerCargasPrevias(fileName);
        //    if (cargasPrevias > 0)
        //    {
        //        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        //        var fileExtension = Path.GetExtension(fileName).ToLower();
        //        return string.Format("{0}-({1}){2}", fileNameWithoutExtension, cargasPrevias, fileExtension);
        //    }
        //    return fileName;
        //}

        private string ObtenerDirectorioCarga(string systemFileName)
        {
            var ano = DateTime.Today.Year.ToString();
            var mes = DateTime.Today.ToString("MMMM");
            mes = mes.Substring(0,1).ToUpper() + mes.Substring(1).ToLower();
            var dia = DateTime.Today.Day.ToString() + "-" + DateTime.Today.ToString("dddd");
            return $"{ano}/{mes}/{dia}/{systemFileName}";
        }

    }
}