using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AquaServer.Extensions.Helper
{
    public class WWWRootResources
    {
        private IWebHostEnvironment _webHost;
        public WWWRootResources(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        public async Task<string> ReadResource(string resourcePath)
        {
            return await File.ReadAllTextAsync(Path.Combine(_webHost.WebRootPath, resourcePath), Encoding.ASCII);
        }

        public async Task WriteResource(string article, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = "/accessory/" + article + ".jpg";

                using (var fileStream = new FileStream(_webHost.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                var image = Bitmap.FromFile(_webHost.WebRootPath + path);
                double ratio = 128.0/ image.Height;
                int newWidth = (int)(image.Width * ratio);
                int newHeight = (int)(image.Height * ratio);
                Bitmap newImage = new Bitmap(newWidth, newHeight);
                using (Graphics g = Graphics.FromImage(newImage))
                {
                    g.DrawImage(image, 0, 0, newWidth, newHeight);
                    image.Dispose();
                }

                File.Delete(_webHost.WebRootPath + path);
                newImage.Save(_webHost.WebRootPath + path);
            }
        }
    }
}
