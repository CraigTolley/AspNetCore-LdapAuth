# AspNetCore-LdapAuth
Experiments with LDAP Forms Authentication in ASP.net Core 3.0

Based on the examples documented by Brecht here: https://www.brechtbaekelandt.net/blog/post/authenticating-against-active-directory-with-aspnet-core-2-and-managing-users with code adapted from https://github.com/brechtb86/dotnet

The example uses LDAP authentication, and then extracts the users group memberships to construct a set of roles and claims for the user. It is a very basic approach to authorization but proves the concept. 
