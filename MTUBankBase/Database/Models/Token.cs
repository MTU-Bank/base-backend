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
    [Table("tokens")]
    public class Token
    {
        [Key]
        [Column("token")]
        public string TokenValue { get; set; }

        [Column("owner")]
        public string OwnerId { get; set; }

        [Column("type")]
        public TokenType TokenType { get; set; }

        [Column("creationDate")]
        public DateTime CreationDate { get; set; }

        [JsonIgnore]
        public bool IsAuthed { get => TokenType == TokenType.Active && (DateTime.Now - CreationDate) <= TimeSpan.FromHours(4); }

        // navigational properties
        [JsonIgnore]
        public User Owner { get; set; }
    }

    public enum TokenType
    {
        Active, // one we use for nearly all operations
        TwoFA // one we use to request the user to confirm the action with 2FA
    }
}
