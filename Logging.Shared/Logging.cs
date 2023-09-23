using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Elasticsearch;
using OpenTelemetry.Logs;

namespace Logging.Shared
{
    public static class Logging
    {
       
        public static void AddOpenTelemetryLog(this WebApplicationBuilder builder)
        {

            builder.Logging.AddOpenTelemetry(cfg =>
            {
                var serviceName = builder.Configuration.GetSection("OpenTelemetry")["ServiceName"];
                var serviceVersion = builder.Configuration.GetSection("OpenTelemetry")["ServiceVersion"];

                cfg.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName!, serviceVersion: serviceVersion));
                cfg.AddOtlpExporter();

            });
        }




        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogging => (builderContext, loggerConfiguration) =>
        {
            var environment = builderContext.HostingEnvironment;


            loggerConfiguration
            .ReadFrom.Configuration(builderContext.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("Env", environment.EnvironmentName)
            .Enrich.WithProperty("AppName", environment.ApplicationName);


            var elasticsearchBaseUrl = builderContext.Configuration.GetSection("Elasticsearch")["BaseUrl"];
            var userName = builderContext.Configuration.GetSection("Elasticsearch")["UserName"];
            var password = builderContext.Configuration.GetSection("Elasticsearch")["Password"];
            var indexName = builderContext.Configuration.GetSection("Elasticsearch")["IndexName"];

            loggerConfiguration.WriteTo.Elasticsearch(new(new Uri(elasticsearchBaseUrl))
            {
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = Serilog.Sinks.Elasticsearch.AutoRegisterTemplateVersion.ESv8,
                IndexFormat = $"{indexName}-{environment.EnvironmentName}-logs-"+ "{0:yyy.MM.dd}",
                ModifyConnectionSettings = x => x.BasicAuthentication(userName, password),
                CustomFormatter = new ElasticsearchJsonFormatter()
            });


        };
   
    }
}
