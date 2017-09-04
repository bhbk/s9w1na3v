using Bmon.Client.Lib.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bmon.Client.Lib.Transport.Vendor
{
    public class Bmon
    {
        private Uri address;

        public Bmon(Uri server)
        {
            address = server;
        }

        public async Task<HttpResponseMessage> PostAsync(BmonTrendsForPost trends)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = address;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsJsonAsync("/readingdb/reading/store/", trends).Result;

                return await Task.FromResult(response);
            }
        }
    }
}
