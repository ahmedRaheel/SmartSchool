using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence.Configurations;

public sealed class AssignmentStudentEntityConfiguration
    : IEntityTypeConfiguration<AssignmentStudentEntity>
{
    public void Configure(EntityTypeBuilder<AssignmentStudentEntity> builder)
    {
        builder.ToTable("assignment_student", "lms");
        builder.HasKey(entity => entity.AssignmentStudentId);

        builder.Property(entity => entity.AssignmentStudentId)
            .HasColumnName("assignment_student_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.Property(entity => entity.AcademicAssignmentId)
            .HasColumnName("academic_assignment_id").IsRequired();
        builder.Property(entity => entity.StudentId).HasColumnName("student_id").IsRequired();
        builder.Property(entity => entity.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version").IsConcurrencyToken();

        builder.HasIndex(entity => new
        {
            entity.TenantId,
            entity.AcademicAssignmentId,
            entity.StudentId
        }).IsUnique();

        builder.HasOne<AssignmentEntity>()
            .WithMany()
            .HasForeignKey(entity => entity.AcademicAssignmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
