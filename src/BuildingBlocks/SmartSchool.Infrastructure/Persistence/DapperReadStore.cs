using System.Data.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Infrastructure.Persistence;

/// <summary>
/// PostgreSQL/SQL Server read store. SQL identifiers are taken from EF metadata
/// so Dapper and EF cannot silently drift to different tables/columns.
/// </summary>
public sealed class DapperReadStore(ApplicationDbContext dbContext) : IDapperReadStore
{
	public async Task<PagedResult<TEntity>> GetPageAsync<TEntity>(
		Guid tenantId,
		int page,
		int pageSize,
		IReadOnlyCollection<string> projectedProperties,
		CancellationToken cancellationToken)
		where TEntity : Entity
	{
		var metadata = GetMetadata<TEntity>();
		var properties = ResolveProjection(metadata, projectedProperties);

		var selectList = string.Join(
			", ",
			properties.Select(property =>
				$"{Quote(property.ColumnName)} AS {Quote(property.PropertyName)}"));

		var tenantColumn = GetColumn(metadata, nameof(Entity.TenantId));
		var idColumn = GetColumn(metadata, nameof(Entity.Id));
		var activeProperty = metadata.FindProperty(nameof(Entity.IsActive));
		var activePredicate = activeProperty is null
			? string.Empty
			: $" AND {Quote(GetColumn(metadata, nameof(Entity.IsActive)))} = @IsActive";

		var table = Qualify(metadata.Schema, metadata.TableName);

		var countSql =
			$"SELECT COUNT(*) FROM {table} " +
			$"WHERE {Quote(tenantColumn)} = @TenantId{activePredicate};";

		var pageSql =
			$"SELECT {selectList} FROM {table} " +
			$"WHERE {Quote(tenantColumn)} = @TenantId{activePredicate} " +
			$"ORDER BY {Quote(idColumn)} " +
			"OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

		// PostgreSQL uses LIMIT/OFFSET rather than FETCH NEXT in the common form.
		if (dbContext.Database.IsNpgsql())
		{
			pageSql =
				$"SELECT {selectList} FROM {table} " +
				$"WHERE {Quote(tenantColumn)} = @TenantId{activePredicate} " +
				$"ORDER BY {Quote(idColumn)} " +
				"LIMIT @PageSize OFFSET @Offset;";
		}

		var parameters = new
		{
			TenantId = tenantId,
			IsActive = true,
			Offset = (page - 1) * pageSize,
			PageSize = pageSize
		};

		var connection = dbContext.Database.GetDbConnection();
		await EnsureOpenAsync(connection, cancellationToken);

		var total = await connection.ExecuteScalarAsync<long>(
			new CommandDefinition(
				countSql,
				parameters,
				cancellationToken: cancellationToken));

		var items = (await connection.QueryAsync<TEntity>(
			new CommandDefinition(
				pageSql,
				parameters,
				cancellationToken: cancellationToken)))
			.AsList();

		return new PagedResult<TEntity>(items, page, pageSize, total);
	}


	public async Task<PagedResult<TEntity>> GetFilteredPageAsync<TEntity>(
		Guid tenantId,
		int page,
		int pageSize,
		IReadOnlyCollection<string> projectedProperties,
		IReadOnlyDictionary<string, object?> filters,
		string orderByProperty,
		bool descending,
		CancellationToken cancellationToken)
		where TEntity : Entity
	{
		var metadata = GetMetadata<TEntity>();
		var properties = ResolveProjection(metadata, projectedProperties);
		var selectList = string.Join(", ", properties.Select(
			property => $"{Quote(property.ColumnName)} AS {Quote(property.PropertyName)}"));
		var table = Qualify(metadata.Schema, metadata.TableName);
		var tenantColumn = GetColumn(metadata, nameof(Entity.TenantId));

		var predicates = new List<string> { $"{Quote(tenantColumn)} = @TenantId" };
		var parameters = new DynamicParameters();
		parameters.Add("TenantId", tenantId);
		parameters.Add("Offset", (page - 1) * pageSize);
		parameters.Add("PageSize", pageSize);

		var index = 0;
		foreach (var filter in filters)
		{
			var parameterName = $"Filter{index++}";
			predicates.Add($"{Quote(GetColumn(metadata, filter.Key))} = @{parameterName}");
			parameters.Add(parameterName, filter.Value);
		}

		var where = string.Join(" AND ", predicates);
		var orderColumn = GetColumn(metadata, orderByProperty);
		var direction = descending ? "DESC" : "ASC";
		var countSql = $"SELECT COUNT(*) FROM {table} WHERE {where};";
		var pageSql = dbContext.Database.IsNpgsql()
			? $"SELECT {selectList} FROM {table} WHERE {where} ORDER BY {Quote(orderColumn)} {direction} LIMIT @PageSize OFFSET @Offset;"
			: $"SELECT {selectList} FROM {table} WHERE {where} ORDER BY {Quote(orderColumn)} {direction} OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

		var connection = dbContext.Database.GetDbConnection();
		await EnsureOpenAsync(connection, cancellationToken);

		var total = await connection.ExecuteScalarAsync<long>(
			new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken));
		var items = (await connection.QueryAsync<TEntity>(
			new CommandDefinition(pageSql, parameters, cancellationToken: cancellationToken))).AsList();

