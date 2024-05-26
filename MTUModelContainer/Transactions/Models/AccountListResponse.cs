using MTUModelContainer.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUModelContainer.Transactions.Models
{
    public class AccountListResponse 
    {
        public bool Success { get; set; }
        public string? Error { get; set; }

        public List<Account> Accounts { get; set; }
    }
}
