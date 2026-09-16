using Dapper;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.GradeLevel;

public static class CreateGradeLevel
{
    public sealed record SectionDraft(string Name, int? Capacity, string? RoomNo);
    public sealed record Request(Guid TenantId, Guid CampusId, Guid? AcademicSystemId, string Name,
        int SortOrder = 0, Guid? EducationLevelId = null, Guid? AcademicYearId = null,
        IReadOnlyList<SectionDraft>? Sections = null) : IRequest<Result<Response>>;
    public sealed record Response(Guid TenantId, Guid Id, string Code, string Name, Guid? EducationLevelId);
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty(); RuleFor(x => x.CampusId).NotEmpty(); RuleFor(x => x.EducationLevelId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
            RuleFor(x => x.AcademicYearId).NotEmpty().When(x => x.Sections is { Count: > 0 });
            RuleFor(x => x.Sections).Must(x => x is null || x.Count <= 30).WithMessage("Up to 30 sections are allowed.");
            RuleForEach(x => x.Sections).ChildRules(v =>
            {
                v.RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
                v.RuleFor(x => x.Capacity).GreaterThan(0).LessThanOrEqualTo(200);
                v.RuleFor(x => x.RoomNo).MaximumLength(50);
            });
        }
    }
    public sealed record Campus(Guid Id, Guid? AcademicSystemId);
    public interface ICreateGradeLevelQuery { Task<Campus?> GetCampusAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class CreateGradeLevelQuery(IDbConnectionFactory factory) : ICreateGradeLevelQuery
    {
        public async Task<Campus?> GetCampusAsync(Request request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT c.campus_id AS "Id", c.academic_system_id AS "AcademicSystemId" FROM org.campus c
                JOIN org.campus_education_level level ON level.tenant_id = c.tenant_id AND level.campus_id = c.campus_id
                WHERE c.tenant_id = @TenantId AND c.campus_id = @CampusId AND c.is_active AND level.education_level_id = @EducationLevelId
                    AND (@AcademicYearId IS NULL OR EXISTS (SELECT 1 FROM academic.academic_year y
                        WHERE y.tenant_id = c.tenant_id AND y.campus_id = c.campus_id AND y.academic_year_id = @AcademicYearId AND y.is_active));
                """;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<Campus>(new CommandDefinition(sql, request, cancellationToken: cancellationToken));
        }
    }
    public interface ICreateGradeLevelCommand { Task AddAsync(GradeLevelEntity entity, IReadOnlyList<ClassSectionEntity> sections, CancellationToken cancellationToken); }
    internal sealed class CreateGradeLevelCommand(IOrganizationDbContext db) : ICreateGradeLevelCommand
    {
        public async Task AddAsync(GradeLevelEntity entity, IReadOnlyList<ClassSectionEntity> sections, CancellationToken cancellationToken)
        {
            db.GradeLevels.Add(entity); db.ClassSections.AddRange(sections); await db.SaveChangesAsync(cancellationToken);
        }
    }
    public sealed class Handler(ICreateGradeLevelQuery query, ICreateGradeLevelCommand command, IBusinessNumberGenerator numbers) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var campus = await query.GetCampusAsync(request, cancellationToken);
            if (campus is null) return Result<Response>.Failure(Error.Validation("Choose an education level enabled for this campus and an academic year in the same campus."));
            var code = await numbers.NextAsync("GradeLevel", "GL", request.TenantId, 3, cancellationToken);
            var grade = GradeLevelEntity.Create(request.TenantId, request.CampusId, campus.AcademicSystemId, code,
                request.Name, request.SortOrder, educationLevelId: request.EducationLevelId);
            var sections = new List<ClassSectionEntity>();
            foreach (var draft in request.Sections ?? [])
            {
                var sectionCode = await numbers.NextAsync("Section", "SEC", request.TenantId, 5, cancellationToken);
                sections.Add(ClassSectionEntity.Create(request.TenantId, request.CampusId, request.AcademicYearId!.Value,
                    grade.GradeLevelId, sectionCode, draft.Name, capacity: draft.Capacity, roomNo: draft.RoomNo));
            }
            await command.AddAsync(grade, sections, cancellationToken);
            return Result<Response>.Success(new(grade.TenantId, grade.GradeLevelId, grade.Code, grade.Name, grade.EducationLevelId));
        }
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/academics/grade-level", async (Request request, ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(request.TenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Request, Result<Response>>(request with { TenantId = tenant.Value }, cancellationToken)).ToHttpResult();
        }).WithName("CreateGradeLevel").WithTags("Organization").RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);
        return endpoints;
    }
}
