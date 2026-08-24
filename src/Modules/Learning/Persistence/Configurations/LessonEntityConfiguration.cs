using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="LessonEntity"/>.
/// </summary>
public sealed class LessonEntityConfiguration
	: IEntityTypeConfiguration<LessonEntity>
{
	public void Configure(EntityTypeBuilder<LessonEntity> builder)
	{
		builder.ToTable("Lesson", schema: "lms");
<<<<<<< HEAD
builder.HasKey(entity => entity.LessonId);
=======
		builder.Ignore(entity => entity.Id);

		builder.HasKey(entity => entity.LessonId);
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0

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


		// Explicit PostgreSQL mappings for synchronized table.
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");
		builder.Property(entity => entity.Code).HasColumnName("code");
		builder.Property(entity => entity.Name).HasColumnName("name");
		builder.Property(entity => entity.LessonId).HasColumnName("lesson_id");
	}
}
