using System.Net.Http;
using Newtonsoft.Json;

namespace HttpClientExt.Common
{
	public class JsonContent<T> : StringContent
	{
		public JsonContent(T content) : base(JsonConvert.SerializeObject(content)) { }
	}
}
