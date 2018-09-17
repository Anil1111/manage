using Manage.Core.Data;
using Manage.Core.Extend;
using Manage.Core.Infrastructure;
using Manage.Core.Pageing;
using Manage.Data;
using Manage.Data.Data;
using Manage.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace Manage.Service
{
    public class ModuleService : IModuleService
    {
        private readonly IRepository<Sys_Module> _moduleRepository;
        private readonly IRepository<Sys_Permission> _permissionRepository;
        public ModuleService(
            IRepository<Sys_Module> moduleRepository, 
            IRepository<Sys_Permission> permissionRepository)
        {
            this._moduleRepository = moduleRepository;
            this._permissionRepository = permissionRepository;
        }

        public void Delete(ModuleVM form)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                this._moduleRepository.Delete(ContextDB.managerDBContext, t => t.Id == form.Id);
                this._permissionRepository.Delete(ContextDB.managerDBContext, t => t.ModuleId == form.Id);
                scope.Complete();
            }
        }

        public Page<Sys_Module> FindPage(ModuleVM form)
        {
            Expression<Func<Sys_Module, bool>> predicate = Ext.True<Sys_Module>();
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

            OrderModelField idOrder = new OrderModelField
            {
                PropertyName = "Id",
                IsDESC = false
            };
            OrderModelField[] orderByExpression = new OrderModelField[] {
               idOrder
            };

            Page<Sys_Module> page = this._moduleRepository.FindPage(ContextDB.managerDBContext, predicate, orderByExpression, form.page, form.rows);
            if (page != null & page.ResultList.Count > 0)
            {
                List<Sys_Module> list = this._moduleRepository.Entities(ContextDB.managerDBContext, t => 1 == 1);
                if (list != null && list.Count > 0)
                {
                    foreach (var item in page.ResultList)
                    {
                        Sys_Module model = list.Where(t => t.Id == item.ParentId).SingleOrDefault();
                        if (model != null)
                        {
                            item.ParentName = model.Name;
                        }
                    }
                }
            }
            return page;
        }

        public Sys_Module GetModule(ModuleVM form)
        {
            return this._moduleRepository.Entity(ContextDB.managerDBContext, t => t.Id == form.Id);
        }

        public List<Sys_Module> GetModuleList()
        {
            return this._moduleRepository.Entities(ContextDB.managerDBContext, t => 1 == 1);
        }

        public int Insert(ModuleVM form)
        {
            if (string.IsNullOrEmpty(form.Name))
            {
                throw new BaseException(SuperConstants.AJAX_RETURN_STATE_ERROR, "模块名称为空");
            }
            if (form.ParentId == 0)
            {
                throw new BaseException(SuperConstants.AJAX_RETURN_STATE_ERROR, "上级模块为空");
            }
            if (string.IsNullOrEmpty(form.LinkUrl))
            {
                throw new BaseException(SuperConstants.AJAX_RETURN_STATE_ERROR, "链接地址为空");
            }
            if (form.Code == 0)
            {
                throw new BaseException(SuperConstants.AJAX_RETURN_STATE_ERROR, "模块编号为空");
            }

            Sys_Module module = new Sys_Module();
            Ext.CopyFrom(module, form);
            module.IsMenu = true;
            module.UpdateDate = DateTime.Now;

            return this._moduleRepository.Insert(ContextDB.managerDBContext, module);
        }

        public int Update(ModuleVM form)
        {
            Sys_Module module = this._moduleRepository.Entity(ContextDB.managerDBContext, t => t.Id == form.Id);
            if (module != null)
            {
                module.UpdateDate = DateTime.Now;
                module.Name = form.Name;
                module.ParentId = form.ParentId;
                module.LinkUrl = form.LinkUrl;
                module.Code = form.Code;
                module.Enabled = form.Enabled;
            }

            return this._moduleRepository.Update(ContextDB.managerDBContext, module);
        }
    }
}
