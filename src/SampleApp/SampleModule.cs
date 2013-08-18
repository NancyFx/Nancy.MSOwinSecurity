namespace SampleApp
{
    using System.Security.Principal;
    using Microsoft.Owin;
    using Microsoft.Owin.Builder;
    using Microsoft.Owin.Infrastructure;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Owin.Security;
    using Owin;

    public class SampleModule : NancyModule
    {
        public SampleModule()
        {
            Get["/"] = _ =>
            {
                IOwinAuth owinAuth = Context.GetOwinAuth();
                if (owinAuth.User == null || !owinAuth.User.Identity.IsAuthenticated)
                {
                    return "Not authenticated";
                }
                return "Hello " + owinAuth.User.Identity.Name;
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
            Post["/login"] = _ =>
            {
                var login = this.Bind<Login>();
                if (login.Username == "user" && login.Password == "pass")
                {
                    IOwinAuth owinAuth = Context.GetOwinAuth();
                    owinAuth.SignIn(new AuthenticationProperties {RedirectUri = "/secured"},
                        new GenericIdentity("User", CookieAuthenticationDefaults.AuthenticationType));
                    return null;
                }
                return System.Net.HttpStatusCode.Unauthorized;
            };
            Get["/logout"] = _ =>
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

    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            SignatureConversions.AddConversions(builder);
            builder.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/login"),
                LogoutPath = new PathString("/logout"),
            });
            builder.UseNancy();
        }
    }
}