using System;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace DDD.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Bootstrapper.Run();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            if (authCookie == null) return;

            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            string[] UserData = authTicket.UserData.Split(new char[] { ';' });

            GenericIdentity userIdentity = new GenericIdentity(authTicket.Name);
            GenericPrincipal userPrincipal = new GenericPrincipal(userIdentity, UserData);
            Context.User = userPrincipal;
        }
    }
}
