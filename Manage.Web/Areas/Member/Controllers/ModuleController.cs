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
    [ActionAuthorizeAttribute()]
    public class ModuleController : BaseController
    {
        private readonly IModuleService _moduleService;
        public ModuleController(IModuleService moduleService)
        {
            this._moduleService = moduleService;
        }

        // GET: Member/Module
        public ActionResult Index(ModuleVM form)
        {
            try
            {
                Page<Sys_Module> page = this._moduleService.FindPage(form);
                this.SavePage(page, form);
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }

            return View();
        }

        public ActionResult ModuleInsert()
        {
            try
            {
                List<Sys_Module> list = this._moduleService.GetModuleList().Where(t => t.ParentId == null || t.ParentId == 0).ToList();
                ViewData["ModuleList"] = list;

                Sys_Module model = new Sys_Module
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

        public ActionResult ModuleEdit(ModuleVM form)
        {
            try
            {
                List<Sys_Module> list = this._moduleService.GetModuleList().Where(t => t.ParentId == null || t.ParentId == 0).ToList();
                ViewData["ModuleList"] = list;

                Sys_Module model = this._moduleService.GetModule(form);
                ViewBag.form = model;
                ViewData["type"] = "edit";
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }

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