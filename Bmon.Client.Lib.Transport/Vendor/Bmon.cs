using Bmon.Client.Lib.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bmon.Client.Lib.Transport.Vendor
{
    public class Bmon
    {
        private Uri server;
        private StringBuilder stdout;

        public StringBuilder Stdout
        {
            get
            {
                return stdout;
            }
        }

        public Bmon(Uri host)
        {
            server = host;
            stdout = new StringBuilder();
        }

        public async Task<HttpResponseMessage> PostAsync(string path, MultipleMomentsTuples trends)
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
