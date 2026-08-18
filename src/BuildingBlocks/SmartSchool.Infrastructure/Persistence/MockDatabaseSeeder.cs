using System.Threading.Tasks;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SmartSchool.SharedKernel;

namespace SmartSchool.Infrastructure.Persistence;

/// <summary>
/// Seeds representative records for every SmartSchool entity type
/// in the development EF Core database.
/// </summary>
public sealed class MockDatabaseSeeder(SmartSchoolMockDbContext dbContext)
{
	/// <summary>
	/// Gets the tenant identifier used by the development mock data.
	/// </summary>
	public static readonly Guid DemoTenantId =
		Guid.Parse("11111111-1111-1111-1111-111111111111");

	/// <summary>
	/// Creates development records for entity types that do not already
	/// contain data for the demo tenant.
	/// </summary>
	/// <param name="cancellationToken">Token used to cancel the asynchronous operation.</param>
	public async Task SeedAsync(CancellationToken cancellationToken = default)
	{
		await dbContext.Database.EnsureCreatedAsync(cancellationToken);

		var entityTypes = dbContext.Model
			.GetEntityTypes()
			.Select(entityType => entityType.ClrType);

		foreach (var entityType in entityTypes)
		{
			var entitySet = dbContext.Set(entityType);

			var tenantAlreadySeeded = await entitySet
				.Cast<Entity>()
				.AnyAsync(
					entity => entity.TenantId == DemoTenantId,
					cancellationToken);

			if (tenantAlreadySeeded)
			{
				continue;
			}

			var createMethod = entityType.GetMethod(
				"Create",
				BindingFlags.Public | BindingFlags.Static);

			if (createMethod is null)
			{
				continue;
			}

			const int recordsPerEntity = 3;

			for (var index = 1; index <= recordsPerEntity; index++)
			{
				var entity = CreateSeedEntity(entityType, createMethod, index);

				if (entity is not null)
				{
					await entitySet.AddAsync(entity, cancellationToken);
				}
			}
		}

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	private static object? CreateSeedEntity(
		Type entityType,
		MethodInfo createMethod,
		int index)
	{
		var entityName = entityType.Name.Replace("Entity", string.Empty);
		var code = $"DEMO-{entityType.Name.ToUpperInvariant()}-{index:000}";
		var name = $"Demo {entityName} {index}";
		const string metadata = "{\"source\":\"ef-inmemory-seed\"}";

		return createMethod.Invoke(
			null,
			[DemoTenantId, code, name, metadata]);
	}
}
