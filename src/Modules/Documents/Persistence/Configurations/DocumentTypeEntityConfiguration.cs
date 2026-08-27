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
		builder.ToTable("documenttype", schema: "document");
		builder.HasKey(documentType => documentType.DocumentTypeId);

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

		// Canonical database mapping generated from SmartSchoolComplete.sql.
		builder.Property(entity => entity.Code).HasColumnName("code");
		builder.Property(entity => entity.Name).HasColumnName("name");
		builder.Property(entity => entity.OwnerCategory).HasColumnName("ownercategory");
		builder.Property(entity => entity.IsIdentityDocument).HasColumnName("isidentitydocument");
		builder.Property(entity => entity.RequiresExpiryDate).HasColumnName("requiresexpirydate");
		builder.Property(entity => entity.RequiresVerification).HasColumnName("requiresverification");
		builder.Property(entity => entity.DocumentTypeId).HasColumnName("id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenantid");
		builder.Property(entity => entity.IsActive).HasColumnName("isactive");
		builder.Property(entity => entity.CreatedAt).HasColumnName("createdat");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updatedat");
		builder.Property(entity => entity.RowVersion).HasColumnName("rowversion");
	}
}
