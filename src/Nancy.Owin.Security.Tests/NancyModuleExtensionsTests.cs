namespace Nancy.Owin.Security.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using global::Owin;
    using global::Owin.Testing;
    using Microsoft.Owin.Infrastructure;
    using Microsoft.Owin.Security.Cookies;
    using Xunit.Extensions;

    public class NancyModuleExtensionsTests
    {
        public static IEnumerable<object[]> Repeat
        {
            get { return Enumerable.Repeat(1, 10).Select(i => new object[]{ i }); }
        }

        [Theory]
        [PropertyData("Repeat")]
        public async Task When_attempt_to_access_secure_area_Then_should_redirect_to_login_page(int _)
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

            HttpClient httpClient = testServer.CreateHttpClient();

            HttpResponseMessage response = await httpClient.GetAsync("http://example.com/secured");
            response.StatusCode.Should().Be(HttpStatusCode.Found);
        }
    }
}