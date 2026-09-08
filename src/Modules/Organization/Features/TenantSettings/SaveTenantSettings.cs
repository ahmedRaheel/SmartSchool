using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Enums;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.TenantSettings;

public static class SaveTenantSettings
{
    public sealed record Request(Guid? TenantId, short AcademicYearStartMonth, string DefaultLanguage, string DateFormat, string TimeZone, short WeekStart, short FeeWarningDays, bool AiRagAssistant, bool AiTutor, bool AiQuiz, bool AiPredictions, bool AiAgent, bool AiParentChatbot, bool InternalChat, bool Notifications, bool Broadcast, bool ParentPortal, bool Assignments, bool StudentLeaveApply, bool Library, bool OnlinePayment, bool FeeReminders, bool DigitalReceipts, bool StaffSelfLeave, bool BiometricAttendance, bool QrAttendance, bool TwoFactor, bool SessionTimeout, bool IpRestriction) : IRequest<Result<Response>>;
    public sealed record Response(Guid TenantId, short AcademicYearStartMonth, string DefaultLanguage, string DateFormat, string TimeZone, short WeekStart, short FeeWarningDays,
        bool AiRagAssistant, bool AiTutor, bool AiQuiz, bool AiPredictions, bool AiAgent, bool AiParentChatbot, bool InternalChat, bool Notifications, bool Broadcast, bool ParentPortal,
        bool Assignments, bool StudentLeaveApply, bool Library, bool OnlinePayment, bool FeeReminders, bool DigitalReceipts, bool StaffSelfLeave, bool BiometricAttendance, bool QrAttendance, bool TwoFactor, bool SessionTimeout, bool IpRestriction);
    public sealed class Validator : AbstractValidator<Request> { public Validator() { RuleFor(x => x.AcademicYearStartMonth).Must(x => x is 1 or 4 or 7 or 9); RuleFor(x => x.DefaultLanguage).Must(x => x is "en" or "ur" or "en-ur"); RuleFor(x => x.DateFormat).Must(x => x is "DD/MM/YYYY" or "MM/DD/YYYY" or "YYYY-MM-DD"); RuleFor(x => x.TimeZone).NotEmpty().MaximumLength(100); RuleFor(x => x.WeekStart).InclusiveBetween((short)0, (short)1); RuleFor(x => x.FeeWarningDays).Must(x => x is 3 or 5 or 7 or 14); } }
    public interface ISaveTenantSettingsCommand { Task<TenantSettingsEntity> ExecuteAsync(Guid tenantId, Request request, CancellationToken cancellationToken); }
    internal sealed class SaveTenantSettingsCommand(IOrganizationDbContext dbContext) : ISaveTenantSettingsCommand
    {
        public async Task<TenantSettingsEntity> ExecuteAsync(Guid tenantId, Request request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.TenantSettings.SingleOrDefaultAsync(x => x.TenantId == tenantId, cancellationToken);
            if (entity is null) { entity = TenantSettingsEntity.Create(tenantId); await dbContext.TenantSettings.AddAsync(entity, cancellationToken); }
            entity.Update(request.AcademicYearStartMonth, ParseLanguage(request.DefaultLanguage), ParseDateFormat(request.DateFormat), request.TimeZone, (SchoolWeekStart)request.WeekStart, request.FeeWarningDays, request.AiRagAssistant, request.AiTutor, request.AiQuiz, request.AiPredictions, request.AiAgent, request.AiParentChatbot, request.InternalChat, request.Notifications, request.Broadcast, request.ParentPortal, request.Assignments, request.StudentLeaveApply, request.Library, request.OnlinePayment, request.FeeReminders, request.DigitalReceipts, request.StaffSelfLeave, request.BiometricAttendance, request.QrAttendance, request.TwoFactor, request.SessionTimeout, request.IpRestriction);
            await dbContext.SaveChangesAsync(cancellationToken); return entity;
        }
    }
    public sealed class Handler(ISaveTenantSettingsCommand command, ICurrentUser currentUser) : IRequestHandler<Request, Result<Response>> { public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken) { var tenantId = ResolveTenant(request.TenantId, currentUser); var entity = await command.ExecuteAsync(tenantId, request, cancellationToken); return Result<Response>.Success(ToResponse(entity)); } }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapPut("/api/organization/settings", async (Request request, IMediator mediator, CancellationToken cancellationToken) => (await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken)).ToHttpResult()).WithName("SaveTenantSettings").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantOnly);
    private static Guid ResolveTenant(Guid? requested, ICurrentUser currentUser) { if (currentUser.IsSuperAdmin) return requested ?? throw new ValidationException("TenantId is required for SuperAdmin."); if (!currentUser.TenantId.HasValue) throw new UnauthorizedAccessException("Authenticated user has no tenant scope."); if (requested.HasValue && requested.Value != currentUser.TenantId.Value) throw new UnauthorizedAccessException("Requested tenant is outside the authenticated tenant scope."); return currentUser.TenantId.Value; }
    private static DefaultLanguage ParseLanguage(string value) => value switch { "ur" => DefaultLanguage.Urdu, "en-ur" => DefaultLanguage.EnglishUrdu, _ => DefaultLanguage.English };
    private static SchoolDateFormat ParseDateFormat(string value) => value switch { "MM/DD/YYYY" => SchoolDateFormat.MonthDayYear, "YYYY-MM-DD" => SchoolDateFormat.Iso, _ => SchoolDateFormat.DayMonthYear };
    private static string Language(DefaultLanguage value) => value switch { DefaultLanguage.Urdu => "ur", DefaultLanguage.EnglishUrdu => "en-ur", _ => "en" };
    private static string DateFormat(SchoolDateFormat value) => value switch { SchoolDateFormat.MonthDayYear => "MM/DD/YYYY", SchoolDateFormat.Iso => "YYYY-MM-DD", _ => "DD/MM/YYYY" };
    private static Response ToResponse(TenantSettingsEntity entity) => new(entity.TenantId, entity.AcademicYearStartMonth, Language(entity.DefaultLanguage), DateFormat(entity.DateFormat), entity.TimeZone, (short)entity.WeekStart, entity.FeeWarningDays, entity.AiRagAssistant, entity.AiTutor, entity.AiQuiz, entity.AiPredictions, entity.AiAgent, entity.AiParentChatbot, entity.InternalChat, entity.Notifications, entity.Broadcast, entity.ParentPortal, entity.Assignments, entity.StudentLeaveApply, entity.Library, entity.OnlinePayment, entity.FeeReminders, entity.DigitalReceipts, entity.StaffSelfLeave, entity.BiometricAttendance, entity.QrAttendance, entity.TwoFactor, entity.SessionTimeout, entity.IpRestriction);
}
