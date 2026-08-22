namespace SmartSchool.SharedKernel;

/// <summary>Stable HTTP contract returned by every SmartSchool API endpoint.</summary>
public sealed record ApiResponse<T>(
    bool Success,
    T? Data,
    ApiError? Error,
    string TraceId,
    DateTimeOffset TimestampUtc)
{
    public static ApiResponse<T> Ok(T? data, string traceId) =>
        new(true, data, null, traceId, DateTimeOffset.UtcNow);

    public static ApiResponse<T> Fail(string code, string message, string traceId) =>
        new(false, default, new ApiError(code, message), traceId, DateTimeOffset.UtcNow);
}

public sealed record ApiError(string Code, string Message);
