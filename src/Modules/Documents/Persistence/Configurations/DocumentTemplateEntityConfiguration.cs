using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="DocumentTemplateEntity"/>.
/// </summary>
public sealed class DocumentTemplateEntityConfiguration
	: IEntityTypeConfiguration<DocumentTemplateEntity>
{
	public void Configure(EntityTypeBuilder<DocumentTemplateEntity> builder)
	{
		builder.ToTable("document_template", schema: "document");
		builder.HasKey(entity => entity.DocumentTemplateId);

		builder
			.Property(entity => entity.TenantId)
			.IsRequired();

		builder
			.Property(entity => entity.IsActive)
			.IsRequired();

		builder.HasIndex(entity => entity.TenantId);

		builder.Property(entity => entity.CreatedAt).IsRequired();
		builder.Property(entity => entity.UpdatedAt);
		builder.Property(entity => entity.RowVersion).IsRequired().IsConcurrencyToken();

		builder
			.Property(entity => entity.Code)
			.HasMaxLength(100)
			.IsRequired();

		builder
			.HasIndex(entity => new { entity.TenantId, entity.Code })
			.IsUnique();

		builder
			.Property(entity => entity.Name)
			.HasMaxLength(250)
			.IsRequired();


		// Canonical database mapping generated from SmartSchoolComplete.sql.
		builder.Property(entity => entity.Code).HasColumnName("code");
		builder.Property(entity => entity.Name).HasColumnName("name");
		builder.Property(entity => entity.MetadataJson).HasColumnName("metadata_json").HasColumnType("jsonb");
		builder.Property(entity => entity.DocumentTemplateId).HasColumnName("document_template_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.CampusId).HasColumnName("campus_id");
		builder.Property(entity => entity.AcademicSystemId).HasColumnName("academic_system_id");
		builder.Property(entity => entity.DocumentTypeCode).HasColumnName("document_type_code");
		builder.Property(entity => entity.SubjectTemplate).HasColumnName("subject_template");
		builder.Property(entity => entity.HeaderHtml).HasColumnName("header_html");
		builder.Property(entity => entity.BodyHtml).HasColumnName("body_html");
		builder.Property(entity => entity.FooterHtml).HasColumnName("footer_html");
		builder.Property(entity => entity.LanguageCode).HasColumnName("language_code");
		builder.Property(entity => entity.Version).HasColumnName("version");
		builder.Property(entity => entity.RequiresApproval).HasColumnName("requires_approval");
	}
}
