namespace Nancy.Security
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using FluentAssertions;
    using Nancy.Owin;
    using Nancy.Security.Annotations;
    using Xunit;

    [UsedImplicitly]
    public class NancyContextExtensionsTests
    {
        public class Given_a_nancy_context_and_no_owin_environment
        {
            private readonly NancyContext _nancyContext;

            public Given_a_nancy_context_and_no_owin_environment()
            {
                _nancyContext = new NancyContext();
            }

            [Fact]
            public void When_get_authentication_manager_and_throw_on_null_then_should_throw()
            {
                Assert.Throws<InvalidOperationException>(() => _nancyContext.GetAuthenticationManager(true));
            }
        }

        public class Given_a_nancy_context_and_invalid_owin_environment_type
        {
            private readonly NancyContext _nancyContext;

            public Given_a_nancy_context_and_invalid_owin_environment_type()
            {
                _nancyContext = new NancyContext();
                _nancyContext.Items.Add(NancyOwinHost.RequestEnvironmentKey, "invalid");
            }

            [Fact]
            public void When_get_authentication_manager_and_throw_on_null_then_should_throw()
            {
                Assert.Throws<InvalidOperationException>(() => _nancyContext.GetAuthenticationManager(true));
            }
        }

        public class Given_a_nancy_context_and_valid_owin_environment
        {
            private readonly NancyContext _nancyContext;

            public Given_a_nancy_context_and_valid_owin_environment()
            {
                _nancyContext = new NancyContext();
                _nancyContext.Items.Add(NancyOwinHost.RequestEnvironmentKey, new Dictionary<string, object>());
            }

            [Fact]
            public void When_get_authentication_manager_should_not_be_null()
            {
                _nancyContext.GetAuthenticationManager().Should().NotBeNull();
            }
        }

        public class Given_a_nancy_context_and_a_authenticated_user
        {
            private readonly NancyContext _nancyContext;

            public Given_a_nancy_context_and_a_authenticated_user()
            {
                _nancyContext = new NancyContext();
                _nancyContext.Items.Add(NancyOwinHost.RequestEnvironmentKey, new Dictionary<string, object>()
                {
                    {"server.User", new ClaimsPrincipal() }
                });
            }

            [Fact]
            public void When_get_user_should_not_be_null()
            {
                _nancyContext.GetMSOwinCurrentUser().Should().NotBeNull();
            }
        }
    }
}