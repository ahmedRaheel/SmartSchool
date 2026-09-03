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
        builder.ToTable("teacherdirectoryread", schema: "public");
        builder.HasKey(readModel => readModel.TeacherDirectoryReadId);
        builder.Property(readModel => readModel.TenantId).IsRequired();
        builder.Property(readModel => readModel.TeacherId).IsRequired();
        builder.HasIndex(readModel => new { readModel.TenantId, readModel.TeacherId }).IsUnique();
        builder.Property(readModel => readModel.RowVersion).IsConcurrencyToken();

        // Canonical database mapping generated from SmartSchoolComplete.sql.
        builder.Property(entity => entity.TeacherId).HasColumnName("teacherid");
        builder.Property(entity => entity.EmployeeNumber).HasColumnName("employeenumber");
        builder.Property(entity => entity.TeacherName).HasColumnName("teachername");
        builder.Property(entity => entity.JobTitle).HasColumnName("jobtitle");
        builder.Property(entity => entity.JobGrade).HasColumnName("jobgrade");
        builder.Property(entity => entity.DepartmentName).HasColumnName("departmentname");
        builder.Property(entity => entity.MobileNumber).HasColumnName("mobilenumber");
        builder.Property(entity => entity.ActiveClassAssignments).HasColumnName("activeclassassignments");
        builder.Property(entity => entity.DocumentCount).HasColumnName("documentcount");
        builder.Property(entity => entity.VerifiedDocumentCount).HasColumnName("verifieddocumentcount");
        builder.Property(entity => entity.TeacherDirectoryReadId).HasColumnName("id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenantid");
        builder.Property(entity => entity.IsActive).HasColumnName("isactive");
        builder.Property(entity => entity.CreatedAt).HasColumnName("createdat");
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updatedat");
        builder.Property(entity => entity.RowVersion).HasColumnName("rowversion");
    }
}
