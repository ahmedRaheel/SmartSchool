using System.Data;
using System.Data.Common;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Infrastructure.Persistence;

/// <summary>
/// Generates business identifiers using a database-backed atomic sequence.
/// </summary>
public sealed class BusinessNumberGenerator(IDbConnectionFactory connectionFactory)
    : IBusinessNumberGenerator
{
    public async Task<string> NextAsync(
        string sequenceName,
        string prefix,
        Guid? tenantId,
        int padding,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sequenceName);
        ArgumentException.ThrowIfNullOrWhiteSpace(prefix);

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        await using var command = connection.CreateCommand();

        var provider = connection.GetType().Name;
        command.CommandText = provider.Contains("Npgsql", StringComparison.OrdinalIgnoreCase)
            ? PostgreSql
            : SqlServer;

        AddParameter(command, "sequenceName", sequenceName.Trim().ToUpperInvariant());
        AddParameter(command, "tenantId", tenantId ?? Guid.Empty);

        var value = Convert.ToInt64(await command.ExecuteScalarAsync(cancellationToken));
        return $"{prefix.Trim().ToUpperInvariant()}{value.ToString().PadLeft(padding, '0')}";
    }

    private static void AddParameter(DbCommand command, string name, object value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        command.Parameters.Add(parameter);
    }

    private const string PostgreSql = """
        INSERT INTO platform.business_number_sequence (tenant_id, sequence_name, last_value)
        VALUES (@tenantId, @sequenceName, 1)
        ON CONFLICT (tenant_id, sequence_name)
        DO UPDATE SET last_value = platform.business_number_sequence.last_value + 1
        RETURNING last_value;
        """;

    private const string SqlServer = """
        MERGE platform.business_number_sequence WITH (HOLDLOCK) AS target
        USING (SELECT @tenantId AS tenant_id, @sequenceName AS sequence_name) AS source
        ON target.tenant_id = source.tenant_id AND target.sequence_name = source.sequence_name
        WHEN MATCHED THEN UPDATE SET last_value = target.last_value + 1
        WHEN NOT MATCHED THEN INSERT (tenant_id, sequence_name, last_value) VALUES (@tenantId, @sequenceName, 1)
        OUTPUT inserted.last_value;
        """;
}
