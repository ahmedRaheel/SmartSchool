using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="ParentMessageEntity"/>.
/// </summary>
public sealed class ParentMessageEntityConfiguration
	: IEntityTypeConfiguration<ParentMessageEntity>
{
	public void Configure(EntityTypeBuilder<ParentMessageEntity> builder)
	{
		builder.ToTable("ParentMessage", schema: "ai_parent");

		builder.HasKey(entity => entity.Id);

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

	}
}
