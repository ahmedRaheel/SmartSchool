using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Reference.Models;

namespace SmartSchool.Modules.Reference.Persistence.Configurations;

public sealed class ProvinceEntityConfiguration : IEntityTypeConfiguration<ProvinceEntity>
{
    public void Configure(EntityTypeBuilder<ProvinceEntity> builder)
    {
        builder.ToTable("province", "reference");
        builder.HasKey(x => x.ProvinceId);
        builder.Property(x => x.ProvinceId).HasColumnName("province_id").ValueGeneratedOnAdd();
        builder.Property(x => x.CountryId).HasColumnName("country_id").IsRequired();
        builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(20).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.HasIndex(x => new { x.CountryId, x.Code }).IsUnique();
        builder.HasOne<CountryEntity>().WithMany().HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Restrict);
    }
}
