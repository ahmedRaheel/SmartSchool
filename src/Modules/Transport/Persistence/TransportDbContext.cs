using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence;

public interface ITransportDbContext
{
	DatabaseFacade Database { get; }

	DbSet<DriverDirectoryReadEntity> DriverDirectoryReads { get; }
	DbSet<DriverDocumentEntity> DriverDocuments { get; }
	DbSet<DriverEntity> Drivers { get; }
	DbSet<RouteEntity> Routes { get; }
	DbSet<StopEntity> Stops { get; }
	DbSet<StudentTransportEntity> StudentTransports { get; }
	DbSet<VehicleEntity> Vehicles { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class TransportDbContext(IApplicationDbContext dbContext) : ITransportDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<DriverDirectoryReadEntity> DriverDirectoryReads => dbContext.Set<DriverDirectoryReadEntity>();
	public DbSet<DriverDocumentEntity> DriverDocuments => dbContext.Set<DriverDocumentEntity>();
	public DbSet<DriverEntity> Drivers => dbContext.Set<DriverEntity>();
	public DbSet<RouteEntity> Routes => dbContext.Set<RouteEntity>();
	public DbSet<StopEntity> Stops => dbContext.Set<StopEntity>();
	public DbSet<StudentTransportEntity> StudentTransports => dbContext.Set<StudentTransportEntity>();
	public DbSet<VehicleEntity> Vehicles => dbContext.Set<VehicleEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
