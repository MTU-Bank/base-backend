using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase.Config
{
    public class ApplicationConfig : ConfigBase<ApplicationConfig>
    {
        public string BaseAPIHost { get; set; } = "127.0.0.1";
        public int BaseAPIPort { get; set; } = 80;

        public string ServiceManagerHost { get; set; } = "127.0.0.1";
        public int ServiceManagerPort { get; set; } = 8090;
        public string PairToken { get; set; } = "testToken";
    }
}
