using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="InquiryConversationEntity"/>.
/// </summary>
public sealed class InquiryConversationEntityConfiguration
	: IEntityTypeConfiguration<InquiryConversationEntity>
{
	public void Configure(EntityTypeBuilder<InquiryConversationEntity> builder)
	{
		builder.ToTable("inquiry_conversation", schema: "ai_inquiry");
builder.HasKey(entity => entity.InquiryConversationId);

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
		builder.Property(entity => entity.InquiryConversationId).HasColumnName("inquiry_conversation_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.CampusId).HasColumnName("campus_id");
		builder.Property(entity => entity.VisitorSessionId).HasColumnName("visitor_session_id");
		builder.Property(entity => entity.UserId).HasColumnName("user_id");
		builder.Property(entity => entity.VisitorName).HasColumnName("visitor_name");
		builder.Property(entity => entity.Phone).HasColumnName("phone");
		builder.Property(entity => entity.Email).HasColumnName("email");
		builder.Property(entity => entity.InterestedProgramId).HasColumnName("interested_program_id");
		builder.Property(entity => entity.StartedAt).HasColumnName("started_at");
		builder.Property(entity => entity.EndedAt).HasColumnName("ended_at");
		builder.Property(entity => entity.Status).HasColumnName("status");
	}
}
