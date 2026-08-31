using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
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
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class DocumentsDbContext(IApplicationDbContext dbContext) : IDocumentsDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<CertificateEntity> Certificates => dbContext.Set<CertificateEntity>();
	public DbSet<DocumentFileEntity> DocumentFiles => dbContext.Set<DocumentFileEntity>();
	public DbSet<DocumentLinkEntity> DocumentLinks => dbContext.Set<DocumentLinkEntity>();
	public DbSet<DocumentTemplateEntity> DocumentTemplates => dbContext.Set<DocumentTemplateEntity>();
	public DbSet<DocumentTypeEntity> DocumentTypes => dbContext.Set<DocumentTypeEntity>();
	public DbSet<GeneratedDocumentEntity> GeneratedDocuments => dbContext.Set<GeneratedDocumentEntity>();
	public DbSet<SchoolLogoEntity> SchoolLogos => dbContext.Set<SchoolLogoEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
