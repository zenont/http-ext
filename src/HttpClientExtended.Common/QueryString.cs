using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientExtended.Common
{
    public class QueryString:List<KeyValuePair<string, string>>
    {
        protected virtual bool TryParseFromDate(object value, out string convertedValue)
        {
            convertedValue = null;
            
            if(value is DateTimeOffset)
            {
                DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
                convertedValue = dateTimeOffset.ToString("o");
                return true;
            }
            else if (value is DateTime)
            {
                DateTime dateTime = (DateTime)value;
                convertedValue = dateTime.ToString("o");
                return true;
            }
            return false;
        }

        protected virtual string ConvertValueToString(object value)
        {
            if (value == null) return null;

            string convertedValue;
            if(TryParseFromDate(value, out convertedValue))
            {
                return convertedValue;
            }

            return Convert.ToString(value)?.Trim();
        }

        public virtual void Add(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            string converted = ConvertValueToString(value);
            if (string.IsNullOrWhiteSpace(converted))
            {
                return;
            }
            Add(new KeyValuePair<string, string>(key, converted));
        }

        public virtual async Task<Uri> AsUriAsync(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl)) throw new ArgumentNullException(nameof(baseUrl));

            const UriKind uriKind = UriKind.RelativeOrAbsolute;

            Uri uri = !this.Any()
                ? new Uri(baseUrl, uriKind)
                : new Uri($"{baseUrl}?{await AsUrlAsync()}",
                    uriKind);

            return uri;
        }

        public virtual async Task<string> AsUrlAsync()
        {
            string query;
            using (var content = new FormUrlEncodedContent(this))
            {
                query = await content.ReadAsStringAsync();
            }

            return query;
        }
    }
}
