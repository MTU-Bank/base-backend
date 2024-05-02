using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUModelContainer.Database.Models
{
    [Table("accounts")]
    public class Account
    {
        [Key]
        [Column("account_id")]
        public string AccountId { get; set; }

        [Column("owner_id")]
        [JsonIgnore]
        [GetOnlyJsonProperty]
        public string OwnerId { get; set; }

        [Column("name")]
        public string FriendlyName { get; set; }

        [Column("currency", TypeName = "VARCHAR(32)")]
        public AccountCurrency AccountCurrency { get; set; }

        [Column("creationDate")]
        [GetOnlyJsonProperty]
        public DateTime CreationDate { get; set; }

        [Column("userLocked")]
        public bool UserLocked { get; set; }

        [Column("systemLocked")]
        public bool SystemLocked { get; set; }

        [Column("balance")]
        public long Balance { get; set; }

        // navigation properties
        [JsonIgnore]
        [GetOnlyJsonProperty]
        public User User { get; set; }
    }

    public enum AccountCurrency
    {
        RUB, USD, EUR
    }
}
