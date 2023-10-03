using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddOpenTelemetry().WithMetrics(options =>
{

    options.AddMeter("metric.meter.api");
    options.ConfigureResource(resource =>
    {
        resource.AddService("Metric.API", serviceVersion: "1.0.0");
    });
    options.AddPrometheusExporter();
    





});





 var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}
app.UseSwagger();
app.UseSwaggerUI();



//app.UseHttpsRedirection();
app.UseAuthorization();
app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.MapControllers();

app.Run();
