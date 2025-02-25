using Grpc.Core;
using WebAdmin.Converters;
using WebAdmin.Db;
using WebAdmin.Grpc;

namespace WebAdmin.Services
{
    public class LogGrpcService : LogService.LogServiceBase
    {
        private readonly ClickHouseRepository _repository;

        public LogGrpcService(ClickHouseRepository repository)
        {
            _repository = repository;
        }

        public override async Task<LogResponse> InsertLog(LogRequest request, ServerCallContext context)
        {
            var logEntity = GrpcConverter.ToLogEntity(request);

            var insertedLog = await _repository.InsertLogAsync(logEntity);

            return new LogResponse
            {
                RequestId = insertedLog?.RequestId ?? string.Empty
            };
        }

        public override async Task<GetLogsResponse> GetLogs(GetLogsRequest request, ServerCallContext context)
        {
            var logs = await _repository.GetLogsAsync(
                timestamp: request.Timestamp.ToDateTime(),
                level: request.Level,
                messageTemplate: request.MessageTemplate,
                method: request.Method,
                path: request.Path,
                requestBody: request.RequestBody,
                statusCode: request.StatusCode,
                responseBody: request.ResponseBody,
                controller: request.Controller,
                application: request.Application,
                sourceContext: request.SourceContext,
                requestId: request.RequestId,
                requestPath: request.RequestPath,
                connectionId: request.ConnectionId,
                user: request.User,
                exception: request.Exception
            );

            var response = new GetLogsResponse();
            response.Logs.AddRange(logs.Select(log => new LogResponse
            {
                RequestId = log.RequestId
            }));

            return response;
        }
    }
}
