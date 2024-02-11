using EmbedIO;
using EmbedIO.WebApi;
using MTUBankBase.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase.ServiceManager
{
    public class ServiceRegistry
    {
        public static ServiceRegistry Instance;

        public event EventHandler<Service> NewServiceAdded;
        public event EventHandler<Service> ServiceRemoved;

        public List<Service> localServices = new List<Service>();
        public string PairToken = Environment.GetEnvironmentVariable("PairToken");
        private List<Type> serviceDefinitions = new List<Type>();

        public void InitListener(string baseUrl, CancellationToken cancellationToken = default)
        {
            Instance = this;

            UpdateServiceDefinitions();

            var server = new WebServer(o => o
                    .WithUrlPrefix(baseUrl)
                    .WithMode(HttpListenerMode.EmbedIO))
                .WithWebApi("/", WebControllerMethods.AsJSON, m => m
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

        public async Task<bool> UnregisterServiceAsync(Service service)
        {
            // if service is online...
            var onlineStatus = await service.IsOnlineAsync();
            // we should alarm the service about the disconnect.
            if (onlineStatus) await service.MessageDisconnect();

            // remove the service from local services registry
            localServices.Remove(service);

            // invoke event handlers
            if (ServiceRemoved != null) ServiceRemoved.Invoke(this, service);

            return true;
        }

        /// <summary>
        /// Updates cached server definitions for easier route lookup
        /// </summary>

        private void UpdateServiceDefinitions()
        {
            var type = typeof(IServiceDefinition);
            var serviceDefinitions = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.GetInterfaces().Contains(type))
                .ToList();
            this.serviceDefinitions = serviceDefinitions;
        }

        /// <summary>
        /// Obtains valid service definitions out of cache
        /// </summary>
        /// <returns>List of server definitions</returns>
        public static List<Type> GetServiceDefinitions() => Instance.serviceDefinitions;
    }
}
