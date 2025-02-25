using Prometheus;

namespace WebAdmin.Services
{
    public class MetricsService
    {
        private static readonly Counter LogsProcessed = Metrics.CreateCounter("logs_processed_total", "Total number of logs processed");

        public void IncrementLogsProcessed()
        {
            LogsProcessed.Inc();
        }
    }
}
