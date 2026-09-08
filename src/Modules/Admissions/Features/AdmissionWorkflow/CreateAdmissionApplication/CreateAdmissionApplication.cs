using SmartSchool.Application.Http;
using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features;

public interface ICreateAdmissionApplicationQuery
{
    Task<bool> BranchBelongsToSchoolAsync(
        Guid tenantId,
        Guid schoolId,
        Guid branchId,
        CancellationToken cancellationToken);

    Task<string?> GetBranchGenderPolicyAsync(
        Guid tenantId,
        Guid branchId,
        CancellationToken cancellationToken);

    Task<bool> ClassIsEligibleForBranchAsync(
        Guid tenantId,
        Guid branchId,
        Guid classId,
        CancellationToken cancellationToken);

    Task<bool> AcademicYearBelongsToBranchAsync(
        Guid tenantId,
        Guid branchId,
        Guid academicYearId,
        CancellationToken cancellationToken);
}

public sealed class CreateAdmissionApplicationQuery(IDbConnectionFactory connectionFactory)
    : ICreateAdmissionApplicationQuery
{
    public Task<bool> BranchBelongsToSchoolAsync(
        Guid tenantId,
        Guid schoolId,
        Guid branchId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT EXISTS (
                SELECT 1
                FROM org.campus
                WHERE tenant_id = @TenantId
                    AND school_id = @SchoolId
                    AND campus_id = @BranchId
                    AND is_active = TRUE
            );
            """;

        return ExistsAsync(
            sql,
            new { TenantId = tenantId, SchoolId = schoolId, BranchId = branchId },
            cancellationToken);
    }

    public async Task<string?> GetBranchGenderPolicyAsync(
        Guid tenantId,
        Guid branchId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT gender_type.code
            FROM org.campus AS branch
            INNER JOIN reference.branch_gender_type AS gender_type
                ON gender_type.branch_gender_type_id = branch.branch_gender_type_id
            WHERE branch.tenant_id = @TenantId
                AND branch.campus_id = @BranchId
                AND branch.is_active = TRUE;
            """;

        await using var connection =
            await connectionFactory.OpenConnectionAsync(cancellationToken);

        return await connection.ExecuteScalarAsync<string?>(
            new CommandDefinition(
                sql,
                new { TenantId = tenantId, BranchId = branchId },
                cancellationToken: cancellationToken));
    }

    public Task<bool> ClassIsEligibleForBranchAsync(
        Guid tenantId,
        Guid branchId,
        Guid classId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT EXISTS (
                SELECT 1
                FROM academic.class AS class
                INNER JOIN org.campus_education_level AS branch_level
                    ON branch_level.campus_id = class.branch_id
                    AND branch_level.education_level_id = class.education_level_id
                WHERE class.tenant_id = @TenantId
                    AND class.branch_id = @BranchId
                    AND class.class_id = @ClassId
                    AND class.is_active = TRUE
            );
            """;

        return ExistsAsync(
            sql,
            new { TenantId = tenantId, BranchId = branchId, ClassId = classId },
            cancellationToken);
    }

    public Task<bool> AcademicYearBelongsToBranchAsync(
        Guid tenantId,
        Guid branchId,
        Guid academicYearId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT EXISTS (
                SELECT 1
                FROM academic.academic_year
                WHERE tenant_id = @TenantId
                    AND branch_id = @BranchId
                    AND academic_year_id = @AcademicYearId
                    AND is_active = TRUE
            );
            """;

        return ExistsAsync(
            sql,
            new
            {
                TenantId = tenantId,
                BranchId = branchId,
                AcademicYearId = academicYearId
            },
            cancellationToken);
    }

    private async Task<bool> ExistsAsync(
        string sql,
        object parameters,
        CancellationToken cancellationToken)
    {
        await using var connection =
            await connectionFactory.OpenConnectionAsync(cancellationToken);

        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                parameters,
                cancellationToken: cancellationToken));
    }
}

