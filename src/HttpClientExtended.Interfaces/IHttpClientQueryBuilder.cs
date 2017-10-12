using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HttpClientExtended.Common;

namespace HttpClientExtended.Interfaces
{
    public interface IHttpClientQueryBuilder
    {
        Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default(CancellationToken));

        HttpClient HttpClient { get; }

        HttpMethod HttpMethod { get; }

        HttpContent Content { get; }

        string RequestUri { get; }

        QueryString QueryString { get; }

        Task<HttpRequestMessage> BuildHttpRequestAsync();
    }

    public interface IHttpClientQueryBuilder<T> : IHttpClientQueryBuilder where T : HttpClient
    {
        new T HttpClient { get; }

        IHttpClientQueryBuilder<T> Query(string key, object value);

        IHttpClientQueryBuilder<T> Header(string key, params string[] value);
    }
}
