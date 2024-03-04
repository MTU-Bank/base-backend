using EmbedIO;
using MTUBankBase.Auth.Models;
using MTUBankBase.Database.Models;
using MTUBankBase.ServiceManager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase.Auth
{
    public class AuthValidator
    {
        public static HttpClient validatorHttp = new HttpClient();

        public static async Task<Token?> GetCurrentToken(IHttpContext context)
        {
            // get token
            string? auth = context.Request.Headers.Get("Authorization");

            if (auth is null) return null;

            // question the server
            var service = ServiceRegistry.GetAssociatedService(ServiceType.Auth);
            var internalUrl = $"{service.BaseUrl}/api/internal/checkToken";
            var msg = new CheckTokenRequest() { Token = auth };
            var json = new StringContent(JsonConvert.SerializeObject(msg), Encoding.UTF8, "application/json");

            var resp = await validatorHttp.PostAsync(internalUrl, json);
            var respText = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Token?>(respText);
        }
    }
}
