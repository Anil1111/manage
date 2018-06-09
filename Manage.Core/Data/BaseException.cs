using System;

namespace Manage.Core.Data
{
    public class BaseException : Exception
    {
        private int exceptionFlag;
        private string message;
        public BaseException()
        { }

        public BaseException(int flag, string msg)
        {
            this.exceptionFlag = flag;
            this.message = msg;
        }

        public int GetExceptionFlag()
        {
            return this.exceptionFlag;
        }

        public void SetExceptionFlag(int exceptionFlag)
        {
            this.exceptionFlag = exceptionFlag;
        }

        public string GetMsg()
        {
            return this.message;
        }

        public void SetMsg(string msg)
        {
            this.message = msg;
        }
    }
}
