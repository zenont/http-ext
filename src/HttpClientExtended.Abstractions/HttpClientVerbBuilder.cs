using HttpClientExtended.Interfaces;
using System.Net.Http;

namespace HttpClientExtended.Abstractions
{
    public class HttpClientVerbBuilder<T> : IHttpClientVerbBuilder<T> where T : HttpClient
    {
        public HttpClientVerbBuilder(T httpClient)
        {
            HttpClient = httpClient;
        }

        protected T HttpClient { get; set; }

        public IHttpClientQueryBuilder<T> Get(string requestUri)
        {
            return new HttpClientQueryBuilder<T>(HttpClient, HttpMethod.Get, requestUri);
        }

        public IHttpClientQueryBuilder<T> Post(string requestUri)
        {
            return new HttpClientQueryBuilder<T>(HttpClient, HttpMethod.Post, requestUri);
        }

        public IHttpClientQueryBuilder<T> Post(string requestUri, HttpContent content)
        {
            return new HttpClientQueryBuilder<T>(HttpClient, HttpMethod.Post, requestUri, content);
        }

        public IHttpClientQueryBuilder<T> Put(string requestUri)
        {
            return new HttpClientQueryBuilder<T>(HttpClient, HttpMethod.Put, requestUri);
        }

        public IHttpClientQueryBuilder<T> Put(string requestUri, HttpContent content)
        {
            return new HttpClientQueryBuilder<T>(HttpClient, HttpMethod.Put, requestUri, content);
        }

        public IHttpClientQueryBuilder<T> Delete(string requestUri)
        {
            return new HttpClientQueryBuilder<T>(HttpClient, HttpMethod.Delete, requestUri);
        }

        public IHttpClientQueryBuilder<T> Head(string requestUri)
        {
            return new HttpClientQueryBuilder<T>(HttpClient, HttpMethod.Head, requestUri);
        }
    }
}
