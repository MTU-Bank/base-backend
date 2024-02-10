using EmbedIO.Routing;
using EmbedIO;
using EmbedIO.WebApi;
using Newtonsoft.Json;
using MTUBankBase.ServiceManager.Models;

namespace MTUBankBase.ServiceManager
{
    internal class ServiceWebController : WebApiController
    {
        [Route(HttpVerbs.Post, "/registerService")]
        public async Task<object> RegisterNewService([JsonData] RegisterRequest registerRequest)
        {
            // confirm token is correct
            if (!registerRequest.PairToken.Equals(ServiceRegistry.Instance)) throw new HttpException(403, "Pair token does not match.");

            // create stub service
            var service = new Service() { BaseUrl = registerRequest.BaseUrl, Name = registerRequest.Name };
            // register the service
            var result = await ServiceRegistry.Instance.RegisterServiceAsync(service);

            if (!result) throw new HttpException(500, "Unknown service pair error.");
            return result;
        }

        [Route(HttpVerbs.Post, "/unregisterService")]
        public async Task<object> UnregisterService([JsonData] UnregisterRequest unregisterRequest)
        {
            // confirm token is correct
            if (!unregisterRequest.PairToken.Equals(ServiceRegistry.Instance)) throw new HttpException(403, "Pair token does not match.");

            // lookup the service
            var service = ServiceRegistry.Instance.localServices.FirstOrDefault((z) => z.Name.Equals(unregisterRequest.Name));
            if (service is null) return true;

            // query the unregister
            var unregQuery = await ServiceRegistry.Instance.UnregisterServiceAsync(service);
            return unregQuery;
        }
    }
}