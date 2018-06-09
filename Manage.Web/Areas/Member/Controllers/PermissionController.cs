using Manage.Core.Data;
using Manage.Core.Infrastructure;
using Manage.Core.Logging;
using Manage.Core.Pageing;
using Manage.Data;
using Manage.Data.Domain;
using Manage.Service;
using Manage.Web.Core.Mvc;
using Manage.Web.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Manage.Web.Areas.Member.Controllers
{
    [ActionAuthorizeAttribute()]
    public class PermissionController : BaseController
    {
        private readonly IPermissionService _permissionService;
        private readonly IModuleService _moduleService;
        public PermissionController(
            IPermissionService permissionService, 
            IModuleService moduleService)
        {
            this._permissionService = permissionService;
            this._moduleService = moduleService;
        }

        // GET: Member/Permission
        public ActionResult Index(PermissionVM form)
        {
            try
            {
                Page<Sys_Permission> page = this._permissionService.FindPage(form);
                this.SavePage(page, form);

                List<Sys_Module> list = this._moduleService.GetModuleList().Where(t => t.ParentId != null && t.ParentId != 0).ToList();
                ViewData["ModuleList"] = list;
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }

            return View();
        }

        public ActionResult PermissionInsert()
        {
            try
            {
                List<Sys_Module> list = this._moduleService.GetModuleList().Where(t => t.ParentId != null && t.ParentId != 0).ToList();
                ViewData["ModuleList"] = list;

                Sys_Permission permission = new Sys_Permission
                {
                    Enabled = true
                };
                ViewBag.form = permission;
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }

            return View();
        }

        [HttpPost]
        public string PermissionInsert(PermissionVM form)
        {
            try
            {
                this._permissionService.Insert(form);
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

        public ActionResult PermissionEdit(PermissionVM form)
        {
            try
            {
                List<Sys_Module> list = this._moduleService.GetModuleList().Where(t => t.ParentId != null && t.ParentId != 0).ToList();
                ViewData["ModuleList"] = list;

                Sys_Permission model = this._permissionService.GetPermission(form);
                ViewBag.form = model;
                ViewData["type"] = "edit";
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }

            return View("PermissionInsert");
        }

        [HttpPost]
        public string PermissionEdit(PermissionVM form, int type = 0)
        {
            try
            {
                this._permissionService.Update(form);
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

        [HttpPost]
        public string PermissionDelete(PermissionVM form)
        {
            try
            {
                this._permissionService.Delete(form);
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
    }
}