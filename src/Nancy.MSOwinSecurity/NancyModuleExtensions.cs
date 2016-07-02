namespace Nancy.Security
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.Owin.Security;
    using Extensions;

    public static class NancyModuleExtensions
    {
        /// <summary>
        ///     This module requires the user is authenticated with Microsoft Owin authentication.
        /// </summary>
        /// <param name="module">Module to enable</param>
        public static void RequiresMSOwinAuthentication(this INancyModule module)
        {
            module.AddBeforeHookOrExecute(ctx =>
            {
                IAuthenticationManager auth = ctx.GetAuthenticationManager();
                return auth?.User?.Identity == null || !auth.User.Identity.IsAuthenticated
                    ? HttpStatusCode.Unauthorized
                    : (Response)null;
            }, "Requires MS Owin authentication");
        }

        /// <summary>
        ///     This module requires the security claims to be validated.
        ///     If the users claims do not match the required claims then a <see cref="HttpStatusCode.Forbidden"/> is returned.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="isValid"></param>
        public static void RequiresSecurityClaims(this INancyModule module, Func<Claim[], bool> isValid)
        {
            module.RequiresMSOwinAuthentication();
            module.AddBeforeHookOrExecute(ctx =>
            {
                IAuthenticationManager auth = ctx.GetAuthenticationManager();
                return isValid(auth.User.Claims.ToArray())
                    ? (Response)null
                    : HttpStatusCode.Forbidden;
            }, "Requires valid security claims");
        }
    }
}