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
		builder.ToTable("InquiryConversation");

		builder.HasKey(entity => entity.Id);

		builder
			.Property(entity => entity.TenantId)
			.IsRequired();

		builder
			.Property(entity => entity.IsActive)
			.IsRequired();

		builder
			.Property(entity => entity.RowVersion)
			.IsConcurrencyToken();

		builder.HasIndex(entity => entity.TenantId);

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
