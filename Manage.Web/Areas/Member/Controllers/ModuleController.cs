using Manage.Core.Data;
using Manage.Core.Infrastructure;
using Manage.Core.Json;
using Manage.Core.Pageing;
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
    public class ModuleController : BaseController
    {
        private readonly IModuleService _moduleService;
        public ModuleController(IModuleService moduleService)
        {
            this._moduleService = moduleService;
        }

        [CustomExceptionFilterAttribute()]
        public ActionResult Index(ModuleVM form)
        {
            Page<Sys_Module> page = this._moduleService.FindPage(form);
            this.SavePage(page, form);

            return View();
        }

        [CustomExceptionFilterAttribute()]
        public ActionResult ModuleInsert()
        {
            List<Sys_Module> list = this._moduleService.GetModuleList().Where(t => t.ParentId == null || t.ParentId == 0).ToList();
            ViewData["ModuleList"] = list;

            Sys_Module model = new Sys_Module
            {
                Enabled = true
            };
            ViewBag.form = model;

            return View();
        }

        [HttpPost]
        public string ModuleInsert(ModuleVM form)
        {
            try
            {
                this._moduleService.Insert(form);
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
        public ActionResult ModuleEdit(ModuleVM form)
        {
            List<Sys_Module> list = this._moduleService.GetModuleList().Where(t => t.ParentId == null || t.ParentId == 0).ToList();
            ViewData["ModuleList"] = list;

            Sys_Module model = this._moduleService.GetModule(form);
            ViewBag.form = model;
            ViewData["type"] = "edit";

            return View("ModuleInsert");
        }

        [HttpPost]
        public string ModuleEdit(ModuleVM form, int type = 0)
        {
            try
            {
                this._moduleService.Update(form);
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
        public string ModuleDelete(ModuleVM form)
        {
            try
            {
                this._moduleService.Delete(form);
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