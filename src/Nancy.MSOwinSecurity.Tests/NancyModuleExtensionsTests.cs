namespace Nancy.Security
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using FluentAssertions;
    using Nancy.Owin;
    using Xunit;

    public class NancyModuleExtensionsTests
    {
        public class Given_a_module_that_requires_owin_authentication
        {
            private readonly TestModule _testModule;

            public Given_a_module_that_requires_owin_authentication()
            {
                _testModule = new TestModule();
                _testModule.RequiresOwinAuthentication();
            }

            [Fact]
            public void With_no_authentication_manager_then_should_get_unauthorized()
            {
                var context = new NancyContext();
                Response response = _testModule
                    .Before
                    .PipelineItems
                    .Single()
                    .Delegate(context, CancellationToken.None)
                    .Result;

                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }

            [Fact]
            public void With_authentication_manager_but_no_user_then_should_get_unauthorized()
            {
                var context = new NancyContext();
                context.Items.Add(NancyOwinHost.RequestEnvironmentKey, new Dictionary<string, object>());
                Response response = _testModule
                    .Before
                    .PipelineItems
                    .Single()
                    .Delegate(context, CancellationToken.None)
                    .Result;

                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }

            [Fact]
            public void With_authentication_manager_and_a_user_then_should_get_null()
            {
                var context = new NancyContext();
                context.Items.Add(NancyOwinHost.RequestEnvironmentKey, new Dictionary<string, object>() { { "server.User", new ClaimsPrincipal() } });
                Response response = _testModule
                    .Before
                    .PipelineItems
                    .Single()
                    .Delegate(context, CancellationToken.None)
                    .Result;

                response.Should().BeNull();
            }

            private class TestModule : NancyModule
            {}
        }
    }
}