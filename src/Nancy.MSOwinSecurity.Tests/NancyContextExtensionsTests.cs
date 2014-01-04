namespace Nancy.Security
{
    using System;
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
    }
}