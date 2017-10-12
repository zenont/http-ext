using HttpClientExtended.Common;
using HttpClientExtended.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientExtended.Abstractions
{
    public class HttpClientQueryBuilder<T> : IHttpClientQueryBuilder<T> where T : HttpClient
    {
        protected const string SetCookieHeader = "Set-Cookie";
        private readonly List<KeyValuePair<string, string[]>> _cookieHeaders = new List<KeyValuePair<string, string[]>>();
        private readonly IDictionary<string, string[]> _headers = new Dictionary<string, string[]>();

        public HttpClientQueryBuilder(T httpClient, HttpMethod httpMethod, string requestUri, HttpContent content = null)
        {
            HttpClient = httpClient;
            HttpMethod = httpMethod;
            Content = content;
            RequestUri = requestUri;
        }

        public T HttpClient { get; protected set; }

        public HttpMethod HttpMethod { get; protected set; }

        public HttpContent Content { get; protected set; }

        public string RequestUri { get; protected set; }

        public QueryString QueryString { get; protected set; } = new QueryString();

        public IEnumerable<KeyValuePair<string, string[]>> Headers => _headers.Union(_cookieHeaders);

        HttpClient IHttpClientQueryBuilder.HttpClient => HttpClient;

        public IHttpClientQueryBuilder<T> Query(string key, object value)
        {
            QueryString.Add(key, value);
            return this;
        }

        public IHttpClientQueryBuilder<T> Header(string key, params string[] value)
        {
            if (key.Equals(SetCookieHeader, StringComparison.OrdinalIgnoreCase))
            {
                // set-cookie headers can be duplicate
                _cookieHeaders.Add(new KeyValuePair<string, string[]>(key, value));
            }
            else
            {
                // all other cookies must be unique
                _headers.Add(key, value);
            }
            return this;
        }

        public virtual async Task<HttpRequestMessage> BuildHttpRequestAsync()
        {
            if (string.IsNullOrWhiteSpace(RequestUri))
                throw new ArgumentNullException(nameof(RequestUri));

            if (HttpMethod == null)
                throw new ArgumentNullException(nameof(HttpMethod));

            if (!Uri.IsWellFormedUriString(RequestUri, UriKind.RelativeOrAbsolute))
                throw new UriFormatException(RequestUri);

            Uri uri = await QueryString.AsUriAsync(RequestUri);

            var request = new HttpRequestMessage(HttpMethod, uri.ToString());
            foreach(var header in Headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            if (Content != null)
            {
                request.Content = Content;
            }

            return request;
        }

        public async Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            using (HttpRequestMessage request = await BuildHttpRequestAsync())
            {
                return await HttpClient.SendAsync(request, cancellationToken);
            }
        }
    }
}
