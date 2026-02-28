using System.IO;
using QRCoder;

namespace EasioCore.BLL
{
    public static class QRCodeHelper
    {
        /// <summary>
        /// Same as Anantnag: generates QR code PNG and saves to wwwroot/QRCode/{fileName}.png.
        /// Returns the relative path for use in img src e.g. ~/QRCode/1533.png
        /// </summary>
        public static string GenerateQRCodeReceipt(string qrText, string fileName, string webRootPath)
        {
            if (string.IsNullOrEmpty(webRootPath)) return null;
            try
            {
                var folderPath = Path.Combine(webRootPath, "QRCode");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, fileName + ".png");
                using (var qrGenerator = new QRCodeGenerator())
                using (var qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q))
                using (var qrCode = new PngByteQRCode(qrCodeData))
                {
                    var pngBytes = qrCode.GetGraphic(20);
                    File.WriteAllBytes(filePath, pngBytes);
                }
                return "~/QRCode/" + fileName + ".png";
            }
            catch
            {
                return null;
            }
        }
    }
}
