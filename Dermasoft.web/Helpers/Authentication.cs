using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Dermasoft.web.Helpers
{
    public class Authentication
    {
        private static Authentication _instanceAuthtenticate;
        private Authentication() { }
        public static Authentication Instance
        {
            get { return _instanceAuthtenticate ?? new Authentication(); }
        }

        public HttpCookie CreateCookie(string userName)
        {
            string userData = string.Join("|", userName);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                userName,
                DateTime.Now,
                DateTime.Now.AddHours(2),
                true,
                FormsAuthentication.FormsCookiePath
                );
            string encryptedTicked = FormsAuthentication.Encrypt(ticket);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicked);
            cookie.HttpOnly = true;
            return cookie;
            
        }
    }
}