using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Finance.Models;
namespace SmartSchool.Modules.Finance.Persistence.Configurations;
public sealed class FeeStructureEntityConfiguration : IEntityTypeConfiguration<FeeStructureEntity>
{
	public void Configure(EntityTypeBuilder<FeeStructureEntity> builder)
	{
		builder.ToTable("fee_structure", "finance");
		builder.HasKey(x => x.FeeStructureId);
		builder.Property(x => x.FeeStructureId).HasColumnName("fee_structure_id");
		builder.Property(x => x.TenantId).HasColumnName("tenant_id");
		builder.Property(x => x.GradeLevelId).HasColumnName("grade_level_id");
		builder.Property(x => x.FeeTypeId).HasColumnName("fee_type_id");
		builder.Property(x => x.AcademicYearId).HasColumnName("academic_year_id");
		builder.Property(x => x.Amount).HasColumnName("amount").HasPrecision(18, 2);
		builder.Property(x => x.Frequency).HasColumnName("frequency").HasMaxLength(30);
		builder.Property(x => x.EffectiveFrom).HasColumnName("effective_from");
		builder.Property(x => x.EffectiveTo).HasColumnName("effective_to");
		builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(100);
		builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(250);
		builder.Property(x => x.MetadataJson).HasColumnName("metadata_json").HasColumnType("jsonb");
		builder.Property(x => x.IsActive).HasColumnName("is_active");
		builder.Property(x => x.CreatedAt).HasColumnName("created_at");
		builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
		builder.Property(x => x.RowVersion).HasColumnName("row_version").IsConcurrencyToken();
		builder.HasIndex(x => new { x.TenantId, x.GradeLevelId, x.FeeTypeId, x.AcademicYearId });

        // Explicit parent-child relationships. Prevents EF Core shadow foreign keys.
        builder.HasOne<FeeTypeEntity>()
            .WithMany()
            .HasForeignKey(entity => entity.FeeTypeId)
            .OnDelete(DeleteBehavior.Restrict);

	}
}
