using MassTransit.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace OpenTelemetry.Shared
{
    public static class OpenTelemetryExtensions
    {
        public static void AddOpenTelemetryExt(this IServiceCollection services, IConfiguration configuration)

        {
            services.Configure<OpenTelemetryConstants>(configuration.GetSection("OpenTelemetry"));
            var openTelemetryConstants = (configuration.GetSection("OpenTelemetry").Get<OpenTelemetryConstants>())!;

            ActivitySourceProvider.Source = new System.Diagnostics.ActivitySource(openTelemetryConstants.ActivitySourceName);

            services.AddOpenTelemetry().WithTracing(options =>
            {
                options.AddSource(openTelemetryConstants.ActivitySourceName)
                .AddSource(DiagnosticHeaders.DefaultListenerName)
                .ConfigureResource(resource =>
                {
                    resource.AddService(openTelemetryConstants.ServiceName, serviceVersion: openTelemetryConstants.ServiceVersion);
                });
                options.AddAspNetCoreInstrumentation(aspnetcoreOptions =>
                {

                   
                    aspnetcoreOptions.Filter = (context) =>
                    {

                        if (!string.IsNullOrEmpty(context.Request.Path.Value))
                        {
                            return context.Request.Path.Value.Contains("api", StringComparison.InvariantCulture);
                        }
                        return false;

                    };
                    
                    // Serilog üzerinden elasticsearch db'ye hatalar gönderildiği için kapatıldı.
                    //aspnetcoreOptions.RecordException = true;

                    //aspnetcoreOptions.EnrichWithException = (activity, exception) =>
                    //{
                        
                    //    // Bilerek boş bırakıldı. Örnek göstermek için
                    //};

                });

                options.AddEntityFrameworkCoreInstrumentation(efcoreOptions =>
                {
                    efcoreOptions.SetDbStatementForText = true;
                    efcoreOptions.SetDbStatementForStoredProcedure = true;
                    efcoreOptions.EnrichWithIDbCommand = (activity, dbCommand) =>
                    {
                        // Bilerek boş bırakıldı. Örnek göstermek için

                    };
                });

                options.AddHttpClientInstrumentation(httpOptions =>
                {




                    httpOptions.FilterHttpRequestMessage = (request) =>
                    {

                     
                            return !request.RequestUri.AbsoluteUri.Contains("9200", StringComparison.InvariantCulture);
                        
                     

                    };


                    httpOptions.EnrichWithHttpRequestMessage = async (activity, request) =>
                    {
                        var requestContent = "empty";

                        if (request.Content != null)
                        {
                            requestContent = await request.Content.ReadAsStringAsync();
                        }


                        activity.SetTag("http.request.body", requestContent);
                    };

                    httpOptions.EnrichWithHttpResponseMessage = async (activity, response) =>
                    {

                        if (response.Content != null)
                        {
                            activity.SetTag("http.response.body", await response.Content.ReadAsStringAsync());
                        }

                    };

                });

                options.AddRedisInstrumentation(redisOptions =>
                {
                    redisOptions.SetVerboseDatabaseStatements = true;
                });

                options.AddConsoleExporter();
                options.AddOtlpExporter(); // Jaeger

            });



        }

    }
}
