using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="EmployeeEntity"/>.
/// </summary>
public sealed class EmployeeEntityConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
	/// <summary>Configures the employee database mapping.</summary>
	/// <param name="builder">Entity Framework entity builder.</param>
	public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
	{
		builder.ToTable("employee", schema: "hr");
<<<<<<< HEAD
builder.HasKey(entity => entity.EmployeeId);
=======
		builder.Ignore(entity => entity.Id);
		builder.HasKey(entity => entity.EmployeeId);
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0

		builder.Property(entity => entity.EmployeeId).HasColumnName("employee_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id").IsRequired();
		builder.Property(entity => entity.UserId).HasColumnName("user_id");
		builder.Property(entity => entity.EmployeeNumber).HasColumnName("employee_number").HasMaxLength(60).IsRequired();
		builder.Property(entity => entity.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
		builder.Property(entity => entity.LastName).HasColumnName("last_name").HasMaxLength(100);
		builder.Property(entity => entity.CnicNumber).HasColumnName("cnic_number").HasMaxLength(20);
		builder.Property(entity => entity.Photo).HasColumnName("photo");
		builder.Property(entity => entity.PhotoContentType).HasColumnName("photo_content_type").HasMaxLength(150);
		builder.Property(entity => entity.PhotoFileName).HasColumnName("photo_file_name").HasMaxLength(255);
		builder.Property(entity => entity.Email).HasColumnName("email").HasMaxLength(250);
		builder.Property(entity => entity.Phone).HasColumnName("phone").HasMaxLength(50);
		builder.Property(entity => entity.HireDate).HasColumnName("hire_date").IsRequired();
		builder.Property(entity => entity.EmploymentTypeCode).HasColumnName("employment_type_code").HasMaxLength(30).IsRequired();
		builder.Property(entity => entity.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
		builder.Property(entity => entity.SourceCandidateId).HasColumnName("source_candidate_id");

		builder.HasIndex(entity => new { entity.TenantId, entity.EmployeeNumber }).IsUnique();
		builder.HasIndex(entity => new { entity.TenantId, entity.CnicNumber }).IsUnique();

		builder.Property(entity => entity.IsActive).HasColumnName("is_active").IsRequired();
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at").IsRequired();
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version").IsRequired().IsConcurrencyToken();

		// Canonical database mapping generated from SmartSchoolComplete.sql.
	}
}
