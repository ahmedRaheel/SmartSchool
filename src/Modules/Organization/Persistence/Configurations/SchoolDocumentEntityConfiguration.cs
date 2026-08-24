using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel.Documents;

namespace SmartSchool.Modules.Organization.Persistence.Configurations;

/// <summary>
/// Configures the SchoolDocument relational table.
/// </summary>
public sealed class SchoolDocumentEntityConfiguration
	: IEntityTypeConfiguration<SchoolDocumentEntity>
{
	public void Configure(EntityTypeBuilder<SchoolDocumentEntity> builder)
	{
		builder.ToTable("schooldocument", schema: "public");
<<<<<<< HEAD
builder.HasKey(document => document.SchoolDocumentId);
=======
		builder.Ignore(entity => entity.Id);

		builder.HasKey(document => document.Id);
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0

		builder.Property(document => document.TenantId).IsRequired();
		builder.Property(document => document.SchoolId).IsRequired();
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
			document.SchoolId,
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

		// Canonical database mapping generated from SmartSchoolComplete.sql.
		builder.Property(entity => entity.SchoolId).HasColumnName("schoolid");
		builder.Property(entity => entity.DocumentTypeId).HasColumnName("documenttypeid");
		builder.Property(entity => entity.OriginalFileName).HasColumnName("originalfilename");
		builder.Property(entity => entity.ContentType).HasColumnName("contenttype");
		builder.Property(entity => entity.FileSizeBytes).HasColumnName("filesizebytes");
		builder.Property(entity => entity.StorageProvider).HasColumnName("storageprovider");
		builder.Property(entity => entity.StorageKey).HasColumnName("storagekey");
		builder.Property(entity => entity.Sha256Hash).HasColumnName("sha256hash");
		builder.Property(entity => entity.DocumentNumber).HasColumnName("documentnumber");
		builder.Property(entity => entity.IssuedOn).HasColumnName("issuedon");
		builder.Property(entity => entity.ExpiresOn).HasColumnName("expireson");
		builder.Property(entity => entity.IsVerified).HasColumnName("isverified");
		builder.Property(entity => entity.VerifiedByUserId).HasColumnName("verifiedbyuserid");
		builder.Property(entity => entity.VerifiedAt).HasColumnName("verifiedat");
		builder.Property(entity => entity.Notes).HasColumnName("notes");
		builder.Property(entity => entity.SchoolId).HasColumnName("id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenantid");
		builder.Property(entity => entity.IsActive).HasColumnName("isactive");
		builder.Property(entity => entity.CreatedAt).HasColumnName("createdat");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updatedat");
		builder.Property(entity => entity.RowVersion).HasColumnName("rowversion");
	}
}
