using System;
using Quartz;

namespace TopShelf_WindowsService.QuartzJobs
{
    public sealed class TestJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            NLog.LogManager.GetCurrentClassLogger().Debug("TestJob测试");
        }
    }
}
