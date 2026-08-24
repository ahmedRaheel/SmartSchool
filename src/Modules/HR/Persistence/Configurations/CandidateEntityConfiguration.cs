using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="CandidateEntity"/>.
/// </summary>
public sealed class CandidateEntityConfiguration
	: IEntityTypeConfiguration<CandidateEntity>
{
	public void Configure(EntityTypeBuilder<CandidateEntity> builder)
	{
		builder.ToTable("candidate", schema: "hr");
builder.HasKey(entity => entity.CandidateId);

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
		builder.Property(entity => entity.CandidateId).HasColumnName("candidate_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.FirstName).HasColumnName("first_name");
		builder.Property(entity => entity.LastName).HasColumnName("last_name");
		builder.Property(entity => entity.Email).HasColumnName("email");
		builder.Property(entity => entity.Phone).HasColumnName("phone");
		builder.Property(entity => entity.CurrentJobTitle).HasColumnName("current_job_title");
		builder.Property(entity => entity.CurrentEmployer).HasColumnName("current_employer");
		builder.Property(entity => entity.TotalExperienceYears).HasColumnName("total_experience_years");
		builder.Property(entity => entity.HighestQualification).HasColumnName("highest_qualification");
		builder.Property(entity => entity.ExpectedSalary).HasColumnName("expected_salary");
		builder.Property(entity => entity.NoticePeriodDays).HasColumnName("notice_period_days");
		builder.Property(entity => entity.StatusCode).HasColumnName("status_code");
	}
}
