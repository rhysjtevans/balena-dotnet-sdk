using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;



namespace BalenaSDK
{
    public partial class BalenaCloudAPI
    {
        public async Task<int> GetDeviceIdAsync(string deviceUUID)
        {
            return (int)(await GetDeviceAsync(deviceUUID)).id;
        }


        public async Task<string> GetDeviceAppIdAsync(int deviceId)
        {
            return (string)(await GetDeviceAsync(deviceId).GetAwaiter().GetResult()).belongs_to__application.__id;
        }

        public async Task<dynamic> GetDeviceAsync(int deviceId)
        {
            using (HttpResponseMessage res = await API_HTTPCLIENT.GetAsync($"device({deviceId})"))
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
        public async Task<dynamic> GetDeviceAsync(string deviceUUID)
        {
            using (HttpResponseMessage res = await API_HTTPCLIENT.GetAsync($"device?$filter=uuid%20eq%20'{deviceUUID}'"))
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
