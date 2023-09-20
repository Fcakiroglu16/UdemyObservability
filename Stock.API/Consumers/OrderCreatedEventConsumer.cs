using Common.Shared.Events;
using MassTransit;
using OpenTelemetry.Shared;
using System.Diagnostics;
using System.Text.Json;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        public Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
         

            Thread.Sleep(2000);


            Activity.Current?.SetTag("message.body", JsonSerializer.Serialize(context.Message));


            return Task.CompletedTask;


        }
    }
}
