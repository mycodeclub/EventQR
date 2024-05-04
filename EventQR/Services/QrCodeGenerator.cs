using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;

namespace EventQR.Services
{
    public class QrCodeGenerator : IQrCodeGenerator
    {
        private readonly IWebHostEnvironment _environment;
        public QrCodeGenerator(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string GetQRCodeSystemPath(Guid guestId)
        {
            string fileName = Path.GetFileName(guestId.ToString());
            string path = Path.Combine(_environment.WebRootPath, "GeneratedQRCode");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path + "\\" + fileName + ".png";
        }
        public string GenerateQRCode(Guid guestId, Guid eventId)
        {
            var qrCodeUri = Common.Static.Variables.GetQrCodeUriStr(guestId, eventId);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeUri, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            string filePath = GetQRCodeSystemPath(guestId);
            qrCodeImage.Save(filePath, ImageFormat.Png);
            return filePath;
        }
    }
}
