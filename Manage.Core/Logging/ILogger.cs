using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Core.Logging
{
    public interface ILogger
    {
        void Info(string msg, Exception ex);

        void Info(string msg);

        void Error(string msg, Exception ex);
    }
}
