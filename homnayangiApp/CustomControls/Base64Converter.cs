using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.CustomControls
{
    public class Base64Converter
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<List<string>> ConvertUrlsToBase64Strings(ObservableCollection<string> imageUrls)
        {
            var base64Strings = new List<string>();

            foreach (var imageUrl in imageUrls)
            {
                var base64String = await ConvertUrlToBase64String(imageUrl);
                base64Strings.Add(base64String);
            }

            return base64Strings;
        }

        private static async Task<string> ConvertUrlToBase64String(string imageUrl)
        {
            using (var stream = await _httpClient.GetStreamAsync(imageUrl))
            {
                var bytes = await ReadStreamAsByteArrayAsync(stream);
                return Convert.ToBase64String(bytes);
            }
        }

        private static async Task<byte[]> ReadStreamAsByteArrayAsync(Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
