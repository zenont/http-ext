using System.Net.Http;

namespace HttpClientExtended.Interfaces
{
    public interface IHttpClientVerbBuilder<T> where T : HttpClient
    {
        IHttpClientQueryBuilder<T> Get(string requestUri);

        IHttpClientQueryBuilder<T> Post(string requestUri);

        IHttpClientQueryBuilder<T> Post(string requestUri, HttpContent content);

        IHttpClientQueryBuilder<T> Put(string requestUri);

        IHttpClientQueryBuilder<T> Put(string requestUri, HttpContent content);

        IHttpClientQueryBuilder<T> Delete(string requestUri);

        IHttpClientQueryBuilder<T> Head(string requestUri);
    }
}
