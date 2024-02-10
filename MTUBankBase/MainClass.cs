using EmbedIO;
using EmbedIO.WebApi;
using MTUBankBase.Config;
using MTUBankBase.Helpers;
using MTUBankBase.ServiceManager;
using System.Runtime.CompilerServices;

namespace MTUBankBase
{
    internal class MainClass
    {
        public static ServiceRegistry serviceRegistry;
        public static ApplicationConfig appConfig;

        static async Task Main(string[] args)
        {
            // load configuration files
            appConfig = ApplicationConfig.Load("appconfig.json");

            // init service registry
            serviceRegistry = new ServiceRegistry();
            serviceRegistry.InitListener(WebControllerMethods.BindString(appConfig.ServiceManagerHost, appConfig.ServiceManagerPort));

            // subscribe to service updates
            serviceRegistry.NewServiceAdded += ServiceRegistry_NewServiceAdded;
            serviceRegistry.ServiceRemoved += ServiceRegistry_ServiceRemoved;

            // initialize base API
            InitListener(WebControllerMethods.BindString(appConfig.BaseAPIHost, appConfig.BaseAPIPort));

            // await forever
            await Task.Delay(-1);
        }

        public static void InitListener(string baseUrl, CancellationToken cancellationToken = default)
        {
            var server = new WebServer(o => o
                    .WithUrlPrefix(baseUrl)
                    .WithMode(HttpListenerMode.EmbedIO))
                .WithWebApi("/", WebControllerMethods.AsJSON, m => m
                    .WithController<BaseAPIController>());

            server.Start(cancellationToken);
        }

        private static void ServiceRegistry_ServiceRemoved(object? sender, Service e)
        {
            Console.WriteLine($"[{e.Name}] Service successfully unsubscribed!");
        }

        private static void ServiceRegistry_NewServiceAdded(object? sender, Service e)
        {
            // TODO: proper logging
            Console.WriteLine($"[{e.Name}] Service successfully subscribed!");
        }
    }
}
