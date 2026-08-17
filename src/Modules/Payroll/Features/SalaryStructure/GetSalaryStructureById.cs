using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Payroll.Contracts;
using SmartSchool.Modules.Payroll.Models;
using SmartSchool.Modules.Payroll.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Payroll.Features.SalaryStructure;

public static class GetSalaryStructureById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<SalaryStructureResponse>>;

    public sealed class Handler(ISalaryStructureQuery entityQuery)
        : IRequestHandler<Query, Result<SalaryStructureResponse>>
    {
        public async Task<Result<SalaryStructureResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<SalaryStructureResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(SalaryStructure))));
            }
            return Result<SalaryStructureResponse>.Success(SalaryStructureResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "salary-structure"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<SalaryStructureResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetSalaryStructureById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
