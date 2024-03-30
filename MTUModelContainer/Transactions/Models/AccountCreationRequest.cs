using MTUModelContainer.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUModelContainer.Transactions.Models
{
    public class AccountCreationRequest
    {
        public AccountCurrency Currency { get; set; }
        public string AccountName { get; set; }
        public bool MakeDefault { get; set; }
    }
}
