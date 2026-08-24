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
		builder.ToTable("studentdirectoryread", schema: "public");
<<<<<<< HEAD
builder.HasKey(readModel => readModel.StudentDirectoryReadId);
=======
		builder.Ignore(entity => entity.Id);
		builder.HasKey(readModel => readModel.Id);
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0
		builder.Property(readModel => readModel.TenantId).IsRequired();
		builder.Property(readModel => readModel.StudentId).IsRequired();
		builder.HasIndex(readModel => new { readModel.TenantId, readModel.StudentId }).IsUnique();
		builder.Property(readModel => readModel.RowVersion).IsConcurrencyToken();

		// Canonical database mapping generated from SmartSchoolComplete.sql.
		builder.Property(entity => entity.StudentId).HasColumnName("studentid");
		builder.Property(entity => entity.AdmissionNumber).HasColumnName("admissionnumber");
		builder.Property(entity => entity.StudentName).HasColumnName("studentname");
		builder.Property(entity => entity.ProgramName).HasColumnName("programname");
		builder.Property(entity => entity.ClassName).HasColumnName("classname");
		builder.Property(entity => entity.SectionName).HasColumnName("sectionname");
		builder.Property(entity => entity.PrimaryGuardianName).HasColumnName("primaryguardianname");
		builder.Property(entity => entity.PrimaryGuardianMobile).HasColumnName("primaryguardianmobile");
		builder.Property(entity => entity.AttendancePercentage).HasColumnName("attendancepercentage");
		builder.Property(entity => entity.LatestExamPercentage).HasColumnName("latestexampercentage");
		builder.Property(entity => entity.OutstandingBalance).HasColumnName("outstandingbalance");
		builder.Property(entity => entity.DocumentCount).HasColumnName("documentcount");
		builder.Property(entity => entity.VerifiedDocumentCount).HasColumnName("verifieddocumentcount");
		builder.Property(entity => entity.StudentId).HasColumnName("id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenantid");
		builder.Property(entity => entity.IsActive).HasColumnName("isactive");
		builder.Property(entity => entity.CreatedAt).HasColumnName("createdat");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updatedat");
		builder.Property(entity => entity.RowVersion).HasColumnName("rowversion");
	}
}
