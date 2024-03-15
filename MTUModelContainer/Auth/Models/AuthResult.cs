using MTUModelContainer.Database.Models;
using Newtonsoft.Json;
using Swan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUModelContainer.Auth.Models
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AuthResult : User
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? Token { get; set; }
        public bool? TwoFARequired { get; set; }
        public string? TwoFAToken { get; set; }

        public AuthResult() { }
        public AuthResult(User u) => u.CopyOnlyPropertiesTo(this);
    }
}
