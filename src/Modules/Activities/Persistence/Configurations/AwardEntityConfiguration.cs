using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence.Configurations;

public sealed class AwardEntityConfiguration : IEntityTypeConfiguration<AwardEntity>
{
    public void Configure(EntityTypeBuilder<AwardEntity> builder)
    {
        builder.ToTable("student_award", "activity");
        builder.HasKey(entity => entity.StudentAwardId);

        builder.Property(entity => entity.StudentAwardId).HasColumnName("student_award_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.Property(entity => entity.StudentId).HasColumnName("student_id").IsRequired();
        builder.Property(entity => entity.AwardTypeCode).HasColumnName("award_type_code").HasMaxLength(50).IsRequired();
        builder.Property(entity => entity.Title).HasColumnName("title").HasMaxLength(180).IsRequired();
        builder.Property(entity => entity.Description).HasColumnName("description");
        builder.Property(entity => entity.AwardDate).HasColumnName("award_date").IsRequired();
        builder.Property(entity => entity.ApprovedBy).HasColumnName("approved_by");
        builder.Property(entity => entity.DocumentId).HasColumnName("generated_document_id");
        builder.Property(entity => entity.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version").IsRequired().IsConcurrencyToken();

        builder.HasIndex(entity => new { entity.TenantId, entity.StudentId, entity.AwardDate });
    }
}