		return new PagedResult<TEntity>(items, page, pageSize, total);
	}

	public async Task<int> CountAsync<TEntity>(
		Guid tenantId,
		IReadOnlyDictionary<string, object?> filters,
		CancellationToken cancellationToken)
		where TEntity : Entity
	{
		var metadata = GetMetadata<TEntity>();
		var table = Qualify(metadata.Schema, metadata.TableName);
		var tenantColumn = GetColumn(metadata, nameof(Entity.TenantId));
		var predicates = new List<string> { $"{Quote(tenantColumn)} = @TenantId" };
		var parameters = new DynamicParameters();
		parameters.Add("TenantId", tenantId);

		var index = 0;
		foreach (var filter in filters)
		{
			var parameterName = $"Filter{index++}";
			predicates.Add($"{Quote(GetColumn(metadata, filter.Key))} = @{parameterName}");
			parameters.Add(parameterName, filter.Value);
		}

		var sql = $"SELECT COUNT(*) FROM {table} WHERE {string.Join(" AND ", predicates)};";
		var connection = dbContext.Database.GetDbConnection();
		await EnsureOpenAsync(connection, cancellationToken);

		return await connection.ExecuteScalarAsync<int>(
			new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
	}

	public async Task<bool> ExistsAsync<TEntity>(
		Guid tenantId,
		string propertyName,
		object value,
		Guid? excludingId,
		CancellationToken cancellationToken)
		where TEntity : Entity
	{
		var metadata = GetMetadata<TEntity>();
		var tenantColumn = GetColumn(metadata, nameof(Entity.TenantId));
		var idColumn = GetColumn(metadata, nameof(Entity.Id));
		var valueColumn = GetColumn(metadata, propertyName);
		var table = Qualify(metadata.Schema, metadata.TableName);

		var sql =
			$"SELECT CASE WHEN EXISTS (" +
			$"SELECT 1 FROM {table} " +
			$"WHERE {Quote(tenantColumn)} = @TenantId " +
			$"AND {Quote(valueColumn)} = @Value " +
			$"AND (@ExcludingId IS NULL OR {Quote(idColumn)} <> @ExcludingId)" +
			$") THEN 1 ELSE 0 END;";

		var connection = dbContext.Database.GetDbConnection();
		await EnsureOpenAsync(connection, cancellationToken);

		var result = await connection.ExecuteScalarAsync<int>(
			new CommandDefinition(
				sql,
				new { TenantId = tenantId, Value = value, ExcludingId = excludingId },
				cancellationToken: cancellationToken));

		return result == 1;
	}

	private EntitySqlMetadata GetMetadata<TEntity>() where TEntity : Entity
	{
		var entityType = dbContext.Model.FindEntityType(typeof(TEntity))
			?? throw new InvalidOperationException(
				$"EF metadata for '{typeof(TEntity).Name}' was not found.");

		var tableName = entityType.GetTableName()
			?? throw new InvalidOperationException(
				$"Table mapping for '{typeof(TEntity).Name}' was not found.");

		return new EntitySqlMetadata(entityType, tableName, entityType.GetSchema());
	}

	private static IReadOnlyCollection<ProjectionColumn> ResolveProjection(
		EntitySqlMetadata metadata,
		IReadOnlyCollection<string> requestedProperties)
	{
		var required = requestedProperties
			.Concat([nameof(Entity.Id), nameof(Entity.TenantId)])
			.Distinct(StringComparer.Ordinal)
			.ToArray();

		return required
			.Select(propertyName =>
			{
				var property = metadata.EntityType.FindProperty(propertyName)
					?? throw new InvalidOperationException(
						$"Property '{propertyName}' is not mapped for '{metadata.EntityType.ClrType.Name}'.");

				var storeObject = StoreObjectIdentifier.Table(
					metadata.TableName,
					metadata.Schema);

				var columnName = property.GetColumnName(storeObject)
					?? throw new InvalidOperationException(
						$"Column mapping for '{metadata.EntityType.ClrType.Name}.{propertyName}' was not found.");

				return new ProjectionColumn(propertyName, columnName);
			})
			.ToArray();
	}

	private static string GetColumn(
		EntitySqlMetadata metadata,
		string propertyName)
	{
		var property = metadata.EntityType.FindProperty(propertyName)
			?? throw new InvalidOperationException(
				$"Property '{propertyName}' is not mapped for '{metadata.EntityType.ClrType.Name}'.");

		var storeObject = StoreObjectIdentifier.Table(
			metadata.TableName,
			metadata.Schema);

		return property.GetColumnName(storeObject)
			?? throw new InvalidOperationException(
				$"Column mapping for '{metadata.EntityType.ClrType.Name}.{propertyName}' was not found.");
	}

	private string Qualify(string? schema, string tableName)
	{
		return string.IsNullOrWhiteSpace(schema)
			? Quote(tableName)
			: $"{Quote(schema)}.{Quote(tableName)}";
	}

	private string Quote(string identifier)
	{
		if (dbContext.Database.IsNpgsql())
		{
			return $"\"{identifier.Replace("\"", "\"\"")}\"";
		}

		return $"[{identifier.Replace("]", "]]")}]";
	}

	private static async Task EnsureOpenAsync(
		DbConnection connection,
		CancellationToken cancellationToken)
	{
		if (connection.State != System.Data.ConnectionState.Open)
		{
			await connection.OpenAsync(cancellationToken);
		}
	}

	private sealed record EntitySqlMetadata(
		IEntityType EntityType,
		string TableName,
		string? Schema);

	private sealed record ProjectionColumn(
		string PropertyName,
		string ColumnName);
}
