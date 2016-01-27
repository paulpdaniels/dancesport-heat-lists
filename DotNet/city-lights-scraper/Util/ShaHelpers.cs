using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DancingDuck.Util
{
    public class ShaHelpers
    {
        public static string ComputeSha1Hash(string p, uint length)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(p);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes, length);
        }

        public static string HexStringFromBytes(byte[] bytes, uint length)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString().Substring(0, (int)length);
        }
    }
}
