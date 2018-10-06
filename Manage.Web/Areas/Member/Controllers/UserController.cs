using AutoMapper;
using Manage.Core.Data;
using Manage.Core.Infrastructure;
using Manage.Core.Json;
using Manage.Core.Pageing;
using Manage.Core.Utility;
using Manage.Data;
using Manage.Data.Domain;
using Manage.Service;
using Manage.Web.Core.Filter;
using Manage.Web.Core.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Manage.Web.Areas.Member.Controllers
{
    [CustomAuthorizeFilterAttribute()]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IUserGroupService _userGroupService;
        private readonly IRoleService _roleService;
        private readonly MapperConfiguration _mapperConfiguration;
        public UserController(
            IUserService userService, 
            IUserGroupService userGroupService, 
            IRoleService roleService, 
            MapperConfiguration mapperConfiguration)
        {
            this._userService = userService;
            this._userGroupService = userGroupService;
            this._roleService = roleService;
            this._mapperConfiguration = mapperConfiguration;
        }

        [CustomExceptionFilterAttribute()]
        public ActionResult Index(UserVM form)
        {
            Page<Sys_User> page = this._userService.FindPage(form);
            this.SavePage(page, form, form.HidSelectPage);

            //SelectPage begin
            ViewBag.SelectUserName = form.SelectUserNameKey;
            //SelectPage end

            //List<UserVM> usersViewModelList = page.ResultList.AsQueryable().ProjectTo<UserVM>(_mapperConfiguration).ToList();

            return View();
        }

        [CustomExceptionFilterAttribute()]
        public ActionResult UserInsert()
        {
            Sys_User model = new Sys_User
            {
                Enabled = true
            };
            ViewBag.form = model;

            return View();
        }

        [HttpPost]
        public string UserInsert(UserVM form)
        {
            try
            {
                this._userService.Insert(form);
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

        [CustomExceptionFilterAttribute()]
        public ActionResult UserEdit(UserVM form)
        {
            Sys_User model = this._userService.GetUser(form);
            ViewBag.form = model;
            ViewData["type"] = "edit";

            return View("UserInsert");
        }

        [HttpPost]
        public string UserEdit(UserVM form, int type = 0)
        {
            try
            {
                this._userService.Update(form);
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
        public string UserDelete(UserVM form)
        {
            try
            {
                this._userService.Delete(form);
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

        [CustomExceptionFilterAttribute()]
        public ActionResult UserSetGroupUser(UserVM form)
        {
            List<Sys_UserGroup> list = this._userGroupService.GetUserGroupList()
                .Where(t => t.Enabled == true)
                .ToList();

            if (list != null && list.Count > 0)
            {
                List<Sys_UserGroupUser> userGroupUserList = this._roleService.GetUserGroupUserList();
                foreach (var item in list)
                {
                    var checkList = userGroupUserList.Where(t => t.UserGroup_Id == item.Id && t.User_Id == form.Id).ToList();
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

            ViewBag.GroupList = list;
            ViewBag.User_Id = form.Id;

            return View();
        }

        [HttpPost]
        public string UserSetGroupUser(string json, int userId)
        {
            try
            {
                List<Sys_UserGroupUser> list = JsonUtil.DeserializeJsonToList<Sys_UserGroupUser>(json);
                this._roleService.InsertUserGroupUser(list, userId);
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

        [CustomExceptionFilterAttribute()]
        public ActionResult UserSetRolesUser(UserVM form)
        {
            List<Sys_Role> list = this._roleService.GetRoleList()
            .Where(t => t.Enabled == true)
            .ToList();

            if (list != null && list.Count > 0)
            {
                List<Sys_RoleUser> userRoleUserList = this._roleService.GetRoleUserList();
                foreach (var item in list)
                {
                    var checkList = userRoleUserList.Where(t => t.Role_Id == item.Id && t.User_Id == form.Id).ToList();
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
            ViewBag.User_Id = form.Id;

            return View();
        }

        [HttpPost]
        public string UserSetRolesUser(string json, int userId)
        {
            try
            {
                List<Sys_RoleUser> list = JsonUtil.DeserializeJsonToList<Sys_RoleUser>(json);
                this._roleService.InsertRoleUser(list, userId);
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
        public string UserResetPwd(UserVM form)
        {
            try
            {
                this._userService.UserResetPwd(form);
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