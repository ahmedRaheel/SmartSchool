using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Reference.Models;

namespace SmartSchool.Modules.Reference.Persistence.Configurations;

public sealed class EducationLevelEntityConfiguration : IEntityTypeConfiguration<EducationLevelEntity>
{
    public void Configure(EntityTypeBuilder<EducationLevelEntity> builder)
    {
        builder.ToTable("education_level", "reference");
        builder.HasKey(x => x.EducationLevelId);
        builder.Property(x => x.EducationLevelId).HasColumnName("education_level_id");
        builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(30).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(x => x.SortOrder).HasColumnName("sort_order").IsRequired();
        builder.Property(x => x.IsActive).HasColumnName("is_active").HasDefaultValue(true).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
    }
}
