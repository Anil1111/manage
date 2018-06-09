using Manage.Core.Data;
using Manage.Core.Infrastructure;
using Manage.Core.Logging;
using Manage.Service;
using Manage.Web.Core.Mvc;
using System;
using System.Web.Mvc;

namespace Manage.Web.Areas.Common.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUserService _userService;
        public HomeController(IUserService userService)
        {
            this._userService = userService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdatePwd()
        {
            return View();
        }

        [HttpPost]
        public string UpdatePwd(string oldPassword, string password)
        {
            try
            {
                UserSession userSession = this.UserSession();
                this._userService.UpdatePwd(oldPassword, password, userSession.UserId);
                return ResponseJson.Success();
            }
            catch (BaseException ex)
            {
                _logger.Info(ex.GetMsg());
                return ResponseJson.Error(ex.GetExceptionFlag(), ex.GetMsg());
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
                return ResponseJson.Error(ex.Message);
            }
        }

        public ActionResult NoPermissions()
        {
            return View();
        }
    }
}