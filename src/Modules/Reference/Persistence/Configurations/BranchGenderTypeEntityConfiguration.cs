using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Reference.Models;

namespace SmartSchool.Modules.Reference.Persistence.Configurations;

public sealed class BranchGenderTypeEntityConfiguration : IEntityTypeConfiguration<BranchGenderTypeEntity>
{
    public void Configure(EntityTypeBuilder<BranchGenderTypeEntity> builder)
    {
        builder.ToTable("branch_gender_type", "reference");
        builder.HasKey(x => x.BranchGenderTypeId);
        builder.Property(x => x.BranchGenderTypeId).HasColumnName("branch_gender_type_id");
        builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(30).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(x => x.SortOrder).HasColumnName("sort_order").IsRequired();
        builder.Property(x => x.IsActive).HasColumnName("is_active").HasDefaultValue(true).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
    }
}
