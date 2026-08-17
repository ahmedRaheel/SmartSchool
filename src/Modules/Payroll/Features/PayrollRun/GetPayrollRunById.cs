using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Payroll.Contracts;
using SmartSchool.Modules.Payroll.Models;
using SmartSchool.Modules.Payroll.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Payroll.Features.PayrollRun;

public static class GetPayrollRunById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<PayrollRunResponse>>;

    public sealed class Handler(IPayrollRunQuery entityQuery)
        : IRequestHandler<Query, Result<PayrollRunResponse>>
    {
        public async Task<Result<PayrollRunResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<PayrollRunResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(PayrollRun))));
            }
            return Result<PayrollRunResponse>.Success(PayrollRunResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "payroll-run"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<PayrollRunResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetPayrollRunById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
