using Common.Shared;
using Logging.Shared;
using MassTransit;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Shared;
using Serilog;
using Stock.API.Consumers;
using Stock.API.Services;

var builder = WebApplication.CreateBuilder(args);
//builder.Host.UseSerilog(Logging.Shared.Logging.ConfigureLogging);
//builder.AddOpenTelemetryLog();
builder.Logging.AddOpenTelemetry(cfg =>
{
    var serviceName = builder.Configuration.GetSection("OpenTelemetry")["ServiceName"];
    var serviceVersion = builder.Configuration.GetSection("OpenTelemetry")["ServiceVersion"];

    cfg.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName!, serviceVersion: serviceVersion));
    cfg.AddOtlpExporter((x, y) => { });

});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<StockService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddOpenTelemetryExt(builder.Configuration);

builder.Services.AddHttpClient<PaymentService>(options =>
{

    options.BaseAddress = new Uri((builder.Configuration.GetSection("ApiServices")["PaymentApi"])!);

});


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedEventConsumer>();


    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", host =>
        {

            host.Username("guest");
            host.Password("guest");
        });


        cfg.ReceiveEndpoint("stock.order-created-event.queue", e =>
        {
            e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
        });


    });


});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<OpenTelemetryTraceIdMiddleware>();
app.UseMiddleware<RequestAndResponseActivityMiddleware>();
app.UseExceptionMiddleware();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
