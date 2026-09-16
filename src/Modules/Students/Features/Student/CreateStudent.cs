using SmartSchool.Application.Identity;
using SmartSchool.Modules.Students.Persistence;
using Dapper;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Student;

public static class CreateStudent
{
    public sealed record Response(
        Guid TenantId,
        Guid Id,
        Guid? UserId,
        string? StudentNumber,
        string FirstName,
        string? LastName,
        DateOnly? DateOfBirth,
        string? Gender,
        byte[]? Photo,
        string? PhotoContentType,
        string? PhotoFileName,
        DateOnly? AdmissionDate,
        string Status);

    public sealed record Request(
        Guid? TenantId,
        Guid SchoolId,
        Guid BranchId,
        Guid AcademicYearId,
        Guid ClassSectionId,
        Guid? UserId,
        string FirstName,
        string? LastName,
        DateOnly? DateOfBirth,
        string? Gender,
        byte[]? Photo,
        string? PhotoContentType,
        string? PhotoFileName,
        DateOnly? AdmissionDate) : IRequest<Result<Response>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.SchoolId).NotEmpty();
            RuleFor(x => x.BranchId).NotEmpty();
            RuleFor(x => x.AcademicYearId).NotEmpty();
            RuleFor(x => x.ClassSectionId).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        }
    }

    public interface ICreateStudentCommand
    {
        Task AddAsync(
                StudentEntity entity,
                AdmissionPlacementEntity placement,
                CancellationToken cancellationToken);

    }

    internal sealed class CreateStudentCommand(IStudentsDbContext dbContext) : ICreateStudentCommand
    {
        public async Task AddAsync(
            StudentEntity entity,
            AdmissionPlacementEntity placement,
            CancellationToken cancellationToken)
        {
            await dbContext.Students.AddAsync(entity, cancellationToken);
            await dbContext.AdmissionPlacements.AddAsync(placement, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

    }

    public sealed record AdmissionPolicy(
        string GenderCode,
        bool IsEducationLevelAllowed);

    public interface ICreateStudentQuery
    {
        Task<AdmissionPolicy?> GetAdmissionPolicyAsync(
            Guid tenantId,
            Guid schoolId,
            Guid branchId,
            Guid academicYearId,
            Guid classSectionId,
            CancellationToken cancellationToken);
    }

    internal sealed class CreateStudentQuery(IDbConnectionFactory connectionFactory)
        : ICreateStudentQuery
    {
        public async Task<AdmissionPolicy?> GetAdmissionPolicyAsync(
            Guid tenantId,
            Guid schoolId,
            Guid branchId,
            Guid academicYearId,
            Guid classSectionId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    gender_type.code AS "GenderCode",
                    EXISTS (
                        SELECT 1
                        FROM academic.class_section AS section
                        INNER JOIN academic.grade_level AS class
                            ON class.grade_level_id = section.grade_level_id
                           AND class.tenant_id = section.tenant_id
                        INNER JOIN org.campus_education_level AS enabled_level
                            ON enabled_level.tenant_id = class.tenant_id
                           AND enabled_level.campus_id = class.campus_id
                           AND enabled_level.education_level_id = class.education_level_id
                        WHERE section.tenant_id = @TenantId
                          AND section.class_section_id = @ClassSectionId
                          AND section.academic_year_id = @AcademicYearId
                          AND class.campus_id = @BranchId
                          AND class.is_active = TRUE
                          AND section.is_active = TRUE
                    ) AS "IsEducationLevelAllowed"
                FROM org.campus AS branch
                INNER JOIN reference.branch_gender_type AS gender_type
                    ON gender_type.branch_gender_type_id = branch.branch_gender_type_id
                WHERE branch.tenant_id = @TenantId
                  AND branch.school_id = @SchoolId
                  AND branch.campus_id = @BranchId
                  AND branch.is_active = TRUE;
                """;

            await using var connection =
                await connectionFactory.OpenConnectionAsync(cancellationToken);

            return await connection.QuerySingleOrDefaultAsync<AdmissionPolicy>(
                new CommandDefinition(
                    sql,
                    new
                    {
                        TenantId = tenantId,
                        SchoolId = schoolId,
                        BranchId = branchId,
                        AcademicYearId = academicYearId,
                        ClassSectionId = classSectionId
                    },
                    cancellationToken: cancellationToken));
        }
    }

    public sealed class Handler(
        ICreateStudentCommand command,
        ICreateStudentQuery query)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var tenantId = request.TenantId!.Value;
            var policy = await query.GetAdmissionPolicyAsync(
                tenantId,
                request.SchoolId,
                request.BranchId,
                request.AcademicYearId,
                request.ClassSectionId,
                cancellationToken);

            if (policy is null)
            {
                return Result<Response>.Failure(
                    Error.Validation("The selected school, branch, academic year or class section is invalid."));
            }

            if (!policy.IsEducationLevelAllowed)
            {
                return Result<Response>.Failure(
                    Error.Validation("The selected class education level is not enabled for this branch."));
            }

            if (string.Equals(policy.GenderCode, "BOYS_ONLY", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(request.Gender, "Male", StringComparison.OrdinalIgnoreCase))
            {
                return Result<Response>.Failure(
                    Error.Validation("This branch accepts male students only."));
            }

            if (string.Equals(policy.GenderCode, "GIRLS_ONLY", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(request.Gender, "Female", StringComparison.OrdinalIgnoreCase))
            {
                return Result<Response>.Failure(
                    Error.Validation("This branch accepts female students only."));
            }

            var entity = StudentEntity.Create(
                tenantId,
                null,
                request.SchoolId,
                request.BranchId,
                null,
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.Gender,
                request.Photo,
                request.PhotoContentType,
                request.PhotoFileName,
                request.AdmissionDate,
                LifecycleStatuses.PendingApproval);

            var placement = AdmissionPlacementEntity.Create(
                tenantId,
                entity.StudentId,
                request.AcademicYearId,
                request.ClassSectionId);

            await command.AddAsync(entity, placement, cancellationToken);
            return Result<Response>.Success(MapResponse(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "student"),
                async (Request request,ITenantScope tenantScope, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var tenantId = tenantScope.Resolve(request.TenantId);
                    if (!tenantId.HasValue) return Results.BadRequest(new { message = "Tenant is required for SuperAdmin." });
                    request = request with { TenantId = tenantId.Value };
                    var result = await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateStudent").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantStudent);
        return endpoints;
    }

    private static Response MapResponse(StudentEntity entity)
    {
        return new Response(
            entity.TenantId,
            entity.StudentId,
            entity.UserId,
            entity.StudentNumber,
            entity.FirstName,
            entity.LastName,
            entity.DateOfBirth,
            entity.Gender,
            entity.Photo,
            entity.PhotoContentType,
            entity.PhotoFileName,
            entity.AdmissionDate,
            entity.Status);
    }
}
