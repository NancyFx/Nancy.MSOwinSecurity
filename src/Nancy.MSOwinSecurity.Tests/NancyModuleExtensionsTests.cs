namespace Nancy.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using FluentAssertions;
    using Owin;
    using Annotations;
    using Xunit;

    [UsedImplicitly]
    public class NancyModuleExtensionsTests
    {
        public class Given_a_module_that_requires_owin_authentication
        {
            private readonly TestModule _testModule;

            public Given_a_module_that_requires_owin_authentication()
            {
                _testModule = new TestModule();
                _testModule.RequiresMSOwinAuthentication();
            }

            [Fact]
            public void With_no_authentication_manager_then_should_get_unauthorized()
            {
                var context = new NancyContext();

                Response response = _testModule.InvokeBeforePipeLine(context);

                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }

            [Fact]
            public void With_authentication_manager_but_no_user_then_should_get_unauthorized()
            {
                var context = new NancyContext();
                context.Items.Add(NancyMiddleware.RequestEnvironmentKey, new Dictionary<string, object>());

                Response response = _testModule.InvokeBeforePipeLine(context);

                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }

            [Fact]
            public void With_authentication_manager_and_a_user_then_should_get_null()
            {
                var context = new NancyContext();
                context.Items.Add(NancyMiddleware.RequestEnvironmentKey, new Dictionary<string, object> { { "server.User", new ClaimsPrincipal() } });

                Response response = _testModule.InvokeBeforePipeLine(context);

                response.Should().BeNull();
            }

            private class TestModule : NancyModule
            {}
        }

        public class Given_a_module_that_requires_security_claims
        {
            private readonly TestModule _testModule;
            private readonly NancyContext _context;
            private readonly Dictionary<string, object> _environment;
            private const string ServerUserKey = "server.User";

            public Given_a_module_that_requires_security_claims()
            {
                _testModule = new TestModule();
                _testModule.RequiresSecurityClaims(claims => claims.Any(
                        claim => claim.Type == ClaimTypes.Country
                        && claim.Value.Equals("IE", StringComparison.Ordinal)));
                _context = new NancyContext();
                _environment = new Dictionary<string, object>();
                _context.Items.Add(NancyMiddleware.RequestEnvironmentKey, _environment);
            }

            [Fact]
            public void When_unauthenticated_should_get_unauthorized()
            {
                _environment[ServerUserKey] = null;

                Response response = _testModule.InvokeBeforePipeLine(_context);

                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }

            [Fact]
            public void When_authenticated_and_claims_not_valid_should_get_Forbidden()
            {
                _environment[ServerUserKey] = new ClaimsPrincipal(new ClaimsIdentity(new Claim[0], "Custom"));

                Response response = _testModule.InvokeBeforePipeLine(_context);

                response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            }

            [Fact]
            public void With_authentication_manager_and_a_user_then_should_get_null()
            {
                _environment[ServerUserKey] = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new [] { new Claim(ClaimTypes.Country, "IE") }));

                Response response = _testModule.InvokeBeforePipeLine(_context);

                response.Should().BeNull();
            }

            private class TestModule : NancyModule
            { }
        }
    }

    internal static class NancyModuleExtensions
    {
        internal static Response InvokeBeforePipeLine(this NancyModule module, NancyContext context)
        {
            return module
                    .Before
                    .PipelineItems
                    .Select(pipelineItem => pipelineItem.Delegate(context, CancellationToken.None).Result)
                    .FirstOrDefault(res => res != null);
        }
    }
}