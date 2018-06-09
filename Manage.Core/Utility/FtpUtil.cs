using System;
using System.IO;
using System.Net;

namespace Manage.Core.Utility
{
    public class FtpUtil
    {
        private static string ftpServerIP = "ftp01.site4future.com";//服务器ip
        private static string ftpUserID = "jiahaiyang88-001";//用户名FTPTEST
        private static string ftpPassword = "woaijingjing1314";//密码

        public static void Upload(FileInfo fileInf, int length, byte[] bytes)
        {
            string uri = "ftp://" + ftpServerIP + "/file/" + fileInf.Name;
            FtpWebRequest reqFTP;

            // 根据uri创建FtpWebRequest对象 
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/file/" + fileInf.Name));
            // ftp用户名和密码
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            // 默认为true，连接不会被关闭
            // 在一个命令之后被执行
            reqFTP.KeepAlive = false;
            // 指定执行什么命令
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            // 指定数据传输类型
            reqFTP.UseBinary = true;
            // 上传文件时通知服务器文件的大小
            reqFTP.ContentLength = length;

            try
            {
                Stream strm = reqFTP.GetRequestStream();

                strm.Write(bytes, 0, bytes.Length);

                strm.Close();
            }
            catch (Exception)
            {

            }
        }

        public static int DownloadFtp(string filename)
        {
            FtpWebRequest reqFTP;
            string serverIP;
            string userName;
            string password;
            string url;

            try
            {
                serverIP = System.Configuration.ConfigurationManager.AppSettings["FTPServerIP"];
                userName = System.Configuration.ConfigurationManager.AppSettings["UserName"];
                password = System.Configuration.ConfigurationManager.AppSettings["Password"];
                url = "ftp://" + serverIP + "/" + Path.GetFileName(filename);

                FileStream outputStream = new FileStream(filename, FileMode.Create);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.KeepAlive = false;
                reqFTP.Credentials = new NetworkCredential(userName, password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return 0;
            }
            catch (Exception)
            {
                return -2;
            }
        }
    }
}
