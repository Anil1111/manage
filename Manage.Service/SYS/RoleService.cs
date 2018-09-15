using Manage.Core.Data;
using Manage.Core.Extend;
using Manage.Core.Infrastructure;
using Manage.Core.Pageing;
using Manage.Data;
using Manage.Data.Data;
using Manage.Data.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace Manage.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<Sys_Role> _roleRepository;
        private readonly IRepository<Sys_PermissionRole> _permissionRoleRepository;
        private readonly IRepository<Sys_Module> _moduleRoleRepository;
        private readonly IRepository<Sys_UserGroupRole> _userGroupRoleRepository;
        private readonly IModuleService _moduleService;
        private readonly IRepository<Sys_Permission> _permissionRepository;
        private readonly IRepository<Sys_UserGroupUser> _userGroupUserRepository;
        private readonly IRepository<Sys_RoleUser> _roleUserRepository;
        public RoleService(IRepository<Sys_Role> roleRepository, IRepository<Sys_PermissionRole> permissionRoleRepository, IRepository<Sys_Module> moduleRoleRepository, IRepository<Sys_UserGroupRole> userGroupRoleRepository, IModuleService moduleService, IRepository<Sys_Permission> permissionRepository, IRepository<Sys_UserGroupUser> userGroupUserRepository, IRepository<Sys_RoleUser> roleUserRepository)
        {
            this._roleRepository = roleRepository;
            this._permissionRoleRepository = permissionRoleRepository;
            this._moduleRoleRepository = moduleRoleRepository;
            this._userGroupRoleRepository = userGroupRoleRepository;
            this._moduleService = moduleService;
            this._permissionRepository = permissionRepository;
            this._userGroupUserRepository = userGroupUserRepository;
            this._roleUserRepository = roleUserRepository;
        }

        public int Delete(UserRoleVM form)
        {
            return this._roleRepository.Delete(ContextDB.managerDBContext, t => t.Id == form.Id);
        }

        public List<UserRoleVM> Entities(int userId)
        {
            string sql = "select * from Sys_Role a, Sys_RoleUser b where a.id = b.Role_Id and b.User_Id = @0";
            SqlParameter[] parameter = new SqlParameter[] {
                new SqlParameter("@0", SqlDbType.Int, 4)
            };
            parameter[0].Value = userId;

            return ContextDB.managerDBContext.Database.SqlQuery<UserRoleVM>(sql, parameter).ToList();
        }

        public List<UserRoleVM> EntitiesByGroupRole(int userId)
        {
            string sql = "select * from Sys_UserGroup a, Sys_UserGroupRole b, Sys_UserGroupUser c where a.id = b.UserGroup_Id and b.UserGroup_Id = c.UserGroup_Id and c.User_Id = @0";
            SqlParameter[] parameter = new SqlParameter[] {
                new SqlParameter("@0", SqlDbType.Int, 4)
            };
            parameter[0].Value = userId;

            return ContextDB.managerDBContext.Database.SqlQuery<UserRoleVM>(sql, parameter).ToList();
        }

        public Page<Sys_Role> FindPage(UserRoleVM form)
        {
            Expression<Func<Sys_Role, bool>> predicate = Ext.True<Sys_Role>();
            OrderModelField idOrder = new OrderModelField
            {
                PropertyName = "Id",
                IsDESC = false
            };
            OrderModelField[] orderByExpression = new OrderModelField[] {
               idOrder
            };

            Page<Sys_Role> page = this._roleRepository.FindPage(ContextDB.managerDBContext, predicate, orderByExpression, form.page, form.rows);

            return page;
        }

        public List<Sys_Module> GetModuleList()
        {
            return this._moduleRoleRepository.Entities(ContextDB.managerDBContext, t => 1 == 1);
        }

        public List<Sys_PermissionRole> GetPermissionRoleList()
        {
            return this._permissionRoleRepository.Entities(ContextDB.managerDBContext, t => 1 == 1);
        }

        public Sys_Role GetRole(UserRoleVM form)
        {
            return this._roleRepository.Entity(ContextDB.managerDBContext, t => t.Id == form.Id);
        }

        public List<Sys_Role> GetRoleList()
        {
            return this._roleRepository.Entities(ContextDB.managerDBContext, t => 1 == 1);
        }

        public List<Sys_UserGroupRole> GetUserGroupRoleList()
        {
            return this._userGroupRoleRepository.Entities(ContextDB.managerDBContext, t => 1 == 1);
        }

        public int Insert(UserRoleVM form)
        {
            Sys_Role model = new Sys_Role();
            Ext.CopyFrom(model, form);
            model.UpdateDate = DateTime.Now;

            return this._roleRepository.Insert(ContextDB.managerDBContext, model);
        }

        public void InsertUserGroupRole(List<Sys_UserGroupRole> list, int userGroupId)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                this._userGroupRoleRepository.Delete(ContextDB.managerDBContext, t => t.UserGroup_Id == userGroupId);
                if (list != null && list.Count > 0)
                {
                    this._userGroupRoleRepository.Insert(ContextDB.managerDBContext, list);
                }
                scope.Complete();
            }
        }

        public Root GetTree(int roleId)
        {
            Root root = new Root();
            List<Parent> parentRoot = new List<Parent>();

            // 获取模块
            List<Sys_Module> moduleList = this._moduleService.GetModuleList();
            // 父节点
            List<Sys_Module> parentList = moduleList.Where(t => t.ParentId == null || t.ParentId == 0)
                .ToList();
            // 权限
            List<Sys_Permission> permissionList = this._permissionRepository.Entities(ContextDB.managerDBContext, t => 1 == 1);
            List<Sys_PermissionRole> permissionRoleList = this.GetPermissionRoleList();

            foreach (var item in parentList)
            {
                Parent parentItem = new Parent()
                {
                    name = item.Name,
                    open = true
                };
                // children
                List<Children> childrenRoot = new List<Children>();
                List<Sys_Module> childrenList = moduleList.Where(t => t.ParentId == item.Id).ToList();
                foreach (var childItem in childrenList)
                {
                    Children children = new Children
                    {
                        name = childItem.Name
                    };

                    List<ChildrenItem> childRoot = new List<ChildrenItem>();
                    List<Sys_Permission> pList = permissionList.Where(t => t.ModuleId == childItem.Id).ToList();
                    foreach (var permissionItem in pList)
                    {
                        ChildrenItem childrenItem = new ChildrenItem()
                        {
                            name = permissionItem.Name,
                            Role_Id = roleId,
                            Permission_Id = permissionItem.Id
                        };
                        if (permissionRoleList.Where(t => t.Role_Id == roleId && t.Permission_Id == permissionItem.Id).ToList().Count > 0)
                        {
                            childrenItem.Checked = true;
                        }
                        childRoot.Add(childrenItem);
                    }

                    children.children = childRoot;
                    childrenRoot.Add(children);
                }
                parentItem.children = childrenRoot;
                parentRoot.Add(parentItem);
            }

            root.parent = parentRoot;

            return root;
        }

        public void InsertPermissionRole(List<Sys_PermissionRole> list)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                int id = list[0].Role_Id;
                this._permissionRoleRepository.Delete(ContextDB.managerDBContext, t => t.Role_Id == id);
                this._permissionRoleRepository.Insert(ContextDB.managerDBContext, list);
                scope.Complete();
            }
        }

        public List<Sys_UserGroupUser> GetUserGroupUserList()
        {
            return this._userGroupUserRepository.Entities(ContextDB.managerDBContext, t => 1 == 1);
        }

        public void InsertUserGroupUser(List<Sys_UserGroupUser> list, int userId)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                this._userGroupUserRepository.Delete(ContextDB.managerDBContext, t => t.User_Id == userId);
                if (list != null && list.Count > 0)
                {
                    this._userGroupUserRepository.Insert(ContextDB.managerDBContext, list);
                }
                scope.Complete();
            }
        }

        public List<Sys_RoleUser> GetRoleUserList()
        {
            return this._roleUserRepository.Entities(ContextDB.managerDBContext, t => 1 == 1);
        }

        public void InsertRoleUser(List<Sys_RoleUser> list, int userId)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                this._roleUserRepository.Delete(ContextDB.managerDBContext, t => t.User_Id == userId);
                if (list != null && list.Count > 0)
                {
                    this._roleUserRepository.Insert(ContextDB.managerDBContext, list);
                }
                scope.Complete();
            }
        }

        public List<Sys_Permission> GetPermissionList()
        {
            return this._permissionRepository.Entities(ContextDB.managerDBContext, t => 1 == 1);
        }

        public int Update(UserRoleVM form)
        {
            Sys_Role model = this._roleRepository.Entity(ContextDB.managerDBContext, t => t.Id == form.Id);
            if (model != null)
            {
                model.UpdateDate = DateTime.Now;
                model.RoleName = form.RoleName;
                model.Description = form.Description;
                model.OrderSort = form.OrderSort;
                model.Enabled = form.Enabled;
            }

            return this._roleRepository.Update(ContextDB.managerDBContext, model);
        }
    }
}
