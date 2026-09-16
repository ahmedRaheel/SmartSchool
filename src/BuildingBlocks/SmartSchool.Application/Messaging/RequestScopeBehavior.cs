using System.Reflection;
using SmartSchool.Application.Identity;

namespace SmartSchool.Application.Messaging;

/// <summary>Rejects cross-tenant and cross-branch request IDs before feature execution.</summary>
public sealed class RequestScopeBehavior<TRequest, TResponse>(ICurrentUser user)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private static readonly PropertyInfo? Tenant = typeof(TRequest).GetProperty("TenantId");
    private static readonly PropertyInfo? Branch = typeof(TRequest).GetProperty("BranchId")
        ?? typeof(TRequest).GetProperty("CampusId");

    public Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (user.IsAuthenticated && !user.IsSuperAdmin &&
            !(typeof(TRequest).Namespace?.StartsWith("SmartSchool.Modules.Identity", StringComparison.Ordinal) ?? false))
        {
            if (Tenant?.GetValue(request) is Guid tenant && tenant != user.TenantId)
                throw new UnauthorizedAccessException("The requested tenant is outside your account scope.");
            if (user.BranchId is Guid branch && Branch?.GetValue(request) is Guid requestedBranch &&
                requestedBranch != Guid.Empty && branch != requestedBranch)
                throw new UnauthorizedAccessException("The requested branch is outside your account scope.");
        }
        return next();
    }
}
