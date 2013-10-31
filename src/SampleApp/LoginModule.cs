namespace SampleApp
{
    using System.Security.Principal;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Owin.Security;
    using Nancy.Responses;
    using SampleApp.Annotations;

    public class LoginModule : NancyModule
    {
        public LoginModule()
        {
            Get["/login"] = _ => View["Login"];

            Post["/login"] = _ =>
            {
                var login = this.Bind<Login>();
                if (login.Username == "user" && login.Password == "pass")
                {
                    var authManager = Context.GetAuthenticationManager();
                    authManager.SignIn(new AuthenticationProperties(),
                        new GenericIdentity("User", CookieAuthenticationDefaults.AuthenticationType));
                    return new RedirectResponse("/");
                }
                Context.GetAuthenticationManager().SignOut();
                return HttpStatusCode.Unauthorized;
            };
        }

        [UsedImplicitly]
        private class Login
        {
            public string Username { get; set; }

            public string Password { get; set; }
        }
    }
}