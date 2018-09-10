using log4net;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Manage.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MvcApplication));
        /// <summary>
        /// 网站第一次启动时执行一次，而且就不再执行了
        /// dll更新/webconfig修改 都会导致网站重启，这里重新执行
        /// </summary>
        protected void Application_Start()
        {
            log.Info("网站启动。。。。");
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
