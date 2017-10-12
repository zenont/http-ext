using HttpClientExtended.Abstractions;
using HttpClientExtended.Interfaces;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using HttpClientExtended.Abstractions.Extensions;
using System.Net.Http.Formatting;
using System.IO;
using System.Text;

namespace HttpClientExtensions.Abstractions.Test
{
    public class WebApiTest
    {
        [Fact]
        public async Task ShouldHttpMethodGet()
        {
            // arrange
            const string requestUri = "/fake";
            string method = null;
            CancellationToken cancellationToken = CancellationToken.None;
            var webHostBuilder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.Run(async context =>
                    {
                        method = context.Request.Method;
                        context.Response.StatusCode = 200;
                    });
                });
            TestServer server = new TestServer(webHostBuilder);
            HttpClient client = server.CreateClient();
            IHttpClientVerbBuilder<HttpClient> builder = new HttpClientVerbBuilder<HttpClient>(client);

            // act
            HttpResponseMessage response = await client
                .Request()
                .Get(requestUri)
                .SendAsync(cancellationToken);

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Matches("GET", method);
        }

        [Fact]
        public async Task ShouldGetSuccessfulHttpResponseWithInt32QueryString()
        {
            // arrange
            const int id = 15;
            const string requestUri = "/fake";
            const string idKey = "id";
            CancellationToken cancellationToken = CancellationToken.None;
            var webHostBuilder = new WebHostBuilder()
                .Configure(app => 
                {
                    app.Run(async context => 
                    {
                        int parsedResult = -1;
                        string r = context.Request.Query.FirstOrDefault(q => q.Key == idKey).Value.FirstOrDefault();
                        int.TryParse(r, out parsedResult);
                        context.Response.ContentType = "application/json";
                        var resultPayload = JsonConvert.SerializeObject(new FakePayload { Id = parsedResult });
                        await context.Response.WriteAsync(resultPayload, cancellationToken);
                    });
                });
            TestServer server = new TestServer(webHostBuilder);
            HttpClient client = server.CreateClient();

            // act
            HttpResponseMessage response = await client
                .Request()
                .Get(requestUri)
                .Query(nameof(id), id)
                .SendAsync(cancellationToken);

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
            var result = await response.Content.ReadAsAsync<FakePayload>(cancellationToken);
            Assert.True(result.Id == id);
        }

        [Fact]
        public async Task ShouldGetSuccessfulHttpResponseWithNonNullableDateTimeQueryString()
        {
            // arrange
            DateTime value = new DateTime(2010, 5, 11, 10, 30, 02, 03);
            const string requestUri = "/fake";
            CancellationToken cancellationToken = CancellationToken.None;
            var webHostBuilder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.Run(async context =>
                    {
                        DateTime parsedResult;
                        string r = context.Request.Query.FirstOrDefault(q => q.Key == nameof(value)).Value.FirstOrDefault();
                        DateTime.TryParse(r, out parsedResult);
                        context.Response.ContentType = "application/json";
                        var resultPayload = JsonConvert.SerializeObject(new FakePayload { NonNullDateTime = parsedResult });
                        await context.Response.WriteAsync(resultPayload, cancellationToken);
                    });
                });
            TestServer server = new TestServer(webHostBuilder);
            HttpClient client = server.CreateClient();
            IHttpClientVerbBuilder<HttpClient> builder = new HttpClientVerbBuilder<HttpClient>(client);

            // act
            HttpResponseMessage response = await client
                .Request()
                .Get(requestUri)
                .Query(nameof(value), value)
                .SendAsync(cancellationToken);

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
            var result = await response.Content.ReadAsAsync<FakePayload>(cancellationToken);
            Assert.True(result.NonNullDateTime.Year == value.Year);
            Assert.True(result.NonNullDateTime.Month == value.Month);
            Assert.True(result.NonNullDateTime.Day == value.Day);
            Assert.True(result.NonNullDateTime.Hour == value.Hour);
            Assert.True(result.NonNullDateTime.Minute == value.Minute);
            Assert.True(result.NonNullDateTime.Second == value.Second);
            Assert.True(result.NonNullDateTime.Millisecond == value.Millisecond);
        }

        [Fact]
        public async Task ShouldGetSuccessfulHttpResponseWithNonNullableDateTimeOffsetQueryString()
        {
            // arrange
            DateTime datetime = new DateTime(2010, 5, 11, 10, 30, 0, 03);
            DateTimeOffset value = new DateTimeOffset(datetime, TimeSpan.FromHours(8));
            const string requestUri = "/fake";
            CancellationToken cancellationToken = CancellationToken.None;
            var webHostBuilder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.Run(async context =>
                    {
                        DateTimeOffset parsedResult;
                        string r = context.Request.Query.FirstOrDefault(q => q.Key == nameof(value)).Value.FirstOrDefault();
                        DateTimeOffset.TryParse(r, out parsedResult);
                        context.Response.ContentType = "application/json";
                        var resultPayload = JsonConvert.SerializeObject(new FakePayload { DateTimeOffset = parsedResult });
                        await context.Response.WriteAsync(resultPayload, cancellationToken);
                    });
                });
            TestServer server = new TestServer(webHostBuilder);
            HttpClient client = server.CreateClient();

            // act
            HttpResponseMessage response = await client
                .Request()
                .Get(requestUri)
                .Query(nameof(value), value)
                .SendAsync(cancellationToken);

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
            var result = await response.Content.ReadAsAsync<FakePayload>(cancellationToken);
            Assert.True(result.DateTimeOffset.Year == value.Year);
            Assert.True(result.DateTimeOffset.Month == value.Month);
            Assert.True(result.DateTimeOffset.Day == value.Day);
            Assert.True(result.DateTimeOffset.Hour == value.Hour);
            Assert.True(result.DateTimeOffset.Minute == value.Minute);
            Assert.True(result.DateTimeOffset.Second == value.Second);
            Assert.True(result.DateTimeOffset.Millisecond == value.Millisecond);
            Assert.True(result.DateTimeOffset.Offset.TotalHours == value.Offset.TotalHours);
        }

        [Fact]
        public async Task ShouldGetSuccessfulHttpResponseWithMultipleQueryString()
        {
            // arrange
            const int id = 15;
            const string lol = "lol";
            const string requestUri = "/fake";
            CancellationToken cancellationToken = CancellationToken.None;
            var webHostBuilder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.Run(async context =>
                    {
                        int parsedResult = -1;
                        string r = context.Request.Query.FirstOrDefault(q => q.Key == nameof(id)).Value.FirstOrDefault();
                        string lolResult = context.Request.Query.FirstOrDefault(q => q.Key == nameof(lol)).Value.FirstOrDefault();
                        int.TryParse(r, out parsedResult);
                        context.Response.ContentType = "application/json";
                        var resultPayload = JsonConvert.SerializeObject(new FakePayload { Id = parsedResult, Note = lolResult });
                        await context.Response.WriteAsync(resultPayload, cancellationToken);
                    });
                });
            TestServer server = new TestServer(webHostBuilder);
            HttpClient client = server.CreateClient();

            // act
            HttpResponseMessage response = await client
                .Request()
                .Get(requestUri)
                .Query(nameof(id), id)
                .Query(nameof(lol), lol)
                .SendAsync(cancellationToken);

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
            var result = await response.Content.ReadAsAsync<FakePayload>(cancellationToken);
            Assert.True(result.Id == id);
            Assert.True(result.Note == lol);
        }

        [Fact]
        public async Task ShouldGetSuccessfulHttpResponseWithArrayQueryString()
        {
            // arrange
            const int id = 15;
            const string lol = "lol";
            const string requestUri = "/fake";
            string[] col = { "col1", "col2", "col3" };
            CancellationToken cancellationToken = CancellationToken.None;
            var webHostBuilder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.Run(async context =>
                    {
                        int parsedResult = -1;
                        string r = context.Request.Query.FirstOrDefault(q => q.Key == nameof(id)).Value.FirstOrDefault();
                        string lolResult = context.Request.Query.FirstOrDefault(q => q.Key == nameof(lol)).Value.FirstOrDefault();
                        string[] colResult = context.Request.Query.FirstOrDefault(q => q.Key == nameof(col)).Value;
                        int.TryParse(r, out parsedResult);
                        context.Response.ContentType = "application/json";
                        var resultPayload = JsonConvert.SerializeObject(new FakePayload { Id = parsedResult, Note = lolResult, SomeArray = colResult });
                        await context.Response.WriteAsync(resultPayload, cancellationToken);
                    });
                });
            TestServer server = new TestServer(webHostBuilder);
            HttpClient client = server.CreateClient();

            // act
            HttpResponseMessage response = await client
                .Request()
                .Get(requestUri)
                .Query(nameof(id), id)
                .Query(nameof(lol), lol)
                .QueryFromArray(nameof(col), col)
                .SendAsync(cancellationToken);

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
            var result = await response.Content.ReadAsAsync<FakePayload>(cancellationToken);
            Assert.True(result.Id == id);
            Assert.True(result.Note == lol);
            Assert.NotEmpty(result.SomeArray);
            Assert.True(result.SomeArray[0] == col[0]);
            Assert.True(result.SomeArray[1] == col[1]);
            Assert.True(result.SomeArray[2] == col[2]);
            Assert.True(result.SomeArray.Count() == 3);
        }

        [Fact]
        public async Task ShouldHttpMethodPost()
        {
            // arrange
            const string requestUri = "/fake";
            string method = null;
            CancellationToken cancellationToken = CancellationToken.None;
            var webHostBuilder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.Run(async context =>
                    {
                        method = context.Request.Method;
                        context.Response.StatusCode = 200;
                    });
                });
            TestServer server = new TestServer(webHostBuilder);
            HttpClient client = server.CreateClient();

            // act
            var response = await client
                .Request()
                .Post(requestUri)
                .SendAsync(cancellationToken);

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Matches("POST", method);
        }

        [Fact]
        public async Task ShouldPostSuccessfulHttpResponseWithPayload()
        {
            // arrange
            var datetime = new DateTime(2013, 9, 1, 10, 9, 0);
            const string requestUri = "/fake";
            var payload = new FakePayload
            {
                Id = 190,
                SomeArray = new []{ "lol1", "lol2", "lol3" },
                DateTimeOffset = new DateTimeOffset(datetime, TimeSpan.FromHours(8)),
                NonNullDateTime = datetime,
                Note = "Note1",
                Token = Guid.NewGuid(),
                NullableDateTime = null
            };
            FakePayload result = null;
            CancellationToken cancellationToken = CancellationToken.None;
            var webHostBuilder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.Run(async context =>
                    {
                        FakePayload content = null;
                        using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                        {
                            content = JsonConvert.DeserializeObject<FakePayload>(reader.ReadToEnd());
                        }
                        context.Response.ContentType = "application/json";
                        var serialized = JsonConvert.SerializeObject(content);
                        await context.Response.WriteAsync(serialized, cancellationToken);
                    });
                });
            TestServer server = new TestServer(webHostBuilder);
            HttpClient client = server.CreateClient();

            // act
            result = await client
                .Request()
                .Post(requestUri, new ObjectContent<FakePayload>(payload, new JsonMediaTypeFormatter()))
                .Query("id", payload.Id)
                .Query("note", payload.Note)
                .AsAsync<FakePayload>(cancellationToken);

            // assert
            Assert.NotNull(result);
            Assert.True(result.Id == payload.Id);
            Assert.True(result.Note == payload.Note);
            Assert.True(result.DateTimeOffset == payload.DateTimeOffset);
            Assert.True(result.SomeArray.Count() == 3);
            Assert.True(result.SomeArray[0] == "lol1");
            Assert.True(result.SomeArray[1] == "lol2");
            Assert.True(result.SomeArray[2] == "lol3");
        }

        [Fact]
        public async Task ShouldHttpMethodPut()
        {
            // arrange
            var datetime = new DateTime(2013, 9, 1, 10, 9, 0);
            const string requestUri = "/fake";
            string method = null;
            CancellationToken cancellationToken = CancellationToken.None;
            var webHostBuilder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.Run(async context =>
                    {
                        method = context.Request.Method;
                        context.Response.StatusCode = 200;
                    });
                });
            TestServer server = new TestServer(webHostBuilder);
            HttpClient client = server.CreateClient();

            // act
            var response = await client
                .Request()
                .Put(requestUri)
                .SendAsync(cancellationToken);

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Matches("PUT", method);
        }

        [Fact]
        public async Task ShouldPutSuccessfulHttpResponseWithPayload()
        {
            // arrange
            var datetime = new DateTime(2013, 9, 1, 10, 9, 0);
            const string requestUri = "/fake";
            var payload = new FakePayload
            {
                Id = 190,
                SomeArray = new[] { "lol1", "lol2", "lol3" },
                DateTimeOffset = new DateTimeOffset(datetime, TimeSpan.FromHours(8)),
                NonNullDateTime = datetime,
                Note = "Note1",
                Token = Guid.NewGuid(),
                NullableDateTime = null
            };
            FakePayload result = null;
            CancellationToken cancellationToken = CancellationToken.None;
            var webHostBuilder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.Run(async context =>
                    {
                        FakePayload content = null;
                        using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                        {
                            content = JsonConvert.DeserializeObject<FakePayload>(reader.ReadToEnd());
                        }
                        context.Response.ContentType = "application/json";
                        var serialized = JsonConvert.SerializeObject(content);
                        await context.Response.WriteAsync(serialized, cancellationToken);
                    });
                });
            TestServer server = new TestServer(webHostBuilder);
            HttpClient client = server.CreateClient();

            // act
            result = await client
                .Request()
                .Put(requestUri, new ObjectContent<FakePayload>(payload, new JsonMediaTypeFormatter()))
                .Query("id", payload.Id)
                .Query("note", payload.Note)
                .AsAsync<FakePayload>(cancellationToken);

            // assert
            Assert.NotNull(result);
            Assert.True(result.Id == payload.Id);
            Assert.True(result.Note == payload.Note);
            Assert.True(result.DateTimeOffset == payload.DateTimeOffset);
            Assert.True(result.SomeArray.Length == 3);
            Assert.True(result.SomeArray[0] == "lol1");
            Assert.True(result.SomeArray[1] == "lol2");
            Assert.True(result.SomeArray[2] == "lol3");
        }

        [Fact]
        public async Task ShouldHttpMethodDelete()
        {
            // arrange
            string requestUri = "/fake";
            string method = null;
            CancellationToken cancellationToken = CancellationToken.None;
            var webHostBuilder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.Run(async context =>
                    {
                        method = context.Request.Method;
                        context.Response.StatusCode = 200;
                    });
                });
            TestServer server = new TestServer(webHostBuilder);
            HttpClient client = server.CreateClient();

            // act
            HttpResponseMessage response = await client
                .Request()
                .Delete(requestUri)
                .SendAsync(cancellationToken);

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Matches("DELETE", method);
        }

        [Fact]
        public async Task ShouldDeleteSuccessfulHttpResponseWithMultipleQueryString()
        {
            // arrange
            const int id = 15;
            const string lol = "lol";
            const string requestUri = "/fake";
            CancellationToken cancellationToken = CancellationToken.None;
            var webHostBuilder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.Run(async context =>
                    {
                        int parsedResult = -1;
                        string r = context.Request.Query.FirstOrDefault(q => q.Key == nameof(id)).Value.FirstOrDefault();
                        string lolResult = context.Request.Query.FirstOrDefault(q => q.Key == nameof(lol)).Value.FirstOrDefault();
                        int.TryParse(r, out parsedResult);
                        context.Response.ContentType = "application/json";
                        var resultPayload = JsonConvert.SerializeObject(new FakePayload { Id = parsedResult, Note = lolResult });
                        await context.Response.WriteAsync(resultPayload, cancellationToken);
                    });
                });
            TestServer server = new TestServer(webHostBuilder);
            HttpClient client = server.CreateClient();

            // act
            HttpResponseMessage response = await client
                .Request()
                .Delete(requestUri)
                .Query(nameof(id), id)
                .Query(nameof(lol), lol)
                .SendAsync(cancellationToken);

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
            var result = await response.Content.ReadAsAsync<FakePayload>(cancellationToken);
            Assert.True(result.Id == id);
            Assert.True(result.Note == lol);
        }
    }
}
