using AutoMapper;
using AutoMapper.QueryableExtensions;
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

        // GET: Member/User
        public ActionResult Index(UserVM form)
        {
            try
            {
                Page<Sys_User> page = this._userService.FindPage(form);
                this.SavePage(page, form);

                //List<UserVM> usersViewModelList = page.ResultList.AsQueryable().ProjectTo<UserVM>(_mapperConfiguration).ToList();
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }

            return View();
        }

        public ActionResult UserInsert()
        {
            try
            {
                Sys_User model = new Sys_User
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
        public string UserInsert(UserVM form)
        {
            try
            {
                this._userService.Insert(form);
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

        public ActionResult UserEdit(UserVM form)
        {
            try
            {
                Sys_User model = this._userService.GetUser(form);
                ViewBag.form = model;
                ViewData["type"] = "edit";
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }

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
        public string UserDelete(UserVM form)
        {
            try
            {
                this._userService.Delete(form);
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

        public ActionResult UserSetGroupUser(UserVM form)
        {
            try
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
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }

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
                _logger.Info(ex.GetMsg());
                return ResponseJson.Error(ex.GetExceptionFlag(), ex.GetMsg());
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
                return ResponseJson.Error(ex.Message);
            }
        }

        public ActionResult UserSetRolesUser(UserVM form)
        {
            try
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
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }

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
        public string UserResetPwd(UserVM form)
        {
            try
            {
                this._userService.UserResetPwd(form);
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