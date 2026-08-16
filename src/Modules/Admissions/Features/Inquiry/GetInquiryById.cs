using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Admissions.Features.Inquiry;

public static class GetInquiryById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<Inquiry> repository)
    {
        public async Task<Result<Inquiry>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<Inquiry>.Failure(
                    Error.NotFound("Inquiry was not found."));
            }

            return Result<Inquiry>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/admissions/inquiry/{id:guid}",
                async (
                    Guid id,
                    Guid tenantId,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var query = new Query(tenantId, id);

                    var result = await handler.HandleAsync(
                        query,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("GetInquiryById")
            .WithTags("Admissions")
            .RequireAuthorization();

        return endpoints;
    }
}
