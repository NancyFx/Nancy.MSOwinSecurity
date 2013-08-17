using System.Collections.Generic;

namespace Nancy.Owin.Security
{
    using Microsoft.Owin;
    using Microsoft.Owin.Security;

    public static class NancyContextExtensions
    {
        public static IAuthenticationManager GetAuthenticationManager(this NancyContext nancyContext)
        {
            return GetOwinContext(nancyContext).Authentication;
        }

        public static IOwinContext GetOwinContext(this NancyContext nancyContext)
        {
            var environment = (IDictionary<string, object>)nancyContext.Items[NancyOwinHost.RequestEnvironmentKey];
            var owinContext = new OwinContext(environment);
            return owinContext;
        }
    }
}
