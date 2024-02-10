using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTUBankBase.Auth.Models;

namespace MTUBankBase.Auth
{
    public interface IAuthProvider
    {
        public AuthResult RegisterUser(RegisterRequest registerRequest);
        public AuthResult LoginUser(AuthRequest authRequest);
    }
}
