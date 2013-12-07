Nancy.MSOwinSecurity
===================

Enables integration with Microsoft's OWIN security middeware from the Katana project. This pacakge will allow you to easily access the IAuthenticationManager. .NET 4.5 only.

How to use
-

Installing the nuget package:
```
install-package Nancy.MSOwinSecurity
```
Getting the authention manager and current user from the context:
```C#
public class MyModule : NancyModule
{
    public MyModule()
    {
        Get["/"] = _ =>
        {
            IAuthenticationManager authenticationManager = Context.GetAuthenticationManager();
            //ClaimsPrincipal user = authenticationManager.User;
            //authenticationManager.SignIn(..);
            //authenticationManager.SignOut(..);
            //authenticationManager.AuthenticateAsync(..);
            //authenticationManager.Challenge(..);
        };
    }
}
```
Securing a module:
```C#
public class MyModule : NancyModule
{
    public MyModule()
    {
        this.RequiresOwinAuthentication();
        Get["/"] = _ => {...}
    }
}
```
Securing a route:
```C#
public class MyModule : NancyModule
{
    public MyModule()
    {
        
        Get["/"] = _ => 
        {
            this.RequiresOwinAuthentication();
            ....
        }
    }
}
```
License
-

MIT

Questions, criticisms, compliments or otherwise, [@randompunter], or pop by the [Nancy Jabbr room].

  [@randompunter]: http://twitter.com/randompunter
  [Nancy Jabbr room]: https://jabbr.net/#/rooms/nancyfx
