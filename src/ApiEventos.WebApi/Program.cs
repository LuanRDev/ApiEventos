using ApiEventos.Application.DI;
using ApiEventos.WebApi.Middlewares;
using System.Text.Json.Serialization;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration(app => app.AddConfiguration(
    new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddUserSecrets<Program>()
        .AddEnvironmentVariables()
        .Build()
    )).ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder
            .UseSerilog((ctx, cfg) =>
            {
                cfg.Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .WriteTo.Console()
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(ctx.Configuration["ElasticConfiguration:Uri"]))
                    {
                        IndexFormat = $"{ctx.Configuration["ApplicationName"]}-logs-{ctx.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                        AutoRegisterTemplate = true,
                        NumberOfShards = 2,
                        NumberOfReplicas = 1
                    })
                   .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName)
                   .ReadFrom.Configuration(ctx.Configuration);
            });
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
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
