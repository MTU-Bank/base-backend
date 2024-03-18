using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase.Helpers
{
    public static class ObjectExtensions
    {

        public static object? DeserializeObjectReflected(string json, Type reflectedType)
        {
            var mis = typeof(JsonConvert).GetMethods(BindingFlags.Static | BindingFlags.Public);
            var methodInfo = mis.FirstOrDefault((z) => z.Name == nameof(JsonConvert.DeserializeObject));
            var genericArguments = new[] { reflectedType };
            var genericMethodInfo = methodInfo?.MakeGenericMethod(genericArguments);
            return genericMethodInfo?.Invoke(null, new[] { json });
        }
    }
}
