using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUModelContainer.Transactions.Models
{
    public class TransactionRequest
    {
        public string FromAccount { get; set; }
        public bool IsDirectAccountTx { get; set; }
        public string? ToWhoever { get; set; }

        public long Amount { get; set; }
    }
}
