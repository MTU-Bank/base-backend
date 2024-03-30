using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUModelContainer.ServiceManager.Models
{
    public class RegisterRequest
    {
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public string PairToken { get; set; }
    }
}
