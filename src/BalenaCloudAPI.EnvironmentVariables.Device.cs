using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace BalenaSDK
{
    public partial class BalenaCloudAPI
    {
        private string DeviceEndpoint = "device_environment_variable";
        public dynamic GetDeviceVariable(int deviceId, string name)
        {
            name = name.ToUpper();
            string uri = $"{DeviceEndpoint}?$filter=device eq {deviceId} and name eq '{name}'";
            using (HttpResponseMessage res = API_HTTPCLIENT.GetAsync(uri).GetAwaiter().GetResult())
            {
                if (res.IsSuccessStatusCode)
                {
                    string data = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (res.IsSuccessStatusCode)
                    {
                        return ((dynamic)JObject.Parse(data)).d[0];
                    }
                }
            }
            return null;
        }
        public dynamic ClearDeviceVariable(int deviceId, string name)
        {
            return SetDeviceVariable(deviceId,name,"");
        }
        public dynamic SetDeviceVariable(int deviceId, string name, string value)
        {
            string id;
            try
            {
                id = GetDeviceVariable(deviceId, name).id;
            }
            catch
            {
                id = null;
            }
            Hashtable ht = new Hashtable();

            if (id == null)
            {   //CREATE
                ht.Add("device", deviceId);
                ht.Add("name", name);
                ht.Add("value", value);
                Console.WriteLine($"Creating new key {name}={value}");
                using (HttpResponseMessage res = API_HTTPCLIENT.PostAsJsonAsync($"{DeviceEndpoint}", ht).GetAwaiter().GetResult())
                {
                    return res.StatusCode;
                }
            }
            else
            {   //UPDATE
                
                ht.Add("name", name);
                ht.Add("value", value);
                
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(ht),Encoding.UTF8,"application/json");

                using (HttpResponseMessage res = API_HTTPCLIENT.PatchAsync($"{DeviceEndpoint}({id})", httpContent).GetAwaiter().GetResult())
                {
                    return res.StatusCode;
                }
            }
            return null;
        }
        
        public HttpStatusCode DeleteDeviceVariable(int deviceId, string name)
        {

            string id;
            try
            {

                id = GetDeviceVariable(deviceId, name).id;
                using (HttpResponseMessage res = API_HTTPCLIENT.DeleteAsync($"{DeviceEndpoint}({id})").GetAwaiter().GetResult())
                {
                    return res.StatusCode;
                }
            }
            catch
            {
                return HttpStatusCode.NotFound;
            }

        }
    }
}
