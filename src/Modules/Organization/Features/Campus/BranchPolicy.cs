using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Campus;

public static class BranchPolicyEndpoints
{
    public sealed record LookupResponse(Guid Id, string Code, string Name);
    public sealed record PolicyResponse(Guid BranchGenderTypeId, string GenderCode, IReadOnlyCollection<LookupResponse> EducationLevels);

    public sealed record GetLookupsRequest(bool GenderTypes) : IRequest<Result<IReadOnlyCollection<LookupResponse>>>;
    public sealed class GetLookupsHandler(IBranchPolicyQuery query) : IRequestHandler<GetLookupsRequest, Result<IReadOnlyCollection<LookupResponse>>>
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
    public sealed class GetPolicyHandler(ITenantScope tenantScope, IBranchPolicyQuery query) : IRequestHandler<GetPolicyRequest, Result<PolicyResponse>>
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
