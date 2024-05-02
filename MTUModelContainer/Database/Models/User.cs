using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swan;

namespace MTUModelContainer.Database.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        [GetOnlyJsonProperty]
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
        public UserSex? Sex { get; set; }

        [Column("pwd")]
        [JsonIgnore]
        [GetOnlyJsonProperty]
        public string PasswordHash { get; set; }

        [Column("creationDate")]
        [JsonIgnore]
        [GetOnlyJsonProperty]
        public DateTime CreationDate { get; set; }

        [Column("phoneVerif")]
        [GetOnlyJsonProperty]
        public bool PhoneVerified { get; set; } = false;

        [Column("phoneVerifCode")]
        [JsonIgnore]
        [GetOnlyJsonProperty]
        public string? PhoneVerificationCode { get; set; }

        [Column("emailVerif")]
        [GetOnlyJsonProperty]
        public bool EmailVerified { get; set; } = false;

        [Column("emailVerifCode")]
        [JsonIgnore]
        [GetOnlyJsonProperty]
        public string? EmailVerificationCode { get; set; }

        [Column("verified")]
        [JsonIgnore]
        [GetOnlyJsonProperty]
        public bool Verified { get; set; }

        [Column("twofasecret")]
        [JsonIgnore]
        [GetOnlyJsonProperty]
        public string? TwoFASecret { get; set; }

        [GetOnlyJsonProperty]
        public ICollection<Account> Accounts { get; set; }
    }

    public enum UserSex
    {
        Male, Female
    }
}
