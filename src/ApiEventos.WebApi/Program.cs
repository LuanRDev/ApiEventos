using ApiEventos.Application.DI;
using ApiEventos.WebApi.Middlewares;
using System.Text.Json.Serialization;
using Serilog;
using ApiEventos.WebApi.Logging;
using ApiEventos.WebApi.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.AddSerilog(builder.Configuration, "API Eventos");
Log.Information("Starting API");

builder.Host.ConfigureAppConfiguration(app => app.AddConfiguration(
    new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddUserSecrets<Program>()
        .AddEnvironmentVariables()
        .Build()
    ));

builder.Services.AddElasticsearch(builder.Configuration);

builder.Services.AddStackExchangeRedisCache(o =>
{
    o.InstanceName = "instance";
    o.Configuration = builder.Configuration["ApiEventosCacheUrl"];
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
// Add services to the container.

Initializer.Configure(builder.Services, builder.Configuration.GetConnectionString("DefaultConnectionString")!);

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insira um token válido",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(jwtBearerOptions =>
{
    jwtBearerOptions.Authority = builder.Configuration["Keycloak:UrlBase"] + builder.Configuration["Keycloak:Authority"];
    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Keycloak:UrlBase"] + builder.Configuration["Keycloak:Authority"],
        ValidAudience = "account",
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiEventos", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("clientId", "ApiEventos");
    });
});
builder.Services.AddAuthorization();
builder.Services.AddSingleton(typeof(IAuthorizationPolicyProvider), typeof(AuthorizationPolicyProvider));
builder.Services.AddSingleton(typeof(IAuthorizationHandler), typeof(HasScopeHandler));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseSerilog();

app.MapControllers();

app.Run();
