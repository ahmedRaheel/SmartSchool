using SmartSchool.Modules.HR.Persistence;
using Dapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Models;

using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Employee;

public static class GetEmployeeById
{
    public sealed record Response(
        Guid TenantId,
        Guid Id,
        Guid? UserId,
        string? EmployeeNumber,
        string FirstName,
        string? LastName,
        string? CnicNumber,
        byte[]? Photo,
        string? PhotoContentType,
        string? PhotoFileName,
        string? Email,
        string? Phone,
        DateOnly HireDate,
        string EmploymentTypeCode,
        string Status,
        Guid? SourceCandidateId);

    public sealed record Query(Guid TenantId, Guid Id) : IRequest<Result<Response>>;

    public sealed class Handler(GetEmployeeByIdEmployeeReadData entityQuery) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(EmployeeEntity))));
            }
            return Result<Response>.Success(MapResponse(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "employee"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Query, Result<Response>>(new Query(tenantId, id), cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetEmployeeById").WithTags(ModuleConstants.Name).RequireAuthorization();
        return endpoints;
    }

    private static Response MapResponse(EmployeeEntity entity)
    {
        return new Response(
            entity.TenantId,
            entity.EmployeeId,
            entity.UserId,
            entity.EmployeeNumber,
            entity.FirstName,
            entity.LastName,
            entity.CnicNumber,
            entity.Photo,
            entity.PhotoContentType,
            entity.PhotoFileName,
            entity.Email,
            entity.Phone,
            entity.HireDate,
            entity.EmploymentTypeCode,
            entity.Status,
            entity.SourceCandidateId);
    }
}

/// <summary>
/// Feature-owned data access for GetEmployeeById. Do not share across slices.
/// </summary>
internal sealed class GetEmployeeByIdEmployeeReadData(IHRDbContext dbContext,
    IDbConnectionFactory connectionFactory)
{
    public Task<EmployeeEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        return dbContext.Employees
            .AsNoTracking()
            .SingleOrDefaultAsync(
                entity => entity.TenantId == tenantId && entity.EmployeeId == id,
                cancellationToken);
    }
}
