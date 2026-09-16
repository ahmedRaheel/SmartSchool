using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Transport.Models;
namespace SmartSchool.Modules.Transport.Persistence.Configurations;
public sealed class RouteNoticeEntityConfiguration : IEntityTypeConfiguration<RouteNoticeEntity>
{
    public void Configure(EntityTypeBuilder<RouteNoticeEntity> builder)
    {
        builder.ToTable("route_notice", "transport");
        builder.HasKey(x => x.RouteNoticeId);
        builder.Property(x => x.RouteNoticeId).HasColumnName("route_notice_id");
        builder.Property(x => x.TenantId).HasColumnName("tenant_id");
        builder.Property(x => x.IsActive).HasColumnName("is_active");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        builder.Property(x => x.RowVersion).HasColumnName("row_version");
        builder.Property(x => x.RouteId).HasColumnName("route_id");
        builder.Property(x => x.ServiceDate).HasColumnName("service_date");
        builder.Property(x => x.DelayMinutes).HasColumnName("delay_minutes");
        builder.Property(x => x.Message).HasColumnName("message");
        builder.Property(x => x.CreatedBy).HasColumnName("created_by");
        builder.Property(x => x.RowVersion).IsConcurrencyToken();
    }
}
