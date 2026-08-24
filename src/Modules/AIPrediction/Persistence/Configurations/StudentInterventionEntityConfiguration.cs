using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="StudentInterventionEntity"/>.
/// </summary>
public sealed class StudentInterventionEntityConfiguration
	: IEntityTypeConfiguration<StudentInterventionEntity>
{
	public void Configure(EntityTypeBuilder<StudentInterventionEntity> builder)
	{
		builder.ToTable("student_intervention", schema: "ai");
		builder.Ignore(entity => entity.Id);

		builder.HasKey(entity => entity.StudentInterventionId);

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
		builder.Property(entity => entity.StudentInterventionId).HasColumnName("student_intervention_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.StudentId).HasColumnName("student_id");
		builder.Property(entity => entity.SubjectId).HasColumnName("subject_id");
		builder.Property(entity => entity.CourseOfferingId).HasColumnName("course_offering_id");
		builder.Property(entity => entity.TeacherEmployeeId).HasColumnName("teacher_employee_id");
		builder.Property(entity => entity.SourcePredictionId).HasColumnName("source_prediction_id");
		builder.Property(entity => entity.SourceRecommendationId).HasColumnName("source_recommendation_id");
		builder.Property(entity => entity.Title).HasColumnName("title");
		builder.Property(entity => entity.Reason).HasColumnName("reason");
		builder.Property(entity => entity.TargetOutcome).HasColumnName("target_outcome");
		builder.Property(entity => entity.StartDate).HasColumnName("start_date");
		builder.Property(entity => entity.TargetDate).HasColumnName("target_date");
		builder.Property(entity => entity.Status).HasColumnName("status");
	}
}
