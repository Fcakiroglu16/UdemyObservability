// See https://aka.ms/new-console-template for more information

using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using UdemyObservability.ConsoleApp;

Console.WriteLine("Hello, World!");


using var traceProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource(OpenTelemetryConstants.ActivitySourceName)
    .ConfigureResource(configure =>
    {
        configure
        .AddService(OpenTelemetryConstants.ServiceName, serviceVersion: OpenTelemetryConstants.ServiceVersion)
        .AddAttributes(new List<KeyValuePair<string, object>>()
                {
            
                    new KeyValuePair<string, object>("host.machineName", Environment.MachineName),
                    new KeyValuePair<string, object>("host.environment", "dev"),

                });
    }).AddConsoleExporter().AddOtlpExporter().Build();

var serviceHelper = new ServiceHelper();

await serviceHelper.Work1();





