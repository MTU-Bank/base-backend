using EmbedIO;
using MTUBankBase.Helpers;
using MTUBankBase.ServiceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase
{
    public class RouteController
    {
        public static RouteController Instance { get; private set; }
        public RouteController() => Instance = this;

        public List<RouteContainer> table = new List<RouteContainer>();

        public static async Task HandleRoute(IHttpContext context)
        {
            var methodName = GetMethodName(context);
            await WebControllerMethods.AsJSON(context, methodName);
        }

        private static string GetMethodName(IHttpContext context)
        {
            var rawUrl = context.Request.RawUrl;
            var methodName = rawUrl.Split('?').FirstOrDefault(); 
            methodName = methodName is null ? rawUrl : methodName;
            return methodName;
        }

        public void BuildRouteTable()
        {
            var routeCont = new List<RouteContainer>();

            // get all service definitions, iterate over definitions
            var allDefinitions = ServiceRegistry.GetServiceDefinitions();
            foreach (var definition in allDefinitions)
            {
                // service type
                var attribute = definition.GetCustomAttributes(typeof(ServiceDefinitionAttribute), true)
                                      .FirstOrDefault();
                ServiceType serviceType = ((ServiceDefinitionAttribute)attribute).ServiceType;

                // get all methods implemented
                var methods = definition.GetMethods();

                // for each method, find method name through attribute. Add method to route container
                foreach (var method in methods)
                {
                    var methodAttribute = method.GetCustomAttributes(typeof(ServiceRouteAttribute), true).FirstOrDefault();
                    if (methodAttribute is null) continue;

                    string methodName = ((ServiceRouteAttribute)methodAttribute).ApiUrl;
                    // find service associated
                    var associated = ServiceRegistry.GetAssociatedService(serviceType);

                    // add to the table
                    var rCont = new RouteContainer();
                    rCont.MethodName = methodName;
                    rCont.ServiceDefinition = definition;
                    rCont.AssociatedService = associated;
                    routeCont.Add(rCont);
                }
            }

            this.table = routeCont;
        }
    }

    public class RouteContainer
    {
        public string MethodName { get; set; }
        public Type ServiceDefinition { get; set; }
        public Service? AssociatedService { get; set; }
    }
}
