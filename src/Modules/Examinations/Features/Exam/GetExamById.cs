using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Examinations.Contracts;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.Modules.Examinations.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Examinations.Features.Exam;

public static class GetExamById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ExamResponse>>;

    public sealed class Handler(IExamQuery entityQuery)
        : IRequestHandler<Query, Result<ExamResponse>>
    {
        public async Task<Result<ExamResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ExamResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Exam))));
            }
            return Result<ExamResponse>.Success(ExamResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "exam"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ExamResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetExamById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
