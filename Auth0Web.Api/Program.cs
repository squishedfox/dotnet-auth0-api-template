using System.Security.Claims;
using Auth0Web.Api.Authorization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

const string CORS_POICY = "Auth0Web.CorsPolicy";

var builder = WebApplication.CreateBuilder(args);

//
// Setup Authentication and Authorization
var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = domain;
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });
// Program.cs

builder.Services.AddAuthorization(options =>
{
    var scopes = builder.Configuration["Scopes"].Split(';');
    foreach (var scope in scopes)
    {
        options.AddPolicy(scope, policy => policy.Requirements.Add(new HasScopeRequirement(scope, domain)));
    }
});
builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(CORS_POICY, corsBuilder =>
    {
        var allowedHosts = builder.Configuration["AllowedHosts"].Split(';');
        foreach (var hostName in allowedHosts)
        {
            corsBuilder.WithOrigins(hostName)
                .WithHeaders("Authorization")
                .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS");
        }
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthorization();
app.MapControllers();
app.UseCors(CORS_POICY);
app.Run();
