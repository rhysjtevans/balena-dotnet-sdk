using System;
using System.Collections;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Carvis.Coyote.Services.CloudAPI.Common
{
    public class Balena
    {
      
        private static String API_URI {  get { return Environment.GetEnvironmentVariable("BALENA_API_URI"); } }
        private static String API_TOKEN {  get { return Environment.GetEnvironmentVariable("BALENA_API_TOKEN"); } }

        private static HttpClient HttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Balena.API_TOKEN);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            client.BaseAddress = new Uri(API_URI);
            return client;
        }


        public static async Task<string> GetDeviceIdAsync(string UUID)
        {
            return (await Balena.GetDeviceAsync(UUID).GetAwaiter().GetResult()).id;
        }


        public static async Task<string> GetDeviceAppIdAsync(string UUID)
        {
            return (await Balena.GetDeviceAsync(UUID).GetAwaiter().GetResult()).belongs_to__application.__id;
        }


        public static async Task<dynamic> GetDeviceAsync(string UUID)
        {
            using (HttpClient client = Balena.HttpClient())
            {
                //                curl - X GET "https://api.balena-cloud.com/v4/device(<ID>)" \
                //-H "Content-Type: application/json" \
                //-H "Authorization: Bearer <AUTH_TOKEN>"$"{APIURI}/v4/device({UUID})"
                using (HttpResponseMessage res = await client.GetAsync($"device?$filter=uuid%20eq%20'{UUID}'"))
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        if (data != null)
                        {
                            if (res.IsSuccessStatusCode && JObject.Parse(data).Count > 0)
                            {
                                return ((dynamic)JObject.Parse(data)).d[0].id;
                            }
                        }
                        return null;
                    }

                }
            }
        }


        public static async Task<bool> SetTenantID(String DeviceUUID,String TenantId)
        {
            using (HttpClient client = Balena.HttpClient())
            {
                Hashtable ht = new Hashtable();
                ht.Add("device", GetDeviceIdAsync(DeviceUUID, client).GetAwaiter().GetResult());
                ht.Add("name", "TENANT_ID");
                ht.Add("value",TenantId);

                using (HttpResponseMessage res = await client.PostAsJsonAsync("device_environment_variable", ht))
                {
                    if (res.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public static async Task<dynamic> GetEnvironmentVariablesAsync(string DeviceUUID) {
            using (HttpClient client = Balena.HttpClient())
            {
                String deviceID = GetDeviceIdAsync(DeviceUUID).GetAwaiter().GetResult();
                using (HttpResponseMessage res = await client.GetAsync($"device_environment_variable?$filter=device eq {deviceID}")){
                    if (res.IsSuccessStatusCode) {
                        string data = await res.Content.ReadAsStringAsync();
                        if (data != null)
                        {
                            if (res.IsSuccessStatusCode)  // && JObject.Parse(data).Count > 0)
                            {
                                return ((dynamic)JObject.Parse(data)).d;
                                //return ((dynamic)JObject.Parse(data)).d[0].id;
                            }
                        }
                    }

                }
            }
            return null;
        }

        public static string GetEnvironmentVariable(string DeviceUUID, string VarName)
        {

            var vars = GetEnvironmentVariablesAsync(DeviceUUID).GetAwaiter().GetResult();
            foreach(var variable in vars)
            {
                Console.WriteLine("{0}:{1}", variable.name, variable.value);
                if (((string)variable.name).Equals(VarName))
                {
                    return (string)variable.value;
                }
            }
            return null;
        }

    }
}
