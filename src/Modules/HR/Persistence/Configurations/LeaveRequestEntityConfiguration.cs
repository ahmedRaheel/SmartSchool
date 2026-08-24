using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="LeaveRequestEntity"/>.
/// </summary>
public sealed class LeaveRequestEntityConfiguration
	: IEntityTypeConfiguration<LeaveRequestEntity>
{
	public void Configure(EntityTypeBuilder<LeaveRequestEntity> builder)
	{
		builder.ToTable("leave_request", schema: "teacher");
builder.HasKey(entity => entity.LeaveRequestId);

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

		builder
			.Property(entity => entity.Code)
			.HasMaxLength(100)
			.IsRequired();

		builder
			.HasIndex(entity => new { entity.TenantId, entity.Code })
			.IsUnique();

		builder
			.Property(entity => entity.Name)
			.HasMaxLength(250)
			.IsRequired();


		// Canonical database mapping generated from SmartSchoolComplete.sql.
		builder.Property(entity => entity.Code).HasColumnName("code");
		builder.Property(entity => entity.Name).HasColumnName("name");
		builder.Property(entity => entity.MetadataJson).HasColumnName("metadata_json").HasColumnType("jsonb");
		builder.Property(entity => entity.LeaveRequestId).HasColumnName("leave_request_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.EmployeeId).HasColumnName("employee_id");
		builder.Property(entity => entity.LeaveType).HasColumnName("leave_type");
		builder.Property(entity => entity.FromDate).HasColumnName("from_date");
		builder.Property(entity => entity.ToDate).HasColumnName("to_date");
		builder.Property(entity => entity.Reason).HasColumnName("reason");
		builder.Property(entity => entity.Status).HasColumnName("status");
		builder.Property(entity => entity.ApprovedBy).HasColumnName("approved_by");
		builder.Property(entity => entity.DecisionAt).HasColumnName("decision_at");
		builder.Property(entity => entity.DecisionNote).HasColumnName("decision_note");
	}
}
