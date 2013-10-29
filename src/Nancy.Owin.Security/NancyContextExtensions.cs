namespace Nancy.Owin.Security
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;

    public static class NancyContextExtensions
    {
        /// <summary>
        ///     Gets the OWIN authentication manager from the nancy context.
        /// </summary>
        /// <param name="context">The current nancy context.</param>
        /// <returns>An <see cref="IAuthenticationManager" />.</returns>
        public static IAuthenticationManager GetAuthenticationManager(this NancyContext context)
        {
            var environment = (IDictionary<string, object>) context.Items[NancyOwinHost.RequestEnvironmentKey];
            var owinContext = new OwinContext(environment);
            return owinContext.Authentication;
        }

        /// <summary>
        ///     Gets the OWIN User.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ClaimsPrincipal GetOwinUser(this NancyContext context)
        {
            IAuthenticationManager authenticationManager = context.GetAuthenticationManager();
            return authenticationManager == null ? null : authenticationManager.User;
        }
    }
}