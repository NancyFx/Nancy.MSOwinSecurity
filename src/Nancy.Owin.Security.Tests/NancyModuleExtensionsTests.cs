namespace Nancy.Owin.Security.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using FluentAssertions;
    using global::Owin;
    using global::Owin.Testing;
    using Microsoft.Owin;
    using Microsoft.Owin.Infrastructure;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Nancy.ModelBinding;
    using Xunit;
    using HttpStatusCode = System.Net.HttpStatusCode;

    public class NancyModuleExtensionsTests
    {
        private readonly HttpClient _httpClient;
        private CookieAuthenticationOptions _cookieAuthenticationOptions;

        public NancyModuleExtensionsTests()
        {
            OwinTestServer testServer = OwinTestServer.Create(builder =>
            {
                SignatureConversions.AddConversions(builder);
                _cookieAuthenticationOptions = new CookieAuthenticationOptions
                {
                    LoginPath = new PathString("/login"),
                    LogoutPath = new PathString("/logout"),
                };
                builder.UseCookieAuthentication(_cookieAuthenticationOptions);
                builder.UseNancy();
            });
            _httpClient = testServer.CreateHttpClient();
        }

        [Fact]
        public async Task When_attempt_to_access_secure_area_then_should_redirect_to_login_page()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://example.com/secured");
            response.StatusCode.Should().Be(HttpStatusCode.Found);
            response.Headers.Location.Should().NotBeNull();
        }

        [Fact]
        public async Task When_login_then_should_get_auth_cookie()
        {
            HttpResponseMessage response = await _httpClient.PostAsync("http://example.com/login", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Username", "user"), 
                new KeyValuePair<string, string>("Password", "pass")
            }));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Headers
                .Any(h => h.Key == "Set-Cookie" && h.Value.Single().StartsWith(_cookieAuthenticationOptions.CookieName))
                .Should().BeTrue();
        }

        [Fact]
        public async Task When_logged_in_then_should_get_be_able_to_access_secure_area()
        {
            await _httpClient.PostAsync("http://example.com/login", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Username", "user"), 
                new KeyValuePair<string, string>("Password", "pass")
            }));
            HttpResponseMessage response =  await _httpClient.GetAsync("http://example.com/secured");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public class TestModule : NancyModule
        {
            public TestModule()
            {
                Get["/"] = _ =>
                {
                    IOwinContext owinContext = Context.GetOwinContext();
                    if (owinContext.Request.User == null || !owinContext.Request.User.Identity.IsAuthenticated)
                    {
                        return "Not authenticated";
                    }
                    return "Hello " + owinContext.Request.User.Identity.Name;
                };
                Get["/secured"] = _ =>
                {
                    IOwinContext owinContext = Context.GetOwinContext();
                    if (owinContext.Request.User == null || !owinContext.Request.User.Identity.IsAuthenticated)
                    {
                        return Nancy.HttpStatusCode.Unauthorized;
                    }
                    return Nancy.HttpStatusCode.OK;
                };
                Post["/login"] = _ =>
                {
                    var login = this.Bind<Login>();
                    if (login.Username == "user" && login.Password == "pass")
                    {
                        var authentication = Context.GetOwinContext().Authentication;
                        authentication.SignIn(
                            new AuthenticationProperties { RedirectUri = "/secured" },
                            new GenericIdentity("User", CookieAuthenticationDefaults.AuthenticationType));
                        return null;
                    }
                    return HttpStatusCode.Unauthorized;
                };
                Get["/logout"] = _ =>
                {
                    Context.GetAuthenticationManager().SignOut();
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
}