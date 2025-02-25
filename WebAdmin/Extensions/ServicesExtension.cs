using WebAdmin.Configuration;
using WebAdmin.Services;

namespace WebAdmin.Extensions
{
    public static class ServicesExtension
    {
        public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Регистрация конфигурации
            services.Configure<ClickHouseSettings>(configuration.GetSection("ClickHouse"));
            services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));

            // Регистрируем ClickHouseService
            services.AddSingleton<ClickHouseService>();

            // Регистрируем RabbitMQService как фоновый сервис
            services.AddHostedService<RabbitMQService>();
        }
    }
}
