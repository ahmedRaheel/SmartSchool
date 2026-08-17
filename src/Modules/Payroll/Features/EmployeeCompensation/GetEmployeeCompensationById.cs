using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Payroll.Contracts;
using SmartSchool.Modules.Payroll.Models;
using SmartSchool.Modules.Payroll.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Payroll.Features.EmployeeCompensation;

public static class GetEmployeeCompensationById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<EmployeeCompensationResponse>>;

    public sealed class Handler(IEmployeeCompensationQuery entityQuery)
        : IRequestHandler<Query, Result<EmployeeCompensationResponse>>
    {
        public async Task<Result<EmployeeCompensationResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<EmployeeCompensationResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(EmployeeCompensation))));
            }
            return Result<EmployeeCompensationResponse>.Success(EmployeeCompensationResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "employee-compensation"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<EmployeeCompensationResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetEmployeeCompensationById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
