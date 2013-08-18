namespace SampleApp
{
    using System.Security.Principal;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Owin.Security;
    using Nancy.Responses;
    using SampleApp.Models;

    public class SampleModule : NancyModule
    {
        public SampleModule()
        {
            Get["/"] = _ =>
            {
                IOwinAuth owinAuth = Context.GetOwinAuth();
                var pageBase = new PageBase();
                if (owinAuth.User != null && owinAuth.User.Identity.IsAuthenticated)
                {
                    pageBase.Username =  owinAuth.User.Identity.Name;
                }
                return View["Index", pageBase];
            };

            Get["/cookies"] = _ => View["Cookies"];

            Post["/cookies"] = _ =>
            {
                var login = this.Bind<Login>();
                if (login.Username == "user" && login.Password == "pass")
                {
                    IOwinAuth owinAuth = Context.GetOwinAuth();
                    owinAuth.SignIn(new AuthenticationProperties(),
                        new GenericIdentity("User", CookieAuthenticationDefaults.AuthenticationType));
                    return new RedirectResponse("/");
                }
                return System.Net.HttpStatusCode.Unauthorized;
            };

            Get["/secured"] = _ =>
            {
                IOwinAuth owinAuth = Context.GetOwinAuth();
                if (owinAuth.User == null || !owinAuth.User.Identity.IsAuthenticated)
                {
                    return HttpStatusCode.Unauthorized;
                }
                return HttpStatusCode.OK;
            };
            Get["/signout"] = _ =>
            {
                Context.GetOwinAuth().SignOut();
                return Response.AsRedirect("/");
            };
        }

        private class Login
        {
            public string Username { get; set; }

            public string Password { get; set; }
        }
    }
}