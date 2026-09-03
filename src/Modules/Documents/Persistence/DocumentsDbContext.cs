using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence;

public interface IDocumentsDbContext
{
	DatabaseFacade Database { get; }

	DbSet<CertificateEntity> Certificates { get; }
	DbSet<DocumentFileEntity> DocumentFiles { get; }
	DbSet<DocumentLinkEntity> DocumentLinks { get; }
	DbSet<DocumentTemplateEntity> DocumentTemplates { get; }
	DbSet<DocumentTypeEntity> DocumentTypes { get; }
	DbSet<GeneratedDocumentEntity> GeneratedDocuments { get; }
	DbSet<SchoolLogoEntity> SchoolLogos { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// EF Core unit-of-work owned by the Documents module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class DocumentsDbContext(DbContextOptions<DocumentsDbContext> options)
	: DbContext(options), IDocumentsDbContext
{
	public DbSet<CertificateEntity> Certificates => Set<CertificateEntity>();
	public DbSet<DocumentFileEntity> DocumentFiles => Set<DocumentFileEntity>();
	public DbSet<DocumentLinkEntity> DocumentLinks => Set<DocumentLinkEntity>();
	public DbSet<DocumentTemplateEntity> DocumentTemplates => Set<DocumentTemplateEntity>();
	public DbSet<DocumentTypeEntity> DocumentTypes => Set<DocumentTypeEntity>();
	public DbSet<GeneratedDocumentEntity> GeneratedDocuments => Set<GeneratedDocumentEntity>();
	public DbSet<SchoolLogoEntity> SchoolLogos => Set<SchoolLogoEntity>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(DocumentsDbContext).Assembly,
			type => type.Namespace is not null
				&& type.Namespace.StartsWith("SmartSchool.Modules.Documents.Persistence.Configurations", StringComparison.Ordinal));
	}
}
