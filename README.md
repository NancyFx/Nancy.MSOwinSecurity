Nancy.MSOwinSecurity
===================

Enables integration with Microsoft's OWIN security middeware from the Katana project. This pacakge will allow you to easily access the IAuthenticationManager.

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
            ClaimsPrincipal claimsPrincipal = Context.GetAuthenticationManager().User;
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
