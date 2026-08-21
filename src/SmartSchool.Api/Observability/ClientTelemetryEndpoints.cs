using System.Diagnostics;
namespace SmartSchool.Api.Observability;
public sealed record ClientErrorRequest(string Message,string? Stack,string? Url,string? Method,int? Status,string? CorrelationId,string? TraceId,string? UserAgent,DateTimeOffset OccurredAt);
public static class ClientTelemetryEndpoints
{
 public static IEndpointRouteBuilder MapClientTelemetryEndpoints(this IEndpointRouteBuilder endpoints)
 {
  endpoints.MapPost("/api/telemetry/client-errors",(ClientErrorRequest request,HttpContext context,ILoggerFactory factory)=>
  {
   var logger=factory.CreateLogger("SmartSchool.Client");
   using(logger.BeginScope(new Dictionary<string,object?>{{"ClientCorrelationId",request.CorrelationId},{"ClientTraceId",request.TraceId},{"ClientUrl",request.Url},{"ClientStatus",request.Status}}))
    logger.LogError("Portal error: {Message}. Stack: {Stack}",request.Message,request.Stack);
   return Results.Accepted(value:new{correlationId=context.TraceIdentifier,traceId=Activity.Current?.TraceId.ToString()});
  }).AllowAnonymous().WithTags("Telemetry");
  return endpoints;
 }
}
