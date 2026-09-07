using Dapper;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features;

public interface IGetAdmissionApplicationsQuery { Task<IReadOnlyList<AdmissionApplicationDto>> GetApplicationsAsync(Guid tenantId, CancellationToken cancellationToken); }
public sealed class GetAdmissionApplicationsQuery(IDbConnectionFactory factory) : IGetAdmissionApplicationsQuery
{
    public async Task<IReadOnlyList<AdmissionApplicationDto>> GetApplicationsAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        const string sql = """
        SELECT application_id AS Id, school_id AS SchoolId, branch_id AS BranchId, academic_year_id AS AcademicYearId,
        class_id AS ClassId, section_id AS SectionId, first_name AS FirstName, last_name AS LastName, date_of_birth AS DateOfBirth,
        gender AS Gender, email AS Email, phone AS Phone, guardian_name AS GuardianName, guardian_email AS GuardianEmail,
        guardian_phone AS GuardianPhone, previous_marks AS PreviousMarks, status AS Status, submitted_at AS SubmittedAt,
        decision_notes AS DecisionNotes, student_id AS StudentId
        FROM admission.student_application WHERE tenant_id=@TenantId AND is_active=TRUE ORDER BY submitted_at DESC;
        """;
        await using var connection = await factory.OpenConnectionAsync(cancellationToken); return (await connection.QueryAsync<AdmissionApplicationDto>(new CommandDefinition(sql,new{TenantId=tenantId},cancellationToken: cancellationToken))).AsList();
    }
}

public static class GetAdmissionApplications
{
    public sealed record Request(Guid? TenantId)
        : IRequest<Result<IReadOnlyList<AdmissionApplicationDto>>>;

    public sealed class Handler(
        ITenantScope tenantScope,
        IGetAdmissionApplicationsQuery query)
        : IRequestHandler<Request, Result<IReadOnlyList<AdmissionApplicationDto>>>
    {
        public async Task<Result<IReadOnlyList<AdmissionApplicationDto>>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<IReadOnlyList<AdmissionApplicationDto>>.Failure(
                    Error.Validation("Tenant context is required."));
            }

            var applications = await query.GetApplicationsAsync(
                tenantId.Value,
                cancellationToken);

            return Result<IReadOnlyList<AdmissionApplicationDto>>.Success(applications);
        }
    }
}

public sealed record AdmissionApplicationDto(
    Guid Id,
    Guid SchoolId,
    Guid BranchId,
    Guid? AcademicYearId,
    Guid? ClassId,
    Guid? SectionId,
    string FirstName,
    string? LastName,
    DateOnly? DateOfBirth,
    string? Gender,
    string? Email,
    string? Phone,
    string GuardianName,
    string? GuardianEmail,
    string? GuardianPhone,
    decimal? PreviousMarks,
    string Status,
    DateTimeOffset SubmittedAt,
    string? DecisionNotes,
    Guid? StudentId);
