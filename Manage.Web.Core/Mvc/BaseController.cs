using Manage.Core.Data;
using Manage.Core.Infrastructure;
using Manage.Core.Logging;
using Manage.Core.Pageing;
using Manage.Web.Core.Infrastructure;
using System.Web.Mvc;

namespace Manage.Web.Core.Mvc
{
    public class BaseController : Controller
    {
        public ILogger _logger
        {
            get
            {
                return ServiceContainer.Resolve<ILogger>();
            }
        }

        public UserSession UserSession()
        {
            return BaseExtensions.UserSession();
        }

        public void SavePage<T>(Page<T> page, object form)
        {
            ViewBag.page = page;
            ViewBag.form = form;
        }
    }
}
