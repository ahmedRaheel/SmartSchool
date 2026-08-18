using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence.Configurations;

/// <summary>
/// Configures the TeacherDirectoryRead materialized read table.
/// </summary>
public sealed class TeacherDirectoryReadEntityConfiguration
	: IEntityTypeConfiguration<TeacherDirectoryReadEntity>
{
	public void Configure(EntityTypeBuilder<TeacherDirectoryReadEntity> builder)
	{
		builder.ToTable("TeacherDirectoryRead");
		builder.HasKey(readModel => readModel.Id);
		builder.Property(readModel => readModel.TenantId).IsRequired();
		builder.Property(readModel => readModel.TeacherId).IsRequired();
		builder.HasIndex(readModel => new { readModel.TenantId, readModel.TeacherId }).IsUnique();
		builder.Property(readModel => readModel.RowVersion).IsConcurrencyToken();
	}
}
