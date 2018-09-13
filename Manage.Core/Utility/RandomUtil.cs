using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Core.Utility
{
    public class RandomUtil
    {
        /// <summary>
        /// 得到随机字符串
        /// </summary>
        /// <returns></returns>
        public static string GetRandomString()
        {
            Random rd = new Random();
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("yyyyMMddHHmmssff"));
            sb.Append(rd.Next(0, 999999).ToString());
            return sb.ToString();
        }

        /// <summary>
        /// 得到N位随机数
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        public static string GetRandomNumber(int N)
        {
            char[] arrChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < N; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }
    }
}
