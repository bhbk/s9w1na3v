using Bmon.Client.Lib.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
    }
}
