using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HttpClientExt.Abstractions.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> FromJsonAsync<T>(this HttpContent content, CancellationToken cancellationToken = default(CancellationToken))
        {
			string data = await content.ReadAsStringAsync();
            T deserialized = JsonConvert.DeserializeObject<T>(data);
            return deserialized;
		}
    }
}
