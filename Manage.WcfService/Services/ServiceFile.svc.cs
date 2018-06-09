using log4net;
using Manage.Core.Utility;
using System;
using System.IO;
using System.Web;

namespace Manage.WcfService.Services
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“ServiceFile”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 ServiceFile.svc 或 ServiceFile.svc.cs，然后开始调试。
    public class ServiceFile : IServiceFile
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ServiceFile));

        public CustomFileInfo UpLoadFileInfo(CustomFileInfo fileInfo)
        {
            log.Info("上传文件开始：" + DateTime.Now);
            byte[] bytes = Convert.FromBase64String(fileInfo.SendByteStr);

            string fileName = StringUtil.GUID() + fileInfo.Extension;

            //log.Info("newbytes：" + newbytes);
            log.Info("图片名字：" + fileName);

            string fullpath = HttpContext.Current.Server.MapPath("~/Upload/") + fileName;
            using (FileStream fileStream = new FileStream(fullpath, FileMode.Create))
            {
                fileStream.Write(bytes, 0, bytes.Length);
            }

            fileInfo.OldName = fileInfo.OldName;
            fileInfo.NewName = fileName;
            fileInfo.Extension = fileInfo.Extension;
            fileInfo.Path = "/Upload/" + fileName;
            fileInfo.FileSize = FileUtil.GetFileSize(fullpath).ToString();
            fileInfo.SendByte = null;

            log.Info("上传文件结束：" + DateTime.Now);

            return fileInfo;
        }

        public CustomFileInfo DeleteFile(CustomFileInfo fileInfo)
        {
            try
            {
                string fullpath = HttpContext.Current.Server.MapPath("~/Upload/") + fileInfo.NewName;

                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }

                fileInfo.State = 0;
            }
            catch (Exception)
            {
                fileInfo.State = 0;
            }

            return fileInfo;
        }
    }
}
