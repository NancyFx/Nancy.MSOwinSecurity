namespace Nancy.Owin.Security.Tests
{
    public class TestModule : NancyModule
    {
        public TestModule()
        {
            Get["/"] = _ => "Home page";
            Get["/secured"] = _ => HttpStatusCode.Unauthorized;
            Get["/login"] = _ => "Login page";
            Get["/logout"] = _ =>
            {
                Context.GetAuthenticationManager().SignOut();
                return Response.AsRedirect("/");
            };
        }
    }
}