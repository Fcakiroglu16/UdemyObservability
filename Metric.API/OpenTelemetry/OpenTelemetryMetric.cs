using System.Diagnostics.Metrics;

namespace Metric.API.OpenTelemetry
{
    public static class OpenTelemetryMetric
    {
        private static readonly Meter meter = new Meter("metric.meter.api");

        public static Counter<int> OrderCreatedEventCounter = meter.CreateCounter<int>("order.created.event.count");


        public static ObservableCounter<int> OrderCancelledCounter = meter.CreateObservableCounter("order.cancelled.count",() => new Measurement<int>(Counter.OrderCancelledCounter));


        public static UpDownCounter<int> CurrentStockCounter = meter.CreateUpDownCounter<int>("current.stock.count");

        public static ObservableUpDownCounter<int> CurrentStockObservableCounter= meter.CreateObservableUpDownCounter("current.stock.observable.counter",()=> new Measurement<int>(Counter.CurrentStockCount));


        public static ObservableGauge<int> rowKitchenTemp = meter.CreateObservableGauge<int>("room.kitchen.temp",
                () => new Measurement<int>(Counter.KitchenTemp));


        public static Histogram<int> XMethodDuration = meter.CreateHistogram<int>("x.method.duration",unit:"milliseconds");
    }
}
