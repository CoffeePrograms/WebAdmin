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

        public override async Task<GetLogsResponse> GetLogsAsync(GetLogsRequest request, ServerCallContext context)
        {
            var logs = await _repository.GetLogsAsync(
                timestamp: request.Filter.Timestamp.ToDateTime(),
                level: request.Filter.Level,
                messageTemplate: request.Filter.MessageTemplate,
                method: request.Filter.Method,
                path: request.Filter.Path,
                requestBody: request.Filter.RequestBody,
                statusCode: request.Filter.StatusCode,
                responseBody: request.Filter.ResponseBody,
                controller: request.Filter.Controller,
                application: request.Filter.Application,
                sourceContext: request.Filter.SourceContext,
                requestId: request.Filter.RequestId,
                requestPath: request.Filter.RequestPath,
                connectionId: request.Filter.ConnectionId,
                user: request.Filter.User,
                exception: request.Filter.Exception
            );

            var response = new GetLogsResponse();
            response.Logs.AddRange(logs.Select(log => new LogResponse
            {
                Data = GrpcConverter.ToGrpcMessage(log)
            }));

            return response;
        }
    }
}
