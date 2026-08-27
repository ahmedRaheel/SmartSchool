using System.Text.Json;

using SmartSchool.Application.Validation;

namespace SmartSchool.Api.Middleware;

/// <summary>
/// Enforces SmartSchool's canonical email, CNIC, telephone and mobile formats
/// for JSON request contracts before a request reaches a feature endpoint.
/// </summary>
public sealed class BusinessContactValidationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (!ShouldValidate(context.Request))
        {
            await next(context);
            return;
        }

        context.Request.EnableBuffering();

        using var document = await JsonDocument.ParseAsync(
            context.Request.Body,
            cancellationToken: context.RequestAborted);

        context.Request.Body.Position = 0;

        var errors = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        ValidateElement(document.RootElement, string.Empty, errors);

        if (errors.Count == 0)
        {
            await next(context);
            return;
        }

        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        await context.Response.WriteAsJsonAsync(
            new
            {
                code = "INVALID_CONTACT_FORMAT",
                message = "One or more contact fields have an invalid format.",
                errors
            },
            context.RequestAborted);
    }

    private static bool ShouldValidate(HttpRequest request)
    {
        if (request.ContentLength is null or 0)
        {
            return false;
        }

        return request.HasJsonContentType();
    }

    private static void ValidateElement(
        JsonElement element,
        string path,
        IDictionary<string, string> errors)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in element.EnumerateObject())
            {
                var propertyPath = string.IsNullOrWhiteSpace(path)
                    ? property.Name
                    : $"{path}.{property.Name}";

                ValidateProperty(property.Name, property.Value, propertyPath, errors);
                ValidateElement(property.Value, propertyPath, errors);
            }

            return;
        }

        if (element.ValueKind != JsonValueKind.Array)
        {
            return;
        }

        var index = 0;
        foreach (var item in element.EnumerateArray())
        {
            ValidateElement(item, $"{path}[{index}]", errors);
            index++;
        }
    }

    private static bool IsEmailProperty(string propertyName)
    {
        return propertyName.Contains("email", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsCnicProperty(string propertyName)
    {
        return propertyName.Contains("cnic", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsTelephoneProperty(string propertyName)
    {
        return propertyName.Contains("telephone", StringComparison.OrdinalIgnoreCase)
            || propertyName.Contains("landline", StringComparison.OrdinalIgnoreCase)
            || propertyName.Contains("fax", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsMobileProperty(string propertyName)
    {
        return !IsTelephoneProperty(propertyName)
            && (propertyName.Contains("mobile", StringComparison.OrdinalIgnoreCase)
                || propertyName.Contains("phone", StringComparison.OrdinalIgnoreCase));
    }

    private static void ValidateProperty(
        string propertyName,
        JsonElement value,
        string path,
        IDictionary<string, string> errors)
    {
        if (value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return;
        }

        if (value.ValueKind != JsonValueKind.String)
        {
            return;
        }

        var text = value.GetString();
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        if (IsEmailProperty(propertyName) && !ContactInformationRules.IsValidEmail(text))
        {
            errors[path] = "Enter a valid email address, for example name@example.com.";
            return;
        }

        if (IsCnicProperty(propertyName) && !ContactInformationRules.IsValidCnic(text))
        {
            errors[path] = $"CNIC must use the format {ContactInformationRules.CnicExample}.";
            return;
        }

        if (IsTelephoneProperty(propertyName) && !ContactInformationRules.IsValidTelephone(text))
        {
            errors[path] = $"Telephone must use the format {ContactInformationRules.TelephoneExample}.";
            return;
        }

        if (IsMobileProperty(propertyName) && !ContactInformationRules.IsValidMobile(text))
        {
            errors[path] = $"Mobile must use the format {ContactInformationRules.MobileExample}.";
        }
    }
}
