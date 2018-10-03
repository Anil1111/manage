using Manage.Core.Infrastructure;
using Manage.Core.Logging;
using System.Web.Mvc;

namespace Manage.Web.Core.Filter
{
    /// <summary>
    /// 自定义异常 action里面不需要try{} catch{}
    /// </summary>
    public class CustomExceptionAttribute : HandleErrorAttribute
    {
        private static ILogger _logger
        {
            get
            {
                return ServiceContainer.Resolve<ILogger>();
            }
        }

        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)//异常有没有被处理过
            {
                string controllerName = (string)filterContext.RouteData.Values["controller"];
                string actionName = (string)filterContext.RouteData.Values["action"];
                string msgTemplate = "在执行 controller[{0}] 的 action[{1}] 时产生异常";
                _logger.Error(string.Format(msgTemplate, controllerName, actionName), filterContext.Exception);

                if (filterContext.HttpContext.Request.IsAjaxRequest())//检查请求头 是不是XMLHttpRequest
                {

                }
                else
                {
                    filterContext.Result = new ViewResult()
                    {
                        ViewName = "~/Views/Shared/Error.cshtml",
                        ViewData = new ViewDataDictionary<string>(filterContext.Exception.Message)
                    };
                }
                filterContext.ExceptionHandled = true;
            }
        }
    }
}
