using System;
using System.Text;
using System.Web;

namespace Manage.Core.Utility
{
    public class CookiesUtil
    {
        public static void SetCookies(string CookiesName, string CookiesValue, DateTime expires)
        {
            HttpContext.Current.Response.Cookies[CookiesName].Value = CookiesValue;
            HttpContext.Current.Response.Cookies[CookiesName].Expires = expires;
        }

        public static void SetCookies(string CookiesName, string CookiesValue)
        {
            HttpCookie cookie = new HttpCookie(CookiesName)
            {
                Value = HttpUtility.UrlEncode(CookiesValue, Encoding.GetEncoding("UTF-8"))
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static string GetCookiesValue(string CookiesName)
        {
            if (HttpContext.Current.Request.Cookies[CookiesName] != null)
            {
                return HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[CookiesName].Value, Encoding.GetEncoding("UTF-8"));
            }
            else
            {
                return "";
            }
        }

        public static void ClearCookies(string CookiesName)
        {
            HttpContext.Current.Response.Cookies[CookiesName].Expires = DateTime.Now.AddMinutes(0);
        }
    }
}
