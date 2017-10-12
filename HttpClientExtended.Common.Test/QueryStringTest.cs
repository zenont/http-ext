using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HttpClientExtended.Common.Test
{
    public class QueryStringTest
    {
        [Theory]
        [InlineData("someKey", true, "True")]
        [InlineData("someKey", 10, "10")]
        [InlineData("someKey", 10.103, "10.103")]
        [InlineData("someKey", "10.103", "10.103")]
        public void ShouldAddKeyValueStrings(string key, object value, string expected)
        {
            // arrange
            QueryString queryString = new QueryString();

            // act
            queryString.Add(key, value?.ToString());

            // assert
            Assert.True(queryString.Single().Value == expected);
        }

        [Theory]
        [InlineData("someKey", new int[] { 1, 2, 3, 4 }, new string[] { "1", "2", "3", "4" })]
        public void SHouldAddArray(string key, int[] values, string[] expected)
        {
            // arrange
            QueryString queryString = new QueryString();
            foreach(var v in values)
            {
                queryString.Add(key, v.ToString());
            }
            KeyValuePair<string, string>[] parsedValue = queryString.Where(x => x.Key == key).ToArray();


            // assert
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.True(parsedValue[i].Value == expected[i]);
            }
        }

        [Fact]
        public void ShouldAddListAndIgnoreNullItem()
        {
            // arrange
            const string key = "someKey";
            List<int?> values = new List<int?> { 1, 2, null, 4 };
            var expected = new string[] { "1", "2", "4" };

            // act
            QueryString queryString = new QueryString();
            foreach (var v in values)
            {
                queryString.Add(key, v?.ToString());
            }
            KeyValuePair<string, string>[] parsedValue = queryString.Where(x => x.Key == key).ToArray();

            // assert
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.True(parsedValue[i].Value == expected[i]);
            }
        }

        [Fact]
        public void ShouldAddListWithDatetimes()
        {
            // arrange
            const string key = "someKey";
            var value1 = new DateTime(2010, 1, 1, 10, 0, 0);
            var value2 = new DateTime(2010, 2, 1, 10, 0, 0);
            var value3 = new DateTime(2010, 3, 1, 10, 0, 0);

            var values = new List<DateTime> { value1, value2, value3 };
            var expected = new string[] {value1.ToString("o"), value2.ToString("o"), value3.ToString("o") };

            // act
            QueryString queryString = new QueryString();
            foreach(var v in values)
            {
                queryString.Add(key, v);
            }
            KeyValuePair<string, string>[] parsedValue = queryString.Where(x => x.Key == key).ToArray();

            // assert
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.True(parsedValue[i].Value == expected[i]);
            }
        }

        [Fact]
        public void ShouldAddListWithDatetimesCastedToParams()
        {
            // arrange
            const string key = "someKey";
            var value1 = new DateTime(2010, 1, 1, 10, 0, 0);
            var value2 = new DateTime(2010, 2, 1, 10, 0, 0);
            var value3 = new DateTime(2010, 3, 1, 10, 0, 0);

            var values = new List<DateTime> { value1, value2, value3 };
            var expected = new string[] { value1.ToString("o"), value2.ToString("o"), value3.ToString("o") };

            // act
            QueryString queryString = new QueryString();
            foreach(var v in values)
            {
                queryString.Add(key, v);
            }
            
            KeyValuePair<string, string>[] parsedValue = queryString.Where(x => x.Key == key).ToArray();

            // assert
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.True(parsedValue[i].Value == expected[i]);
            }
        }

        [Fact]
        public void ShouldAddArrayWithDatetimes()
        {
            // arrange
            const string key = "someKey";
            var value1 = new DateTime(2010, 1, 1, 10, 0, 0);
            var value2 = new DateTime(2010, 2, 1, 10, 0, 0);
            var value3 = new DateTime(2010, 3, 1, 10, 0, 0);

            var values = new DateTime[] { value1, value2, value3 };
            var expected = new string[] { value1.ToString("o"), value2.ToString("o"), value3.ToString("o") };

            // act
            QueryString queryString = new QueryString();
            foreach (var v in values)
            {
                queryString.Add(key, v);
            }
            KeyValuePair<string, string>[] parsedValue = queryString.Where(x => x.Key == key).ToArray();

            // assert
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.True(parsedValue[i].Value == expected[i]);
            }
        }

        [Fact]
        public void ShouldAddArrayWithDatetimesCastedAsParams()
        {
            // arrange
            const string key = "someKey";
            var value1 = new DateTime(2010, 1, 1, 10, 0, 0);
            var value2 = new DateTime(2010, 2, 1, 10, 0, 0);
            var value3 = new DateTime(2010, 3, 1, 10, 0, 0);

            var values = new DateTime[] { value1, value2, value3 };
            var expected = new string[] { value1.ToString("o"), value2.ToString("o"), value3.ToString("o") };

            // act
            QueryString queryString = new QueryString();
            foreach(var v in values)
            {
                queryString.Add(key, v);
            }
            KeyValuePair<string, string>[] parsedValue = queryString.Where(x => x.Key == key).ToArray();

            // assert
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.True(parsedValue[i].Value == expected[i]);
            }
        }

        [Fact]
        public void ShouldAddListWithNullableDatetimes()
        {
            // arrange
            const string key = "someKey";
            var value1 = new DateTime(2010, 1, 1, 10, 0, 0);
            DateTime? value2 = null;
            var value3 = new DateTime(2010, 3, 1, 10, 0, 0);

            List<DateTime?> values = new List<DateTime?> { value1, value2, value3 };
            var expected = new string[] { value1.ToString("o"), value3.ToString("o") };

            // act
            QueryString queryString = new QueryString();
            foreach (var v in values)
            {
                queryString.Add(key, v);
            }
            KeyValuePair<string, string>[] parsedValue = queryString.Where(x => x.Key == key).ToArray();

            // assert
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.True(parsedValue[i].Value == expected[i]);
            }
        }

        [Fact]
        public void ShouldAddDateTime()
        {
            // arrange
            const string key = "key1";
            var value1 = new DateTime(2010, 1, 1, 10, 0, 0);
            QueryString queryString = new QueryString();

            // act
            queryString.Add(key, value1);

            // assert
            Assert.True(queryString.Single().Value == value1.ToString("o"));
        }

        [Fact]
        public void ShouldAddNullableDateTime()
        {
            // arrange
            const string key = "key1";
            DateTime? value1 = null;
            QueryString queryString = new QueryString();

            // act
            queryString.Add(key, value1?.ToString());

            // assert
            Assert.Empty(queryString);
        }

        [Fact]
        public async Task ShouldConvertValuesToUrl()
        {
            // arrange
            QueryString queryString = new QueryString();

            // act
            queryString.Add("key1", "value1");
            queryString.Add("key2", "value2");
            string url = await queryString.AsUrlAsync();

            // assert
            Assert.True(url == "key1=value1&key2=value2");
        }

        [Fact]
        public async Task ShouldConvertArrayToUrl()
        {
            // arrange
            QueryString queryString = new QueryString();
            var values = new int[] { 1, 2, 3 };

            // act
            foreach (var v in values)
            {
                queryString.Add("key1", v.ToString());
            }
            queryString.Add("key2", "value2");
            string url = await queryString.AsUrlAsync();

            // assert
            Assert.True(url == "key1=1&key1=2&key1=3&key2=value2");
        }

        [Fact]
        public async Task ShouldConvertEmptyCollectionEdgeCaseToUrl()
        {
            // arrange
            QueryString queryString = new QueryString();

            // act
            string url = await queryString.AsUrlAsync();

            // assert
            Assert.True(url == "");
        }

        [Fact]
        public async Task ShouldConvertNullValueEdgeCaseToUrl()
        {
            // arrange
            QueryString queryString = new QueryString();

            // act
            queryString.Add("key1", "");
            string url = await queryString.AsUrlAsync();

            // assert
            Assert.True(url == "");
        }

        [Fact]
        public async Task ShouldConvertArrayToUri()
        {
            // arrange
            const string baseUrl = "http://localhost";
            QueryString queryString = new QueryString();
            var values = new int[] { 1, 2, 3 };

            // act
            foreach(var v in values)
            {
                queryString.Add("key1", v.ToString());
            }
            queryString.Add("key2", "value2");
            Uri uri = await queryString.AsUriAsync(baseUrl);

            // assert
            Assert.NotNull(uri);
            Assert.True(uri.ToString() == "http://localhost/?key1=1&key1=2&key1=3&key2=value2");
        }

        [Fact]
        public void ShouldThrowWhenKeyIsNull()
        {
            // arrange
            QueryString queryString = new QueryString();

            // act
            Action action = () => queryString.Add("", "value2");

            // assert
            action.ShouldThrow<ArgumentNullException>();
        }
    }
}