public interface ICreateAdmissionApplicationCommand
{
    Task<Guid> CreateApplicationAsync(
        Guid tenantId,
        CreateAdmissionApplication.Request request,
        CancellationToken cancellationToken);
}

public sealed class CreateAdmissionApplicationCommand(IAdmissionsDbContext dbContext)
    : ICreateAdmissionApplicationCommand
{
    public async Task<Guid> CreateApplicationAsync(
        Guid tenantId,
        CreateAdmissionApplication.Request request,
        CancellationToken cancellationToken)
    {
        var entity = AdmissionApplicationWriteEntity.Create(tenantId, request);

        await dbContext.AdmissionApplications.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return entity.ApplicationId;
    }
}

public static class CreateAdmissionApplication
{
    public sealed record Request(
        Guid? TenantId,
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
        string? Address,
        string GuardianName,
        string? GuardianCnic,
        string? GuardianEmail,
        string? GuardianPhone,
        string? Relationship,
        string? PreviousSchool,
        decimal? PreviousMarks)
        : IRequest<Result<Response>>;

    public sealed record Response(Guid Id, string Status);

    public sealed class Handler(
        ITenantScope tenantScope,
        ICreateAdmissionApplicationQuery query,
        ICreateAdmissionApplicationCommand command)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            var branchIsValid = await query.BranchBelongsToSchoolAsync(
                tenantId.Value,
                request.SchoolId,
                request.BranchId,
                cancellationToken);

            if (!branchIsValid)
            {
                return Result<Response>.Failure(
                    Error.Validation("Selected branch does not belong to the selected school."));
            }

            if (string.IsNullOrWhiteSpace(request.Gender))
            {
                return Result<Response>.Failure(Error.Validation("Applicant gender is required."));
            }

            var branchGenderPolicy = await query.GetBranchGenderPolicyAsync(
                tenantId.Value,
                request.BranchId,
                cancellationToken);

            if (!GenderIsAllowed(branchGenderPolicy, request.Gender))
            {
                return Result<Response>.Failure(
                    Error.Validation("Applicant gender is not eligible for the selected branch."));
            }

            if (request.ClassId.HasValue)
            {
                var classIsEligible = await query.ClassIsEligibleForBranchAsync(
                    tenantId.Value,
                    request.BranchId,
                    request.ClassId.Value,
                    cancellationToken);

                if (!classIsEligible)
                {
                    return Result<Response>.Failure(
                        Error.Validation("The selected class is not available for this branch education level."));
                }
            }

            if (request.AcademicYearId.HasValue)
            {
                var academicYearIsValid = await query.AcademicYearBelongsToBranchAsync(
                    tenantId.Value,
                    request.BranchId,
                    request.AcademicYearId.Value,
                    cancellationToken);

                if (!academicYearIsValid)
                {
                    return Result<Response>.Failure(
                        Error.Validation("Academic year is not available for the selected branch."));
                }
            }

            var applicationId = await command.CreateApplicationAsync(
                tenantId.Value,
                request,
                cancellationToken);

            return Result<Response>.Success(
                new Response(
                    applicationId,
                    AdmissionApplicationStatus.SubmittedApplication.ToDatabaseValue()));
        }

        private static bool GenderIsAllowed(string? branchPolicy, string applicantGender)
        {
            if (string.Equals(branchPolicy, "CO_EDUCATION", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(branchPolicy, "BOYS_ONLY", StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(applicantGender, "MALE", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(applicantGender, "BOY", StringComparison.OrdinalIgnoreCase);
            }

            if (string.Equals(branchPolicy, "GIRLS_ONLY", StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(applicantGender, "FEMALE", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(applicantGender, "GIRL", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/admissions/workflow/applications", async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
            (await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken)).ToHttpResult())
            .WithName("CreateAdmissionApplication").WithTags("Admissions").RequireAuthorization();
    }
}
