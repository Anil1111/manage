using System.Security.Cryptography;
using System.Text;

namespace Manage.Core.Utility
{
    public class Md5Util
    {
        public static string MD5Encrypt(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.Default.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0').ToUpper());
            }

            return sb.ToString();
        }
    }
}
