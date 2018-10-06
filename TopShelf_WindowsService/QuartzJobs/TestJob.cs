using System;
using System.Collections.Generic;
using log4net;
using Manage.Core.Json;
using Manage.Data;
using Manage.Service.QuartzJobs;
using Quartz;

namespace TopShelf_WindowsService.QuartzJobs
{
    public sealed class TestJob : IJob
    {
        private ILog _logger;
        private UserServiceJob _userServiceJob;
        public TestJob()
        {
            _logger = LogManager.GetLogger(this.GetType());
            _userServiceJob = new UserServiceJob();
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                List<Sys_User> list = this._userServiceJob.Entities("admin");
                string jsonStr = JsonUtil.SerializerObject(list);
                _logger.Info(jsonStr);
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }
        }
    }
}
