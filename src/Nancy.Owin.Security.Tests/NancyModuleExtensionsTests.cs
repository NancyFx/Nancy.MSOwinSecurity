namespace Nancy.Owin.Security.Tests
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Owin;
    using Microsoft.Owin.Infrastructure;
    using Microsoft.Owin.Security.Cookies;
    using Xunit;
    using global::Owin;
    using global::Owin.Testing;

    public class NancyModuleExtensionsTests
    {
        [Fact]
        public async Task When_attempt_to_access_secure_area_Then_should_redirect_to_login_page()
        {
            OwinTestServer testServer = OwinTestServer.Create(builder =>
            {
                SignatureConversions.AddConversions(builder);
                var cookieAuthenticationOptions = new CookieAuthenticationOptions
                {
                    LoginPath = "/login",
                    LogoutPath = "/logout"
                };
                builder.UseCookieAuthentication(cookieAuthenticationOptions);
                builder.UseNancy();
            });

            HttpClient httpClient = testServer.CreateHttpClient(env =>
            {
                var owinContext = new OwinContext(env);
                owinContext.Response.OnSendingHeaders(o => { }, null);
            });

            HttpResponseMessage response = await httpClient.GetAsync("http://example.com/secured");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Found);
        }
    }
}