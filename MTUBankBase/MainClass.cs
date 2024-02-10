using MTUBankBase.ServiceManager;

namespace MTUBankBase
{
    internal class MainClass
    {
        public static ServiceRegistry serviceRegistry;

        static void Main(string[] args)
        {
            // init service registry
            serviceRegistry = new ServiceRegistry();
            serviceRegistry.InitListener("");
        }
    }
}
