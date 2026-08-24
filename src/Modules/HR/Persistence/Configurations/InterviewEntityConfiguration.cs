using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="InterviewEntity"/>.
/// </summary>
public sealed class InterviewEntityConfiguration
	: IEntityTypeConfiguration<InterviewEntity>
{
	public void Configure(EntityTypeBuilder<InterviewEntity> builder)
	{
		builder.ToTable("interview", schema: "hr");

		builder.HasKey(entity => entity.Id);

		builder
			.Property(entity => entity.TenantId)
			.IsRequired();

		builder
			.Property(entity => entity.IsActive)
			.IsRequired();

		builder
			.Property(entity => entity.RowVersion)
			.IsConcurrencyToken();

		builder.HasIndex(entity => entity.TenantId);

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
		builder.Property(entity => entity.Id).HasColumnName("interview_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.JobApplicationId).HasColumnName("job_application_id");
		builder.Property(entity => entity.InterviewTypeCode).HasColumnName("interview_type_code");
		builder.Property(entity => entity.RoundNumber).HasColumnName("round_number");
		builder.Property(entity => entity.ScheduledAt).HasColumnName("scheduled_at");
		builder.Property(entity => entity.DurationMinutes).HasColumnName("duration_minutes");
		builder.Property(entity => entity.Location).HasColumnName("location");
		builder.Property(entity => entity.MeetingUrl).HasColumnName("meeting_url");
		builder.Property(entity => entity.Status).HasColumnName("status");
		builder.Property(entity => entity.OverallScore).HasColumnName("overall_score");
		builder.Property(entity => entity.Recommendation).HasColumnName("recommendation");
		builder.Property(entity => entity.Notes).HasColumnName("notes");
	}
}
