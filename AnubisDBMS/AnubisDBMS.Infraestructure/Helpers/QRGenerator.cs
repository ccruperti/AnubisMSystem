using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Infraestructure.Helpers
{
    public class QRGenerator
    {

        QRCodeGenerator qrGenerator = new QRCodeGenerator();


        public string GenerarQR(string codigo)
        {
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(codigo, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap QR = qrCode.GetGraphic(20);
            MemoryStream ms = new MemoryStream();
            QR.Save(ms, ImageFormat.Gif);
            var base64Data = Convert.ToBase64String(ms.ToArray()); 
            return base64Data;
        }

    }
}
