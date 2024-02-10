using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase.Auth.Models
{
    public class AuthResult : UserProfile
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? Token { get; set; }
    }
}
