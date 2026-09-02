using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="LoanEntity"/>.
/// </summary>
public sealed class LoanEntityConfiguration
	: IEntityTypeConfiguration<LoanEntity>
{
	public void Configure(EntityTypeBuilder<LoanEntity> builder)
	{
		builder.ToTable("book_loan", schema: "library");
		builder.HasKey(entity => entity.BookLoanId);

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
		builder.Property(entity => entity.BookLoanId).HasColumnName("book_loan_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.BookCopyId).HasColumnName("book_copy_id");
		builder.Property(entity => entity.StudentId).HasColumnName("student_id");
		builder.Property(entity => entity.EmployeeId).HasColumnName("employee_id");
		builder.Property(entity => entity.IssuedAt).HasColumnName("issued_at");
		builder.Property(entity => entity.DueAt).HasColumnName("due_at");
		builder.Property(entity => entity.ReturnedAt).HasColumnName("returned_at");

        // Explicit parent-child relationships. Prevents EF Core shadow foreign keys.
        builder.HasOne<BookCopyEntity>()
            .WithMany()
            .HasForeignKey(entity => entity.BookCopyId)
            .OnDelete(DeleteBehavior.Restrict);

	}
}
