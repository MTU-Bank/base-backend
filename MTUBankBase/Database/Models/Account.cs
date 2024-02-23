using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MTUBankBase.Database.Models
{
    [Table("accounts")]
    public class Account
    {
        [Key]
        [Column("account_id")]
        public string AccountId { get; set; }

        [Column("owner_id")]
        [JsonIgnore]
        public string OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [JsonIgnore]
        public User User { get; set; }

        [Column("currency", TypeName = "VARCHAR(32)")]
        public AccountCurrency AccountCurrency { get; set; }

        [Column("creationDate")]
        public DateTime CreationDate { get; set; }

        [Column("userLocked")]
        public bool UserLocked { get; set; }

        [Column("systemLocked")]
        public bool SystemLocked { get; set; }
    }

    public enum AccountCurrency
    {
        RUB, USD, EUR
    }
}
