using SmartSchool.Modules.Students.Persistence;
using Dapper;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;

using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Enrollment;

public static class GetEnrollmentById
{
    /// <summary>
    /// Represents the response returned by this EnrollmentEntity feature.
    /// </summary>
    /// <param name="TenantId">The owning tenant identifier.</param>
    /// <param name="Id">The entity identifier.</param>
    /// <param name="Code">The business code.</param>
    /// <param name="Name">The display name.</param>
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    Guid StudentId,
    Guid AcademicYearId,
    Guid ClassSectionId,
    DateOnly EnrollmentDate,
    string Status);

    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;

    public sealed class Handler(GetEnrollmentByIdEnrollmentReadData entityQuery)
        : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(EnrollmentEntity))));
            }
            return Result<Response>.Success(MapResponse(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "enrollment"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetEnrollmentById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantStudent);
        return endpoints;
    }

    private static Response MapResponse(EnrollmentEntity entity)
    {
        return new Response(
            entity.TenantId,
            entity.StudentEnrollmentId,
            entity.StudentId,
            entity.AcademicYearId,
            entity.ClassSectionId,
            entity.EnrollmentDate,
            entity.Status);
    }
}

/// <summary>
/// Feature-owned data access for GetEnrollmentById. Do not share across slices.
/// </summary>
internal sealed class GetEnrollmentByIdEnrollmentReadData(IStudentsDbContext dbContext,
    IDbConnectionFactory connectionFactory)
{
    public Task<EnrollmentEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        return dbContext.Enrollments
            .AsNoTracking()
            .SingleOrDefaultAsync(
                entity => entity.TenantId == tenantId && entity.StudentEnrollmentId == id,
                cancellationToken);
    }
}
