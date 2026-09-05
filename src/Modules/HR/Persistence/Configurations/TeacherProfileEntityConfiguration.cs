using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="TeacherProfileEntity"/>.
/// </summary>
public sealed class TeacherProfileEntityConfiguration
    : IEntityTypeConfiguration<TeacherProfileEntity>
{
    public void Configure(EntityTypeBuilder<TeacherProfileEntity> builder)
    {
        builder.ToTable("TeacherProfile", SmartSchool.Modules.HR.ModuleConstants.Schema);

        builder.HasKey(entity => entity.TeacherProfileId);

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

    }
}
