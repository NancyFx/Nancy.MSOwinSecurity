namespace SampleApp
{
    using Microsoft.Owin.Infrastructure;
    using Microsoft.Owin.Security.Cookies;
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            SignatureConversions.AddConversions(builder);
            builder.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = "/cookies",
                LogoutPath = "/logout",
            });
            builder.UseNancy(opt => opt.Bootstrapper = new SampleAppBootstrapper());
        }
    }
}