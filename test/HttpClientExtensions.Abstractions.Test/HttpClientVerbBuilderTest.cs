using HttpClientExtended.Abstractions;
using HttpClientExtended.Abstractions.Extensions;
using HttpClientExtended.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using Xunit;

namespace HttpClientExtensions.Abstractions.Test
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
            IHttpClientQueryBuilder queryBuilder = builder.Post(url, new ObjectContent<FakePayload>(payload, new JsonMediaTypeFormatter()));

            // assert
            Assert.NotNull(queryBuilder.Content);
            Assert.Matches(queryBuilder.HttpMethod.Method, "POST");
            ObjectContent<FakePayload> content = Assert.IsType<ObjectContent<FakePayload>>(queryBuilder.Content);
            var resultPayload = Assert.IsType<FakePayload>(content.Value);
            Assert.NotNull(resultPayload);
            Assert.True(resultPayload.Id == payload.Id);
            Assert.True(resultPayload.NonNullDateTime == payload.NonNullDateTime);
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
            ObjectContent content = Assert.IsType<ObjectContent>(queryBuilder.Content);
            var resultPayload = Assert.IsType<FakePayload>(content.Value);
            Assert.NotNull(resultPayload);
            Assert.True(resultPayload.Id == payload.Id);
            Assert.True(resultPayload.NonNullDateTime == payload.NonNullDateTime);
        }

        [Fact]
        public void ShouldBuildPostHttpMethodWithXmlPayload()
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
            IHttpClientQueryBuilder queryBuilder = builder.PostAsXml(url, payload);

            // assert
            Assert.NotNull(queryBuilder.Content);
            Assert.Matches(queryBuilder.HttpMethod.Method, "POST");
            ObjectContent content = Assert.IsType<ObjectContent>(queryBuilder.Content);
            var resultPayload = Assert.IsType<FakePayload>(content.Value);
            Assert.NotNull(resultPayload);
            Assert.True(resultPayload.Id == payload.Id);
            Assert.True(resultPayload.NonNullDateTime == payload.NonNullDateTime);
        }
    }
}
