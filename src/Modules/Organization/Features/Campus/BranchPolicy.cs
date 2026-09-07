using Dapper;
using SmartSchool.Application.Persistence;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;

using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Campus;

public static class BranchPolicyEndpoints
{
    public sealed record LookupResponse(Guid Id, string Code, string Name);
    public sealed record PolicyResponse(Guid BranchGenderTypeId, string GenderCode, IReadOnlyCollection<LookupResponse> EducationLevels);

    public sealed record GetLookupsRequest(bool GenderTypes) : IRequest<Result<IReadOnlyCollection<LookupResponse>>>;
    public sealed class GetLookupsHandler(BranchPolicyBranchPolicyQuery query) : IRequestHandler<GetLookupsRequest, Result<IReadOnlyCollection<LookupResponse>>>
    {
        public async Task<Result<IReadOnlyCollection<LookupResponse>>> HandleAsync(GetLookupsRequest request, CancellationToken cancellationToken)
        {
            var items = request.GenderTypes
                ? await query.GetGenderTypesAsync(cancellationToken)
                : await query.GetEducationLevelsAsync(cancellationToken);
            return Result<IReadOnlyCollection<LookupResponse>>.Success(items.Select(x => new LookupResponse(x.Id, x.Code, x.Name)).ToArray());
        }
    }

    public sealed record GetPolicyRequest(Guid? TenantId, Guid BranchId) : IRequest<Result<PolicyResponse>>;
    public sealed class GetPolicyHandler(ITenantScope tenantScope, BranchPolicyBranchPolicyQuery query) : IRequestHandler<GetPolicyRequest, Result<PolicyResponse>>
    {
        public async Task<Result<PolicyResponse>> HandleAsync(GetPolicyRequest request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue) return Result<PolicyResponse>.Failure(Error.Validation("Tenant context is required."));
            var policy = await query.GetBranchPolicyAsync(tenantId.Value, request.BranchId, cancellationToken);
            if (policy is null) return Result<PolicyResponse>.Failure(Error.NotFound("Branch policy was not found."));
            return Result<PolicyResponse>.Success(new PolicyResponse(policy.BranchGenderTypeId, policy.GenderCode, policy.EducationLevels.Select(x => new LookupResponse(x.Id, x.Code, x.Name)).ToArray()));
        }
    }

    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/organization/lookups/branch-gender-types", async (IMediator mediator, CancellationToken ct) =>
            (await mediator.SendAsync<GetLookupsRequest, Result<IReadOnlyCollection<LookupResponse>>>(new GetLookupsRequest(true), ct)).ToHttpResult())
            .WithTags(ModuleConstants.Name).RequireAuthorization();
        endpoints.MapGet("/api/organization/lookups/education-levels", async (IMediator mediator, CancellationToken ct) =>
            (await mediator.SendAsync<GetLookupsRequest, Result<IReadOnlyCollection<LookupResponse>>>(new GetLookupsRequest(false), ct)).ToHttpResult())
            .WithTags(ModuleConstants.Name).RequireAuthorization();
        endpoints.MapGet("/api/organization/branches/{branchId:guid}/policy", async (Guid branchId, Guid? tenantId, IMediator mediator, CancellationToken ct) =>
            (await mediator.SendAsync<GetPolicyRequest, Result<PolicyResponse>>(new GetPolicyRequest(tenantId, branchId), ct)).ToHttpResult())
            .WithTags(ModuleConstants.Name).RequireAuthorization();
    }
}

/// <summary>
/// Feature-owned data access for BranchPolicy. Do not share across slices.
/// </summary>
public sealed class BranchPolicyBranchPolicyQuery(IDbConnectionFactory connectionFactory)
{
    public Task<IReadOnlyCollection<LookupItem>> GetGenderTypesAsync(CancellationToken cancellationToken) =>
        GetLookupsAsync("SELECT branch_gender_type_id AS Id, code AS Code, name AS Name FROM reference.branch_gender_type WHERE is_active = TRUE ORDER BY sort_order, name;", cancellationToken);


    public Task<IReadOnlyCollection<LookupItem>> GetEducationLevelsAsync(CancellationToken cancellationToken) =>
        GetLookupsAsync("SELECT education_level_id AS Id, code AS Code, name AS Name FROM reference.education_level WHERE is_active = TRUE ORDER BY sort_order, name;", cancellationToken);


    private async Task<IReadOnlyCollection<LookupItem>> GetLookupsAsync(string sql, CancellationToken cancellationToken)
    {
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        var items = await connection.QueryAsync<LookupItem>(
            new CommandDefinition(sql, cancellationToken: cancellationToken)).ConfigureAwait(false);
        return items.AsList();
    }

    public async Task<BranchPolicy?> GetBranchPolicyAsync(Guid tenantId, Guid branchId, CancellationToken cancellationToken)
    {
        const string headerSql = """
            SELECT c.branch_gender_type_id AS BranchGenderTypeId, g.code AS GenderCode
            FROM org.campus c
            INNER JOIN reference.branch_gender_type g ON g.branch_gender_type_id = c.branch_gender_type_id
            WHERE c.tenant_id = @TenantId AND c.campus_id = @BranchId AND c.is_active = TRUE;
            """;
        const string levelsSql = """
            SELECT l.education_level_id AS Id, l.code AS Code, l.name AS Name
            FROM org.campus_education_level b
            INNER JOIN reference.education_level l ON l.education_level_id = b.education_level_id
            WHERE b.tenant_id = @TenantId AND b.campus_id = @BranchId AND l.is_active = TRUE
            ORDER BY l.sort_order, l.name;
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var (genderTypeId, genderCode) = await connection.QuerySingleOrDefaultAsync<(Guid BranchGenderTypeId, string GenderCode)>(new CommandDefinition(headerSql, new
        {
            TenantId = tenantId,
            BranchId = branchId
        }, cancellationToken: cancellationToken));
        if (genderTypeId == Guid.Empty) return null;
        var levels = (await connection.QueryAsync<LookupItem>(new CommandDefinition(levelsSql, new
        {
            TenantId = tenantId,
            BranchId = branchId
        }, cancellationToken: cancellationToken))).AsList();
        return new BranchPolicy(genderTypeId, genderCode, levels);
    }
}
