using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SmartSchool.SharedKernel;

namespace SmartSchool.Infrastructure.Persistence;

/// <summary>
/// Operational SmartSchool EF Core context used by PostgreSQL and SQL Server.
/// </summary>
public sealed class SmartSchoolDbContext(
	DbContextOptions<SmartSchoolDbContext> options)
	: DbContext(options)
{
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		var entityTypes = AppDomain.CurrentDomain
			.GetAssemblies()
			.Where(assembly =>
				!assembly.IsDynamic
				&& assembly.GetName().Name?.StartsWith(
					"SmartSchool.Modules.",
					StringComparison.Ordinal) == true)
			.SelectMany(GetLoadableTypes)
			.Where(type =>
				!type.IsAbstract
				&& typeof(Entity).IsAssignableFrom(type));

		foreach (var entityType in entityTypes)
		{
			var entityBuilder = modelBuilder.Entity(entityType);

			entityBuilder.HasKey(nameof(Entity.Id));
			entityBuilder.Property(nameof(Entity.TenantId)).IsRequired();
			entityBuilder.Property(nameof(Entity.IsActive)).IsRequired();
			entityBuilder.Property(nameof(Entity.RowVersion)).IsConcurrencyToken();

			if (entityType.GetProperty("Code") is not null)
			{
				entityBuilder
					.HasIndex(nameof(Entity.TenantId), "Code")
					.IsUnique();
			}
		}

		base.OnModelCreating(modelBuilder);
	}

	private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
	{
		try
		{
			return assembly.GetTypes();
		}
		catch (ReflectionTypeLoadException exception)
		{
			return exception.Types.OfType<Type>();
		}
	}
}
