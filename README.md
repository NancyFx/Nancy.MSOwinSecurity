Nancy.MSOwinSecurity
===================

Enables integration with Microsoft's OWIN security middeware from the Katana project. This package will allow you to easily access the IAuthenticationManager. .NET 4.5 only.

How to use
-

Installing the nuget package:
```
install-package Nancy.MSOwinSecurity
```
Getting the authentication manager and current user from the context:
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
        this.RequiresMSOwinAuthentication();
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
            this.RequiresMSOwinAuthentication();
            ....
        }
    }
}
```
Getting the current user (just a helper extension around IAuthenticationManager.User):
```C#
public class MyModule : NancyModule
{
    public MyModule()
    {
        
        Get["/"] = _ => 
        {
            ClaimsPrincipal = Context.GetMSOwinUser();
            ....
        }
    }
}
```
Authorizing the user at module level:
```C#
public class MyModule : NancyModule
{
    public MyModule()
    {
        this.RequiresSecurityClaims(claims => claims.Any(
            claim.ClaimType = ClaimTypes.Country &&
            claim.Value.Equals("IE", StringComparision.Ordinal)));
        Get["/"] = _ => 
        {
           ....
        }
    }
}
```
Authorizing the user at route level:
```C#
public class MyModule : NancyModule
{
    public MyModule()
    {
        
        Get["/"] = _ => 
        {
            this.RequiresSecurityClaims(claims => claims.Any(claim =>
                claim.ClaimType = ClaimTypes.Country &&
                claim.Value.Equals("IE", StringComparision.Ordinal)));
            ...
        }
    }
}
```

Personal note: this nancy extension package would integrate much better if we had extenstion properties in c# :(

License
-

MIT

Please report issues on github.
Questions, criticisms, compliments or otherwise, [@randompunter], or pop by the [Nancy Jabbr room].

  [@randompunter]: http://twitter.com/randompunter
  [Nancy Jabbr room]: https://jabbr.net/#/rooms/nancyfx
