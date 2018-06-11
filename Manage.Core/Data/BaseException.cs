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

        public string GetMessage()
        {
            return this.message;
        }

        public void SetMessage(string msg)
        {
            this.message = msg;
        }
    }
}
