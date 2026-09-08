using Dapper;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.TenantSettings;

public static class GetTenantSettings
{
    public sealed record Request(Guid? TenantId) : IRequest<Result<Response>>;
    public sealed record Response(Guid TenantId, short AcademicYearStartMonth, string DefaultLanguage, string DateFormat, string TimeZone, short WeekStart, short FeeWarningDays,
        bool AiRagAssistant, bool AiTutor, bool AiQuiz, bool AiPredictions, bool AiAgent, bool AiParentChatbot, bool InternalChat, bool Notifications, bool Broadcast, bool ParentPortal,
        bool Assignments, bool StudentLeaveApply, bool Library, bool OnlinePayment, bool FeeReminders, bool DigitalReceipts, bool StaffSelfLeave, bool BiometricAttendance, bool QrAttendance, bool TwoFactor, bool SessionTimeout, bool IpRestriction);
    public interface IGetTenantSettingsQuery { Task<Response?> ExecuteAsync(Guid tenantId, CancellationToken cancellationToken); }
    internal sealed class GetTenantSettingsQuery(IDbConnectionFactory connectionFactory) : IGetTenantSettingsQuery
    {
        public async Task<Response?> ExecuteAsync(Guid tenantId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT tenant_id AS "TenantId", academic_year_start_month AS "AcademicYearStartMonth",
                  CASE default_language WHEN 1 THEN 'en' WHEN 2 THEN 'ur' ELSE 'en-ur' END AS "DefaultLanguage",
                  CASE date_format WHEN 1 THEN 'DD/MM/YYYY' WHEN 2 THEN 'MM/DD/YYYY' ELSE 'YYYY-MM-DD' END AS "DateFormat",
                  time_zone AS "TimeZone", week_start AS "WeekStart", fee_warning_days AS "FeeWarningDays",
                  ai_rag_assistant AS "AiRagAssistant", ai_tutor AS "AiTutor", ai_quiz AS "AiQuiz", ai_predictions AS "AiPredictions", ai_agent AS "AiAgent", ai_parent_chatbot AS "AiParentChatbot",
                  internal_chat AS "InternalChat", notifications AS "Notifications", broadcast AS "Broadcast", parent_portal AS "ParentPortal", assignments AS "Assignments", student_leave_apply AS "StudentLeaveApply", library_enabled AS "Library",
                  online_payment AS "OnlinePayment", fee_reminders AS "FeeReminders", digital_receipts AS "DigitalReceipts", staff_self_leave AS "StaffSelfLeave", biometric_attendance AS "BiometricAttendance", qr_attendance AS "QrAttendance",
                  two_factor AS "TwoFactor", session_timeout AS "SessionTimeout", ip_restriction AS "IpRestriction"
                FROM saas.tenant_settings WHERE tenant_id = @TenantId AND is_active = TRUE;
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<Response>(new CommandDefinition(sql, new { TenantId = tenantId }, cancellationToken: cancellationToken));
        }
    }
    public sealed class Handler(IGetTenantSettingsQuery query, ICurrentUser currentUser) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var tenantId = ResolveTenant(request.TenantId, currentUser);
            var response = await query.ExecuteAsync(tenantId, cancellationToken);
            return Result<Response>.Success(response ?? Defaults(tenantId));
        }
    }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapGet("/api/organization/settings", async (Guid? tenantId, IMediator mediator, CancellationToken cancellationToken) => (await mediator.SendAsync<Request, Result<Response>>(new Request(tenantId), cancellationToken)).ToHttpResult()).WithName("GetTenantSettings").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantOnly);
    private static Guid ResolveTenant(Guid? requested, ICurrentUser currentUser) { if (currentUser.IsSuperAdmin) return requested ?? throw new ValidationException("TenantId is required for SuperAdmin."); if (!currentUser.TenantId.HasValue) throw new UnauthorizedAccessException("Authenticated user has no tenant scope."); if (requested.HasValue && requested.Value != currentUser.TenantId.Value) throw new UnauthorizedAccessException("Requested tenant is outside the authenticated tenant scope."); return currentUser.TenantId.Value; }
    private static Response Defaults(Guid tenantId) => new(tenantId, 4, "en", "DD/MM/YYYY", "Asia/Karachi", 1, 5, true, true, true, true, false, true, true, true, true, true, true, true, true, false, true, true, true, false, false, false, true, false);
}
