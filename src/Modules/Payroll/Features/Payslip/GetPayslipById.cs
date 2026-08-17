using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Payroll.Contracts;
using SmartSchool.Modules.Payroll.Models;
using SmartSchool.Modules.Payroll.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Payroll.Features.Payslip;

public static class GetPayslipById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<PayslipResponse>>;

    public sealed class Handler(IPayslipQuery entityQuery)
        : IRequestHandler<Query, Result<PayslipResponse>>
    {
        public async Task<Result<PayslipResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<PayslipResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Payslip))));
            }
            return Result<PayslipResponse>.Success(PayslipResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "payslip"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<PayslipResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetPayslipById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
