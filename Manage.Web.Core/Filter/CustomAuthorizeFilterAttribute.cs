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
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class CustomAuthorizeFilterAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }

            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute),true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;//��ʾ֧�ֿ�������action��AllowAnonymousAttribute
            }

            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;

            // �ж�Ȩ��
            UserSession userSession = BaseExtensions.UserSession();
            if (userSession == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new ContentResult
                    {
                        Content = JsonUtil.SerializerObject(new ReturnResult(SuperConstants.AJAX_RETURN_STATE_LOGIN, "δ��¼"))
                    };
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Login/Index");
                }
            }
            else
            {
                // �û���ɫ����
                List<UserRoleVM> roleList = ServiceContainer.Resolve<IRoleService>().Entities(userSession.UserId);
                // ��ɫID����
                List<int> roleIdsByUser = roleList.Select(r => r.Role_Id).ToList();
                // �û����ɫ����
                List<UserRoleVM> roleListByGroupRole = ServiceContainer.Resolve<IRoleService>().EntitiesByGroupRole(userSession.UserId);
                List<int> roleIdsByUserGroup = roleListByGroupRole.Select(r => r.Role_Id).ToList();
                // �ϲ��ظ���ɫ����õ�ǰ�û��漰�������а��ݵĽ�ɫRole_Id
                roleIdsByUser.AddRange(roleIdsByUserGroup);
                var roleIds = roleIdsByUser.Distinct().ToList();
                // Ȩ�ޱ������ѯ����Щ��ɫ�漰��������Ȩ�޼���
                List<Sys_PermissionRole> permissions = ServiceContainer.Resolve<IRoleService>().GetPermissionRoleList()
                    .Where(t => roleIds.Contains(t.Role_Id))
                    .ToList();
                // Ȩ��
                List<int> permissionId = permissions.Select(t => t.Permission_Id).ToList();

                IEnumerable<ModulePermissionVM> modulePermissionList = ServiceContainer.Resolve<IPermissionService>().GetModulePermissionList();
                List<ModulePermissionVM> list = modulePermissionList.Where(t => permissionId.Contains(t.Id) && t.code == actionName && t.LinkUrl.Contains(controllerName)).ToList();
                if (list == null || list.Count == 0)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new ContentResult
                        {
                            Content = JsonUtil.SerializerObject(new ReturnResult(SuperConstants.AJAX_RETURN_STATE_LOGIN, "û��Ȩ��"))
                        };
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
