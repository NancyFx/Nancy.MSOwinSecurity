namespace Nancy.Owin.Security.Tests
{
    using System.Collections.Generic;
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
    using HttpStatusCode = Nancy.HttpStatusCode;

    public class NancyModuleExtensionsTests
    {
        private readonly HttpClient _httpClient;

        public NancyModuleExtensionsTests()
        {
            OwinTestServer testServer = OwinTestServer.Create(builder =>
            {
                SignatureConversions.AddConversions(builder);
                var cookieAuthenticationOptions = new CookieAuthenticationOptions
                {
                    LoginPath = new PathString("/login"),
                    LogoutPath = new PathString("/logout"),
                };
                builder.UseCookieAuthentication(cookieAuthenticationOptions);
                builder.UseNancy();
            });
            _httpClient = testServer.CreateHttpClient();
        }

        [Fact]
        public async Task When_attempt_to_access_secure_area_then_should_redirect_to_login_page()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://example.com/secured");
            response.StatusCode.Should().Be(HttpStatusCode.Found);
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
        }

        public class TestModule : NancyModule
        {
            public TestModule()
            {
                Get["/"] = _ => "Home page";
                Get["/secured"] = _ => HttpStatusCode.Unauthorized;
                Post["/login"] = _ =>
                {
                    var login = this.Bind<Login>();
                    if (login.Username == "user" && login.Password == "pass")
                    {
                        var authentication = Context.GetAuthenticationManager();
                        authentication.SignIn(
                            new AuthenticationProperties { RedirectUri = "/secured" },
                            new GenericIdentity("User"));
                        return null; // umm ...
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