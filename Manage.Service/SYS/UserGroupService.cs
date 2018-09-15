using Manage.Core.Data;
using Manage.Core.Extend;
using Manage.Core.Infrastructure;
using Manage.Core.Pageing;
using Manage.Data;
using Manage.Data.Data;
using Manage.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Manage.Service
{
    public class UserGroupService : IUserGroupService
    {
        private readonly IRepository<Sys_UserGroup> _userGroupRepository;
        public UserGroupService(IRepository<Sys_UserGroup> userGroupRepository)
        {
            this._userGroupRepository = userGroupRepository;
        }

        public Page<Sys_UserGroup> FindPage(UserGroupVM form)
        {
            Expression<Func<Sys_UserGroup, bool>> predicate = Ext.True<Sys_UserGroup>();
            OrderModelField idOrder = new OrderModelField
            {
                PropertyName = "Id",
                IsDESC = false
            };
            OrderModelField[] orderByExpression = new OrderModelField[] {
               idOrder
            };

            Page<Sys_UserGroup> page = this._userGroupRepository.FindPage(ContextDB.managerDBContext, predicate, orderByExpression, form.page, form.rows);

            return page;
        }

        public int Delete(UserGroupVM form)
        {
            return this._userGroupRepository.Delete(ContextDB.managerDBContext, t => t.Id == form.Id);
        }

        public Sys_UserGroup GetUserGroup(UserGroupVM form)
        {
            return this._userGroupRepository.Entity(ContextDB.managerDBContext, t => t.Id == form.Id);
        }

        public List<Sys_UserGroup> GetUserGroupList()
        {
            return this._userGroupRepository.Entities(ContextDB.managerDBContext, t => 1 == 1);
        }

        public int Insert(UserGroupVM form)
        {
            Sys_UserGroup model = new Sys_UserGroup();
            Ext.CopyFrom(model, form);
            model.UpdateDate = DateTime.Now;

            return this._userGroupRepository.Insert(ContextDB.managerDBContext, model);
        }

        public int Update(UserGroupVM form)
        {
            Sys_UserGroup model = this._userGroupRepository.Entity(ContextDB.managerDBContext, t => t.Id == form.Id);
            if (model != null)
            {
                model.UpdateDate = DateTime.Now;
                model.GroupName = form.GroupName;
                model.Description = form.Description;
                model.OrderSort = form.OrderSort;
                model.Enabled = form.Enabled;
            }

            return this._userGroupRepository.Update(ContextDB.managerDBContext, model);
        }
    }
}
