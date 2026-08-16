using SmartSchool.Modules.Documents;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Documents.Features.SchoolLogo;

public static class GetSchoolLogoById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        ISchoolLogoQuery query)
    {
        public async Task<Result<SchoolLogo>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<SchoolLogo>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(SchoolLogo))));
            }

            return Result<SchoolLogo>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/documents/school-logo/{id:guid}",
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
            .WithName("GetSchoolLogoById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
