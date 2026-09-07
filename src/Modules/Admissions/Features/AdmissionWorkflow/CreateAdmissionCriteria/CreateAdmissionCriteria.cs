using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features;

public interface ICreateAdmissionCriteriaQuery
{
    Task<bool> CriteriaContextIsValidAsync(
        Guid tenantId,
        Guid schoolId,
        Guid branchId,
        Guid academicYearId,
        Guid classId,
        CancellationToken cancellationToken);
}

public sealed class CreateAdmissionCriteriaQuery(IDbConnectionFactory connectionFactory)
    : ICreateAdmissionCriteriaQuery
{
    public async Task<bool> CriteriaContextIsValidAsync(
        Guid tenantId,
        Guid schoolId,
        Guid branchId,
        Guid academicYearId,
        Guid classId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT EXISTS (
                SELECT 1
                FROM academic.class AS class
                INNER JOIN academic.academic_year AS academic_year
                    ON academic_year.branch_id = class.branch_id
                    AND academic_year.tenant_id = class.tenant_id
                INNER JOIN org.campus AS branch
                    ON branch.campus_id = class.branch_id
                    AND branch.tenant_id = class.tenant_id
                INNER JOIN org.campus_education_level AS branch_level
                    ON branch_level.campus_id = class.branch_id
                    AND branch_level.education_level_id = class.education_level_id
                WHERE class.tenant_id = @TenantId
                    AND branch.school_id = @SchoolId
                    AND class.branch_id = @BranchId
                    AND class.class_id = @ClassId
                    AND academic_year.academic_year_id = @AcademicYearId
                    AND class.is_active = TRUE
                    AND academic_year.is_active = TRUE
                    AND branch.is_active = TRUE
            );
            """;

        await using var connection =
            await connectionFactory.OpenConnectionAsync(cancellationToken);

        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new
                {
                    TenantId = tenantId,
                    SchoolId = schoolId,
                    BranchId = branchId,
                    AcademicYearId = academicYearId,
                    ClassId = classId
                },
                cancellationToken: cancellationToken));
    }
}

public interface ICreateAdmissionCriteriaCommand
{
    Task<Guid> CreateCriteriaAsync(
        Guid tenantId,
        CreateAdmissionCriteria.Request request,
        CancellationToken cancellationToken);
}

public sealed class CreateAdmissionCriteriaCommand(IAdmissionsDbContext dbContext)
    : ICreateAdmissionCriteriaCommand
{
    public async Task<Guid> CreateCriteriaAsync(
        Guid tenantId,
        CreateAdmissionCriteria.Request request,
        CancellationToken cancellationToken)
    {
        var criteria = AdmissionCriteriaWriteEntity.Create(tenantId, request);

        await dbContext.AdmissionCriteria.AddAsync(criteria, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return criteria.AdmissionCriteriaId;
    }
}

public sealed class AdmissionCriteriaWriteEntity
{
    public Guid AdmissionCriteriaId { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid SchoolId { get; private set; }
    public Guid BranchId { get; private set; }
    public Guid AcademicYearId { get; private set; }
    public Guid ClassId { get; private set; }
    public decimal MinimumMarks { get; private set; }
    public decimal? EntranceTestMinimum { get; private set; }
    public int? MinimumAge { get; private set; }
    public int? MaximumAge { get; private set; }
    public bool InterviewRequired { get; private set; }
    public string? RequiredDocuments { get; private set; }

    private AdmissionCriteriaWriteEntity()
    {
    }

    public static AdmissionCriteriaWriteEntity Create(
        Guid tenantId,
        CreateAdmissionCriteria.Request request)
    {
        return new AdmissionCriteriaWriteEntity
        {
            AdmissionCriteriaId = Guid.NewGuid(),
            TenantId = tenantId,
            SchoolId = request.SchoolId,
            BranchId = request.BranchId,
            AcademicYearId = request.AcademicYearId,
            ClassId = request.ClassId,
            MinimumMarks = request.MinimumMarks,
            EntranceTestMinimum = request.EntranceTestMinimum,
            MinimumAge = request.MinimumAge,
            MaximumAge = request.MaximumAge,
            InterviewRequired = request.InterviewRequired,
            RequiredDocuments = request.RequiredDocuments?.Trim()
        };
    }
}

public static class CreateAdmissionCriteria
{
    public sealed record Request(
        Guid? TenantId,
        Guid SchoolId,
        Guid BranchId,
        Guid AcademicYearId,
        Guid ClassId,
        decimal MinimumMarks,
        decimal? EntranceTestMinimum,
        int? MinimumAge,
        int? MaximumAge,
        bool InterviewRequired,
        string? RequiredDocuments)
        : IRequest<Result<Response>>;

    public sealed record Response(Guid Id);

    public sealed class Handler(
        ITenantScope tenantScope,
        ICreateAdmissionCriteriaQuery query,
        ICreateAdmissionCriteriaCommand command)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<Response>.Failure(
                    Error.Validation("Tenant context is required."));
            }

            var contextIsValid = await query.CriteriaContextIsValidAsync(
                tenantId.Value,
                request.SchoolId,
                request.BranchId,
                request.AcademicYearId,
                request.ClassId,
                cancellationToken);

            if (!contextIsValid)
            {
                return Result<Response>.Failure(
                    Error.Validation(
                        "School, branch, academic year and class must belong to the same tenant context."));
            }

            var criteriaId = await command.CreateCriteriaAsync(
                tenantId.Value,
                request,
                cancellationToken);

            return Result<Response>.Success(new Response(criteriaId));
        }
    }
}
