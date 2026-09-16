using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Transport.Models;
namespace SmartSchool.Modules.Transport.Persistence.Configurations;
public sealed class TripRecordEntityConfiguration : IEntityTypeConfiguration<TripRecordEntity>
{
    public void Configure(EntityTypeBuilder<TripRecordEntity> builder)
    {
        builder.ToTable("trip_record", "transport");
        builder.HasKey(x => x.TripRecordId);
        builder.Property(x => x.TripRecordId).HasColumnName("trip_record_id");
        builder.Property(x => x.TenantId).HasColumnName("tenant_id");
        builder.Property(x => x.IsActive).HasColumnName("is_active");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        builder.Property(x => x.RowVersion).HasColumnName("row_version");
        builder.Property(x => x.RouteId).HasColumnName("route_id");
        builder.Property(x => x.StudentId).HasColumnName("student_id");
        builder.Property(x => x.ServiceDate).HasColumnName("service_date");
        builder.Property(x => x.Direction).HasColumnName("direction");
        builder.Property(x => x.Status).HasColumnName("status");
        builder.Property(x => x.RecordedBy).HasColumnName("recorded_by");
        builder.HasIndex(x => new { x.TenantId, x.RouteId, x.StudentId, x.ServiceDate, x.Direction }).IsUnique();
        builder.Property(x => x.RowVersion).IsConcurrencyToken();
    }
}
