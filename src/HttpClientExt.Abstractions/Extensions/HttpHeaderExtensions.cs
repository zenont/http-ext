using System.Net.Http.Headers;

namespace HttpClientExtended.Abstractions.Extensions
{
    public static class HttpHeaderExtensions
    {
        public static void Add(this HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> header, string mediaType)
        {
            header.Add(new MediaTypeWithQualityHeaderValue(mediaType));
        }

        public static void Add(this HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> header, string mediaType, double quality)
        {
            header.Add(new MediaTypeWithQualityHeaderValue(mediaType, quality));
        }

        public static void Add(this HttpHeaderValueCollection<StringWithQualityHeaderValue> header, string value)
        {
            header.Add(new StringWithQualityHeaderValue(value));
        }

        public static void Add(this HttpHeaderValueCollection<StringWithQualityHeaderValue> header, string value, double quality)
        {
            header.Add(new StringWithQualityHeaderValue(value, quality));
        }
    }
}
