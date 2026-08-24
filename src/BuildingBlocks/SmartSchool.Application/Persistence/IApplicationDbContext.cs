using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.SharedKernel;

namespace SmartSchool.Application.Persistence;

/// <summary>
/// Exposes the EF Core unit of work required by module-specific query and command implementations.
/// </summary>
public interface IApplicationDbContext
{
	/// <summary>Returns the EF Core set for a domain entity.</summary>
	DbSet<TEntity> Set<TEntity>()
		where TEntity : AggregateRootEntity;

	/// <summary>Persists all tracked changes atomically.</summary>
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
