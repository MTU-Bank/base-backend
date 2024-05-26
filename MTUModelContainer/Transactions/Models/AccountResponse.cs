using MTUModelContainer.Database.Models;
using MTUModelContainer.Interfaces;
using Swan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUModelContainer.Transactions.Models
{
    public class AccountResponse : Account, ISuccessResponse
    {
        public AccountResponse() { }
        public AccountResponse(Account account)
        {
            account.CopyPropertiesTo(this);
        }

        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
