using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HttpClientExtended.Abstractions.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> AsJsonAsync<T>(this HttpContent content, CancellationToken cancellationToken = default(CancellationToken))
        {
			string data = await content.ReadAsStringAsync();
            T deserialized = JsonConvert.DeserializeObject<T>(data);
            return deserialized;
		}
    }
}
