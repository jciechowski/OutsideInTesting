using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OutsideInTesting
{
    public static class ContentReader
    {
        public static Task<dynamic> ReadAsJsonAsync(this HttpContent content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            return content.ReadAsStringAsync().ContinueWith(t =>
                JsonConvert.DeserializeObject(t.Result));
        }

        public static dynamic ToJObject(this object value)
        {
            return JsonConvert.DeserializeObject(
                JsonConvert.SerializeObject(value));
        }
    }
}