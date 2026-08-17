using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Contracts;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Department;

public static class GetDepartmentById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<DepartmentResponse>>;

    public sealed class Handler(IDepartmentQuery entityQuery)
        : IRequestHandler<Query, Result<DepartmentResponse>>
    {
        public async Task<Result<DepartmentResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<DepartmentResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Department))));
            }
            return Result<DepartmentResponse>.Success(DepartmentResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "department"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<DepartmentResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetDepartmentById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
