using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase.ServiceManager
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ServiceType
    {
        [EnumMember(Value = "Auth")]
        Auth
    }

    public class Service
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string BaseUrl { get; set; }

        public ServiceType ServiceType { get; set; }

        public async Task<bool> IsOnlineAsync()
        {
            try
            {
                using (var http = new HttpClient())
                {
                    var resp = await http.GetAsync($"{BaseUrl}/getStatus");
                    return resp.IsSuccessStatusCode;
                }
            } catch { return false; }
        }

        public async Task<bool> PopulateServiceInfo()
        {
            try
            {
                using (var http = new HttpClient())
                {
                    var resp = await http.GetAsync($"{BaseUrl}/getServiceInfo");
                    var jsonStr = await resp.Content.ReadAsStringAsync();
                    var serviceInfo = JsonConvert.DeserializeObject<Service>(jsonStr);
                    this.CopyFrom(serviceInfo);
                    return true;
                }
            } catch { return false; }
        }

        public async Task<bool> MessageDisconnect()
        {
            try
            {
                using (var http = new HttpClient())
                {
                    var resp = await http.GetAsync($"{BaseUrl}/disconnectService");
                    return resp.IsSuccessStatusCode;
                }
            }
            catch { return false; }
        }

        private void CopyFrom(Service service)
        {
            PropertyInfo[] destinationProperties = this.GetType().GetProperties();
            foreach (PropertyInfo destinationPi in destinationProperties)
            {
                PropertyInfo sourcePi = service.GetType().GetProperty(destinationPi.Name);
                if (sourcePi.CanWrite) destinationPi.SetValue(this, sourcePi.GetValue(service, null), null);
            }
        }
    }
}
