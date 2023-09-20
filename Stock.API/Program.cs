using Common.Shared;
using MassTransit;
using OpenTelemetry.Shared;
using Stock.API.Consumers;
using Stock.API.Services;

var builder = WebApplication.CreateBuilder(args);

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
app.UseMiddleware<RequestAndResponseActivityMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
