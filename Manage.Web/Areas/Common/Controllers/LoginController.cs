using Manage.Core.Caching;
using Manage.Core.Data;
using Manage.Core.Encrypt;
using Manage.Core.Extend;
using Manage.Core.Infrastructure;
using Manage.Core.Json;
using Manage.Core.Utility;
using Manage.Data.Domain;
using Manage.Service;
using Manage.Web.Core.Enums;
using Manage.Web.Core.Filter;
using Manage.Web.Core.Mvc;
using System;
using System.Web.Mvc;

namespace Manage.Web.Areas.Common.Controllers
{
    public class LoginController : BaseController
    {
        private readonly UserService _userService;
        private readonly IRoleService _roleService;
        private readonly ICacheManager _cacheManager;
        public LoginController(
            UserService userService,
            IRoleService roleService,
            ICacheManager cacheManager)
        {
            this._userService = userService;
            this._roleService = roleService;
            this._cacheManager = cacheManager;
        }

        [MyActionFilterAttribute]
        // GET: Common/Login
        public ActionResult Index()
        {
            //string remark = EnumExtension.GetRemark(UserState.Normal);
            //string desEn = DesEncrypt.Encrypt("王殃殃", "learun###***");
            //string desDe = DesEncrypt.Decrypt(desEn, "learun###***");

            return View();
        }

        public ActionResult ValidateCode()
        {
            string validateCode = CheckCodeUtil.RndNum(4);
            Session[SuperConstants.LOGIN_VALIDATE_CODE] = validateCode;
            return File(CheckCodeUtil.CreateCheckCode(validateCode), @"image/jpeg");
        }

        [HttpPost]
        public string Login(UserVM form)
        {
            try
            {
                this._userService.Login(form);
                return ResponseJson.Success();
            }
            catch (BaseException ex)
            {
                _logger.Info(ex.GetMessage());
                return ResponseJson.Error(ex.GetExceptionFlag(), ex.GetMessage());
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
                return ResponseJson.Error(ex.Message);
            }
        }

        public ActionResult LogOut()
        {
            try
            {
                this._cacheManager.Remove(CookiesUtil.GetCookiesValue(SuperConstants.COOKIESID));
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }
            return RedirectToAction("Index", "Login");
        }
    }
}