using Manage.Core.Caching;
using Manage.Core.Data;
using Manage.Core.Extend;
using Manage.Core.Pageing;
using Manage.Core.Utility;
using Manage.Data;
using Manage.Data.Data;
using Manage.Data.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Manage.Core.Encrypt;

namespace Manage.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<Sys_User> _userRepository;
        private readonly IRoleService _roleService;
        private readonly ICacheManager _cacheManager;
        public UserService(IRepository<Sys_User> userRepository, IRoleService roleService, ICacheManager cacheManager)
        {
            this._userRepository = userRepository;
            this._roleService = roleService;
            this._cacheManager = cacheManager;
        }

        public Page<Sys_User> FindPage(UserVM form)
        {
            Expression<Func<Sys_User, bool>> predicate = ExtLinq.True<Sys_User>();
            if (!string.IsNullOrEmpty(form.UserName))
            {
                predicate = predicate.And(s => s.UserName.Contains(form.UserName));
            }
            if (!string.IsNullOrEmpty(form.BeginDate))
            {
                DateTime dt = Ext.ToDate(form.BeginDate);
                predicate = predicate.And(s => s.UpdateDate >= dt);
            }
            if (!string.IsNullOrEmpty(form.EndDate))
            {
                DateTime dt = Ext.ToDate(form.EndDate);
                predicate = predicate.And(s => s.UpdateDate <= dt);
            }

            OrderModelField idOrder = new OrderModelField
            {
                PropertyName = "Id",
                IsDESC = false
            };
            OrderModelField[] orderByExpression = new OrderModelField[] {
               idOrder
            };

            Page<Sys_User> page = this._userRepository.FindPage(ContextDB.managerDBContext, predicate, orderByExpression, form.page, form.rows);

            return page;
        }

        public int Insert(UserVM form)
        {
            //string sql = "insert into Sys_User (UserName) values (@0)";
            //SqlParameter[] parameter = new SqlParameter[] {
            //    new SqlParameter("@0", SqlDbType.VarChar, 50)
            //};
            //parameter[0].Value = user.UserName;

            //return this.userRepository.ExecuteSqlCommand(ContextDB.managerDBContext, sql, parameter);
            //return this.userRepository.Insert(ContextDB.managerDBContext, user);

            //SqlParameter[] parameters = {
            //          new SqlParameter("@UserName", user.UserName),
            //          new SqlParameter("@Ret", SqlDbType.Int)
            //};
            //parameters[1].Direction = ParameterDirection.Output;

            //var result = ContextDB.managerDBContext.Database.ExecuteSqlCommand("exec proc_userInsert @UserName, @Ret out", parameters);
            //int ret = (int)parameters[1].Value;

            Sys_User model = new Sys_User();
            Ext.CopyFrom(model, form);
            model.UpdateDate = DateTime.Now;
            model.Password = MD5Encrypt.Encrypt(model.Password);

            return this._userRepository.Insert(ContextDB.managerDBContext, model);
        }

        public int Insert(List<Sys_User> entities)
        {
            return this._userRepository.Insert(ContextDB.managerDBContext, entities);
        }

        public int Delete(Sys_User user)
        {
            return this._userRepository.Delete(ContextDB.managerDBContext, t => t.Id == user.Id);
        }

        public List<Sys_User> Entities(string username)
        {
            //List<Sys_User> list = this.userRepository.Entities(ContextDB.managerDBContext, t => t.UserName == username);
            OrderModelField idOrder = new OrderModelField
            {
                PropertyName = "id",
                IsDESC = false
            };
            OrderModelField usernameOrder = new OrderModelField
            {
                PropertyName = "Password",
                IsDESC = false
            };

            OrderModelField[] orderByExpression = new OrderModelField[] {
               idOrder,
               usernameOrder
            };
            List<Sys_User> list = this._userRepository.Entities(ContextDB.managerDBContext, t => t.UserName == username, orderByExpression);

            return list;
        }

        public Sys_User GetUser(UserVM form)
        {
            return this._userRepository.Entity(ContextDB.managerDBContext, t => t.Id == form.Id);
        }

        public int Update(UserVM form)
        {
            //int ret = 0;
            //Sys_User user = this._userRepository.Entity(ContextDB.managerDBContext, t => t.UserName == username);
            //if (user != null)
            //{
            //    user.Password = "11";
            //    ret = this._userRepository.Update(ContextDB.managerDBContext, user);
            //}
            //return ret;
            Sys_User model = this._userRepository.Entity(ContextDB.managerDBContext, t => t.Id == form.Id);
            if (model != null)
            {
                model.UpdateDate = DateTime.Now;
                model.UserName = form.UserName;
                model.TrueName = form.TrueName;
                model.Enabled = form.Enabled;
            }

            return this._userRepository.Update(ContextDB.managerDBContext, model);
        }

        public void Login(UserVM form)
        {
            if (string.IsNullOrEmpty(form.UserName))
            {
                throw new BaseException(SuperConstants.AJAX_RETURN_STATE_ERROR, "用户名为空");
            }
            else if (string.IsNullOrEmpty(form.Password))
            {
                throw new BaseException(SuperConstants.AJAX_RETURN_STATE_ERROR, "密码为空");
            }
            else if (string.IsNullOrEmpty(form.CheckCode))
            {
                throw new BaseException(SuperConstants.AJAX_RETURN_STATE_ERROR, "验证码为空");
            }
            else
            {
                string code = Ext.ToString(HttpContext.Current.Session[SuperConstants.LOGIN_VALIDATE_CODE]);
                if (!code.ToLower().Equals(form.CheckCode.ToLower()))
                {
                    throw new BaseException(SuperConstants.AJAX_RETURN_STATE_ERROR, "验证码错误");
                }

                form.Password = MD5Encrypt.Encrypt(form.Password);
                Sys_User user = this._userRepository.Entity(ContextDB.managerDBContext, t => t.UserName == form.UserName && t.Password == form.Password);
                if (user == null)
                {
                    throw new BaseException(SuperConstants.AJAX_RETURN_STATE_ERROR, "用户名或密码错误");
                }

                UserSession us = new UserSession
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    TokenId = CommonUtil.GUID()
                };
                CookiesUtil.SetCookies("username", us.UserName);
                CookiesUtil.SetCookies(SuperConstants.COOKIESID, us.TokenId);
                this._cacheManager.Set(us.TokenId, us);
                List<ModuleVM> moduleList = GetModule(us);
                CreateMenu(moduleList);
            }
        }

        private List<ModuleVM> GetModule(UserSession us)
        {
            List<ModuleVM> parentMenuList = new List<ModuleVM>();
            // 用户角色集合
            List<UserRoleVM> roleList = this._roleService.Entities(us.UserId);
            // 角色ID集合
            List<int> roleIdsByUser = roleList.Select(r => r.Role_Id).ToList();
            // 用户组角色集合
            List<UserRoleVM> roleListByGroupRole = this._roleService.EntitiesByGroupRole(us.UserId);
            List<int> roleIdsByUserGroup = roleListByGroupRole.Select(r => r.Role_Id).ToList();
            // 合并重复角色，获得当前用户涉及到的所有扮演的角色Role_Id
            roleIdsByUser.AddRange(roleIdsByUserGroup);
            var roleIds = roleIdsByUser.Distinct().ToList();
            // 权限表里面查询出这些角色涉及到的所有权限集合
            List<Sys_PermissionRole> permissions = this._roleService.GetPermissionRoleList()
                .Where(t => roleIds.Contains(t.Role_Id))
                .ToList();
            List<int> permissionIds = permissions.Distinct().Select(p => p.Permission_Id).ToList();
            // 根据权限集合推导出用户涉及到的显示菜单也就是显示模块,因为每种权限都有一个所属菜单的字段
            List<Sys_Permission> permissionList = this._roleService.GetPermissionList()
                .Where(t => permissionIds.Contains(t.Id))
                .ToList();
            List<int> moduleIds = permissionList.Distinct().Select(t => t.ModuleId).ToList();

            List<Sys_Module> childModules = this._roleService.GetModuleList()
                .Where(t => moduleIds.Contains(t.Id) && t.Enabled == true)
                .Distinct()
                .ToList();
            if (childModules.Count > 0)
            {
                List<int> parentIds = childModules.Select(t => Convert.ToInt32(t.ParentId)).ToList();
                parentMenuList = this._roleService.GetModuleList()
                    .Where(t => parentIds.Contains(t.Id))
                    .Select(c => new ModuleVM { Id = c.Id, Name = c.Name, LinkUrl = c.LinkUrl, Code = c.Code, Icon = c.Icon })
                    .ToList();
                foreach (var item in parentMenuList.OrderBy(c => c.Code).ToList())
                {
                    var children = childModules.Where(c => c.ParentId == item.Id).OrderBy(c => c.Code).ToList();
                    if (children.Count > 0)
                    {
                        item.ChildModules = children;
                    }
                }
            }

            return parentMenuList;
        }

        private void CreateMenu(List<ModuleVM> moduleList)
        {
            string html = "";
            foreach (var item in moduleList)
            {
                html += "<li class=\"fitstMenu\" onclick=\"fitstMenuClick(this)\" text=\"" + item.Name + "\">";
                html += "<a href=\"#\" class=\"dropdown-toggle\"><i class=\"menu-icon " + item.Icon + "\"></i><span class=\"menu-text\"> " + item.Name + " </span><b class=\"arrow fa fa-angle-down\"></b></a><b class=\"arrow\"></b>";
                html += "<ul class=\"submenu\">";

                foreach (var childItem in item.ChildModules)
                {
                    html += "<li class=\"fitstMenuChild\" onclick=\"fitstMenuChildClick(this)\" text=\"" + childItem.Name + "\"><a href=\"" + childItem.LinkUrl + "\"><i class=\"menu-icon fa fa-caret-right\"></i>" + childItem.Name + "</a><b class=\"arrow\"></b></li>";
                }

                html += "</ul>";
                html += "</li>";
            }
            HttpContext.Current.Session[SuperConstants.MENUHTML] = html;
        }

        public int UserResetPwd(UserVM form)
        {
            Sys_User model = this._userRepository.Entity(ContextDB.managerDBContext, t => t.Id == form.Id);
            if (model != null)
            {
                model.Password = MD5Encrypt.Encrypt("123456");
            }

            return this._userRepository.Update(ContextDB.managerDBContext, model);
        }

        public int UpdatePwd(string oldPassword, string password, int userId)
        {
            Sys_User user = this._userRepository.Entity(ContextDB.managerDBContext, m => m.Id == userId);

            if (user.Password != MD5Encrypt.Encrypt(oldPassword))
                throw new BaseException(SuperConstants.AJAX_RETURN_STATE_ERROR, "原始密码错误");
            else
            {
                user.Password = MD5Encrypt.Encrypt(oldPassword);
                return this._userRepository.Update(ContextDB.managerDBContext, user);
            }
        }
    }
}
