using EmbedIO;
using EmbedIO.WebApi;
using Microsoft.IdentityModel.Tokens;
using MTUBankBase.Config;
using MTUBankBase.Helpers;
using MTUBankBase.ServiceManager;
using MTUModelContainer;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

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
            // set up JwtService
            var jwtService = new JwtService("MTUBank", JwtKeyGenerator.GetSecurityKey(appConfig.PairToken));

            // build route table
            var rc = new RouteController();
            rc.BuildRouteTable();

            var server = new WebServer(o => o
                    .WithUrlPrefix(baseUrl)
                    .WithMode(HttpListenerMode.EmbedIO), jwtService);

            // add route table entries
            foreach (var route in rc.table)
            {
                server = server.OnAny(route.MethodName, RouteController.HandleRoute);
            }
            
            // add base API controller
            server = server.WithWebApi("/", WebControllerMethods.AsJSON, m => m
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
