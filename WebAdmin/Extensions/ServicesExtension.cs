using WebAdmin.BackgroundServices;
using WebAdmin.Configuration;
using WebAdmin.Services;

namespace WebAdmin.Extensions
{
    public static class ServicesExtension
    {
        public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ClickHouseSettings>(configuration.GetSection("ClickHouse"));
            services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));

            services.AddSingleton<ClickHouseService>();

            services.AddHostedService<RabbitMQService>();
            services.AddHostedService<ClickHouseCleanupService>();
        }
    }
}
