using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase.ServiceManager
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    internal class ServiceRouteAttribute : System.Attribute
    {
        public string ApiUrl { get; set; }

        public ServiceRouteAttribute(string ApiUrl)
        {
            this.ApiUrl = ApiUrl;
        }
    }
}
