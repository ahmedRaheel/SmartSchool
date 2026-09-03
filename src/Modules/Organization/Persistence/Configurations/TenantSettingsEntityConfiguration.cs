using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Persistence.Configurations;

public sealed class TenantSettingsEntityConfiguration : IEntityTypeConfiguration<TenantSettingsEntity>
{
    public void Configure(EntityTypeBuilder<TenantSettingsEntity> builder)
    {
        builder.ToTable("tenant_settings", "saas");
        builder.HasKey(x => x.TenantSettingsId);
        builder.Property(x => x.TenantSettingsId).HasColumnName("tenant_settings_id");
        builder.Property(x => x.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.HasIndex(x => x.TenantId).IsUnique();
        builder.Property(x => x.AcademicYearStartMonth).HasColumnName("academic_year_start_month");
        builder.Property(x => x.DefaultLanguage).HasColumnName("default_language").HasColumnType("smallint");
        builder.Property(x => x.DateFormat).HasColumnName("date_format").HasColumnType("smallint");
        builder.Property(x => x.TimeZone).HasColumnName("time_zone").HasMaxLength(100).IsRequired();
        builder.Property(x => x.WeekStart).HasColumnName("week_start").HasColumnType("smallint");
        builder.Property(x => x.FeeWarningDays).HasColumnName("fee_warning_days");
        builder.Property(x => x.AiRagAssistant).HasColumnName("ai_rag_assistant"); builder.Property(x => x.AiTutor).HasColumnName("ai_tutor"); builder.Property(x => x.AiQuiz).HasColumnName("ai_quiz");
        builder.Property(x => x.AiPredictions).HasColumnName("ai_predictions"); builder.Property(x => x.AiAgent).HasColumnName("ai_agent"); builder.Property(x => x.AiParentChatbot).HasColumnName("ai_parent_chatbot");
        builder.Property(x => x.InternalChat).HasColumnName("internal_chat"); builder.Property(x => x.Notifications).HasColumnName("notifications"); builder.Property(x => x.Broadcast).HasColumnName("broadcast"); builder.Property(x => x.ParentPortal).HasColumnName("parent_portal");
        builder.Property(x => x.Assignments).HasColumnName("assignments"); builder.Property(x => x.StudentLeaveApply).HasColumnName("student_leave_apply"); builder.Property(x => x.Library).HasColumnName("library_enabled");
        builder.Property(x => x.OnlinePayment).HasColumnName("online_payment"); builder.Property(x => x.FeeReminders).HasColumnName("fee_reminders"); builder.Property(x => x.DigitalReceipts).HasColumnName("digital_receipts");
        builder.Property(x => x.StaffSelfLeave).HasColumnName("staff_self_leave"); builder.Property(x => x.BiometricAttendance).HasColumnName("biometric_attendance"); builder.Property(x => x.QrAttendance).HasColumnName("qr_attendance");
        builder.Property(x => x.TwoFactor).HasColumnName("two_factor"); builder.Property(x => x.SessionTimeout).HasColumnName("session_timeout"); builder.Property(x => x.IpRestriction).HasColumnName("ip_restriction");
        builder.Property(x => x.IsActive).HasColumnName("is_active"); builder.Property(x => x.CreatedAt).HasColumnName("created_at"); builder.Property(x => x.UpdatedAt).HasColumnName("updated_at"); builder.Property(x => x.RowVersion).HasColumnName("row_version").IsConcurrencyToken();
    }
}
