using System;
using System.Text;
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
    class Client
    {
        public HttpClient client { get; set; }
        private string baseUrl_ = "https://localhost:44333/api/Files";

        Client()
        {
            client = new HttpClient();
        }
        public async Task<HttpResponseMessage> SendFile(string fileSpec)
        {
            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            byte[] data = File.ReadAllBytes(fileSpec);
            ByteArrayContent bytes = new ByteArrayContent(data);
            string fileName = Path.GetFileName(fileSpec);
            multiContent.Add(bytes, "files", fileName);
            multiContent.Add(new StringContent("TestName"), "name");
            multiContent.Add(new StringContent("TestDescription"), "description");
            return await client.PostAsync(baseUrl_, multiContent);
        }
        static void Main(string[] args)
        {
            Console.ReadKey();
            Client cl = new Client();
            Task<HttpResponseMessage> tup=cl.SendFile(args[0]);
            Console.Write(tup.Result);
            Console.ReadKey();
        }
    }
}
