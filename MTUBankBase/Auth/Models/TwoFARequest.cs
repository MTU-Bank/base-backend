using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase.Auth.Models
{
    public class TwoFARequest
    {
        public string TwoFAToken { get; set; }
        public string TwoFAValue { get; set; }
    }
}
