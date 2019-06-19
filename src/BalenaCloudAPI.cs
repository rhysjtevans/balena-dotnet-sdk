using System;
using System.Collections;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BalenaSDK
{
    public partial class BalenaCloudAPI
    {   
        public String API_URI { get; set; } 
        public String API_TOKEN { get; set; }
        public HttpClient API_HTTPCLIENT { get; set; }

        public BalenaCloudAPI(String API_URI, String API_TOKEN)
        {
            this.API_URI = API_URI;
            this.API_TOKEN = API_TOKEN;
            this.API_HTTPCLIENT = this.HttpClient();
        }
        private HttpClient HttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.API_TOKEN);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            client.BaseAddress = new Uri(this.API_URI);
            return client;
        }
    }
}
