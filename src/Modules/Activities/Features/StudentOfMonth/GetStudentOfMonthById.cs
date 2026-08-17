using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Activities.Contracts;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.Modules.Activities.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Activities.Features.StudentOfMonth;

public static class GetStudentOfMonthById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<StudentOfMonthResponse>>;

    public sealed class Handler(IStudentOfMonthQuery entityQuery)
        : IRequestHandler<Query, Result<StudentOfMonthResponse>>
    {
        public async Task<Result<StudentOfMonthResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<StudentOfMonthResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(StudentOfMonth))));
            }
            return Result<StudentOfMonthResponse>.Success(StudentOfMonthResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student-of-month"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<StudentOfMonthResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentOfMonthById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
