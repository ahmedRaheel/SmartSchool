using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.ClassSection;

public static class GetClassSectionsByParentId
{
    public sealed record Response(Guid Id, string Code, string Name, Guid CampusId, Guid AcademicYearId, Guid GradeLevelId);
    public sealed record Query(Guid TenantId, Guid? CampusId, Guid? AcademicYearId, Guid? GradeLevelId) : IRequest<Result<IReadOnlyCollection<Response>>>;
    public interface IGetClassSectionsByParentIdQuery { Task<IReadOnlyCollection<Response>> GetAsync(Guid tenantId, Guid? campusId, Guid? academicYearId, Guid? gradeLevelId, CancellationToken cancellationToken); }
    internal sealed class GetClassSectionsByParentIdQuery(IDbConnectionFactory connectionFactory) : IGetClassSectionsByParentIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(Guid tenantId, Guid? campusId, Guid? academicYearId, Guid? gradeLevelId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT class_section_id AS "Id", code AS "Code", name AS "Name", campus_id AS "CampusId", academic_year_id AS "AcademicYearId", grade_level_id AS "GradeLevelId"
                FROM academic.class_section
                WHERE tenant_id=@TenantId AND is_active=TRUE
                  AND (@CampusId IS NULL OR campus_id=@CampusId)
                  AND (@AcademicYearId IS NULL OR academic_year_id=@AcademicYearId)
                  AND (@GradeLevelId IS NULL OR grade_level_id=@GradeLevelId)
                ORDER BY name;
                """;
            await using var connection=await connectionFactory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<Response>(new CommandDefinition(sql,new { TenantId=tenantId, CampusId=campusId, AcademicYearId=academicYearId, GradeLevelId=gradeLevelId },cancellationToken:cancellationToken))).AsList();
        }
    }
    public sealed class Handler(IGetClassSectionsByParentIdQuery query) : IRequestHandler<Query, Result<IReadOnlyCollection<Response>>>
    {
        public async Task<Result<IReadOnlyCollection<Response>>> HandleAsync(Query request,CancellationToken cancellationToken) => Result<IReadOnlyCollection<Response>>.Success(await query.GetAsync(request.TenantId,request.CampusId,request.AcademicYearId,request.GradeLevelId,cancellationToken));
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/academics/class-sections/by-parent", async (Guid tenantId, Guid? campusId, Guid? academicYearId, Guid? gradeLevelId, IMediator mediator, CancellationToken cancellationToken) => (await mediator.SendAsync<Query,Result<IReadOnlyCollection<Response>>>(new Query(tenantId,campusId,academicYearId,gradeLevelId),cancellationToken)).ToHttpResult())
            .WithName("GetClassSectionsByParentId").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantTeacher);
        return endpoints;
    }
}
