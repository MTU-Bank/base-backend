using MTUBankBase.Database;
using MTUBankBase.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase.Auth.Models
{
    public class CheckTokenRequest
    {
        public string? Token { get; set; }
    }
}
