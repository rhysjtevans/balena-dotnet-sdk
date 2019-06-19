using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;



namespace BalenaSDK
{
    public partial class BalenaCloudAPI
    {
        public async Task<int> GetApplicationIdAsync(string applicationUUID)
        {
            return (int)(await GetApplicationAsync(applicationUUID)).id;
        }


        public async Task<string> GetApplicationAppIdAsync(int applicationId)
        {
            return (string)(await GetApplicationAsync(applicationId).GetAwaiter().GetResult()).belongs_to__application.__id;
        }

        public async Task<dynamic> GetApplicationAsync(int applicationId)
        {
            using (HttpResponseMessage res = await API_HTTPCLIENT.GetAsync($"application({applicationId})"))
            {
                using (HttpContent content = res.Content)
                {
                    string data = await content.ReadAsStringAsync();
                    if (data != null)
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            return ((dynamic)JObject.Parse(data)).d[0];
                        }
                    }
                    return null;
                }
            }
        }
        public async Task<dynamic> GetApplicationAsync(string applicationUUID)
        {
            using (HttpResponseMessage res = await API_HTTPCLIENT.GetAsync($"application?$filter=app_name eq '{applicationUUID}'"))
            {
                using (HttpContent content = res.Content)
                {
                    string data = await content.ReadAsStringAsync();
                    if (data != null)
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            return ((dynamic)JObject.Parse(data)).d[0];
                        }
                    }
                    return null;
                }
            }
        
        }

    }
}
