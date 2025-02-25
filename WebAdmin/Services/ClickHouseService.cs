using Microsoft.Extensions.Options;
using WebAdmin.Configuration;
using WebAdmin.Db;
using WebAdmin.Entities;

namespace WebAdmin.Services
{
    public class ClickHouseService
    {
        private readonly ClickHouseRepository _repository;

        public ClickHouseService(IOptions<ClickHouseSettings> settings)
        {
            var connectionString = GetConnectionString(settings.Value);
            _repository = new ClickHouseRepository(connectionString);
        }

        private string GetConnectionString(ClickHouseSettings settings)
        {
            return $"Host={settings.Host};Port={settings.Port};Database={settings.Database};User={settings.User};Password={settings.Password};";
        }

        public async Task InsertLogAsync(LogEntity log)
        {
            await _repository.InsertLogAsync(log);
        }

        public async Task<IEnumerable<LogEntity>> GetLogsAsync()
        {
            return await _repository.GetLogsAsync();
        }
    }
}
