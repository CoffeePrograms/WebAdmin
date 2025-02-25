using Google.Protobuf.WellKnownTypes;
using WebAdmin.Entities;
using WebAdmin.Grpc;

namespace WebAdmin.Converters
{
    public static class GrpcConverter
    {
        public static LogData ToGrpcMessage(LogEntity log)
        {
            return new LogData
            {
                Timestamp = Timestamp.FromDateTime(log.Timestamp.ToUniversalTime()),
                Level = log.Level,
                MessageTemplate = log.MessageTemplate,
                Method = log.Method,
                Path = log.Path,
                RequestBody = log.RequestBody,
                StatusCode = log.StatusCode,
                ResponseBody = log.ResponseBody,
                Controller = log.Controller,
                Application = log.Application,
                SourceContext = log.SourceContext,
                RequestId = log.RequestId,
                RequestPath = log.RequestPath,
                ConnectionId = log.ConnectionId,
                User = log.User,
                Exception = log.Exception
            };
        }

        public static LogEntity ToLogEntity(LogData log)
        {
            return new LogEntity
            {
                Timestamp = log.Timestamp.ToDateTime(),
                Level = log.Level,
                MessageTemplate = log.MessageTemplate,
                Method = log.Method,
                Path = log.Path,
                RequestBody = log.RequestBody,
                StatusCode = log.StatusCode,
                ResponseBody = log.ResponseBody,
                Controller = log.Controller,
                Application = log.Application,
                SourceContext = log.SourceContext,
                RequestId = log.RequestId,
                RequestPath = log.RequestPath,
                ConnectionId = log.ConnectionId,
                User = log.User,
                Exception = log.Exception
            };
        }
    }
}
