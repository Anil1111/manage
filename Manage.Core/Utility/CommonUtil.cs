using Manage.Core.Data;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Manage.Core.Utility
{
    public class CommonUtil
    {
        public static string GUID()
        {
            return Guid.NewGuid().ToString();
        }

        #region 在字符串左边用 paddingChar 补足 totalWidth 长度
        /// <summary>
        /// 在字符串左边用 paddingChar 补足 totalWidth 长度
        /// StringUtil.GetPadLeftStr("1", '0', 6) 000001
        /// </summary>
        /// <param name="str">1</param>
        /// <param name="paddingChar">0</param>
        /// <param name="totalWidth">6</param>
        /// <returns></returns>
        public static string GetPadLeftStr(string str, char paddingChar, int totalWidth)
        {
            return str.PadLeft(totalWidth, paddingChar);
        }
        #endregion

        #region 去除html标记
        /// <summary>
        /// 去除html标记
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string NoHtmlTag(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&ldquo;", "“", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&rdquo;", "”", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring = Htmlstring.Replace("<", "&lt;");
            Htmlstring = Htmlstring.Replace(">", "&gt;");
            return Htmlstring;
        }
        #endregion

        #region 得到字符串长度，一个汉字长度为2
        /// <summary>
        /// 得到字符串长度，一个汉字长度为2
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetStrLength(string str)
        {
            Regex r = new Regex("[\u4e00-\u9fa5]+", RegexOptions.IgnoreCase);
            int length = str.Length;
            for (int i = 0; i < str.Length; i++)
            {
                if (r.IsMatch(str.Substring(i, 1)))
                {
                    length++;
                }
            }
            return length;
        }
        #endregion

        #region 截取字符串
        /// <summary>
        /// 截取字符串(一般截取，无汉字英文判断)
        /// </summary>
        /// <param name="originalStr">待截取的字符串</param>
        /// <param name="len">截取的长度</param>
        /// <param name="type">保留形式(0无省略号，1有省略号,省略号长度不计算在内)</param>
        /// <returns>截取后的字符串</returns>
        public static string CutStrNormal(string originalStr, int len, int type)
        {
            if (originalStr.Trim().Length <= len)
            {
                return originalStr.Trim();
            }
            else
            {
                return originalStr.Trim().Substring(0, len) + (type == 1 ? "..." : "");
            }
        }

        /// <summary>   
        /// 截取文本，区分中英文字符，中文算两个长度，英文算一个长度
        /// </summary>
        /// <param name="str">待截取的字符串</param>
        /// <param name="length">需计算长度的字符串</param>
        /// <param name="type">保留形式(0无省略号，1有省略号,省略号长度不计算在内)</param>
        /// <returns>string</returns>
        public static string CutStr(string str, int length, int type)
        {
            string temp = str;
            int j = 0, k = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (Regex.IsMatch(temp.Substring(i, 1), @"[\u4e00-\u9fa5]+"))
                {
                    j += 2;
                }
                else
                {
                    j += 1;
                }
                if (j <= length)
                {
                    k += 1;
                }
                if (j > length)
                {
                    return temp.Substring(0, k) + (type == 1 ? "..." : "");
                }
            }
            return temp + (type == 1 ? "..." : "");
        }
        #endregion

        #region 把字符串第一个字母转成大写
        /// <summary>
        /// 把字符串第一个字母转成大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetFirstUpper(string str)
        {
            string newStr = "";
            if (str.Length > 0)
            {
                newStr = str.Substring(0, 1).ToUpper() + str.Substring(1, str.Length);
            }
            return newStr;
        }
        #endregion

        #region PinYin(获取汉字的拼音简码)
        /// <summary>
        /// 获取汉字的拼音简码，即首字母缩写,范例：中国,返回zg
        /// </summary>
        /// <param name="chineseText">汉字文本,范例： 中国</param>
        public static string PinYin(string chineseText)
        {
            if (string.IsNullOrWhiteSpace(chineseText))
                return string.Empty;
            var result = new StringBuilder();
            foreach (char text in chineseText)
                result.AppendFormat("{0}", ResolvePinYin(text));
            return result.ToString().ToLower();
        }

        /// <summary>
        /// 解析单个汉字的拼音简码
        /// </summary>
        /// <param name="text">单个汉字</param>
        private static string ResolvePinYin(char text)
        {
            byte[] charBytes = Encoding.Default.GetBytes(text.ToString());
            if (charBytes[0] <= 127)
                return text.ToString();
            var unicode = (ushort)(charBytes[0] * 256 + charBytes[1]);
            string pinYin = ResolvePinYinByCode(unicode);
            if (!string.IsNullOrWhiteSpace(pinYin))
                return pinYin;
            return ResolvePinYinByFile(text.ToString());
        }

        /// <summary>
        /// 使用字符编码方式获取拼音简码
        /// </summary>
        private static string ResolvePinYinByCode(ushort unicode)
        {
            if (unicode >= '\uB0A1' && unicode <= '\uB0C4')
                return "A";
            if (unicode >= '\uB0C5' && unicode <= '\uB2C0' && unicode != 45464)
                return "B";
            if (unicode >= '\uB2C1' && unicode <= '\uB4ED')
                return "C";
            if (unicode >= '\uB4EE' && unicode <= '\uB6E9')
                return "D";
            if (unicode >= '\uB6EA' && unicode <= '\uB7A1')
                return "E";
            if (unicode >= '\uB7A2' && unicode <= '\uB8C0')
                return "F";
            if (unicode >= '\uB8C1' && unicode <= '\uB9FD')
                return "G";
            if (unicode >= '\uB9FE' && unicode <= '\uBBF6')
                return "H";
            if (unicode >= '\uBBF7' && unicode <= '\uBFA5')
                return "J";
            if (unicode >= '\uBFA6' && unicode <= '\uC0AB')
                return "K";
            if (unicode >= '\uC0AC' && unicode <= '\uC2E7')
                return "L";
            if (unicode >= '\uC2E8' && unicode <= '\uC4C2')
                return "M";
            if (unicode >= '\uC4C3' && unicode <= '\uC5B5')
                return "N";
            if (unicode >= '\uC5B6' && unicode <= '\uC5BD')
                return "O";
            if (unicode >= '\uC5BE' && unicode <= '\uC6D9')
                return "P";
            if (unicode >= '\uC6DA' && unicode <= '\uC8BA')
                return "Q";
            if (unicode >= '\uC8BB' && unicode <= '\uC8F5')
                return "R";
            if (unicode >= '\uC8F6' && unicode <= '\uCBF9')
                return "S";
            if (unicode >= '\uCBFA' && unicode <= '\uCDD9')
                return "T";
            if (unicode >= '\uCDDA' && unicode <= '\uCEF3')
                return "W";
            if (unicode >= '\uCEF4' && unicode <= '\uD188')
                return "X";
            if (unicode >= '\uD1B9' && unicode <= '\uD4D0')
                return "Y";
            if (unicode >= '\uD4D1' && unicode <= '\uD7F9')
                return "Z";
            return string.Empty;
        }

        /// <summary>
        /// 从拼音简码文件获取
        /// </summary>
        /// <param name="text">单个汉字</param>
        private static string ResolvePinYinByFile(string text)
        {
            int index = SuperConstants.ChinesePinYin.IndexOf(text, StringComparison.Ordinal);
            if (index < 0)
                return string.Empty;
            return SuperConstants.ChinesePinYin.Substring(index + 1, 1);
        }

        #endregion
    }
}
