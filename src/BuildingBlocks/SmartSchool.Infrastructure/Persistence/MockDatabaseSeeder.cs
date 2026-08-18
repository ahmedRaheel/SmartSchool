using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SmartSchool.SharedKernel;

namespace SmartSchool.Infrastructure.Persistence;

/// <summary>
/// Seeds representative records for every SmartSchool entity type
/// in the development EF Core database.
/// </summary>
public sealed class MockDatabaseSeeder(
	SmartSchoolMockDbContext dbContext)
{
	private const int RecordsPerEntity = 3;

	private const string SeedMetadata =
		"{\"source\":\"ef-inmemory-seed\"}";

	/// <summary>
	/// Gets the tenant identifier used by the development mock data.
	/// </summary>
	public static readonly Guid DemoTenantId =
		Guid.Parse("11111111-1111-1111-1111-111111111111");

	/// <summary>
	/// Creates development records for entity types that do not already
	/// contain data for the demo tenant.
	/// </summary>
	/// <param name="cancellationToken">
	/// Token used to cancel the asynchronous operation.
	/// </param>
	public async Task SeedAsync(
		CancellationToken cancellationToken = default)
	{
		await dbContext.Database.EnsureCreatedAsync(
			cancellationToken);

		var entityTypes = dbContext.Model
			.GetEntityTypes()
			.Select(entityType => entityType.ClrType)
			.Where(entityType => typeof(Entity).IsAssignableFrom(entityType))
			.Distinct()
			.ToList();

		foreach (var entityType in entityTypes)
		{
			var tenantAlreadySeeded = await IsTenantSeededAsync(
				entityType,
				cancellationToken);

			if (tenantAlreadySeeded)
			{
				continue;
			}

			var createMethod = FindCreateMethod(entityType);

			if (createMethod is null)
			{
				continue;
			}

			await SeedEntityTypeAsync(
				entityType,
				createMethod,
				cancellationToken);
		}

		await dbContext.SaveChangesAsync(
			cancellationToken);
	}

	/// <summary>
	/// Determines whether the specified entity type already contains
	/// records for the development tenant.
	/// </summary>
	private async Task<bool> IsTenantSeededAsync(
		Type entityType,
		CancellationToken cancellationToken)
	{
		var setMethod = typeof(DbContext)
			.GetMethods(BindingFlags.Public | BindingFlags.Instance)
			.Single(method =>
				method.Name == nameof(DbContext.Set) &&
				method.IsGenericMethodDefinition &&
				method.GetParameters().Length == 0);

		var genericSetMethod = setMethod.MakeGenericMethod(entityType);

		var entitySet = genericSetMethod.Invoke(
			dbContext,
			null);

		if (entitySet is not IQueryable queryable)
		{
			return false;
		}

		return await TenantExistsAsync(
			queryable,
			entityType,
			cancellationToken);
	}

	/// <summary>
	/// Executes the tenant existence query for a runtime entity type.
	/// </summary>
	private static async Task<bool> TenantExistsAsync(
		IQueryable queryable,
		Type entityType,
		CancellationToken cancellationToken)
	{
		var method = typeof(MockDatabaseSeeder)
			.GetMethod(
				nameof(TenantExistsCoreAsync),
				BindingFlags.NonPublic | BindingFlags.Static);

		if (method is null)
		{
			throw new InvalidOperationException(
				"Unable to locate the tenant existence method.");
		}

		var genericMethod = method.MakeGenericMethod(entityType);

		var task = genericMethod.Invoke(
			null,
			[
				queryable,
				cancellationToken
			]);

		if (task is not Task<bool> resultTask)
		{
			throw new InvalidOperationException(
				$"Unable to query entity type '{entityType.Name}'.");
		}

		return await resultTask;
	}

	/// <summary>
	/// Determines whether the demo tenant already has data
	/// for the specified entity type.
	/// </summary>
	private static Task<bool> TenantExistsCoreAsync<TEntity>(
		IQueryable queryable,
		CancellationToken cancellationToken)
		where TEntity : Entity
	{
		return queryable
			.Cast<TEntity>()
			.AnyAsync(
				entity => entity.TenantId == DemoTenantId,
				cancellationToken);
	}

	/// <summary>
	/// Finds the public static factory method used to create
	/// the specified entity type.
	/// </summary>
	private static MethodInfo? FindCreateMethod(
		Type entityType)
	{
		return entityType.GetMethod(
			"Create",
			BindingFlags.Public | BindingFlags.Static);
	}

	/// <summary>
	/// Creates and tracks development records for one entity type.
	/// </summary>
	private async Task SeedEntityTypeAsync(
		Type entityType,
		MethodInfo createMethod,
		CancellationToken cancellationToken)
	{
		for (var index = 1; index <= RecordsPerEntity; index++)
		{
			var entity = CreateSeedEntity(
				entityType,
				createMethod,
				index);

			if (entity is null)
			{
				continue;
			}

			await dbContext.AddAsync(
				entity,
				cancellationToken);
		}
	}

	/// <summary>
	/// Creates a representative development entity by invoking
	/// its static factory method.
	/// </summary>
	private static object? CreateSeedEntity(
		Type entityType,
		MethodInfo createMethod,
		int index)
	{
		var entityName = entityType.Name.Replace(
			"Entity",
			string.Empty,
			StringComparison.Ordinal);

		var code =
			$"DEMO-{entityName.ToUpperInvariant()}-{index:000}";

		var name =
			$"Demo {entityName} {index}";

		return createMethod.Invoke(
			null,
			[
				DemoTenantId,
				code,
				name,
				SeedMetadata
			]);
	}
}
