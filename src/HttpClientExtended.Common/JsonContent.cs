using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HttpClientExtended.Common
{
    public class JsonContent<T>: StringContent
    {
        public JsonContent(T content):base(JsonConvert.SerializeObject(content)) { }
    }
}
