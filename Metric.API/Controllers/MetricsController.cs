using Metric.API.OpenTelemetry;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Metric.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MetricsController : ControllerBase
    {

        [HttpGet]
        public IActionResult CounterMetric()
        {

            OpenTelemetryMetric.OrderCreatedEventCounter.Add(1,
                new KeyValuePair<string, object?>("event", "add"),
                new KeyValuePair<string, object?>("queue.name", "event.created.queue")
                );

            return Ok();
        }

        [HttpGet]
        public IActionResult CounterObservableMetric()
        {
            Counter.OrderCancelledCounter += new Random().Next(1, 100);


            return Ok();
        }


        [HttpGet]
        public IActionResult UpDownCounterMetric()
        {

            OpenTelemetryMetric.CurrentStockCounter.Add(new Random().Next(-300, 300));

            return Ok();

        }

        [HttpGet]
        public IActionResult UpDownCounterObservableMetric()
        {

            Counter.CurrentStockCount += new Random().Next(-300, 300);

            return Ok();

        }



        [HttpGet]
        public IActionResult GaugeObservableMetric()
        {

            Counter.KitchenTemp = new Random().Next(-30, 60);

            return Ok();

        }


        [HttpGet]
        public IActionResult HistogramMetric()
        {


            OpenTelemetryMetric.XMethodDuration.Record(new Random().Next(500, 50000));

            return Ok();

        }

    }
}
