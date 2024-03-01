using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase.Database.Models
{
    [Table("txs")]
    public class Transaction
    {
        [Key]
        [Column("tx_id")]
        public string TxId { get; set; }

        [Column("source_acc")]
        public string SourceAccId { get; set; }

        [ForeignKey(nameof(SourceAccId))]
        [JsonIgnore]
        public Account SourceAcc { get; set; }

        [Column("dest_acc")]
        public string DestinationAccId { get; set; }

        [ForeignKey(nameof(DestinationAccId))]
        [JsonIgnore]
        public Account DestinationAcc { get; set; }

        [Column("sum")]
        public uint Sum { get; set; }

        [Column("type")]
        public TransactionType TxType { get; set; }

        [Column("creationDate")]
        public DateTime CreationDate { get; set; }
    }

    public enum TransactionType
    {
        DEPOSIT, // incoming transaction
        WITHDRAW, // used for withdrawal to physical money
        PAYMENT, // payment for a service
        TRANSFER // money transfer to another user
    }
}
