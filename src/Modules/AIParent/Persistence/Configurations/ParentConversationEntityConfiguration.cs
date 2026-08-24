using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="ParentConversationEntity"/>.
/// </summary>
public sealed class ParentConversationEntityConfiguration
	: IEntityTypeConfiguration<ParentConversationEntity>
{
	public void Configure(EntityTypeBuilder<ParentConversationEntity> builder)
	{
		builder.ToTable("parent_conversation", schema: "ai_parent");
<<<<<<< HEAD
builder.HasKey(entity => entity.ParentConversationId);
=======
		builder.Ignore(entity => entity.Id);

		builder.HasKey(entity => entity.ParentConversationId);
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


		// Canonical database mapping generated from SmartSchoolComplete.sql.
		builder.Property(entity => entity.Code).HasColumnName("code");
		builder.Property(entity => entity.Name).HasColumnName("name");
		builder.Property(entity => entity.MetadataJson).HasColumnName("metadata_json").HasColumnType("jsonb");
		builder.Property(entity => entity.ParentConversationId).HasColumnName("parent_conversation_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.GuardianId).HasColumnName("guardian_id");
		builder.Property(entity => entity.SelectedStudentId).HasColumnName("selected_student_id");
		builder.Property(entity => entity.Title).HasColumnName("title");
		builder.Property(entity => entity.StartedAt).HasColumnName("started_at");
		builder.Property(entity => entity.EndedAt).HasColumnName("ended_at");
		builder.Property(entity => entity.Status).HasColumnName("status");
	}
}
