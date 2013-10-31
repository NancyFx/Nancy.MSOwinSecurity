namespace SampleApp
{
    using Nancy;
    using Nancy.Owin.Security;
    using SampleApp.Models;

    public class SampleModule : NancyModule
    {
        public SampleModule()
        {
            Get["/"] = _ =>
            {
                var authManager = Context.GetAuthenticationManager();
                var pageBase = new PageBase();
                if (authManager.User != null && authManager.User.Identity.IsAuthenticated)
                {
                    pageBase.Username =  authManager.User.Identity.Name;
                }
                return View["Index", pageBase];
            };

            Get["/secured"] = _ =>
            {
                var authManager = Context.GetAuthenticationManager();
                if (authManager.User == null || !authManager.User.Identity.IsAuthenticated)
                {
                    return HttpStatusCode.Unauthorized;
                }
                return HttpStatusCode.OK;
            };

            Get["/signout"] = _ =>
            {
                Context.GetAuthenticationManager().SignOut();
                return Response.AsRedirect("/");
            };
        }
    }
}