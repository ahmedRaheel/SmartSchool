using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="ParentProfileEntity"/>.
/// </summary>
public sealed class ParentProfileEntityConfiguration
	: IEntityTypeConfiguration<ParentProfileEntity>
{
	public void Configure(EntityTypeBuilder<ParentProfileEntity> builder)
	{
		builder.ToTable("ParentProfile", schema: "student");
<<<<<<< HEAD
builder.HasKey(entity => entity.ParentProfileId);
=======
		builder.Ignore(entity => entity.Id);

		builder.HasKey(entity => entity.ParentProfileId);
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


		// Explicit PostgreSQL mappings for synchronized table.
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");
		builder.Property(entity => entity.ParentProfileId).HasColumnName("parent_profile_id");
	}
}
