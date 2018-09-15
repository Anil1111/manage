using Manage.Core.Data;
using Manage.Core.Infrastructure;
using Manage.Core.Json;
using Manage.Data;
using Manage.Data.Domain;
using Manage.Service;
using Manage.Web.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Manage.Web.Core.Filter
{
    /// <summary>
    /// IAuthorizationFilter
    /// </summary>
    public class ActionAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }

            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;

            // 判断权限
            UserSession userSession = BaseExtensions.UserSession();
            if (userSession == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new ContentResult { Content = ResponseJson.UnLogin() };
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Login/Index");
                }
            }
            else
            {
                // 用户角色集合
                List<UserRoleVM> roleList = ServiceContainer.Resolve<IRoleService>().Entities(userSession.UserId);
                // 角色ID集合
                List<int> roleIdsByUser = roleList.Select(r => r.Role_Id).ToList();
                // 用户组角色集合
                List<UserRoleVM> roleListByGroupRole = ServiceContainer.Resolve<IRoleService>().EntitiesByGroupRole(userSession.UserId);
                List<int> roleIdsByUserGroup = roleListByGroupRole.Select(r => r.Role_Id).ToList();
                // 合并重复角色，获得当前用户涉及到的所有扮演的角色Role_Id
                roleIdsByUser.AddRange(roleIdsByUserGroup);
                var roleIds = roleIdsByUser.Distinct().ToList();
                // 权限表里面查询出这些角色涉及到的所有权限集合
                List<Sys_PermissionRole> permissions = ServiceContainer.Resolve<IRoleService>().GetPermissionRoleList()
                    .Where(t => roleIds.Contains(t.Role_Id))
                    .ToList();
                // 权限
                List<int> permissionId = permissions.Select(t => t.Permission_Id).ToList();

                IEnumerable<ModulePermissionVM> modulePermissionList = ServiceContainer.Resolve<IPermissionService>().GetModulePermissionList();
                List<ModulePermissionVM> list = modulePermissionList.Where(t => permissionId.Contains(t.Id) && t.code == actionName && t.LinkUrl.Contains(controllerName)).ToList();
                if (list == null || list.Count == 0)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new ContentResult { Content = ResponseJson.UnPower() };
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("/Home/NoPermissions");
                    }
                }
            }
        }
    }
}
