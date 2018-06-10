using Manage.Core.Data;
using Manage.Core.Extend;
using Manage.Core.Pageing;
using Manage.Data;
using Manage.Data.Data;
using Manage.Data.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace Manage.Service
{
    public class PermissionService : IPermissionService
    {
        private readonly IRepository<Sys_Permission> _permissionRepository;
        private readonly IRepository<Sys_Module> _moduleRepository;
        private readonly IRepository<Sys_PermissionRole> _permissionRoleRepository;
        public PermissionService(IRepository<Sys_Permission> permissionRepository, IRepository<Sys_Module> moduleRepository, IRepository<Sys_PermissionRole> permissionRoleRepository)
        {
            this._permissionRepository = permissionRepository;
            this._moduleRepository = moduleRepository;
            this._permissionRoleRepository = permissionRoleRepository;
        }

        public Page<Sys_Permission> FindPage(PermissionVM form)
        {
            Expression<Func<Sys_Permission, bool>> predicate = ExtLinq.True<Sys_Permission>();
            if (!string.IsNullOrEmpty(form.Name))
            {
                predicate = predicate.And(s => s.Name.Contains(form.Name));
            }
            if (!string.IsNullOrEmpty(form.EnabledStr))
            {
                if (form.EnabledStr.Equals("1"))
                {
                    predicate = predicate.And(s => s.Enabled == true);
                }
                else
                {
                    predicate = predicate.And(s => s.Enabled == false);
                }
            }
            if (!form.ModuleId.Equals(0))
            {
                predicate = predicate.And(s => s.ModuleId == form.ModuleId);
            }

            OrderModelField idOrder = new OrderModelField
            {
                PropertyName = "Id",
                IsDESC = false
            };
            OrderModelField[] orderByExpression = new OrderModelField[] {
               idOrder
            };

            Page<Sys_Permission> page = this._permissionRepository.FindPage(ContextDB.managerDBContext, predicate, orderByExpression, form.page, form.rows);
            if (page != null & page.ResultList.Count > 0)
            {
                List<Sys_Module> list = this._moduleRepository.Entities(ContextDB.managerDBContext, t => 1 == 1);
                if (list != null && list.Count > 0)
                {
                    foreach (var item in page.ResultList)
                    {
                        Sys_Module model = list.Where(t => t.Id == item.ModuleId).SingleOrDefault();
                        if (model != null)
                        {
                            item.ModuleName = model.Name;
                        }
                    }
                }
            }

            return page;
        }

        public void Delete(PermissionVM form)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                this._permissionRepository.Delete(ContextDB.managerDBContext, t => t.Id == form.Id);
                this._permissionRoleRepository.Delete(ContextDB.managerDBContext, t => t.Permission_Id == form.Id);
                scope.Complete();
            }
        }

        public Sys_Permission GetPermission(PermissionVM form)
        {
            return this._permissionRepository.Entity(ContextDB.managerDBContext, t => t.Id == form.Id);
        }

        public List<Sys_Permission> GetPermissionList()
        {
            return this._permissionRepository.Entities(ContextDB.managerDBContext, t => 1 == 1);
        }

        public int Insert(PermissionVM form)
        {
            Sys_Permission module = new Sys_Permission();
            Ext.CopyFrom(module, form);
            module.UpdateDate = DateTime.Now;

            return this._permissionRepository.Insert(ContextDB.managerDBContext, module);
        }

        public int Update(PermissionVM form)
        {
            Sys_Permission permission = this._permissionRepository.Entity(ContextDB.managerDBContext, t => t.Id == form.Id);
            if (permission != null)
            {
                permission.UpdateDate = DateTime.Now;
                permission.Name = form.Name;
                permission.Code = form.Code;
                permission.Description = form.Description;
                permission.Enabled = form.Enabled;
            }

            return this._permissionRepository.Update(ContextDB.managerDBContext, permission);
        }

        public IEnumerable<ModulePermissionVM> GetModulePermissionList()
        {
            string sql = "select a.code, LinkUrl, a.Id from Sys_Permission a, Sys_Module b where a.ModuleId = b.Id";
            var t = ContextDB.managerDBContext.Database.SqlQuery<ModulePermissionVM>(sql);
            return t.ToList();
        }
    }
}
