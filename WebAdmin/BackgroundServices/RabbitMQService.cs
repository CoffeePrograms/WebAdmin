using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using WebAdmin.Configuration;
using WebAdmin.Converters;
using WebAdmin.Services;

namespace WebAdmin.BackgroundServices
{
    public class RabbitMQService : BackgroundService
    {
        private IConnection _connection;
        private IChannel _channel;
        private readonly ClickHouseService _clickHouseService;
        private readonly string _rabbitMqConnectionString;

        public RabbitMQService(IOptions<RabbitMQSettings> settings, ClickHouseService clickHouseService)
        {
            _rabbitMqConnectionString = GetConnectionString(settings.Value);
            _clickHouseService = clickHouseService;

            InitializeAsync().Wait();
        }

        private string GetConnectionString(RabbitMQSettings settings)
        {
            return $"amqp://{settings.User}:{settings.Password}@{settings.Host}:{settings.Port}/";
        }

        private async Task InitializeAsync()
        {
            var factory = new ConnectionFactory() { Uri = new Uri(_rabbitMqConnectionString) };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            // Объявляем обмен
            await _channel.ExchangeDeclareAsync(
                exchange: "logs_exchange",
                type: ExchangeType.Direct,
                durable: false,
                autoDelete: false,
                arguments: null);

            // Объявляем очередь
            await _channel.QueueDeclareAsync(
                queue: "logs",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Привязываем очередь к обмену с указанием routing_key
            await _channel.QueueBindAsync(
                queue: "logs",
                exchange: "logs_exchange",
                routingKey: "log"); // Ключ маршрутизации
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Создаем consumer для обработки сообщений
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var log = JsonConverter.FromJsonToLogEntity(message);

                // Сохраняем лог в ClickHouse
                if (log != null)
                {
                    await _clickHouseService.InsertLogAsync(log);
                }
            };

            // Начинаем слушать очередь
            await _channel.BasicConsumeAsync(
                queue: "logs",
                autoAck: true,
                consumer: consumer);

            // Ожидаем завершения работы сервиса
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override void Dispose()
        {
            // Освобождаем ресурсы RabbitMQ
            _channel.CloseAsync();
            _connection.CloseAsync();
            base.Dispose();
        }
    }
}
