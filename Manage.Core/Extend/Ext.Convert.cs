using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Manage.Core.Extend
{
    public static partial class Ext
    {
        #region 转换
        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="data">数据</param>
        public static string ToString(this object data)
        {
            return data == null ? string.Empty : data.ToString().Trim();
        }

        /// <summary>
        /// 转换为数字
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int ToInt(this string text)
        {
            if (string.IsNullOrEmpty(text)) return 0;
            int outResult;
            Int32.TryParse(text.Trim(), out outResult);
            return outResult;
        }
        #endregion

        #region 转换为16进制
        /// <summary>
        /// 转换为16进制
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }
        #endregion

        public static T1 CopyFrom<T1, T2>(this T1 obj, T2 otherObject)
        where T1 : class
        where T2 : class
        {
            PropertyInfo[] srcFields = otherObject.GetType().GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty
            );
            PropertyInfo[] destFields = obj.GetType().GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty
            );

            foreach (var property in srcFields)
            {
                var dest = destFields.FirstOrDefault(x => x.Name == property.Name);
                if (dest != null && dest.CanWrite)
                    dest.SetValue(obj, property.GetValue(otherObject, null), null);
            }
            return obj;
        }
    }
}
