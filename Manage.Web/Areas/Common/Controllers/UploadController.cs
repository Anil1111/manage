using Manage.Core.Infrastructure;
using Manage.Core.Json;
using Manage.Web.ServiceFileClient;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Manage.Web.Areas.Common.Controllers
{
    public class UploadController : Controller
    {
        public string UploadImg()
        {
            try
            {
                FileInfo file = new FileInfo(Request.Files[0].FileName);
                string extension = file.Extension;
                string fileName = file.Name;

                string[] allowExt = { ".jpg", ".jpge", ".gif", ".png" };
                if (!allowExt.Contains(extension.ToLower()))
                {
                    return ResponseJson.Error("文件格式不正确");
                }

                int FileLen = Request.Files[0].ContentLength;
                byte[] bytes = new byte[FileLen];
                Stream MyStream = Request.Files[0].InputStream;
                MyStream.Read(bytes, 0, FileLen);
                MyStream.Close();

                //调用文件服务
                //FtpUtil.Upload(file, FileLen, bytes);

                ServiceFileClient.ServiceFileClient client = new ServiceFileClient.ServiceFileClient();
                CustomFileInfo customFileInfo = new CustomFileInfo
                {
                    OldName = fileName,
                    SendByte = bytes
                };
                string baseFileString = Convert.ToBase64String(bytes);
                customFileInfo.SendByteStr = baseFileString;
                customFileInfo.Extension = extension;

                customFileInfo = client.UpLoadFileInfo(customFileInfo);

                return ResponseJson.Success(customFileInfo, "上传成功");
            }
            catch (Exception ex)
            {
                return ResponseJson.Error(ex.Message);
            }
        }

        public string DeleteFile(string fileName)
        {
            try
            {
                //调用文件服务
                ServiceFileClient.ServiceFileClient client = new ServiceFileClient.ServiceFileClient();
                CustomFileInfo customFileInfo = new CustomFileInfo();
                customFileInfo.NewName = fileName;

                customFileInfo = client.DeleteFile(customFileInfo);

                if (customFileInfo.State == 0)
                    return ResponseJson.Success(customFileInfo, "删除成功");
                else
                    return ResponseJson.Error("删除失败");
            }
            catch (Exception ex)
            {
                return ResponseJson.Error(ex.Message);
            }
        }
    }
}