using SmartSchool.Application.Http;
using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features;

public interface IGetAdmissionCriteriaQuery
{
    Task<IReadOnlyList<AdmissionCriteriaDto>> GetCriteriaAsync(
        Guid tenantId,
        CancellationToken cancellationToken);
}

public sealed class GetAdmissionCriteriaQuery(IDbConnectionFactory connectionFactory)
    : IGetAdmissionCriteriaQuery
{
    public async Task<IReadOnlyList<AdmissionCriteriaDto>> GetCriteriaAsync(
        Guid tenantId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                admission_criteria_id AS Id,
                school_id AS SchoolId,
                branch_id AS BranchId,
                academic_year_id AS AcademicYearId,
                class_id AS ClassId,
                minimum_marks AS MinimumMarks,
                entrance_test_minimum AS EntranceTestMinimum,
                minimum_age AS MinimumAge,
                maximum_age AS MaximumAge,
                interview_required AS InterviewRequired,
                required_documents AS RequiredDocuments,
                status AS Status
            FROM admission.admission_criteria
            WHERE tenant_id = @TenantId
            ORDER BY created_at DESC;
            """;

        await using var connection =
            await connectionFactory.OpenConnectionAsync(cancellationToken);

        var criteria = await connection.QueryAsync<AdmissionCriteriaDto>(
            new CommandDefinition(
                sql,
                new { TenantId = tenantId },
                cancellationToken: cancellationToken));

        return criteria.AsList();
    }
}

public static class GetAdmissionCriteria
{
    public sealed record Request(Guid? TenantId)
        : IRequest<Result<IReadOnlyList<AdmissionCriteriaDto>>>;

    public sealed class Handler(
        ITenantScope tenantScope,
        IGetAdmissionCriteriaQuery query)
        : IRequestHandler<Request, Result<IReadOnlyList<AdmissionCriteriaDto>>>
    {
        public async Task<Result<IReadOnlyList<AdmissionCriteriaDto>>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<IReadOnlyList<AdmissionCriteriaDto>>.Failure(
                    Error.Validation("Tenant context is required."));
            }

            var criteria = await query.GetCriteriaAsync(
                tenantId.Value,
                cancellationToken);

            return Result<IReadOnlyList<AdmissionCriteriaDto>>.Success(criteria);
        }
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/admissions/criteria", async (Guid? tenantId, Guid? branchId, Guid? classId, IMediator mediator, CancellationToken cancellationToken) =>
            (await mediator.SendAsync<Request, Result<IReadOnlyList<AdmissionCriteriaDto>>>(new Request(tenantId), cancellationToken)).ToHttpResult())
            .WithName("GetAdmissionCriteria").WithTags("Admission Criteria").RequireAuthorization();
    }
}

public sealed record AdmissionCriteriaDto(
    Guid Id,
    Guid SchoolId,
    Guid BranchId,
    Guid AcademicYearId,
    Guid ClassId,
    decimal MinimumMarks,
    decimal? EntranceTestMinimum,
    int? MinimumAge,
    int? MaximumAge,
    bool InterviewRequired,
    string? RequiredDocuments,
    string Status);
