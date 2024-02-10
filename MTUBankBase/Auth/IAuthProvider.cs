using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTUBankBase.Auth.Models;
using MTUBankBase.ServiceManager;

namespace MTUBankBase.Auth
{
    public interface IAuthProvider
    {
        [ServiceRoute("/api/registerUser")]
        public AuthResult RegisterUser(RegisterRequest registerRequest);

        [ServiceRoute("/api/loginUser")]
        public AuthResult LoginUser(AuthRequest authRequest);

        [ServiceRoute("/api/2FA")]
        public AuthRequest Submit2FA(TwoFARequest twoFA);
    }
}
