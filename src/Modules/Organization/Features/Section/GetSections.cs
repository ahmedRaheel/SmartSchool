using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Section;

public static class GetSections
{
    public sealed record Response(Guid Id, string Code, string Name);
    public sealed record Query(Guid TenantId) : IRequest<Result<IReadOnlyCollection<Response>>>;
    public interface IGetSectionsQuery { Task<IReadOnlyCollection<Response>> GetAsync(Guid tenantId, CancellationToken cancellationToken); }
    internal sealed class GetSectionsQuery(IDbConnectionFactory connectionFactory) : IGetSectionsQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(Guid tenantId,CancellationToken cancellationToken)
        {
            const string sql="""SELECT section_id AS "Id", code AS "Code", name AS "Name" FROM academic.section WHERE tenant_id=@TenantId AND is_active=TRUE ORDER BY name;""";
            await using var connection=await connectionFactory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<Response>(new CommandDefinition(sql,new { TenantId=tenantId },cancellationToken:cancellationToken))).AsList();
        }
    }
    public sealed class Handler(IGetSectionsQuery query) : IRequestHandler<Query,Result<IReadOnlyCollection<Response>>>
    {
        public async Task<Result<IReadOnlyCollection<Response>>> HandleAsync(Query request,CancellationToken cancellationToken)=>Result<IReadOnlyCollection<Response>>.Success(await query.GetAsync(request.TenantId,cancellationToken));
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/academics/sections",async (Guid tenantId,IMediator mediator,CancellationToken cancellationToken)=>(await mediator.SendAsync<Query,Result<IReadOnlyCollection<Response>>>(new Query(tenantId),cancellationToken)).ToHttpResult())
            .WithName("GetSections").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantTeacher);
        return endpoints;
    }
}
