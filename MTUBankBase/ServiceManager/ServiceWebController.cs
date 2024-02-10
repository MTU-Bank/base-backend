using EmbedIO.Routing;
using EmbedIO;
using EmbedIO.WebApi;
using Newtonsoft.Json;
using MTUBankBase.ServiceManager.Models;

namespace MTUBankBase.ServiceManager
{
    internal class ServiceWebController : WebApiController
    {
        public static async Task AsJSON(IHttpContext context, object? data)
        {
            if (data is null) return;

            context.Response.ContentType = "application/json";
            using var text = context.OpenResponseText();
            await text.WriteAsync(JsonConvert.SerializeObject(data)).ConfigureAwait(false);
        }

        [Route(HttpVerbs.Post, "/registerService")]
        public async Task<object> RegisterNewService([JsonData] RegisterRequest registerRequest)
        {
            // confirm token is correct
            if (!registerRequest.PairToken.Equals(ServiceRegistry.Instance)) throw new HttpException(403, "Pair token does not match.");

            // create stub service
            var service = new Service();
            // register the service
            var result = await ServiceRegistry.Instance.RegisterServiceAsync(service);

            if (!result) throw new HttpException(500, "Unknown service pair error.");
            return result;
        }
    }
}