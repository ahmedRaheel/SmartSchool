using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="LeadCaptureEntity"/>.
/// </summary>
public sealed class LeadCaptureEntityConfiguration
	: IEntityTypeConfiguration<LeadCaptureEntity>
{
	public void Configure(EntityTypeBuilder<LeadCaptureEntity> builder)
	{
		builder.ToTable("lead_capture", schema: "ai_inquiry");
		builder.Ignore(entity => entity.Id);

		builder.HasKey(entity => entity.LeadCaptureId);

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
		builder.Property(entity => entity.LeadCaptureId).HasColumnName("lead_capture_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.InquiryConversationId).HasColumnName("inquiry_conversation_id");
		builder.Property(entity => entity.Phone).HasColumnName("phone");
		builder.Property(entity => entity.Email).HasColumnName("email");
		builder.Property(entity => entity.InterestedCampusId).HasColumnName("interested_campus_id");
		builder.Property(entity => entity.InterestedProgramId).HasColumnName("interested_program_id");
		builder.Property(entity => entity.InterestedGradeId).HasColumnName("interested_grade_id");
		builder.Property(entity => entity.Notes).HasColumnName("notes");
		builder.Property(entity => entity.CapturedAt).HasColumnName("captured_at");
		builder.Property(entity => entity.ConvertedInquiryId).HasColumnName("converted_inquiry_id");
	}
}
