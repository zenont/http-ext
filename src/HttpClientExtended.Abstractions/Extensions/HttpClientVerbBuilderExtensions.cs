using HttpClientExtended.Interfaces;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace HttpClientExtended.Abstractions.Extensions
{
    public static class HttpClientVerbBuilderExtensions
    {
        public static IHttpClientVerbBuilder<T> Request<T>(this T httpClient) where T : HttpClient
        {
            return new HttpClientVerbBuilder<T>(httpClient);
        }

        public static IHttpClientQueryBuilder<T> PostAsJson<T, TContent>(this IHttpClientVerbBuilder<T> builder, string requestUri, TContent content) where T : HttpClient
        {
            return builder.Post(requestUri, new ObjectContent(typeof(TContent), content, new JsonMediaTypeFormatter()));
        }

        public static IHttpClientQueryBuilder<T> PostAsXml<T, TContent>(this IHttpClientVerbBuilder<T> builder, string requestUri, TContent content) where T : HttpClient
        {
            return builder.Post(requestUri, new ObjectContent(typeof(TContent), content, new XmlMediaTypeFormatter()));
        }

        public static IHttpClientQueryBuilder<T> PutAsJson<T, TContent>(this IHttpClientVerbBuilder<T> builder, string requestUri, TContent content) where T : HttpClient
        {
            return builder.Put(requestUri, new ObjectContent(typeof(TContent), content, new JsonMediaTypeFormatter()));
        }

        public static IHttpClientQueryBuilder<T> PutAsXml<T, TContent>(this IHttpClientVerbBuilder<T> builder, string requestUri, TContent content) where T : HttpClient
        {
            return builder.Put(requestUri, new ObjectContent(typeof(TContent), content, new XmlMediaTypeFormatter()));
        }
    }
}
