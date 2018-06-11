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
using System.Linq;
using System.Web.Mvc;

namespace Manage.Web.Areas.Member.Controllers
{
    [ActionAuthorizeAttribute()]
    public class UserGroupController : BaseController
    {
        private readonly IUserGroupService _userGroupService;
        private readonly IRoleService _roleService;
        public UserGroupController(
            IUserGroupService userGroupService, 
            IRoleService roleService)
        {
            this._userGroupService = userGroupService;
            this._roleService = roleService;
        }

        // GET: Member/UserGroup
        public ActionResult Index(UserGroupVM form)
        {
            try
            {
                Page<Sys_UserGroup> page = this._userGroupService.FindPage(form);
                this.SavePage(page, form);
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }

            return View();
        }

        public ActionResult UserGroupInsert()
        {
            try
            {
                Sys_UserGroup model = new Sys_UserGroup
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
        public string UserGroupInsert(UserGroupVM form)
        {
            try
            {
                this._userGroupService.Insert(form);
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

        public ActionResult UserGroupEdit(UserGroupVM form)
        {
            try
            {
                Sys_UserGroup model = this._userGroupService.GetUserGroup(form);
                ViewBag.form = model;
                ViewData["type"] = "edit";
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }

            return View("UserGroupInsert");
        }

        [HttpPost]
        public string UserGroupEdit(UserGroupVM form, int type = 0)
        {
            try
            {
                this._userGroupService.Update(form);
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

        [HttpPost]
        public string UserGroupDelete(UserGroupVM form)
        {
            try
            {
                this._userGroupService.Delete(form);
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

        public ActionResult SetRolesUserGroup(UserGroupVM form)
        {
            try
            {
                List<Sys_Role> list = this._roleService.GetRoleList();
                if (list != null && list.Count > 0)
                {
                    List<Sys_UserGroupRole> userGroupRoleList = this._roleService.GetUserGroupRoleList();
                    foreach (var item in list)
                    {
                        var checkList = userGroupRoleList.Where(t => t.Role_Id == item.Id && t.UserGroup_Id == form.Id).ToList();
                        if (checkList != null && checkList.Count > 0)
                        {
                            item.Check = true;
                        }
                        else
                        {
                            item.Check = false;
                        }
                    }
                }
                ViewBag.RoleList = list;
                ViewBag.UserGroup_Id = form.Id;
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }

            return View();
        }

        [HttpPost]
        public string SetRolesUserGroup(string json, int userGroupId)
        {
            try
            {
                List<Sys_UserGroupRole> list = JsonUtil.DeserializeJsonToList<Sys_UserGroupRole>(json);
                this._roleService.InsertUserGroupRole(list, userGroupId);
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
    }
}