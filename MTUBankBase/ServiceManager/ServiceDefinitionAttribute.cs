using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase.ServiceManager
{
    [System.AttributeUsage(System.AttributeTargets.Class |
                           System.AttributeTargets.Interface)]
    internal class ServiceDefinitionAttribute : System.Attribute
    {
        public ServiceType ServiceType { get; set; }

        public ServiceDefinitionAttribute(ServiceType serviceType)
        {
            this.ServiceType = serviceType;
        }
    }
}
