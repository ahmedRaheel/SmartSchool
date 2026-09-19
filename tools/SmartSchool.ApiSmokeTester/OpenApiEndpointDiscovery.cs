using System.Text.Json;

namespace SmartSchool.ApiSmokeTester;

internal sealed class OpenApiDocumentModel
{
    private readonly Dictionary<string, JsonElement> _schemas;

    private OpenApiDocumentModel(
        IReadOnlyList<OpenApiOperation> operations,
        Dictionary<string, JsonElement> schemas)
    {
        Operations = operations;
        _schemas = schemas;
    }

    public IReadOnlyList<OpenApiOperation> Operations { get; }

    public static OpenApiDocumentModel Parse(string json)
    {
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        var operations = new List<OpenApiOperation>();
        var schemas = ReadSchemas(root);

        if (!root.TryGetProperty("paths", out var paths))
        {
            return new OpenApiDocumentModel(operations, schemas);
        }

        foreach (var pathProperty in paths.EnumerateObject())
        {
            var pathItem = pathProperty.Value;
            var pathParameters = ReadParameters(pathItem);

            foreach (var operationProperty in pathItem.EnumerateObject())
            {
                var method = operationProperty.Name.ToUpperInvariant();

                if (method is not ("GET" or "POST" or "PUT" or "DELETE" or "PATCH"))
                {
                    continue;
                }

                var operation = operationProperty.Value;
                var operationParameters = ReadParameters(operation);
                var parameters = pathParameters
                    .Concat(operationParameters)
                    .GroupBy(
                        parameter => $"{parameter.Location}:{parameter.Name}",
                        StringComparer.OrdinalIgnoreCase)
                    .Select(group => group.Last())
                    .ToArray();

                var operationId = GetString(operation, "operationId")
                    ?? $"{method}_{pathProperty.Name}";

                var summary = GetString(operation, "summary") ?? string.Empty;
                JsonElement? requestBody = null;

                if (operation.TryGetProperty("requestBody", out var requestBodyElement))
                {
                    requestBody = requestBodyElement.Clone();
                }

                operations.Add(
                    new OpenApiOperation(
                        method,
                        pathProperty.Name,
                        operationId,
                        summary,
                        parameters,
                        requestBody));
            }
        }

        return new OpenApiDocumentModel(
            operations
                .OrderBy(operation => operation.Path, StringComparer.OrdinalIgnoreCase)
                .ThenBy(operation => operation.Method, StringComparer.OrdinalIgnoreCase)
                .ToArray(),
            schemas);
    }

    public bool TryResolveSchemaReference(
        string reference,
        out JsonElement schema)
    {
        const string prefix = "#/components/schemas/";

        if (!reference.StartsWith(prefix, StringComparison.Ordinal))
        {
            schema = default;
            return false;
        }

        var name = Uri.UnescapeDataString(reference[prefix.Length..]);
        return _schemas.TryGetValue(name, out schema);
    }

    private static Dictionary<string, JsonElement> ReadSchemas(JsonElement root)
    {
        var result = new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);

        if (!root.TryGetProperty("components", out var components)
            || !components.TryGetProperty("schemas", out var schemas))
        {
            return result;
        }

        foreach (var property in schemas.EnumerateObject())
        {
            result[property.Name] = property.Value.Clone();
        }

        return result;
    }

    private static IReadOnlyList<OpenApiParameter> ReadParameters(JsonElement element)
    {
        if (!element.TryGetProperty("parameters", out var parameters)
            || parameters.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        var result = new List<OpenApiParameter>();

        foreach (var parameter in parameters.EnumerateArray())
        {
            var name = GetString(parameter, "name") ?? string.Empty;
            var location = GetString(parameter, "in") ?? string.Empty;
            var required = parameter.TryGetProperty("required", out var requiredElement)
                && requiredElement.ValueKind == JsonValueKind.True;

            JsonElement schema = default;

            if (parameter.TryGetProperty("schema", out var schemaElement))
            {
                schema = schemaElement.Clone();
            }

            if (!string.IsNullOrWhiteSpace(name)
                && !string.IsNullOrWhiteSpace(location))
            {
                result.Add(new OpenApiParameter(name, location, required, schema));
            }
        }

        return result;
    }

    private static string? GetString(JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var property)
            && property.ValueKind == JsonValueKind.String
                ? property.GetString()
                : null;
    }
}
