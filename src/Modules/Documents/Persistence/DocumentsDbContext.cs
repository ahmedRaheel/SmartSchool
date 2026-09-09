using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence;

public interface IDocumentsDbContext
{
    DatabaseFacade Database { get; }
    DbSet<DocumentFileEntity> DocumentFiles { get; }
    DbSet<DocumentTypeEntity> DocumentTypes { get; }
    DbSet<RequiredDocumentTypeEntity> RequiredDocumentTypes { get; }
    DbSet<RequiredDocumentEntity> RequiredDocuments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// EF Core unit-of-work owned by the Documents module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class DocumentsDbContext(DbContextOptions<DocumentsDbContext> options)
    : DbContext(options), IDocumentsDbContext
{
    public DbSet<DocumentFileEntity> DocumentFiles => Set<DocumentFileEntity>();
    public DbSet<DocumentTypeEntity> DocumentTypes => Set<DocumentTypeEntity>();
    public DbSet<RequiredDocumentTypeEntity> RequiredDocumentTypes => Set<RequiredDocumentTypeEntity>();
    public DbSet<RequiredDocumentEntity> RequiredDocuments => Set<RequiredDocumentEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(DocumentsDbContext).Assembly,
            type => type.Namespace is not null
                && type.Namespace.StartsWith("SmartSchool.Modules.Documents.Persistence.Configurations", StringComparison.Ordinal));
    }
}
