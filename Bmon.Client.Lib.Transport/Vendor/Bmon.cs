using Bmon.Client.Lib.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bmon.Client.Lib.Transport.Vendor
{
    public class Bmon
    {
        private Uri Server;
        private string StoreKey;
        private StringBuilder StandardOutput;

        public StringBuilder Output
        {
            get
            {
                return StandardOutput;
            }
        }

        public Bmon(Uri host)
        {
            Server = host;
            StandardOutput = new StringBuilder();
        }

        public Bmon(Uri host, string key)
        {
            Server = host;
            StoreKey = key;
            StandardOutput = new StringBuilder();
        }

        public async Task<HttpResponseMessage> PostAsync(string path, MomentArrays moments)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = Server;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsJsonAsync(path, moments).Result;

                return await Task.FromResult(response);
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string path, byte[] file)
        {
            using (var client = new HttpClient())
            {
                HttpContent content = new ByteArrayContent(file);
                MultipartFormDataContent mime = new MultipartFormDataContent();

                mime.Add(content);

                client.BaseAddress = Server;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

                HttpResponseMessage response = client.PostAsync(path, mime).Result;

                return await Task.FromResult(response);
            }
        }
    }
}
