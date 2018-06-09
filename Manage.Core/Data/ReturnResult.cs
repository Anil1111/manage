namespace Manage.Core.Data
{
    public class ReturnResult
    {
        public ReturnResult()
        { }

        public object state { get; set; }

        public string message { get; set; }

        public object data { get; set; }

        public ReturnResult(int state)
        {
            this.state = state;
            if (state == SuperConstants.AJAX_RETURN_STATE_OK)
                this.message = "操作成功";
            else if (state == SuperConstants.AJAX_RETURN_STATE_ERROR)
                this.message = "操作失败";
            else if (state == SuperConstants.AJAX_RETURN_STATE_LOGIN)
                this.message = "未登录";
            else if (state == SuperConstants.AJAX_RETURN_STATE_AUTH)
                this.message = "没有操作权限";
        }

        public ReturnResult(int state, string message)
        {
            this.state = state;
            this.message = message;
        }

        public ReturnResult(int state, string message, object data)
        {
            this.state = state;
            this.message = message;
            this.data = data;
        }

        //public override string ToString()
        //{
        //    return JsonUtil.ToJSON(this);
        //}
    }
}
