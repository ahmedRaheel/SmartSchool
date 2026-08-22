using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using SmartSchool.Application.Persistence;
using SmartSchool.Infrastructure.Options;

namespace SmartSchool.Infrastructure.Persistence;

/// <summary>
/// Creates provider-specific connections for Dapper read queries.
/// </summary>
public sealed class DbConnectionFactory(
	IConfiguration configuration,
	IOptions<PersistenceOptions> persistenceOptions) : IDbConnectionFactory
{
	public async Task<DbConnection> OpenConnectionAsync(
		CancellationToken cancellationToken = default)
	{
		var options = persistenceOptions.Value;

		var connectionString = configuration.GetConnectionString(
			options.ConnectionStringName);

		if (string.IsNullOrWhiteSpace(connectionString))
		{
			throw new InvalidOperationException(
				$"Connection string '{options.ConnectionStringName}' was not found.");
		}

		DbConnection connection = options.Provider switch
		{
			PersistenceProvider.PostgreSql => new NpgsqlConnection(connectionString),
			PersistenceProvider.SqlServer => new SqlConnection(connectionString),
			_ => throw new InvalidOperationException(
				"Dapper reads require PostgreSQL or SQL Server.")
		};

		await connection.OpenAsync(cancellationToken);

		return connection;
	}
}
