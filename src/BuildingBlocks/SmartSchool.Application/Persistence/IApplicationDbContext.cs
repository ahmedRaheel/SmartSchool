using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.SharedKernel;

namespace SmartSchool.Application.Persistence;

/// <summary>
/// Exposes the EF Core unit of work required by module-specific query and command implementations.
/// </summary>
public interface IApplicationDbContext
{
	/// <summary>Exposes EF Core relational database operations for feature-owned command persistence.</summary>
	DatabaseFacade Database { get; }

	/// <summary>Returns the EF Core set for a domain entity.</summary>
	DbSet<TEntity> Set<TEntity>()
		where TEntity : Entity;

	/// <summary>Persists all tracked changes atomically.</summary>
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
