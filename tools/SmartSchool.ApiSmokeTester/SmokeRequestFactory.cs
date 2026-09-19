using System.Globalization;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SmartSchool.ApiSmokeTester;

internal sealed class SmokeRequestFactory(
    SmokeTestOptions options,
    OpenApiDocumentModel openApi)
{
    public HttpRequestMessage Create(OpenApiOperation operation)
    {
        var path = operation.Path;
        var queryValues = new List<KeyValuePair<string, string>>();

        foreach (var parameter in operation.Parameters)
        {
            var value = GenerateScalar(parameter.Name, parameter.Schema);

            if (parameter.Location.Equals("path", StringComparison.OrdinalIgnoreCase))
            {
                path = path.Replace(
                    "{" + parameter.Name + "}",
                    Uri.EscapeDataString(value),
                    StringComparison.OrdinalIgnoreCase);
            }
            else if (parameter.Location.Equals("query", StringComparison.OrdinalIgnoreCase)
                     && parameter.Required)
            {
                queryValues.Add(
                    new KeyValuePair<string, string>(parameter.Name, value));
            }
        }

        // Some OpenAPI generators omit explicit parameter metadata for constrained route values.
        path = ReplaceRemainingRouteTokens(path);

        var uriBuilder = new StringBuilder(options.BaseUrl);

        if (!path.StartsWith('/', StringComparison.Ordinal))
        {
            uriBuilder.Append('/');
        }

        uriBuilder.Append(path);

        if (queryValues.Count > 0)
        {
            uriBuilder.Append('?');
            uriBuilder.Append(
                string.Join(
                    "&",
                    queryValues.Select(
                        item =>
                            $"{Uri.EscapeDataString(item.Key)}={Uri.EscapeDataString(item.Value)}")));
        }

        var request = new HttpRequestMessage(
            new HttpMethod(operation.Method),
            uriBuilder.ToString());

        request.Headers.TryAddWithoutValidation("X-Smoke-Test", "true");
        request.Headers.TryAddWithoutValidation(
            "X-Correlation-ID",
            $"smoke-{Guid.NewGuid():N}");

        if (!string.IsNullOrWhiteSpace(options.BearerToken))
        {
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    options.BearerToken);
        }

        AddRequestBody(request, operation);

        return request;
    }

    private void AddRequestBody(
        HttpRequestMessage request,
        OpenApiOperation operation)
    {
        if (operation.RequestBody is null)
        {
            return;
        }

        var requestBody = operation.RequestBody.Value;

        if (!requestBody.TryGetProperty("content", out var content))
        {
            return;
        }

        if (content.TryGetProperty("application/json", out var jsonContent))
        {
            JsonNode? payload = new JsonObject();

            if (jsonContent.TryGetProperty("schema", out var schema))
            {
                payload = GenerateNode(schema, null, 0);
            }

            request.Content = JsonContent.Create(payload ?? new JsonObject());
            return;
        }

        if (content.TryGetProperty("multipart/form-data", out var multipartContent))
        {
            request.Content = CreateMultipartContent(multipartContent);
            return;
        }

        if (content.TryGetProperty(
                "application/x-www-form-urlencoded",
                out var formContent))
        {
            request.Content = CreateFormContent(formContent);
        }
    }

    private HttpContent CreateMultipartContent(JsonElement content)
    {
        var multipart = new MultipartFormDataContent();

        if (!content.TryGetProperty("schema", out var schema))
        {
            return multipart;
        }

        schema = ResolveSchema(schema);

        if (!schema.TryGetProperty("properties", out var properties))
        {
            return multipart;
        }

        foreach (var property in properties.EnumerateObject())
        {
            var propertySchema = ResolveSchema(property.Value);
            var format = GetString(propertySchema, "format");

            if (string.Equals(format, "binary", StringComparison.OrdinalIgnoreCase))
            {
                var file = new ByteArrayContent("SmartSchool smoke test"u8.ToArray());
                file.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
                multipart.Add(file, property.Name, "smoke-test.txt");
                continue;
            }

            multipart.Add(
                new StringContent(
                    GenerateScalar(property.Name, propertySchema),
                    Encoding.UTF8),
                property.Name);
        }

        return multipart;
    }

    private HttpContent CreateFormContent(JsonElement content)
    {
        var values = new List<KeyValuePair<string, string>>();

        if (!content.TryGetProperty("schema", out var schema))
        {
            return new FormUrlEncodedContent(values);
        }

        schema = ResolveSchema(schema);

        if (!schema.TryGetProperty("properties", out var properties))
        {
            return new FormUrlEncodedContent(values);
        }

        foreach (var property in properties.EnumerateObject())
        {
            values.Add(
                new KeyValuePair<string, string>(
                    property.Name,
                    GenerateScalar(property.Name, property.Value)));
        }

        return new FormUrlEncodedContent(values);
    }

    private JsonNode? GenerateNode(
        JsonElement schema,
        string? propertyName,
        int depth)
    {
        if (depth > 8)
        {
            return null;
        }

        schema = ResolveSchema(schema);

        if (schema.TryGetProperty("enum", out var enumValues)
            && enumValues.ValueKind == JsonValueKind.Array)
        {
            var first = enumValues.EnumerateArray().FirstOrDefault();
            return JsonNode.Parse(first.GetRawText());
        }

        if (schema.TryGetProperty("oneOf", out var oneOf)
            && oneOf.ValueKind == JsonValueKind.Array)
        {
            var first = oneOf.EnumerateArray().FirstOrDefault();
            return GenerateNode(first, propertyName, depth + 1);
        }

        if (schema.TryGetProperty("anyOf", out var anyOf)
            && anyOf.ValueKind == JsonValueKind.Array)
        {
            foreach (var candidate in anyOf.EnumerateArray())
            {
                if (candidate.TryGetProperty("type", out var typeElement)
                    && typeElement.ValueKind == JsonValueKind.String
                    && typeElement.GetString() == "null")
                {
                    continue;
                }

                return GenerateNode(candidate, propertyName, depth + 1);
            }
        }

        var type = GetString(schema, "type");

        if (string.Equals(type, "array", StringComparison.OrdinalIgnoreCase))
        {
            var array = new JsonArray();

            if (schema.TryGetProperty("items", out var items))
            {
                array.Add(GenerateNode(items, propertyName, depth + 1));
            }

            return array;
        }

        if (string.Equals(type, "object", StringComparison.OrdinalIgnoreCase)
            || schema.TryGetProperty("properties", out _))
        {
            var result = new JsonObject();

            if (!schema.TryGetProperty("properties", out var properties))
            {
                return result;
            }

            var required = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (schema.TryGetProperty("required", out var requiredElement)
                && requiredElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in requiredElement.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.String
                        && item.GetString() is { } requiredName)
                    {
                        required.Add(requiredName);
                    }
                }
            }

            foreach (var property in properties.EnumerateObject())
            {
                // Populate all properties, not only required ones. This produces more useful
                // requests for feature endpoints while remaining generic.
                result[property.Name] = GenerateNode(
                    property.Value,
                    property.Name,
                    depth + 1);
            }

            return result;
        }

        var scalar = GenerateScalar(propertyName ?? "value", schema);

        return type?.ToLowerInvariant() switch
        {
            "integer" => long.TryParse(
                scalar,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out var integerValue)
                    ? JsonValue.Create(integerValue)
                    : JsonValue.Create(1),
            "number" => decimal.TryParse(
                scalar,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out var numberValue)
                    ? JsonValue.Create(numberValue)
                    : JsonValue.Create(1m),
            "boolean" => JsonValue.Create(
                bool.TryParse(scalar, out var booleanValue) && booleanValue),
            _ => JsonValue.Create(scalar)
        };
    }

    private string GenerateScalar(string name, JsonElement schema)
    {
        if (TryGetKnownValue(name, out var knownValue))
        {
            return knownValue;
        }

        schema = ResolveSchema(schema);

        if (schema.TryGetProperty("enum", out var enumValues)
            && enumValues.ValueKind == JsonValueKind.Array)
        {
            var first = enumValues.EnumerateArray().FirstOrDefault();

            if (first.ValueKind == JsonValueKind.String)
            {
                return first.GetString() ?? "smoke-test";
            }

            return first.GetRawText();
        }

        var type = GetString(schema, "type");
        var format = GetString(schema, "format");

        if (IsGuidName(name)
            || string.Equals(format, "uuid", StringComparison.OrdinalIgnoreCase)
            || string.Equals(format, "guid", StringComparison.OrdinalIgnoreCase))
        {
            return DeterministicGuid(name).ToString();
        }

        if (string.Equals(format, "date", StringComparison.OrdinalIgnoreCase))
        {
            return DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyy-MM-dd");
        }

        if (string.Equals(format, "date-time", StringComparison.OrdinalIgnoreCase))
        {
            return DateTimeOffset.UtcNow.ToString("O", CultureInfo.InvariantCulture);
        }

        if (string.Equals(format, "email", StringComparison.OrdinalIgnoreCase)
            || name.Contains("email", StringComparison.OrdinalIgnoreCase))
        {
            return "smoke.test@example.com";
        }

        if (name.Contains("phone", StringComparison.OrdinalIgnoreCase)
            || name.Contains("mobile", StringComparison.OrdinalIgnoreCase))
        {
            return "+923001234567";
        }

        if (name.Contains("cnic", StringComparison.OrdinalIgnoreCase))
        {
            return "4210112345671";
        }

        if (name.Contains("gender", StringComparison.OrdinalIgnoreCase))
        {
            return "Male";
        }

        if (name.Contains("status", StringComparison.OrdinalIgnoreCase))
        {
            return "Active";
        }

        return type?.ToLowerInvariant() switch
        {
            "integer" => "1",
            "number" => "1",
            "boolean" => "true",
            _ => "smoke-test"
        };
    }

    private bool TryGetKnownValue(string name, out string value)
    {
        if (options.KnownValues.TryGetValue(name, out value!))
        {
            return true;
        }

        var normalized = NormalizeName(name);

        foreach (var item in options.KnownValues)
        {
            if (NormalizeName(item.Key).Equals(
                    normalized,
                    StringComparison.OrdinalIgnoreCase))
            {
                value = item.Value;
                return true;
            }
        }

        value = string.Empty;
        return false;
    }

    private string ReplaceRemainingRouteTokens(string path)
    {
        var output = path;
        var start = output.IndexOf('{');

        while (start >= 0)
        {
            var end = output.IndexOf('}', start + 1);

            if (end < 0)
            {
                break;
            }

            var token = output[(start + 1)..end];
            var name = token.Split(':', 2)[0].TrimEnd('?');
            var replacement = TryGetKnownValue(name, out var knownValue)
                ? knownValue
                : IsGuidName(name)
                    ? DeterministicGuid(name).ToString()
                    : name.Contains("id", StringComparison.OrdinalIgnoreCase)
                        ? DeterministicGuid(name).ToString()
                        : "smoke-test";

            output = output[..start]
                     + Uri.EscapeDataString(replacement)
                     + output[(end + 1)..];
            start = output.IndexOf('{');
        }

        return output;
    }

    private JsonElement ResolveSchema(JsonElement schema)
    {
        var current = schema;
        var attempts = 0;

        while (attempts < 8
               && current.ValueKind == JsonValueKind.Object
               && current.TryGetProperty("$ref", out var referenceElement)
               && referenceElement.ValueKind == JsonValueKind.String
               && referenceElement.GetString() is { } reference
               && openApi.TryResolveSchemaReference(reference, out var resolved))
        {
            current = resolved;
            attempts++;
        }

        return current;
    }

    private static bool IsGuidName(string name)
    {
        return name.Equals("id", StringComparison.OrdinalIgnoreCase)
               || name.EndsWith("Id", StringComparison.OrdinalIgnoreCase)
               || name.EndsWith("Ids", StringComparison.OrdinalIgnoreCase);
    }

    private static Guid DeterministicGuid(string name)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(name.ToLowerInvariant()));
        return new Guid(bytes.AsSpan(0, 16));
    }

    private static string NormalizeName(string name)
    {
        return new string(
            name.Where(char.IsLetterOrDigit)
                .Select(char.ToLowerInvariant)
                .ToArray());
    }

    private static string? GetString(JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var property)
               && property.ValueKind == JsonValueKind.String
            ? property.GetString()
            : null;
    }
}
