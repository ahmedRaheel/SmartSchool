using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel.Documents;

namespace SmartSchool.Modules.Students.Persistence.Configurations;

/// <summary>
/// Configures the ParentDocument relational table.
/// </summary>
public sealed class ParentDocumentEntityConfiguration
	: IEntityTypeConfiguration<ParentDocumentEntity>
{
	public void Configure(EntityTypeBuilder<ParentDocumentEntity> builder)
	{
		builder.ToTable("ParentDocument");

		builder.HasKey(document => document.Id);

		builder.Property(document => document.TenantId).IsRequired();
		builder.Property(document => document.ParentId).IsRequired();
		builder.Property(document => document.DocumentTypeId).IsRequired();

		builder
			.Property(document => document.OriginalFileName)
			.HasMaxLength(DocumentConstants.MaximumOriginalFileNameLength)
			.IsRequired();

		builder
			.Property(document => document.ContentType)
			.HasMaxLength(DocumentConstants.MaximumContentTypeLength)
			.IsRequired();

		builder.Property(document => document.FileSizeBytes).IsRequired();

		builder
			.Property(document => document.StorageProvider)
			.HasMaxLength(DocumentConstants.MaximumStorageProviderLength)
			.IsRequired();

		builder
			.Property(document => document.StorageKey)
			.HasMaxLength(DocumentConstants.MaximumStorageKeyLength)
			.IsRequired();

		builder
			.Property(document => document.Sha256Hash)
			.HasMaxLength(DocumentConstants.Sha256Length)
			.IsRequired();

		builder
			.Property(document => document.DocumentNumber)
			.HasMaxLength(DocumentConstants.MaximumDocumentNumberLength);

		builder
			.Property(document => document.Notes)
			.HasMaxLength(DocumentConstants.MaximumNotesLength);

		builder.Property(document => document.RowVersion).IsConcurrencyToken();

		builder.HasIndex(document => new
		{
			document.TenantId,
			document.ParentId,
			document.DocumentTypeId
		});

		builder.HasIndex(document => new
		{
			document.TenantId,
			document.Sha256Hash
		});

		builder
			.HasIndex(document => new
			{
				document.TenantId,
				document.StorageProvider,
				document.StorageKey
			})
			.IsUnique();
	}
}
