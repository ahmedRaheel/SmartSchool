using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Campus;

public static class GetBranchPolicy
{
    public sealed record Request(Guid? TenantId, Guid BranchId) : IRequest<Result<Response>>;
    public sealed record EducationLevelResponse(Guid Id, string Code, string Name);
    public sealed record Response(Guid BranchGenderTypeId, string GenderCode, IReadOnlyCollection<EducationLevelResponse> EducationLevels);
    private sealed record Header(Guid BranchGenderTypeId, string GenderCode);
    public interface IGetBranchPolicyQuery { Task<Response?> ExecuteAsync(Guid tenantId, Guid branchId, CancellationToken cancellationToken); }
    internal sealed class GetBranchPolicyQuery(IDbConnectionFactory connectionFactory) : IGetBranchPolicyQuery
    {
        public async Task<Response?> ExecuteAsync(Guid tenantId, Guid branchId, CancellationToken cancellationToken)
        {
            const string headerSql = """SELECT c.branch_gender_type_id AS "BranchGenderTypeId", g.code AS "GenderCode" FROM org.campus c INNER JOIN reference.branch_gender_type g ON g.branch_gender_type_id = c.branch_gender_type_id WHERE c.tenant_id = @TenantId AND c.campus_id = @BranchId AND c.is_active = TRUE;""";
            const string levelsSql = """SELECT l.education_level_id AS "Id", l.code AS "Code", l.name AS "Name" FROM org.campus_education_level b INNER JOIN reference.education_level l ON l.education_level_id = b.education_level_id WHERE b.tenant_id = @TenantId AND b.campus_id = @BranchId AND l.is_active = TRUE ORDER BY l.sort_order, l.name;""";
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var header = await connection.QuerySingleOrDefaultAsync<Header>(new CommandDefinition(headerSql, new { TenantId = tenantId, BranchId = branchId }, cancellationToken: cancellationToken));
            if (header is null) return null;
            var levels = (await connection.QueryAsync<EducationLevelResponse>(new CommandDefinition(levelsSql, new { TenantId = tenantId, BranchId = branchId }, cancellationToken: cancellationToken))).AsList();
            return new Response(header.BranchGenderTypeId, header.GenderCode, levels);
        }
    }
    public sealed class Handler(ITenantScope tenantScope, IGetBranchPolicyQuery query) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken) { var tenantId = tenantScope.Resolve(request.TenantId); if (!tenantId.HasValue) return Result<Response>.Failure(Error.Validation("Tenant context is required.")); var response = await query.ExecuteAsync(tenantId.Value, request.BranchId, cancellationToken); return response is null ? Result<Response>.Failure(Error.NotFound("Branch policy was not found.")) : Result<Response>.Success(response); }
    }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapGet("/api/organization/branches/{branchId:guid}/policy", async (Guid branchId, Guid? tenantId, IMediator mediator, CancellationToken cancellationToken) => (await mediator.SendAsync<Request, Result<Response>>(new Request(tenantId, branchId), cancellationToken)).ToHttpResult()).WithTags(ModuleConstants.Name).RequireAuthorization();
}
