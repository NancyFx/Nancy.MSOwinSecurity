namespace Nancy.Security
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.Owin.Security;
    using Nancy.Extensions;

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
                return (auth == null || auth.User == null)
                    ? HttpStatusCode.Unauthorized
                    : (Response)null;
            }, "Requires MS Owin authentication");
        }
        
        /// <summary>
        ///     This module requires the security claims to be validated.
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
                    : HttpStatusCode.Unauthorized;
            }, "Requires valid security claims");
        }
    }
}