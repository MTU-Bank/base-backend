using EmbedIO;
using MTUBankBase.Helpers;
using MTUBankBase.ServiceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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
            var routeContainer = GetRoute(context);

            var service = routeContainer.AssociatedService;
            if (service is null) throw new HttpException(500, "This service is not currently available.");

            // replicate the request to the service
            var input = context.Request.InputStream as MemoryStream;
            var method = context.Request.HttpMethod;
            var headers = context.Request.Headers;
            
            StringContent stringContent = null;
            StreamContent streamContent = null;

            if (input is not null)
            {
                string contentType = context.Request.ContentType;

                if (contentType is null) contentType = "application/json";

                if (contentType != "application/octet-stream")
                    stringContent = new StringContent(Encoding.UTF8.GetString(input.ToArray()), Encoding.UTF8, contentType);
                else
                    streamContent = new StreamContent(input);
            }

            var serviceURL = $"{service.BaseUrl}{routeContainer.MethodName}";

            HttpResponseMessage response = null;

            using (var http = new HttpClient())
            {
                // attempt to incorporate all headers
                var headerList = headers.AllKeys.ToList();
                foreach (var hKey in headerList)
                {
                    try { http.DefaultRequestHeaders.Add(hKey, headers[hKey]); }
                    catch { }
                }

                switch (method)
                {
                    case "GET":
                        response = await http.GetAsync(serviceURL);
                        break;
                    case "POST":
                        response = await http.PostAsync(serviceURL, stringContent is null ? streamContent : stringContent);
                        break;
                    // TODO implement the rest of methods?
                }
            }

            var statusCode = response.StatusCode;

            if (!response.IsSuccessStatusCode) throw new HttpException(statusCode);

            var responseStream = await response.Content.ReadAsStreamAsync() as MemoryStream;
            string responseType = "application/json";
            try
            {
                responseType = response.Headers.NonValidated["Content-Type"].FirstOrDefault();
            }
            catch { }

            if (responseType != "application/octet-stream")
                await context.SendStringAsync(Encoding.UTF8.GetString(responseStream.ToArray()), responseType, Encoding.UTF8);
            else
                await responseStream.CopyToAsync(context.Response.OutputStream);
        }

        private static RouteContainer? GetRoute(IHttpContext context)
        {
            // get method
            var methodName = GetMethodName(context);

            // lookup the route
            var route = Instance.table.FirstOrDefault(z => z.MethodName.Equals(methodName));
            return route;
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
