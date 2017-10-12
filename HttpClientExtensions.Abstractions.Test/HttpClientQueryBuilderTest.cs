using HttpClientExtended.Abstractions;
using HttpClientExtended.Abstractions.Extensions;
using HttpClientExtended.Interfaces;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace HttpClientExtensions.Abstractions.Test
{
    public class HttpClientQueryBuilderTest
    {
        [Fact]
        public async Task ShouldSetAcceptHeaderInRequest()
        {
            // arrange
            const string requestUri = "/fake";
            const string charset = "utf-8";
            const double quality = 1.02;
            const string mediaType = "text/html";
            string value = $"{mediaType};charset={charset};q={quality}";
            HttpClient client = new HttpClient();
            IHttpClientVerbBuilder<HttpClient> builder = new HttpClientVerbBuilder<HttpClient>(client);

            // act
            HttpRequestMessage httpRequest = await client
                .Request()
                .Get(requestUri)
                .Header("Accept", value)
                .BuildHttpRequestAsync();
            MediaTypeWithQualityHeaderValue header = httpRequest.Headers.Accept.FirstOrDefault();

            // assert
            Assert.NotNull(header);
            Assert.True(header.MediaType == mediaType);
            Assert.True(header.CharSet == charset);
            Assert.True(header.Quality == quality);
        }

        [Fact]
        public async Task ShouldSetAcceptCharsetHeaderInRequest()
        {
            // arrange
            const string requestUri = "/fake";
            const double quality = 1;
            const string charset = "utf-8";
            string value = $"{charset};q={quality}";
            HttpClient client = new HttpClient();
            IHttpClientVerbBuilder<HttpClient> builder = new HttpClientVerbBuilder<HttpClient>(client);

            // act
            HttpRequestMessage httpRequest = await client
                .Request()
                .Get(requestUri)
                .Header("Accept-Charset", value)
                .BuildHttpRequestAsync();
            StringWithQualityHeaderValue header = httpRequest.Headers.AcceptCharset.FirstOrDefault();

            // assert
            Assert.NotNull(header);
            Assert.True(header.Quality == quality);
            Assert.True(header.Value == charset);
        }

        [Theory]
        [InlineData("Basic", "SOMEKEY")]
        [InlineData("Bearer", "100920ZKDLLWO")]
        public async Task ShouldSetAuthorizationHeaderInRequest(string scheme, string parameter)
        {
            // arrange
            const string requestUri = "/fake";
            HttpClient client = new HttpClient();
            IHttpClientVerbBuilder<HttpClient> builder = new HttpClientVerbBuilder<HttpClient>(client);

            // act
            HttpRequestMessage httpRequest = await client
                .Request()
                .Get(requestUri)
                .Header("Authorization", $"{scheme} {parameter}")
                .BuildHttpRequestAsync();
            AuthenticationHeaderValue header = httpRequest.Headers.Authorization;

            // assert
            Assert.NotNull(header);
            Assert.True(header.Scheme == scheme);
            Assert.True(header.Parameter == parameter);
        }
    }
}
