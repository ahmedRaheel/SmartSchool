using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Finance.Contracts;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.Modules.Finance.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Finance.Features.StudentFee;

public static class GetStudentFeeById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<StudentFeeResponse>>;

    public sealed class Handler(IStudentFeeQuery entityQuery)
        : IRequestHandler<Query, Result<StudentFeeResponse>>
    {
        public async Task<Result<StudentFeeResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<StudentFeeResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(StudentFee))));
            }
            return Result<StudentFeeResponse>.Success(StudentFeeResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student-fee"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<StudentFeeResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentFeeById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
