using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="TeachingRecommendationEntity"/>.
/// </summary>
public sealed class TeachingRecommendationEntityConfiguration
    : IEntityTypeConfiguration<TeachingRecommendationEntity>
{
    public void Configure(EntityTypeBuilder<TeachingRecommendationEntity> builder)
    {
        builder.ToTable("teaching_recommendation", schema: "ai");
        builder.HasKey(entity => entity.TeachingRecommendationId);

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
        builder.Property(entity => entity.TeachingRecommendationId).HasColumnName("teaching_recommendation_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
        builder.Property(entity => entity.IsActive).HasColumnName("is_active");
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

        // Database columns synchronized from SmartSchoolComplete.sql.
        builder.Property(entity => entity.ClassPerformanceInsightId).HasColumnName("class_performance_insight_id");
        builder.Property(entity => entity.ClassSectionId).HasColumnName("class_section_id");
        builder.Property(entity => entity.CourseOfferingId).HasColumnName("course_offering_id");
        builder.Property(entity => entity.TeacherEmployeeId).HasColumnName("teacher_employee_id");
        builder.Property(entity => entity.SubjectId).HasColumnName("subject_id");
        builder.Property(entity => entity.Topic).HasColumnName("topic");
        builder.Property(entity => entity.RecommendationType).HasColumnName("recommendation_type");
        builder.Property(entity => entity.Title).HasColumnName("title");
        builder.Property(entity => entity.RecommendationText).HasColumnName("recommendation_text");
        builder.Property(entity => entity.Rationale).HasColumnName("rationale");
        builder.Property(entity => entity.Priority).HasColumnName("priority");
        builder.Property(entity => entity.Status).HasColumnName("status");
        builder.Property(entity => entity.GeneratedAt).HasColumnName("generated_at");
        builder.Property(entity => entity.ReviewedAt).HasColumnName("reviewed_at");
        builder.Property(entity => entity.ReviewedBy).HasColumnName("reviewed_by");
        builder.Property(entity => entity.TeacherComments).HasColumnName("teacher_comments");

        // Explicit parent-child relationships. Prevents EF Core shadow foreign keys.
        builder.HasOne<ClassPerformanceInsightEntity>()
            .WithMany()
            .HasForeignKey(entity => entity.ClassPerformanceInsightId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
