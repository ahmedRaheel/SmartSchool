using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Contracts;
using SmartSchool.Modules.HR.Models;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Employee;

public static class GetEmployeeById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<EmployeeResponse>>;

    public sealed class Handler(IEmployeeQuery entityQuery)
        : IRequestHandler<Query, Result<EmployeeResponse>>
    {
        public async Task<Result<EmployeeResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<EmployeeResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Employee))));
            }
            return Result<EmployeeResponse>.Success(EmployeeResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "employee"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<EmployeeResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetEmployeeById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
