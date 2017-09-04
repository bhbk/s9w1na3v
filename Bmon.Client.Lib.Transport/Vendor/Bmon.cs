using Bmon.Client.Lib.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bmon.Client.Lib.Transport.Vendor
{
    public class Bmon
    {
        private Uri server;

        public Bmon(Uri host)
        {
            server = host;
        }

        public async Task<HttpResponseMessage> PostAsync(string path, BmonPostTrendMultiple trends)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = server;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsJsonAsync(path, trends).Result;

                return await Task.FromResult(response);
            }
        }
    }
}
