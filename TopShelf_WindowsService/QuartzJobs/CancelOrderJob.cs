using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopShelf_WindowsService.QuartzJobs
{
    public class CancelOrderJob : IJob
    {
        private ILog logger;
        public CancelOrderJob()
        {
            logger = LogManager.GetLogger(this.GetType());
        }

        public void Execute(IJobExecutionContext context)
        {
            logger.Info("取消订单");
        }
    }
}
