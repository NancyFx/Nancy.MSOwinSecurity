namespace Nancy.Security
{
    using Microsoft.Owin.Security;
    using Nancy.Extensions;

    public static class NancyModuleExtensions
    {
        /// <summary>
        ///     This module requires Microsoft Owin authentication
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
            }, "Requires owin authentication");
        }
    }
}