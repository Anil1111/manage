using System;
using System.IO;
using Topshelf;

namespace TopShelf_WindowsService
{
    class Program
    {
        public static void Main(string[] args)
        {

            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
            HostFactory.Run(x =>
            {
                x.UseLog4Net();
                x.Service<ServiceRunner>();
                x.RunAsLocalSystem();

                x.SetDescription("TopShelf服务");
                x.SetDisplayName("TopShelfService");
                x.SetServiceName("TopShelfService");//批处理文件根据这个打开和关闭服务

                x.EnablePauseAndContinue();
            });
        }
    }
}
