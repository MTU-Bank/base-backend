using EmbedIO;
using MTUBankBase.Helpers;
using MTUBankBase.ServiceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase
{
    public class RouteController
    {
        public static async Task HandleRoute(IHttpContext context)
        {
            var methodName = GetMethodName(context);
            var serviceType = ResolveServiceType(methodName);
            await WebControllerMethods.AsJSON(context, serviceType);
        }

        private static string GetMethodName(IHttpContext context)
        {
            var rawUrl = context.Request.RawUrl;
            var methodName = rawUrl.Split('?').FirstOrDefault(); 
            methodName = methodName is null ? rawUrl : methodName;
            return methodName;
        }

        private static ServiceType? ResolveServiceType(string methodName)
        {
            // acquire all service definitions
            var serviceDefinitions = ServiceRegistry.GetServiceDefinitions();

            // look for the definition with matching method names in methods
            foreach (var definition in serviceDefinitions)
            {
                // get methods for method name
                var methods = definition.GetMethods()
                                        .Where(z => {
                                            var attr = z.GetCustomAttributes(typeof(ServiceRouteAttribute), true).FirstOrDefault();
                                            return attr is not null && ((ServiceRouteAttribute)attr).ApiUrl.Equals(methodName);
                                        }).FirstOrDefault();

                if (methods is null) continue;

                // now we assume the method is contained within definition, look for attribute
                var attribute = definition.GetCustomAttributes(typeof(ServiceDefinitionAttribute), true)
                                      .FirstOrDefault();

                if (attribute is not null) return ((ServiceDefinitionAttribute)attribute).ServiceType;
            }
            return null;
        }
    }

    public class RouteContainer
    {
        public IServiceDefinition ServiceDefinition { get; set; }

    }
}
