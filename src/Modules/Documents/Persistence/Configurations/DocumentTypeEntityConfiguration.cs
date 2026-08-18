using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence.Configurations;

/// <summary>
/// Configures normalized document classifications.
/// </summary>
public sealed class DocumentTypeEntityConfiguration
	: IEntityTypeConfiguration<DocumentTypeEntity>
{
	public void Configure(EntityTypeBuilder<DocumentTypeEntity> builder)
	{
		builder.ToTable("DocumentType");

		builder.HasKey(documentType => documentType.Id);

		builder.Property(documentType => documentType.Code)
			.HasMaxLength(80)
			.IsRequired();

		builder.Property(documentType => documentType.Name)
			.HasMaxLength(150)
			.IsRequired();

		builder.Property(documentType => documentType.OwnerCategory)
			.HasMaxLength(50)
			.IsRequired();

		builder.HasIndex(documentType => new
		{
			documentType.TenantId,
			documentType.Code
		}).IsUnique();

		builder.Property(documentType => documentType.RowVersion)
			.IsConcurrencyToken();
	}
}
