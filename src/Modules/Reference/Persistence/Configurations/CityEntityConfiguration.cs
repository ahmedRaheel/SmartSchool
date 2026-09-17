using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Reference.Models;

namespace SmartSchool.Modules.Reference.Persistence.Configurations;

public sealed class CityEntityConfiguration : IEntityTypeConfiguration<CityEntity>
{
    public void Configure(EntityTypeBuilder<CityEntity> builder)
    {
        builder.ToTable("city", "reference");
        builder.HasKey(x => x.CityId);
        builder.Property(x => x.CityId).HasColumnName("city_id").ValueGeneratedOnAdd();
        builder.Property(x => x.ProvinceId).HasColumnName("province_id").IsRequired();
        builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(30).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(120).IsRequired();
        builder.HasIndex(x => new { x.ProvinceId, x.Code }).IsUnique();
        builder.HasOne<ProvinceEntity>().WithMany().HasForeignKey(x => x.ProvinceId).OnDelete(DeleteBehavior.Restrict);
    }
}
