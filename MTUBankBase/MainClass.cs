using MTUBankBase.Config;
using MTUBankBase.ServiceManager;

namespace MTUBankBase
{
    internal class MainClass
    {
        public static ServiceRegistry serviceRegistry;
        public static ApplicationConfig appConfig;

        static void Main(string[] args)
        {
            // load configuration files
            appConfig = ApplicationConfig.Load("appconfig.json");

            // init service registry
            serviceRegistry = new ServiceRegistry();
            serviceRegistry.InitListener($"http://{appConfig.ServiceManagerHost}:{appConfig.ServiceManagerPort}/");

            // subscribe to service updates
            serviceRegistry.NewServiceAdded += ServiceRegistry_NewServiceAdded;
            serviceRegistry.ServiceRemoved += ServiceRegistry_ServiceRemoved;
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
