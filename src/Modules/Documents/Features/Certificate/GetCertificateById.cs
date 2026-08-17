using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Documents.Contracts;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Documents.Features.Certificate;

public static class GetCertificateById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<CertificateResponse>>;

    public sealed class Handler(ICertificateQuery entityQuery)
        : IRequestHandler<Query, Result<CertificateResponse>>
    {
        public async Task<Result<CertificateResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<CertificateResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Certificate))));
            }
            return Result<CertificateResponse>.Success(CertificateResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "certificate"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<CertificateResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetCertificateById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
