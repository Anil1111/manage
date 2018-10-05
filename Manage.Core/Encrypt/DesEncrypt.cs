using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Manage.Core.Encrypt
{
    /// <summary>
    /// DES AES Blowfish
    ///  对称加密算法的优点是速度快，
    ///  缺点是密钥管理不方便，要求共享密钥。
    /// 可逆对称加密  密钥长度8
    /// </summary>
    public class DesEncrypt
    {
        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="text">需要加密的值</param>
        /// <returns>加密后的结果</returns>
        public static string Encrypt(string text, string sKey)
        {
            DESCryptoServiceProvider dsp = new DESCryptoServiceProvider();
            using (MemoryStream memStream = new MemoryStream())
            {
                byte[] _rgbKey = ASCIIEncoding.ASCII.GetBytes(sKey.Substring(0, 8));
                byte[] _rgbIV = ASCIIEncoding.ASCII.GetBytes(sKey.Substring(0, 8));
                CryptoStream crypStream = new CryptoStream(memStream, dsp.CreateEncryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write);
                StreamWriter sWriter = new StreamWriter(crypStream);
                sWriter.Write(text);
                sWriter.Flush();
                crypStream.FlushFinalBlock();
                memStream.Flush();
                return Convert.ToBase64String(memStream.GetBuffer(), 0, (int)memStream.Length);
            }
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="encryptText"></param>
        /// <returns>解密后的结果</returns>
        public static string Decrypt(string encryptText, string sKey)
        {
            DESCryptoServiceProvider dsp = new DESCryptoServiceProvider();
            byte[] buffer = Convert.FromBase64String(encryptText);

            using (MemoryStream memStream = new MemoryStream())
            {
                byte[] _rgbKey = ASCIIEncoding.ASCII.GetBytes(sKey.Substring(0, 8));
                byte[] _rgbIV = ASCIIEncoding.ASCII.GetBytes(sKey.Substring(0, 8));
                CryptoStream crypStream = new CryptoStream(memStream, dsp.CreateDecryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write);
                crypStream.Write(buffer, 0, buffer.Length);
                crypStream.FlushFinalBlock();
                return ASCIIEncoding.UTF8.GetString(memStream.ToArray());
            }
        }
    }
}
