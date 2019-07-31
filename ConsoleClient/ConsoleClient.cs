using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;  // must install Newtonsoft package from Nuget
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ConsoleClient
{
    class ConsoleClient
    {
        public HttpClient client { get; set; }

        private string baseUrl_;

        ConsoleClient(string url)
        {
            baseUrl_ = url;
            client = new HttpClient();
        }
        public async Task<HttpResponseMessage> SendFile(string fileSpec,string name,string description)
        {
            MultipartFormDataContent multiContent = new MultipartFormDataContent();

            byte[] data = File.ReadAllBytes(fileSpec);
            ByteArrayContent bytes = new ByteArrayContent(data);
            string fileName = Path.GetFileName(fileSpec);
            multiContent.Add(bytes, "files", fileName);
            Dictionary<string, object> postReq = new Dictionary<string, object>();
            postReq["name"] = name;
            postReq["description"] = description;
            postReq["File"] = multiContent;
            HttpContent content = new FormUrlEncodedContent(postReq);
            //return await client.PostAsync(baseUrl_, postReq);
        }
    }
}
