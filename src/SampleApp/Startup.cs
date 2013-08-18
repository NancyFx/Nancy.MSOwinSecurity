namespace SampleApp
{
    using Microsoft.Owin;
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
                LoginPath = new PathString("/cookies"),
                LogoutPath = new PathString("/logout"),
            });
            builder.UseNancy(new SampleAppBootstrapper());
        }
    }
}