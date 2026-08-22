using Microsoft.EntityFrameworkCore;

namespace SmartSchool.Modules.Identity.Persistence;

internal static class IdentityDatabaseProvider
{
	public const string PostgreSql = "PostgreSql";
	public const string SqlServer = "SqlServer";

	public static void Configure(
		DbContextOptionsBuilder builder,
		string provider,
		string connectionString,
		string migrationsAssembly,
		string migrationsHistoryTable,
		string migrationsHistorySchema)
	{
		if (provider.Equals(PostgreSql, StringComparison.OrdinalIgnoreCase))
		{
			builder.UseNpgsql(connectionString, options =>
			{
				options.MigrationsAssembly(migrationsAssembly);
				options.MigrationsHistoryTable(migrationsHistoryTable, migrationsHistorySchema);
			});
			return;
		}

		if (provider.Equals(SqlServer, StringComparison.OrdinalIgnoreCase))
		{
			builder.UseSqlServer(connectionString, options =>
			{
				options.MigrationsAssembly(migrationsAssembly);
				options.MigrationsHistoryTable(migrationsHistoryTable, migrationsHistorySchema);
			});
			return;
		}

		throw new NotSupportedException(
			$"Identity database provider '{provider}' is not supported. Use PostgreSql or SqlServer.");
	}
}
