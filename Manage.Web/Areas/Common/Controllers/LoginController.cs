﻿using Manage.Core.Caching;
using Manage.Core.Data;
using Manage.Core.Infrastructure;
using Manage.Core.Utility;
using Manage.Data.Domain;
using Manage.Service;
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

        // GET: Common/Login
        public ActionResult Index()
        {
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
                _logger.Info(ex.GetMsg());
                return ResponseJson.Error(ex.GetExceptionFlag(), ex.GetMsg());
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