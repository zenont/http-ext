using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HttpClientExt.Common
{
	public class JsonContent : StringContent
	{
		public JsonContent(object content) : this(JsonConvert.SerializeObject(content)) 
        {
            Content = content;
        }

        private JsonContent(string content) : base(content)
        {
            SerializedContent = content;
        }

        protected object Content { get; private set; }

        protected string SerializedContent { get; private set; }
	}
}
