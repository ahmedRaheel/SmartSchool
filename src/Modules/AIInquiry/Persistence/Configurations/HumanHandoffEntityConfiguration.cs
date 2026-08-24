using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="HumanHandoffEntity"/>.
/// </summary>
public sealed class HumanHandoffEntityConfiguration
	: IEntityTypeConfiguration<HumanHandoffEntity>
{
	public void Configure(EntityTypeBuilder<HumanHandoffEntity> builder)
	{
		builder.ToTable("human_handoff", schema: "ai_inquiry");
		builder.HasKey(entity => entity.HumanHandoffId);

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
		builder.Property(entity => entity.HumanHandoffId).HasColumnName("human_handoff_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.InquiryConversationId).HasColumnName("inquiry_conversation_id");
		builder.Property(entity => entity.RequestedAt).HasColumnName("requested_at");
		builder.Property(entity => entity.Reason).HasColumnName("reason");
		builder.Property(entity => entity.AssignedToUserId).HasColumnName("assigned_to_user_id");
		builder.Property(entity => entity.AcceptedAt).HasColumnName("accepted_at");
		builder.Property(entity => entity.ResolvedAt).HasColumnName("resolved_at");
		builder.Property(entity => entity.Status).HasColumnName("status");
	}
}
