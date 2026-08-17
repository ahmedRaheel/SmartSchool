namespace SmartSchool.SharedKernel.Constants;

/// <summary>
/// Default API messages. These are fallback messages only.
/// TenantEntity-configurable business messages should be resolved through IMessageProvider.
/// </summary>
public static class ErrorMessages
{
    public const string RequestFailed = "The request could not be completed.";
    public const string UnexpectedError = "An unexpected error occurred.";
    public const string ValidationFailed = "One or more validation errors occurred.";

    public static string EntityNotFound(string entityName) =>
        $"{entityName} was not found.";

    public static string DuplicateCode(
        string entityName,
        string code) =>
        $"A {entityName} with code '{code}' already exists.";
}
