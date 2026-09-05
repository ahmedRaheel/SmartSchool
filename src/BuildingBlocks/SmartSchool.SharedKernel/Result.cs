using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.SharedKernel;

public sealed record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static Error Validation(string message) =>
        new(ErrorCodes.Validation, message);

    public static Error NotFound(string message) =>
        new(ErrorCodes.NotFound, message);

    public static Error Conflict(string message) =>
        new(ErrorCodes.Conflict, message);

    public static Error Unauthorized(string message) =>
        new(ErrorCodes.Unauthorized, message);

    public static Error Forbidden(string message) =>
        new(ErrorCodes.Forbidden, message);

    public static Error InternalServerError(string message) =>
        new(ErrorCodes.InternalServerError, message);
}

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() =>
        new(true, Error.None);

    public static Result Failure(Error error) =>
        new(false, error);
}

public sealed class Result<T> : Result
{
    private Result(T? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public T? Value { get; }

    public static Result<T> Success(T value) =>
        new(value, true, Error.None);

    public static new Result<T> Failure(Error error) =>
        new(default, false, error);
}

public sealed record PagedResult<T>(
    IReadOnlyCollection<T> Items,
    int Page,
    int PageSize,
    long TotalCount);
