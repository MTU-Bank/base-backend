using EmbedIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase.Helpers
{
    public class WebControllerMethods
    {
        public static async Task AsJSON(IHttpContext context, object? data)
        {
            if (data is null) return;

            context.Response.ContentType = "application/json";
            using var text = context.OpenResponseText();
            await text.WriteAsync(JsonConvert.SerializeObject(data)).ConfigureAwait(false);
        }

        public static string BindString(string host, int port) => $"http://{host}:{port}/";
    }
}
