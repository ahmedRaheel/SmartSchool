using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="ConversationParticipantEntity"/>.
/// </summary>
public sealed class ConversationParticipantEntityConfiguration
	: IEntityTypeConfiguration<ConversationParticipantEntity>
{
	public void Configure(EntityTypeBuilder<ConversationParticipantEntity> builder)
	{
		builder.ToTable("conversation_participant", schema: "communication");
		builder.HasKey(entity => entity.ConversationParticipantId);

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
		builder.Property(entity => entity.ConversationParticipantId).HasColumnName("conversation_participant_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.ConversationId).HasColumnName("conversation_id");
		builder.Property(entity => entity.UserId).HasColumnName("user_id");
		builder.Property(entity => entity.JoinedAt).HasColumnName("joined_at");
		builder.Property(entity => entity.LeftAt).HasColumnName("left_at");

        // Explicit parent-child relationships. Prevents EF Core shadow foreign keys.
        builder.HasOne<ConversationEntity>()
            .WithMany()
            .HasForeignKey(entity => entity.ConversationId)
            .OnDelete(DeleteBehavior.Restrict);

	}
}
