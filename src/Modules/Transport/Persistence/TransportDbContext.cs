using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence;

public interface ITransportDbContext
{
    DatabaseFacade Database { get; }

    DbSet<DriverDirectoryReadEntity> DriverDirectoryReads { get; }
    DbSet<DriverEntity> Drivers { get; }
    DbSet<TripRecordEntity> TripRecords { get; }
    DbSet<RouteNoticeEntity> RouteNotices { get; }
    DbSet<RouteEntity> Routes { get; }
    DbSet<StopEntity> Stops { get; }
    DbSet<StudentTransportEntity> StudentTransports { get; }
    DbSet<VehicleEntity> Vehicles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// EF Core unit-of-work owned by the Transport module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class TransportDbContext(DbContextOptions<TransportDbContext> options)
    : DbContext(options), ITransportDbContext
{
    public DbSet<DriverDirectoryReadEntity> DriverDirectoryReads => Set<DriverDirectoryReadEntity>();
    public DbSet<DriverEntity> Drivers => Set<DriverEntity>();
    public DbSet<TripRecordEntity> TripRecords => Set<TripRecordEntity>();
    public DbSet<RouteNoticeEntity> RouteNotices => Set<RouteNoticeEntity>();
    public DbSet<RouteEntity> Routes => Set<RouteEntity>();
    public DbSet<StopEntity> Stops => Set<StopEntity>();
    public DbSet<StudentTransportEntity> StudentTransports => Set<StudentTransportEntity>();
    public DbSet<VehicleEntity> Vehicles => Set<VehicleEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(TransportDbContext).Assembly,
            type => type.Namespace is not null
                && type.Namespace.StartsWith("SmartSchool.Modules.Transport.Persistence.Configurations", StringComparison.Ordinal));

        // These retired projections and document models are not part of the active persistence model.
        modelBuilder.Ignore<DriverDirectoryReadEntity>();
        modelBuilder.Ignore<DriverDocumentEntity>();
    }
}
