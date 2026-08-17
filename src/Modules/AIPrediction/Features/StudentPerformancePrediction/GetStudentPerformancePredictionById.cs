using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Contracts;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.Modules.AIPrediction.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.StudentPerformancePrediction;

public static class GetStudentPerformancePredictionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<StudentPerformancePredictionResponse>>;

    public sealed class Handler(IStudentPerformancePredictionQuery entityQuery)
        : IRequestHandler<Query, Result<StudentPerformancePredictionResponse>>
    {
        public async Task<Result<StudentPerformancePredictionResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<StudentPerformancePredictionResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(StudentPerformancePrediction))));
            }
            return Result<StudentPerformancePredictionResponse>.Success(StudentPerformancePredictionResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student-performance-prediction"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<StudentPerformancePredictionResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentPerformancePredictionById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
