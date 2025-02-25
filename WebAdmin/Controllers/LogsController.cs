using Microsoft.AspNetCore.Mvc;
using WebAdmin.Entities;
using WebAdmin.Services;

namespace WebAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ClickHouseService _clickHouseService;

        public LogsController(ClickHouseService clickHouseService)
        {
            _clickHouseService = clickHouseService;
        }

        [HttpPost]
        public async Task<IActionResult> AddLog([FromBody] LogEntity log)
        {
            await _clickHouseService.InsertLogAsync(log);
            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<LogEntity>> GetAllLogs()
        {
            return await _clickHouseService.GetLogsAsync();
        }
    }
}