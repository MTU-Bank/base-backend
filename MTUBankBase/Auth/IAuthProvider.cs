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
        // public service API methods
        [ServiceRoute("/api/registerUser")]
        public AuthResult RegisterUser(RegisterRequest registerRequest);

        [ServiceRoute("/api/loginUser")]
        public AuthResult LoginUser(AuthRequest authRequest);

        [ServiceRoute("/api/2FA")]
        public AuthResult Submit2FA(TwoFARequest twoFA);

        [ServiceRoute("/api/set2FA")]
        [RequiresAuth]
        public AuthResult Set2FAStatus(Set2FAStatus new2faStatus);
    }
}
