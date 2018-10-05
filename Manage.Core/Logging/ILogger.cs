using System;

namespace Manage.Core.Logging
{
    public interface ILogger
    {
        void Info(string msg, Exception ex);

        void Info(string msg);

        void Error(string msg, Exception ex);
    }
}
