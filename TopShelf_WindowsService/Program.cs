using Topshelf;

namespace TopShelf_WindowsService
{
    class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.UseNLog();
                x.Service<ServiceRunner>();
                x.RunAsLocalSystem();

                x.SetDescription("TopShelf服务简单测试");
                x.SetDisplayName("TopShelfService");
                x.SetServiceName("TopShelfService");//批处理文件根据这个打开和关闭服务

                x.EnablePauseAndContinue();
            });
        }
    }
}
