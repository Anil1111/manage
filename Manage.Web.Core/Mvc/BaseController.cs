using Manage.Core.Data;
using Manage.Core.Infrastructure;
using Manage.Core.Logging;
using Manage.Core.Pageing;
using Manage.Web.Core.Infrastructure;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
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

        public void SavePage<T>(Page<T> page, object form, int hidSelectPage = 0)
        {
            if (page.HidSelectPage.Equals(1))
            {
                page.HidSelectPage = 1;
            }
            page.HidSelectPage = hidSelectPage;
            ViewBag.page = page;
            ViewBag.form = form;
        }
    }
}
