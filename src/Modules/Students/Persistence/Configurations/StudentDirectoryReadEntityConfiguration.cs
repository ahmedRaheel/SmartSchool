using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence.Configurations;

/// <summary>
/// Configures the StudentDirectoryRead materialized read table.
/// </summary>
public sealed class StudentDirectoryReadEntityConfiguration
	: IEntityTypeConfiguration<StudentDirectoryReadEntity>
{
	public void Configure(EntityTypeBuilder<StudentDirectoryReadEntity> builder)
	{
		builder.ToTable("StudentDirectoryRead");
		builder.HasKey(readModel => readModel.Id);
		builder.Property(readModel => readModel.TenantId).IsRequired();
		builder.Property(readModel => readModel.StudentId).IsRequired();
		builder.HasIndex(readModel => new { readModel.TenantId, readModel.StudentId }).IsUnique();
		builder.Property(readModel => readModel.RowVersion).IsConcurrencyToken();
	}
}
