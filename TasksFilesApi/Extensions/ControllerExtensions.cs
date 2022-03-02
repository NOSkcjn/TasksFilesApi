using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace TasksFilesApi.Presentation.Extensions
{
    public static class ControllerExtensions
    {
        public static async Task<byte[]> GetBytesAsync(this IFormFile file)
        {
            if (file?.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    return ms.ToArray();
                }
            }

            return null;
        }
    }
}
