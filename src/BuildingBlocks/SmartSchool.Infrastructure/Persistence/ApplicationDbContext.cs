using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Infrastructure.Persistence;

/// <summary>
/// Primary EF Core database context for SmartSchool.
/// The physical provider is selected through <see cref="PersistenceOptions"/>.
/// </summary>
public sealed class ApplicationDbContext(
	DbContextOptions<ApplicationDbContext> options)
	: DbContext(options), IApplicationDbContext
{
	/// <inheritdoc />
	public new DbSet<TEntity> Set<TEntity>()
		where TEntity : AggregateRootEntity
	{
		return base.Set<TEntity>();
	}

	/// <inheritdoc />
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		foreach (var assembly in GetModuleAssemblies())
		{
			modelBuilder.ApplyConfigurationsFromAssembly(assembly);
		}

		base.OnModelCreating(modelBuilder);
	}

	private static IEnumerable<Assembly> GetModuleAssemblies()
	{
		return AppDomain.CurrentDomain
			.GetAssemblies()
			.Where(assembly =>
				!assembly.IsDynamic
				&& assembly.GetName().Name?.StartsWith(
					"SmartSchool.Modules.",
					StringComparison.Ordinal) == true);
	}
}
