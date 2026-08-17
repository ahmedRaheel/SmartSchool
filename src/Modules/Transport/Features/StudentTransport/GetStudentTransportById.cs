using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Transport.Contracts;
using SmartSchool.Modules.Transport.Models;
using SmartSchool.Modules.Transport.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Transport.Features.StudentTransport;

public static class GetStudentTransportById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<StudentTransportResponse>>;

    public sealed class Handler(IStudentTransportQuery entityQuery)
        : IRequestHandler<Query, Result<StudentTransportResponse>>
    {
        public async Task<Result<StudentTransportResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<StudentTransportResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(StudentTransport))));
            }
            return Result<StudentTransportResponse>.Success(StudentTransportResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student-transport"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<StudentTransportResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentTransportById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
