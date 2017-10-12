using System;
using HttpClientExt.Abstractions;
using HttpClientExt.Common;
using HttpClientExt.Interfaces;
using HttpClientExt.Abstractions.Extensions;
using Xunit;

namespace HttpClientExt.UnitTests
{
    public class HttpClientVerbBuilderTest
    {
        [Fact]
        public void ShouldBuildGetHttpMethod()
        {
            // arrange
            const string url = "http://fakeurl/";
            var httpClient = new MockedHttpClient();
            IHttpClientVerbBuilder<MockedHttpClient> builder = new HttpClientVerbBuilder<MockedHttpClient>(httpClient);

            // act
            IHttpClientQueryBuilder queryBuilder = builder.Get(url);

            // assert
            Assert.Matches(queryBuilder.HttpMethod.Method, "GET");
        }

        [Fact]
        public void ShouldBuildPostHttpMethod()
        {
            // arrange
            const string url = "http://fakeurl/";
            var httpClient = new MockedHttpClient();
            IHttpClientVerbBuilder<MockedHttpClient> builder = new HttpClientVerbBuilder<MockedHttpClient>(httpClient);

            // act
            IHttpClientQueryBuilder queryBuilder = builder.Post(url);

            // assert
            Assert.Matches(queryBuilder.HttpMethod.Method, "POST");
        }

        [Fact]
        public void ShouldBuildPostHttpMethodWithPayload()
        {
            // arrange
            const string url = "http://fakeurl/";
            var payload = new FakePayload
            {
                Id = 10,
                NonNullDateTime = new DateTime(2010, 1, 1, 10, 00, 00),
                Note = "Note1",
                NullableDateTime = new DateTime(2012, 1, 1, 1, 0, 0, 0),
                Token = Guid.NewGuid()
            };
            var httpClient = new MockedHttpClient();
            IHttpClientVerbBuilder<MockedHttpClient> builder = new HttpClientVerbBuilder<MockedHttpClient>(httpClient);

            // act
            IHttpClientQueryBuilder queryBuilder = builder.Post(url, new JsonContent(payload));

            // assert
            Assert.NotNull(queryBuilder.Content);
            Assert.Matches(queryBuilder.HttpMethod.Method, "POST");
            JsonContent content = Assert.IsType<JsonContent>(queryBuilder.Content);
        }

        [Fact]
        public void ShouldBuildPostHttpMethodWithJsonPayload()
        {
            // arrange
            const string url = "http://fakeurl/";
            var payload = new FakePayload
            {
                Id = 10,
                NonNullDateTime = new DateTime(2010, 1, 1, 10, 00, 00),
                Note = "Note1",
                NullableDateTime = new DateTime(2012, 1, 1, 1, 0, 0, 0),
                Token = Guid.NewGuid()
            };
            var httpClient = new MockedHttpClient();
            IHttpClientVerbBuilder<MockedHttpClient> builder = new HttpClientVerbBuilder<MockedHttpClient>(httpClient);

            // act
            IHttpClientQueryBuilder queryBuilder = builder.PostAsJson(url, payload);

            // assert
            Assert.NotNull(queryBuilder.Content);
            Assert.Matches(queryBuilder.HttpMethod.Method, "POST");
            JsonContent content = Assert.IsType<JsonContent>(queryBuilder.Content);
        }
    }
}
