namespace Nancy.Owin.Security.Tests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using global::Owin.Testing;
    using SampleApp;
    using Xunit;

    public class NancyModuleExtensionsTests
    {
        private readonly HttpClient _httpClient;

        public NancyModuleExtensionsTests()
        {
            OwinTestServer testServer = OwinTestServer.Create(builder => new Startup().Configuration(builder));
            _httpClient = testServer.CreateHttpClient();
        }

        [Fact]
        public async Task When_attempt_to_access_secure_area_then_should_redirect_to_login_page()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://example.com/secured");
            response.StatusCode.Should().Be(HttpStatusCode.Found);
            response.Headers.Location.Should().NotBeNull();
        }
/*
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
                .Any(h => h.Key == "Set-Cookie")
                .Should().BeTrue();
        }*/

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
    }
}