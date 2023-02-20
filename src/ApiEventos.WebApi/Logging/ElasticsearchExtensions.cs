using Nest;

namespace ApiEventos.WebApi.Logging;
public static class ElasticsearchExtensions
{
    public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
    {
        var uri = configuration["ElasticConfiguration:Uri"];
        var defaultIndex = configuration["ElasticConfiguration:DefaultIndex"];
        var basicAuthUser = configuration["ElasticConfiguration:Username"];
        var basicAuthPassword = configuration["ElasticConfiguration:Password"];

        var settings = new ConnectionSettings(new Uri(uri));

        if (!string.IsNullOrEmpty(defaultIndex))
            settings = settings.DefaultIndex(defaultIndex);

        if (!string.IsNullOrEmpty(basicAuthUser) && !string.IsNullOrEmpty(basicAuthPassword))
            settings = settings.BasicAuthentication(basicAuthUser, basicAuthPassword);

        settings.EnableApiVersioningHeader();

        var client = new ElasticClient(settings);

        services.AddSingleton<IElasticClient>(client);
    }
}