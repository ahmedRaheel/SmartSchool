using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="JobEntity"/>.
/// </summary>
public sealed class JobEntityConfiguration
	: IEntityTypeConfiguration<JobEntity>
{
	public void Configure(EntityTypeBuilder<JobEntity> builder)
	{
		builder.ToTable("job", schema: "hr");
<<<<<<< HEAD
builder.HasKey(entity => entity.JobId);
=======
		builder.Ignore(entity => entity.Id);

		builder.HasKey(entity => entity.JobId);
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0

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
		builder.Property(entity => entity.JobId).HasColumnName("job_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.DepartmentId).HasColumnName("department_id");
		builder.Property(entity => entity.JobFamilyId).HasColumnName("job_family_id");
		builder.Property(entity => entity.Title).HasColumnName("title");
		builder.Property(entity => entity.Description).HasColumnName("description");
		builder.Property(entity => entity.Responsibilities).HasColumnName("responsibilities");
		builder.Property(entity => entity.MinimumQualification).HasColumnName("minimum_qualification");
		builder.Property(entity => entity.MinimumExperienceYears).HasColumnName("minimum_experience_years");
		builder.Property(entity => entity.IsTeachingPosition).HasColumnName("is_teaching_position");
	}
}
