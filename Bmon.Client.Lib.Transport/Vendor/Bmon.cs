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
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsJsonAsync(path, moments).Result;

                return await Task.FromResult(response);
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string path, byte[] file)
        {
            HttpContent content = new ByteArrayContent(file);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            using (var form = new MultipartFormDataContent())
            {
                form.Add(content);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = Server;
                    HttpResponseMessage response = await client.PostAsync(path, form);

                    return await Task.FromResult(response);
                }
            }
        }
    }
}
