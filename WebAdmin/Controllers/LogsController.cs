using Microsoft.AspNetCore.Mvc;
using WebAdmin.Entities;
using WebAdmin.Grpc;
using WebAdmin.Services;

namespace WebAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ClickHouseService _clickHouseService;
        private readonly LogGrpcService _logGrpcService;

        public LogsController(ClickHouseService clickHouseService, LogGrpcService logGrpcService)
        {
            _clickHouseService = clickHouseService;
            _logGrpcService = logGrpcService;
        }

        [HttpGet]
        public async Task<IEnumerable<LogEntity>> GetAllLogsLikeEntities()
        {
            return await _clickHouseService.GetLogsAsync();
        }

        [HttpGet("grpc")]
        public async Task<GetLogsResponse> GetAllLogsLikeGrpc()
        {
            // Создаем запрос для gRPC-сервиса
            var request = new GetLogsRequest();

            // Вызываем gRPC-сервис и получаем ответ
            var response = await _logGrpcService.GetLogsAsync(request, null);

            // Возвращаем ответ в формате gRPC
            return response;
        }
    }
}