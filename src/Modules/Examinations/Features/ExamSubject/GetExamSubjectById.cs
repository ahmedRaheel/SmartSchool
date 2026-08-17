using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Examinations.Contracts;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.Modules.Examinations.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Examinations.Features.ExamSubject;

public static class GetExamSubjectById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ExamSubjectResponse>>;

    public sealed class Handler(IExamSubjectQuery entityQuery)
        : IRequestHandler<Query, Result<ExamSubjectResponse>>
    {
        public async Task<Result<ExamSubjectResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ExamSubjectResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(ExamSubject))));
            }
            return Result<ExamSubjectResponse>.Success(ExamSubjectResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "exam-subject"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ExamSubjectResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetExamSubjectById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
