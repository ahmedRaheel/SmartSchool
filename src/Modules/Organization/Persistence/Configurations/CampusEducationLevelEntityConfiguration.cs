using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Persistence.Configurations;

public sealed class CampusEducationLevelEntityConfiguration : IEntityTypeConfiguration<CampusEducationLevelEntity>
{
    public void Configure(EntityTypeBuilder<CampusEducationLevelEntity> builder)
    {
        builder.ToTable("campus_education_level", "org");
        builder.HasKey(x => new { x.TenantId, x.CampusId, x.EducationLevelId });
        builder.Property(x => x.TenantId).HasColumnName("tenant_id");
        builder.Property(x => x.CampusId).HasColumnName("campus_id");
        builder.Property(x => x.EducationLevelId).HasColumnName("education_level_id");
        builder.Ignore(x => x.IsActive);
        builder.Ignore(x => x.CreatedAt);
        builder.Ignore(x => x.UpdatedAt);
        builder.Ignore(x => x.RowVersion);
    }
}
