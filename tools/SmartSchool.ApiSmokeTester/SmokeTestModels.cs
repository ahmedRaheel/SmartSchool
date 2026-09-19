using System.Text.Json;

namespace SmartSchool.ApiSmokeTester;

internal enum SmokeResultKind
{
    Passed,
    ValidationHandled,
    AuthenticationBlocked,
    FailedServerError,
    NetworkError,
    Timeout,
    Skipped
}

internal sealed record OpenApiParameter(
    string Name,
    string Location,
    bool Required,
    JsonElement Schema);

internal sealed record OpenApiOperation(
    string Method,
    string Path,
    string OperationId,
    string Summary,
    IReadOnlyList<OpenApiParameter> Parameters,
    JsonElement? RequestBody);

internal sealed record SmokeResult(
    string Method,
    string Path,
    string RequestUri,
    string OperationId,
    SmokeResultKind Result,
    int? StatusCode,
    long DurationMilliseconds,
    string Message,
    string ResponseBody);
