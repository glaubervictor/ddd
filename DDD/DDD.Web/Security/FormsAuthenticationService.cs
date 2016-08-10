using System;
using System.Web;
using System.Web.Security;

namespace DDD.Web.Security
{
    public class FormsAuthenticationService
    {
        public static void SignIn(int version, string userName, bool createPersistentCookie, string UserData)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException("userName não pode ser nulo.", nameof(userName));

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(version, userName, DateTime.Now, DateTime.Now.AddMinutes(40.00), createPersistentCookie, UserData, FormsAuthentication.FormsCookiePath);
            string encTicket = FormsAuthentication.Encrypt(authTicket);

            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            HttpContext.Current.Response.Cookies.Add(faCookie);
        }

        public static void SignOut()
        {
            HttpContext.Current.Session.Clear();
            FormsAuthentication.SignOut();
        }
    }
}