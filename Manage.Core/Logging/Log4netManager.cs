using log4net;
using System;

namespace Manage.Core.Logging
{
    public class Log4netManager : ILogger
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Log4netManager));
        public void Error(string msg, Exception ex)
        {
            log.Error(msg, ex);
        }

        public void Info(string msg, Exception ex)
        {
            log.Info(msg, ex);
        }

        public void Info(string msg)
        {
            log.Info(msg);
        }
    }
}
