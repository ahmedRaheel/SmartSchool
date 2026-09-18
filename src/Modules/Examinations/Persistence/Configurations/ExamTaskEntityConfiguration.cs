using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Persistence.Configurations;

public sealed class ExamTaskEntityConfiguration : IEntityTypeConfiguration<ExamTaskEntity>
{
    public void Configure(EntityTypeBuilder<ExamTaskEntity> builder)
    {
        builder.ToTable("exam_task", "exam");
        builder.HasKey(entity => entity.ExamTaskId);

        builder.Property(entity => entity.ExamTaskId).HasColumnName("exam_task_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.Property(entity => entity.ExamId).HasColumnName("exam_id").IsRequired();
        builder.Property(entity => entity.ExamSubjectId).HasColumnName("exam_subject_id").IsRequired();
        builder.Property(entity => entity.CourseOfferingId).HasColumnName("course_offering_id").IsRequired();
        builder.Property(entity => entity.TeacherCourseAssignmentId).HasColumnName("teacher_course_assignment_id").IsRequired();
        builder.Property(entity => entity.TeacherEmployeeId).HasColumnName("teacher_employee_id").IsRequired();
        builder.Property(entity => entity.TeacherUserId).HasColumnName("teacher_user_id").IsRequired();
        builder.Property(entity => entity.TaskType).HasColumnName("task_type").HasMaxLength(30).IsRequired();
        builder.Property(entity => entity.Title).HasColumnName("title").HasMaxLength(250).IsRequired();
        builder.Property(entity => entity.Instructions).HasColumnName("instructions").HasMaxLength(4000);
        builder.Property(entity => entity.AssignedByUserId).HasColumnName("assigned_by_user_id").IsRequired();
        builder.Property(entity => entity.AssignedAt).HasColumnName("assigned_at").IsRequired();
        builder.Property(entity => entity.DueAt).HasColumnName("due_at").IsRequired();
        builder.Property(entity => entity.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
        builder.Property(entity => entity.SubmittedAt).HasColumnName("submitted_at");
        builder.Property(entity => entity.CompletedAt).HasColumnName("completed_at");
        builder.Property(entity => entity.CompletedByUserId).HasColumnName("completed_by_user_id");
        builder.Property(entity => entity.SubmissionNotes).HasColumnName("submission_notes").HasMaxLength(4000);
        builder.Property(entity => entity.SubmissionFileName).HasColumnName("submission_file_name").HasMaxLength(255);
        builder.Property(entity => entity.SubmissionContentType).HasColumnName("submission_content_type").HasMaxLength(150);
        builder.Property(entity => entity.SubmissionFileData).HasColumnName("submission_file_data");
        builder.Property(entity => entity.AssignmentNotifiedAt).HasColumnName("assignment_notified_at");
        builder.Property(entity => entity.Reminder24HoursSentAt).HasColumnName("reminder_24_hours_sent_at");
        builder.Property(entity => entity.Reminder2HoursSentAt).HasColumnName("reminder_2_hours_sent_at");
        builder.Property(entity => entity.OverdueReminderSentAt).HasColumnName("overdue_reminder_sent_at");
        builder.Property(entity => entity.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version").IsConcurrencyToken();

        builder.HasIndex(entity => new { entity.TenantId, entity.TeacherUserId, entity.Status });
        builder.HasIndex(entity => new { entity.TenantId, entity.ExamId, entity.ExamSubjectId });
        builder.HasIndex(entity => new { entity.TenantId, entity.DueAt, entity.Status });
        builder.HasIndex(entity => new
        {
            entity.TenantId,
            entity.ExamSubjectId,
            entity.TeacherEmployeeId,
            entity.TaskType,
            entity.IsActive
        }).IsUnique();

        builder.HasOne<ExamEntity>()
            .WithMany()
            .HasForeignKey(entity => entity.ExamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ExamSubjectEntity>()
            .WithMany()
            .HasForeignKey(entity => entity.ExamSubjectId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
