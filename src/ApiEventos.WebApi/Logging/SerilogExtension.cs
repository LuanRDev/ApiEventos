using ApiEventos.WebApi.Logging;
using Elastic.CommonSchema.Serilog;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Filters;
using Serilog.Sinks.Elasticsearch;

namespace ApiEventos.WebApi.Logging;

public static class SerilogExtension
{
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder, IConfiguration configuration, string applicationName)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", $"{applicationName} - {Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}")
            .Enrich.WithCorrelationId()
            .Enrich.WithExceptionDetails()
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
            .WriteTo.Async(writeTo => writeTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]!))
            {
                TypeName = null,
                AutoRegisterTemplate = true,
                IndexFormat = "indexlogs",
                BatchAction = ElasticOpType.Create,
                CustomFormatter = new EcsTextFormatter(),
                ModifyConnectionSettings = x => x.BasicAuthentication(configuration["ElasticConfiguration:Username"], configuration["ElasticConfiguration:Password"])
            }))
            .WriteTo.Async(writeTo => writeTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"))
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Host.UseSerilog(Log.Logger, true);

        return builder;
    }

    public static WebApplication UseSerilog(this WebApplication app)
    {
        app.UseSerilogRequestLogging(opts =>
        {
            opts.EnrichDiagnosticContext = LogEnricherExtensions.EnrichFromRequest;
        });

        return app;
    }
}