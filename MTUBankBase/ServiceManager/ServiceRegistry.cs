using EmbedIO;
using EmbedIO.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase.ServiceManager
{
    public class ServiceRegistry
    {
        public static ServiceRegistry Instance;

        public event EventHandler<Service> NewServiceAdded;

        public List<Service> localServices = new List<Service>();
        public string PairToken = Environment.GetEnvironmentVariable("PairToken");

        public void InitListener(string baseUrl, CancellationToken cancellationToken = default)
        {
            Instance = this;

            var server = new WebServer(o => o
                    .WithUrlPrefix(baseUrl)
                    .WithMode(HttpListenerMode.EmbedIO))
                .WithWebApi("/", m => m
                    .WithController<ServiceWebController>());

            server.Start(cancellationToken);
        }

        public async Task<bool> RegisterServiceAsync(Service service)
        {
            // evaluate whether or not service is online
            var onlineStatus = await service.IsOnlineAsync();
            if (!onlineStatus) return false;

            // get additional info about the service
            var populateResult = await service.PopulateServiceInfo();
            if (!populateResult) return false;

            // add the service to the registry
            localServices.Add(service);

            // invoke event handlers
            if (NewServiceAdded != null) NewServiceAdded.Invoke(this, service);

            return true;
        }
    }
}
