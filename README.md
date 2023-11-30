# dotnet-auth0-api-template

A repository for creating a Web API with Auth0 using dotnet core 3+

## Using this template

### Update Scopes

Your scopes should ideally match the feature or the domain purpose of the API. For example if your domain is notifications you would simplay have `read:notifications` and `write:notifications`

It is possible if you have a "feature" api which calls into other APIs you may need to require additional scopes for the user to perform the successful actions necessary to use the Web API.

Learn more at Auth0 [define permissions](https://auth0.com/docs/quickstart/backend/aspnet-core-webapi/01-authorization#define-permissions)

### Setup your domain and audience in the template

### Find and Replace

Find and replace all of the `Auth0Web` namespaces with the namespace prefix of your choice for the Web API. For example if your API handles notifications you will want to replace `Auth0Web` with `Notifications`