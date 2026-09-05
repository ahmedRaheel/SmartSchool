using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Enums;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.TenantSettings;

public static class TenantSettingsFeature
{
    public sealed record Response(Guid TenantId, short AcademicYearStartMonth, string DefaultLanguage, string DateFormat, string TimeZone, short WeekStart, short FeeWarningDays,
        bool AiRagAssistant, bool AiTutor, bool AiQuiz, bool AiPredictions, bool AiAgent, bool AiParentChatbot, bool InternalChat, bool Notifications, bool Broadcast, bool ParentPortal,
        bool Assignments, bool StudentLeaveApply, bool Library, bool OnlinePayment, bool FeeReminders, bool DigitalReceipts, bool StaffSelfLeave, bool BiometricAttendance, bool QrAttendance, bool TwoFactor, bool SessionTimeout, bool IpRestriction);

    public sealed record GetQuery(Guid? TenantId) : IRequest<Result<Response>>;
    public sealed record SaveRequest(Guid? TenantId, short AcademicYearStartMonth, string DefaultLanguage, string DateFormat, string TimeZone, short WeekStart, short FeeWarningDays,
        bool AiRagAssistant, bool AiTutor, bool AiQuiz, bool AiPredictions, bool AiAgent, bool AiParentChatbot, bool InternalChat, bool Notifications, bool Broadcast, bool ParentPortal,
        bool Assignments, bool StudentLeaveApply, bool Library, bool OnlinePayment, bool FeeReminders, bool DigitalReceipts, bool StaffSelfLeave, bool BiometricAttendance, bool QrAttendance, bool TwoFactor, bool SessionTimeout, bool IpRestriction) : IRequest<Result<Response>>;

    public sealed class Validator : AbstractValidator<SaveRequest>
    {
        public Validator()
        {
            RuleFor(x => x.AcademicYearStartMonth).Must(x => x is 1 or 4 or 7 or 9);
            RuleFor(x => x.DefaultLanguage).Must(x => x is "en" or "ur" or "en-ur");
            RuleFor(x => x.DateFormat).Must(x => x is "DD/MM/YYYY" or "MM/DD/YYYY" or "YYYY-MM-DD");
            RuleFor(x => x.TimeZone).NotEmpty().MaximumLength(100);
            RuleFor(x => x.WeekStart).InclusiveBetween((short)0, (short)1);
            RuleFor(x => x.FeeWarningDays).Must(x => x is 3 or 5 or 7 or 14);
        }
    }

    private static Guid ResolveTenant(Guid? requested, ICurrentUser currentUser)
    {
        if (currentUser.IsSuperAdmin) return requested ?? throw new ValidationException("TenantId is required for SuperAdmin.");
        if (!currentUser.TenantId.HasValue) throw new UnauthorizedAccessException("Authenticated user has no tenant scope.");
        if (requested.HasValue && requested.Value != currentUser.TenantId.Value) throw new UnauthorizedAccessException("Requested tenant is outside the authenticated tenant scope.");
        return currentUser.TenantId.Value;
    }

    public sealed class GetHandler(IDbConnectionFactory connectionFactory, ICurrentUser currentUser) : IRequestHandler<GetQuery, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(GetQuery request, CancellationToken cancellationToken)
        {
            var tenantId = ResolveTenant(request.TenantId, currentUser);
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
            var response = await connection.QuerySingleOrDefaultAsync<Response>(new CommandDefinition(sql, new { TenantId = tenantId }, cancellationToken: cancellationToken));
            return Result<Response>.Success(response ?? Defaults(tenantId));
        }
    }

    public sealed class SaveHandler(IOrganizationDbContext db, ICurrentUser currentUser) : IRequestHandler<SaveRequest, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(SaveRequest request, CancellationToken cancellationToken)
        {
            var tenantId = ResolveTenant(request.TenantId, currentUser);
            var entity = await db.TenantSettings.SingleOrDefaultAsync(x => x.TenantId == tenantId, cancellationToken);
            if (entity is null)
            {
                entity = TenantSettingsEntity.Create(tenantId);
                await db.TenantSettings.AddAsync(entity, cancellationToken);
            }
            entity.Update(request.AcademicYearStartMonth, ParseLanguage(request.DefaultLanguage), ParseDateFormat(request.DateFormat), request.TimeZone, (SchoolWeekStart)request.WeekStart, request.FeeWarningDays,
                request.AiRagAssistant, request.AiTutor, request.AiQuiz, request.AiPredictions, request.AiAgent, request.AiParentChatbot, request.InternalChat, request.Notifications, request.Broadcast, request.ParentPortal,
                request.Assignments, request.StudentLeaveApply, request.Library, request.OnlinePayment, request.FeeReminders, request.DigitalReceipts, request.StaffSelfLeave, request.BiometricAttendance, request.QrAttendance, request.TwoFactor, request.SessionTimeout, request.IpRestriction);
            await db.SaveChangesAsync(cancellationToken);
            return Result<Response>.Success(ToResponse(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/organization/settings", async (Guid? tenantId, IMediator mediator, CancellationToken ct) => (await mediator.SendAsync<GetQuery, Result<Response>>(new GetQuery(tenantId), ct)).ToHttpResult())
            .WithName("GetTenantSettings").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantOnly);
        endpoints.MapPut("/api/organization/settings", async (SaveRequest request, IMediator mediator, CancellationToken ct) => (await mediator.SendAsync<SaveRequest, Result<Response>>(request, ct)).ToHttpResult())
            .WithName("SaveTenantSettings").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantOnly);
        return endpoints;
    }

    private static DefaultLanguage ParseLanguage(string value) => value switch { "ur" => DefaultLanguage.Urdu, "en-ur" => DefaultLanguage.EnglishUrdu, _ => DefaultLanguage.English };
    private static SchoolDateFormat ParseDateFormat(string value) => value switch { "MM/DD/YYYY" => SchoolDateFormat.MonthDayYear, "YYYY-MM-DD" => SchoolDateFormat.Iso, _ => SchoolDateFormat.DayMonthYear };
    private static string Language(DefaultLanguage value) => value switch { DefaultLanguage.Urdu => "ur", DefaultLanguage.EnglishUrdu => "en-ur", _ => "en" };
    private static string DateFormat(SchoolDateFormat value) => value switch { SchoolDateFormat.MonthDayYear => "MM/DD/YYYY", SchoolDateFormat.Iso => "YYYY-MM-DD", _ => "DD/MM/YYYY" };
    private static Response ToResponse(TenantSettingsEntity x) => new(x.TenantId, x.AcademicYearStartMonth, Language(x.DefaultLanguage), DateFormat(x.DateFormat), x.TimeZone, (short)x.WeekStart, x.FeeWarningDays, x.AiRagAssistant, x.AiTutor, x.AiQuiz, x.AiPredictions, x.AiAgent, x.AiParentChatbot, x.InternalChat, x.Notifications, x.Broadcast, x.ParentPortal, x.Assignments, x.StudentLeaveApply, x.Library, x.OnlinePayment, x.FeeReminders, x.DigitalReceipts, x.StaffSelfLeave, x.BiometricAttendance, x.QrAttendance, x.TwoFactor, x.SessionTimeout, x.IpRestriction);
    private static Response Defaults(Guid tenantId) => new(tenantId, 4, "en", "DD/MM/YYYY", "Asia/Karachi", 1, 5, true, true, true, true, false, true, true, true, true, true, true, true, true, false, true, true, true, false, false, false, true, false);
}
