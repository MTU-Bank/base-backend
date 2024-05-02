using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTUModelContainer.Auth.Models;
using MTUModelContainer.Database.Models;
using MTUBankBase.ServiceManager;

namespace MTUBankBase.Auth
{
    [ServiceDefinition(ServiceType.Auth)]
    public interface IAuthProvider : IServiceDefinition
    {
        [ServiceRoute("/api/registerUser")]
        public AuthResult RegisterUser(RegisterRequest registerRequest);

        [ServiceRoute("/api/loginUser")]
        public AuthResult LoginUser(AuthRequest authRequest);

        [ServiceRoute("/api/getCurrentUser")]
        [RequiresAuth]
        public User GetCurrentUser();

        [ServiceRoute("/api/2FA")]
        public AuthResult Submit2FA(TwoFARequest twoFA);

        [ServiceRoute("/api/set2FA")]
        [RequiresAuth]
        public AuthResult Set2FAStatus(Set2FAStatus new2faStatus);

        [ServiceRoute("/api/requestEmailVerif")]
        public AuthResult RequestEmailVerification();

        [ServiceRoute("/api/confirmEmail")]
        public AuthResult ConfirmEmail(ConfirmStuffRequest emailConfirm);

        [ServiceRoute("/api/requestPhoneVerif")]
        public AuthResult RequestPhoneVerification();

        [ServiceRoute("/api/confirmPhone")]
        public AuthResult ConfirmPhone(ConfirmStuffRequest emailConfirm);
    }
}
