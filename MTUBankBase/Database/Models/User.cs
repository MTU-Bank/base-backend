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
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("firstName")]
        public string FirstName { get; set; }
        [Column("middleName")]
        public string? MiddleName { get; set; }
        [Column("lastName")]
        public string LastName { get; set; }

        [Column("email")]
        public string Email { get; set; }
        [Column("phoneNum")]
        public string PhoneNum { get; set; }
        [Column("sex", TypeName = "varchar(8)")]
        public UserSex Sex { get; set; }

        [Column("pwd")]
        [JsonIgnore]
        public string PasswordHash { get; set; }
        [Column("creationDate")]
        [JsonIgnore]
        public DateTime CreationDate { get; set; }

        [Column("verified")]
        public bool Verified { get; set; }
        [Column("twofasecret")]
        public string? TwoFASecret { get; set; }

        [ForeignKey(nameof(Account.OwnerId))]
        public ICollection<Account> Accounts { get; set; }
    }

    public enum UserSex
    {
        Male, Female
    }
}
