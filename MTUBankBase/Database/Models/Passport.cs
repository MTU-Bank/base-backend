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
    [Table("passports")]
    public class Passport
    {
        [Key]
        [Column("passport_id")]
        public string PassportId { get; set; }

        [Column("owner_id")]
        public string OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [JsonIgnore]
        public User User { get; set; }

        [Column("image_blob")]
        public string ImageBlob { get; set; }

        [Column("complete_id")]
        public string CompleteId { get; set; }

        [Column("country")]
        public PassportCountry Country { get; set; }

        [Column("issuedDate")]
        public DateTime IssuedDate { get; set; }

        [Column("processed")]
        public bool Processed { get; set; }

        [Column("accepted")]
        public bool Accepted { get; set; }
    }

    public enum PassportCountry
    {
        RU // добавим больше но ПОТОМ
    }
}
