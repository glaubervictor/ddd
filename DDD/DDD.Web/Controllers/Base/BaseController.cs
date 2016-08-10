using DDD.Application.Base;
using DDD.Infra.Session;
using System.Security.Authentication;
using System.Web.Mvc;

namespace DDD.Web.Controllers.Base
{
    public class BaseController : Controller
    {
        protected readonly IAppServiceFactory _appServiceFactory;
        protected readonly ISessionHandler _sessionHandler;

        protected IAppServiceFactory AppServiceFactory
        {
            get { return _appServiceFactory; }
        }

        protected ISessionHandler SessionHandler
        {
            get { return _sessionHandler; }
        }

        public BaseController(IAppServiceFactory appServiceFactory, ISessionHandler sessionHandler)
        {
            _appServiceFactory = appServiceFactory;
            _sessionHandler = sessionHandler;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (!System.Web.HttpContext.Current.IsDebuggingEnabled)
            {
                if (filterContext.Exception is AuthenticationException)
                {
                    if (Request.IsAjaxRequest())
                    {

                    }
                    else
                    {
                        filterContext.ExceptionHandled = true;
                        filterContext.Result = RedirectToAction("NoAccess", "Error");
                    }
                }

                if (filterContext.Exception is InvalidCredentialException)
                {
                    if (Request.IsAjaxRequest())
                    {

                    }
                    else
                    {
                        filterContext.ExceptionHandled = true;
                        filterContext.Result = RedirectToAction("NoAccess", "Error");
                    }
                }

            }
            base.OnException(filterContext);
        }
    }
}