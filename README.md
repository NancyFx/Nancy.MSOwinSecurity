Nancy.MSOwinSecurity
===================

Enables integration with Microsoft's OWIN security middeware from the Katana
project. This package will allow you to easily access the *IAuthenticationManager*.
This version supports *Nancy v2* on .NET 4.5.2 (or greater) only. When using
*Nancy v1*, please use *Nancy.MSOwinSecurity v2* instead.

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
        // v1.x Route Syntax
        Get["/"] = _ =>
        {
            IAuthenticationManager authenticationManager = Context.GetAuthenticationManager();
            //ClaimsPrincipal user = authenticationManager.User;
            //authenticationManager.SignIn(..);
            //authenticationManager.SignOut(..);
            //authenticationManager.AuthenticateAsync(..);
            //authenticationManager.Challenge(..);
        };

        // v2.x Route Syntax
        Get("/", _ =>
        {
            IAuthenticationManager authenticationManager = Context.GetAuthenticationManager();
            //ClaimsPrincipal user = authenticationManager.User;
            //authenticationManager.SignIn(..);
            //authenticationManager.SignOut(..);
            //authenticationManager.AuthenticateAsync(..);
            //authenticationManager.Challenge(..);
        });
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

        // v1.x Route Syntax
        Get["/"] = _ => {...});

        // v2.x Route Syntax
        Get("/", _ => {...});
    }
}
```

Securing a route:

```C#
public class MyModule : NancyModule
{
    public MyModule()
    {
        // v1.x route syntax
        Get["/"] = _ => 
        {
            this.RequiresMSOwinAuthentication();
            ....
        });

        // v2.x Route Syntax
        Get("/", _ => 
        {
            this.RequiresMSOwinAuthentication();
            ....
        });
    }
}
```

Getting the current user (just a helper extension around IAuthenticationManager.User):

```C#
public class MyModule : NancyModule
{
    public MyModule()
    {
        // v1.x route syntax
        Get["/"] = _ => 
        {
            ClaimsPrincipal = Context.GetMSOwinUser();
            ....
        });

        // v2.x Route Syntax 
        Get("/", _ => 
        {
            ClaimsPrincipal = Context.GetMSOwinUser();
            ....
        });
    }
}

```
Authorizing the user at module level:
```C#

public class MyModule : NancyModule
{
    public MyModule()
    {
        this.RequiresSecurityClaims(claims => claims.Any(claim =>
            claim.ClaimType = ClaimTypes.Country &&
            claim.Value.Equals("IE", StringComparision.Ordinal)));

        // v1.x route syntax 
        Get["/"] = _ => 
        {
           ....
        });

        // v2.x Route Syntax 
        Get("/",  _ => 
        {
           ....
        });
    }
}

```
Authorizing the user at route level:
```C#

public class MyModule : NancyModule
{
    public MyModule()
    {
        // v1.x route syntax 
        Get["/"] = _ => 
        {
           this.RequiresSecurityClaims(claims => claims.Any(claim =>
                claim.ClaimType = ClaimTypes.Country &&
                claim.Value.Equals("IE", StringComparision.Ordinal)));
        });
    
        // v2.x Route Syntax 
        Get("/", _ => 
        {
            this.RequiresSecurityClaims(claims => claims.Any(claim =>
                claim.ClaimType = ClaimTypes.Country &&
                claim.Value.Equals("IE", StringComparision.Ordinal)));
            ...
        });
    }
}
```

Personal note: this nancy extension package would integrate much better if we had extension properties in c# :(

License
-

MIT

Please report issues on github.
Questions, criticisms, compliments or otherwise, [@randompunter], or pop by the [Nancy Jabbr room].

  [@randompunter]: http://twitter.com/randompunter
  [Nancy Jabbr room]: https://jabbr.net/#/rooms/nancyfx
