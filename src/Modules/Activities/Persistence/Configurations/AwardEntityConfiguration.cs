using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="AwardEntity"/>.
/// </summary>
public sealed class AwardEntityConfiguration
	: IEntityTypeConfiguration<AwardEntity>
{
	public void Configure(EntityTypeBuilder<AwardEntity> builder)
	{
		builder.ToTable("student_award", schema: "activity");
		builder.Ignore(entity => entity.Id);

		builder.HasKey(entity => entity.StudentAwardId);

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
		builder.Property(entity => entity.StudentAwardId).HasColumnName("student_award_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.StudentId).HasColumnName("student_id");
		builder.Property(entity => entity.AwardTypeCode).HasColumnName("award_type_code");
		builder.Property(entity => entity.Title).HasColumnName("title");
		builder.Property(entity => entity.Description).HasColumnName("description");
		builder.Property(entity => entity.AwardDate).HasColumnName("award_date");
		builder.Property(entity => entity.ApprovedBy).HasColumnName("approved_by");
		builder.Property(entity => entity.GeneratedDocumentId).HasColumnName("generated_document_id");
	}
}
