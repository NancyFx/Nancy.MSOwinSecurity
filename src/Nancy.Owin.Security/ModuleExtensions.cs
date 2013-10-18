namespace Nancy.Owin.Security
{
    using System;
    using System.Security.Claims;
    using Microsoft.Owin.Security;
    using Nancy.Extensions;

    public static class ModuleExtensions
    {
        /// <summary>
        ///     This module requires authentication
        /// </summary>
        /// <param name="module">Module to enable</param>
        public static void RequiresOwinAuthentication(this INancyModule module)
        {
            module.AddBeforeHookOrExecute(ctx =>
            {
                IAuthenticationManager auth = ctx.GetAuthenticationManager();
                if (auth == null || auth.User == null)
                {
                    return new Response { StatusCode = HttpStatusCode.Unauthorized } ;
                }
                return null;
            }, "Requires Authentication");
        }

        /// <summary>
        /// This module requires authroization.
        /// </summary>
        /// <param name="module">Module to enable</param>
        /// <param name="isAuthorized">Delegate to test whether the user is authorized.</param>
        public static void Authorize(this INancyModule module, Func<ClaimsPrincipal, bool> isAuthorized)
        {
            RequiresOwinAuthentication(module);
            module.AddBeforeHookOrExecute(ctx =>
            {
                IAuthenticationManager auth = ctx.GetAuthenticationManager();
                return !isAuthorized(auth.User) ? new Response { StatusCode = HttpStatusCode.Forbidden } : null;
            }, "Requires authorization");
        }
    }
}