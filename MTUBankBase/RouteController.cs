using EmbedIO;
using MTUBankBase.Database;
using MTUBankBase.Helpers;
using MTUBankBase.ServiceManager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Net.Sockets;
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
            if (context.Request.HttpMethod == "OPTIONS")
            {
                await context.SendStringAsync("{}", "application/json", Encoding.UTF8);
                return;
            }

            var routeContainer = GetRoute(context);

            // check for auth
            if ((context.CurrentClaims is null ||
                context.CurrentClaims.Claims.Any((z) => z.Type == "type" && z.Value != "Active")) 
                && routeContainer.RequiresAuth) 
                throw new HttpException(HttpStatusCode.Unauthorized, "Only authorized users can access this method.");

            var service = routeContainer.AssociatedService;
            if (service is null) throw new HttpException(HttpStatusCode.InternalServerError, "This service is not currently available.");

            // replicate the request to the service
            var input = context.Request.InputStream;
            var inputMs = new MemoryStream(); input.CopyTo(inputMs);
            var method = context.Request.HttpMethod;
            var headers = context.Request.Headers;

            // verify the input model is correct before proceeding
            if (routeContainer.InputModel is not null)
            {
                if (input is null || inputMs.Length == 0) throw new HttpException(HttpStatusCode.BadRequest, "POST JSON model expected, nothing received.");
                var inputModelJson = Encoding.UTF8.GetString(inputMs.ToArray());
                var inputModel = JsonConvert.DeserializeObject(inputModelJson, routeContainer.InputModel);
                if (inputModel is null) throw new HttpException(HttpStatusCode.BadRequest, "The model is corrupt.");
                ModelValidator.ValidateModel(inputModel);
            }
            
            StringContent stringContent = null;
            StreamContent streamContent = null;

            if (input is not null)
            {
                string contentType = context.Request.ContentType;

                if (contentType is null) contentType = "application/json";

                if (contentType != "application/octet-stream")
                    stringContent = new StringContent(Encoding.UTF8.GetString(inputMs.ToArray()), Encoding.UTF8, contentType);
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

                    if (hKey == "Authorization") // edge case :woozy_face:
                    {
                        var val = headers[hKey].Split();
                        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(val[0], val.Length > 1 ? val[1] : null);
                    }
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

            // decode Gzip first
            var newResp = new MemoryStream();
            using (var gzip = new GZipStream(responseStream, CompressionMode.Decompress))
            {
                gzip.CopyTo(newResp);
            }

            if (responseType != "application/octet-stream")
                await context.SendStringAsync(Encoding.UTF8.GetString(newResp.ToArray()), responseType, Encoding.UTF8);
            else
                await newResp.CopyToAsync(context.Response.OutputStream);
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

                    // get input model
                    Type? inputModel = null;
                    var inputParams = method.GetParameters().ToList();
                    // check if parameter-less (no model on input)
                    if (inputParams.Count != 0) inputModel = inputParams[0].ParameterType;

                    // check if method requires auth
                    var authAttr = method.GetCustomAttributes(typeof(RequiresAuthAttribute), true).FirstOrDefault();
                    bool requiresAuth = authAttr is not null;

                    // add to the table
                    var rCont = new RouteContainer();
                    rCont.MethodName = methodName;
                    rCont.ServiceDefinition = definition;
                    rCont.AssociatedService = associated;
                    rCont.InputModel = inputModel;
                    rCont.OutputModel = method.ReturnType;
                    rCont.RequiresAuth = requiresAuth;
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

        public Type? InputModel { get; set; }
        public Type OutputModel { get; set; }

        public bool RequiresAuth { get; set; }
    }
}
