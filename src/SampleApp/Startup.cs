namespace SampleApp
{
    using Microsoft.Owin;
    using Microsoft.Owin.Infrastructure;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.Jwt;
    using Microsoft.Owin.Security.OAuth;
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            SignatureConversions.AddConversions(builder);
            builder
                .UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    LoginPath = new PathString("/cookies"),
                    LogoutPath = new PathString("/logout"),
                })
                .UseMicrosoftAccountAuthentication("000000004810984D", "tmUEupwT4jLWg8XRbPhEbIyi0ZNPMTKk")
                .UseFacebookAuthentication("appid", "appsecret")
                .UseGoogleAuthentication()
                .UseTwitterAuthentication("consumerkey", "consumersecret")
                .UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions())
                .UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions())
                .UseNancy(opt => opt.Bootstrapper = new SampleAppBootstrapper());
        }
    }
}