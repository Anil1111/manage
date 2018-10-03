using System;
using log4net;
using Quartz;

namespace TopShelf_WindowsService.QuartzJobs
{
    public sealed class TestJob : IJob
    {
        private ILog logger;
        public TestJob()
        {
            logger = LogManager.GetLogger(this.GetType());
        }

        public void Execute(IJobExecutionContext context)
        {
            logger.Info("TestJob测试");
        }
    }
}
