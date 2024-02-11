using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase
{
    public class BaseAPIController : WebApiController
    {
        [Route(HttpVerbs.Get, "/status")]
        public async Task<object> GetStatus()
        {
            return "OK";
        }
    }
}
