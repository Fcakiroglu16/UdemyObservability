using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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
                    

                    aspnetcoreOptions.RecordException = true;

                    aspnetcoreOptions.EnrichWithException = (activity, exception) =>
                    {
                        
                        // Bilerek boş bırakıldı. Örnek göstermek için
                    };

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
                options.AddConsoleExporter();
                options.AddOtlpExporter(); // Jaeger

            });



        }

    }
}
