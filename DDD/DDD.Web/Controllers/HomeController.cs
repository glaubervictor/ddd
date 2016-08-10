using DDD.Web.Controllers.Base;
using System.Web.Mvc;
using DDD.Application.Base;
using DDD.Infra.Session;
using DDD.DTO;
using System;
using DDD.Web.Helpers;

namespace DDD.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IAppServiceFactory appServiceFactory, ISessionHandler sessionHandler) 
            : base(appServiceFactory, sessionHandler)
        {
        }

        public ActionResult Index()
        {
            AppServiceFactory.TenantAppService.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult Index(TenantDTO tenantDTO)
        {
            try
            {
                AppServiceFactory.TenantAppService.Add(tenantDTO);
            }
            catch (Exception ex)
            {
               ControllerError.Processing(this, ex, false);
            }
            
            return RedirectToAction("index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}