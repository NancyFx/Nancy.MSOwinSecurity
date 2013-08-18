using System.Collections.Generic;

namespace Nancy.Owin.Security
{
    using System;
    using System.Security.Claims;
    using System.Security.Principal;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;

    public static class NancyContextExtensions
    {
        public static IOwinAuth GetOwinAuth(this NancyContext nancyContext)
        {
            var environment = (IDictionary<string, object>)nancyContext.Items[NancyOwinHost.RequestEnvironmentKey];
            var owinContext = new OwinContext(environment);
            return new OwinAuth(owinContext.Authentication);
        }

        private class OwinAuth : IOwinAuth
        {
            private readonly IAuthenticationManager _authenticationManager;

            public OwinAuth(IAuthenticationManager authenticationManager)
            {
                _authenticationManager = authenticationManager;
            }

            public ClaimsPrincipal User
            {
                get
                {
                    try
                    {
                        return _authenticationManager.User;
                    }
                    catch (ArgumentNullException) // https://katanaproject.codeplex.com/workitem/53
                    {
                        return null;
                    }
                }
            }

            public void SignOut()
            {
                _authenticationManager.SignOut();
            }

            public void SignIn(AuthenticationProperties authenticationProperties, GenericIdentity genericIdentity)
            {
               _authenticationManager.SignIn(authenticationProperties, genericIdentity);
            }
        }
    }

    public interface IOwinAuth
    {
        ClaimsPrincipal User { get; }

        void SignOut();

        void SignIn(AuthenticationProperties authenticationProperties, GenericIdentity genericIdentity);
    }
}
