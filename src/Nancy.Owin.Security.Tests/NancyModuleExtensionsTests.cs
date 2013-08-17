namespace Nancy.Owin.Security.Tests
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using global::Owin;
    using global::Owin.Testing;
    using Microsoft.Owin;
    using Microsoft.Owin.Infrastructure;
    using Microsoft.Owin.Security.Cookies;
    using Xunit;

    public class NancyModuleExtensionsTests
    {
        [Fact]
        public async Task When_attempt_to_access_secure_area_then_should_redirect_to_login_page()
        {
            OwinTestServer testServer = OwinTestServer.Create(builder =>
            {
                SignatureConversions.AddConversions(builder);
                var cookieAuthenticationOptions = new CookieAuthenticationOptions
                {
                    LoginPath = new PathString("/login"),
                    LogoutPath = new PathString("/logout")
                };
                builder.UseCookieAuthentication(cookieAuthenticationOptions);
                builder.UseNancy();
            });

            HttpClient httpClient = testServer.CreateHttpClient();

            HttpResponseMessage response = await httpClient.GetAsync("http://example.com/secured");
            response.StatusCode.Should().Be(HttpStatusCode.Found);
        }
    }
}