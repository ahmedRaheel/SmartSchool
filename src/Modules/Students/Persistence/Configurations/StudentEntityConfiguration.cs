using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence.Configurations;

/// <summary>Defines persistence rules for <see cref="StudentEntity"/>.</summary>
public sealed class StudentEntityConfiguration : IEntityTypeConfiguration<StudentEntity>
{
	public void Configure(EntityTypeBuilder<StudentEntity> builder)
	{
		builder.ToTable("student", schema: "student");
		builder.Ignore(entity => entity.Id);
		builder.HasKey(entity => entity.StudentId);

		builder.Property(entity => entity.StudentId).HasColumnName("student_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id").IsRequired();
		builder.Property(entity => entity.UserId).HasColumnName("user_id");
		builder.Property(entity => entity.StudentNumber).HasColumnName("student_number").HasMaxLength(60).IsRequired();
		builder.Property(entity => entity.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
		builder.Property(entity => entity.LastName).HasColumnName("last_name").HasMaxLength(100);
		builder.Property(entity => entity.DateOfBirth).HasColumnName("date_of_birth");
		builder.Property(entity => entity.Gender).HasColumnName("gender").HasMaxLength(30);
		builder.Property(entity => entity.Photo).HasColumnName("photo");
		builder.Property(entity => entity.PhotoContentType).HasColumnName("photo_content_type").HasMaxLength(150);
		builder.Property(entity => entity.PhotoFileName).HasColumnName("photo_file_name").HasMaxLength(255);
		builder.Property(entity => entity.AdmissionDate).HasColumnName("admission_date");
		builder.Property(entity => entity.Status).HasColumnName("status").HasMaxLength(30).IsRequired();

		builder.HasIndex(entity => new { entity.TenantId, entity.StudentNumber }).IsUnique();

		builder.Property(entity => entity.IsActive).HasColumnName("is_active").IsRequired();
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at").IsRequired();
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version").IsRequired().IsConcurrencyToken();

		// Canonical database mapping generated from SmartSchoolComplete.sql.
	}
}
