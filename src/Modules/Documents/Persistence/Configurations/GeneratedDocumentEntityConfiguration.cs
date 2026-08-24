using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="GeneratedDocumentEntity"/>.
/// </summary>
public sealed class GeneratedDocumentEntityConfiguration
	: IEntityTypeConfiguration<GeneratedDocumentEntity>
{
	public void Configure(EntityTypeBuilder<GeneratedDocumentEntity> builder)
	{
		builder.ToTable("generated_document", schema: "document");

		builder.HasKey(entity => entity.Id);

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
		builder.Property(entity => entity.Id).HasColumnName("generated_document_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.DocumentTemplateId).HasColumnName("document_template_id");
		builder.Property(entity => entity.TemplateVersion).HasColumnName("template_version");
		builder.Property(entity => entity.StudentId).HasColumnName("student_id");
		builder.Property(entity => entity.EmployeeId).HasColumnName("employee_id");
		builder.Property(entity => entity.DocumentNumber).HasColumnName("document_number");
		builder.Property(entity => entity.RenderedContentSnapshot).HasColumnName("rendered_content_snapshot");
		builder.Property(entity => entity.FileUrl).HasColumnName("file_url");
		builder.Property(entity => entity.VerificationCode).HasColumnName("verification_code");
		builder.Property(entity => entity.IssuedBy).HasColumnName("issued_by");
		builder.Property(entity => entity.ApprovedBy).HasColumnName("approved_by");
		builder.Property(entity => entity.IssuedAt).HasColumnName("issued_at");
		builder.Property(entity => entity.Status).HasColumnName("status");
	}
}
