using Manage.Core.Data;
using Manage.Core.Infrastructure;
using Manage.Core.Logging;
using Manage.Core.Pageing;
using Manage.Core.Utility;
using Manage.Data;
using Manage.Data.Domain;
using Manage.Service;
using Manage.Web.Core.Mvc;
using Manage.Web.Core.Security;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Manage.Web.Areas.Member.Controllers
{
    [ActionAuthorizeAttribute()]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            this._roleService = roleService;
        }

        // GET: Member/Role
        public ActionResult Index(UserRoleVM form)
        {
            try
            {
                Page<Sys_Role> page = this._roleService.FindPage(form);
                this.SavePage(page, form);
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }

            return View();
        }

        public ActionResult RoleInsert()
        {
            try
            {
                Sys_Role model = new Sys_Role
                {
                    Enabled = true
                };
                ViewBag.form = model;
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }

            return View();
        }

        [HttpPost]
        public string RoleInsert(UserRoleVM form)
        {
            try
            {
                this._roleService.Insert(form);
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

        public ActionResult RoleEdit(UserRoleVM form)
        {
            try
            {
                Sys_Role model = this._roleService.GetRole(form);
                ViewBag.form = model;
                ViewData["type"] = "edit";
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }

            return View("RoleInsert");
        }

        [HttpPost]
        public string RoleEdit(UserRoleVM form, int typ = 0)
        {
            try
            {
                this._roleService.Update(form);
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
        public string RoleDelete(UserRoleVM form)
        {
            try
            {
                this._roleService.Delete(form);
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

        public ActionResult AuthorizePermission(int Id)
        {
            try
            {
                Root root = this._roleService.GetTree(Id);
                ViewBag.json = JsonUtil.SerializerObject(root.parent);
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }

            return View();
        }

        [HttpPost]
        public string AuthorizePermission(string json)
        {
            try
            {
                List<Sys_PermissionRole> list = JsonUtil.DeserializeJsonToList<Sys_PermissionRole>(json);
                this._roleService.InsertPermissionRole(list);
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