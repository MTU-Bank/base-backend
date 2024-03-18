using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUModelContainer
{
    public class JwtKeyGenerator
    {
        public static byte[] GetSecurityKey(string bindToken)
        {
            var kR = bindToken;
            if (kR.Length > 32) kR = kR.Substring(0, 32);
            if (kR.Length < 32) kR = kR.PadRight(32, 'x');

            return Encoding.UTF8.GetBytes(kR);
        }
    }
}
