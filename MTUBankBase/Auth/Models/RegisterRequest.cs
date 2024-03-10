using MTUBankBase.Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase.Auth.Models
{
    public class RegisterRequest : User
    {
        [Required]
        public string Password { get; set; }
    }
}
