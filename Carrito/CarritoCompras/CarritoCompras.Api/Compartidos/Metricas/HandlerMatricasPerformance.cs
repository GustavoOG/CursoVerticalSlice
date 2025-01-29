using System.Diagnostics.Metrics;

namespace CarritoCompras.Api.Compartidos.Metricas
{
    public sealed class HandlerMatricasPerformance
    {
        private readonly Counter<long> _milliSecondsElapsed;

        public HandlerMatricasPerformance(IMeterFactory meterFactory)
        {
            var meter = meterFactory.Create("carritocompras.tienda");
            _milliSecondsElapsed = meter.CreateCounter<long>(
                "carritocompras.api.requesthandler.millisecondselapsed");
        }

        public void MilliSecondsElapsed(long millisecondselapse)
            => _milliSecondsElapsed.Add(millisecondselapse);
    }
}