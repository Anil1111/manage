using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Manage.Web.Core.Filter
{
    /// <summary>
    /// IActionFilter，IResultFilter
    /// </summary>
    public class MyActionFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// action执行之前处理
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            System.Diagnostics.Debug.WriteLine("方法名称：OnActionExecuting");
            //输出请求的控制器与action名称
            String controllerName = filterContext.RouteData.Values["controller"].ToString();
            String actionName = filterContext.RouteData.Values["action"].ToString();
            String message = String.Format("控制器:{0},方法:{1}", controllerName, actionName);
            System.Diagnostics.Debug.WriteLine(message);
            System.Diagnostics.Debug.WriteLine("------------------华丽的分割线------------------<br>");
        }

        /// <summary>
        /// action执行之后处理
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string action = filterContext.ActionDescriptor.ActionName;
        }

        /// <summary>
        /// action执行之后，视图执行之前处理
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

        }

        /// <summary>
        /// action执行之后，视图执行之后处理
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {

        }
    }
}
