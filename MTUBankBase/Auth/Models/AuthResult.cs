using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase.Auth.Models
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AuthResult : UserProfile
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? Token { get; set; }
        public bool TwoFARequired { get; set; }
        public string? TwoFAToken { get; set; }
    }
}
