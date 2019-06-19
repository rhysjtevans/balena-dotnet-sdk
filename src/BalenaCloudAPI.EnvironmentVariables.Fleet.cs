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
        private string ApplicationEndpoint = "application_environment_variable";
        public dynamic GetApplicationVariable(int applicationId, string name)
        {
            name = name.ToUpper();
            string uri = $"{ApplicationEndpoint}?$filter=application eq {applicationId} and name eq '{name}'";
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
        public dynamic ClearApplicationVariable(int deviceId, string name)
        {
            return SetDeviceVariable(deviceId,name,"");
        }
        public dynamic SetApplicationVariable(int applicationId, string name, string value)
        {
            string id;
            try
            {
                id = GetApplicationVariable(applicationId, name).id;
            }
            catch
            {
                id = null;
            }
            Hashtable ht = new Hashtable();

            if (id == null)
            {   //CREATE
                ht.Add("application", applicationId);
                ht.Add("name", name);
                ht.Add("value", value);
                Console.WriteLine($"Creating new key {name}={value}");
                using (HttpResponseMessage res = API_HTTPCLIENT.PostAsJsonAsync($"{ApplicationEndpoint}", ht).GetAwaiter().GetResult())
                {
                    return res.StatusCode;
                }
            }
            else
            {   //UPDATE
                
                ht.Add("name", name);
                ht.Add("value", value);
                
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(ht),Encoding.UTF8,"application/json");

                using (HttpResponseMessage res = API_HTTPCLIENT.PatchAsync($"{ApplicationEndpoint}({id})", httpContent).GetAwaiter().GetResult())
                {
                    return res.StatusCode;
                }
            }
            return null;
        }
        
        public HttpStatusCode DeleteApplicationVariable(int applicationId, string name)
        {

            string id;
            try
            {

                id = GetApplicationVariable(applicationId, name).id;
                using (HttpResponseMessage res = API_HTTPCLIENT.DeleteAsync($"{ApplicationEndpoint}({id})").GetAwaiter().GetResult())
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
