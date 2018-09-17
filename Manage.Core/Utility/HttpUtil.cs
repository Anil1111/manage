using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace Manage.Core.Utility
{
    public class HttpUtil
    {
        private string _url = "";
        private int _timeout = 200000;
        private string _result = "";
        private string _error = "";
        private long _revtime = 0;
        private Encoding _chtEncoding = Encoding.Default;

        /// <summary>
        /// 返回结果
        /// </summary>
        public string Result { get { return _result; } }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string Error { get { return _error; } }

        /// <summary>
        /// 响应时间
        /// </summary>
        public long RevTime { get { return _revtime; } }

        public HttpUtil(string method, String url, String param = "", int timeout = 120, Encoding chtEncoding = null)
        {
            _chtEncoding = chtEncoding == null ? Encoding.Default : chtEncoding;
            _url = url;

            if (method.ToLower() == "post")
                _result = Post_Http(_url, param, _timeout);
            else if (method.ToLower() == "get")
                _result = Get_Http(url, _timeout);
        }

        public String Post_Http(String a_strUrl, String _params, int timeout)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            string strResult = "";
            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(a_strUrl);
                request.Timeout = timeout;
                request.Method = "POST";

                byte[] bs = _chtEncoding.GetBytes(_params);
                string responseData = String.Empty;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bs.Length;

                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                    reqStream.Close();
                }

                response = (HttpWebResponse)request.GetResponse();
                Stream myStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, Encoding.UTF8);
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }

                strResult = strBuilder.ToString();
            }
            catch (Exception exp)
            {
                strResult = "SYSERROR";
                _error = exp.Message;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            stopwatch.Stop();
            _revtime = stopwatch.ElapsedMilliseconds;

            return strResult;
        }

        public String Get_Http(String a_strUrl, int timeout)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            string strResult;
            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(a_strUrl);
                request.Timeout = timeout;
                response = (HttpWebResponse)request.GetResponse();
                Stream myStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, _chtEncoding);
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }
                strResult = strBuilder.ToString();
            }
            catch (Exception ex)
            {
                strResult = "SYSERROR";
                _error = ex.Message;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            stopwatch.Stop();
            _revtime = stopwatch.ElapsedMilliseconds;

            return strResult;
        }
    }
}
