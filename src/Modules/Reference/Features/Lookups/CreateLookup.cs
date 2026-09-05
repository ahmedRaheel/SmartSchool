using Dapper;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Reference.Models;
using SmartSchool.Modules.Reference.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Reference.Features.Lookups;

public static class CreateLookup
{
    public sealed record Request(
        string TypeCode,
        string Code,
        string Name,
        int SortOrder = 0,
        string? Metadata = null,
        Guid? TenantId = null) : IRequest<Result<Response>>;

    public sealed record Response(
        long Id,
        long LookupTypeId,
        string TypeCode,
        string Code,
        string Name,
        int SortOrder,
        bool IsActive,
        Guid? TenantId);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TypeCode).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public interface ICreateLookup
    {
        Task<(long Id, bool TenantScoped)?> GetTypeAsync(string typeCode, CancellationToken cancellationToken);
        Task AddAsync(LookupValueEntity entity, CancellationToken cancellationToken);
    }

    internal sealed class CreateLookupPersistence(
        IReferenceDbContext dbContext,
        IDbConnectionFactory connections) : ICreateLookup
    {
        public async Task<(long Id, bool TenantScoped)?> GetTypeAsync(
            string typeCode,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT lookup_type_id AS Id,
                       is_tenant_scoped AS TenantScoped
                FROM saas.lookup_type
                WHERE code = @Code;
                """;

            await using var connection = await connections.OpenConnectionAsync(cancellationToken);

            return await connection.QuerySingleOrDefaultAsync<(long, bool)?>(
                new CommandDefinition(
                    sql,
                    new { Code = typeCode.Trim().ToUpperInvariant() },
                    cancellationToken: cancellationToken));
        }

        public async Task AddAsync(LookupValueEntity entity, CancellationToken cancellationToken)
        {
            await dbContext.LookupValues.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public sealed class Handler(
        ICreateLookup persistence,
        ITenantScope tenantScope) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var lookupType = await persistence.GetTypeAsync(request.TypeCode, cancellationToken);
            if (lookupType is null)
            {
                return Result<Response>.Failure(Error.Validation("Unknown lookup type."));
            }

            if (!lookupType.Value.TenantScoped)
            {
                return Result<Response>.Failure(
                    Error.Validation("Universal lookup values are platform managed and cannot be changed by a tenant."));
            }

            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<Response>.Failure(Error.Validation("A tenant is required."));
            }

            var entity = LookupValueEntity.Create(
                lookupType.Value.Id,
                tenantId.Value,
                request.Code,
                request.Name,
                request.SortOrder,
                request.Metadata);

            await persistence.AddAsync(entity, cancellationToken);

            return Result<Response>.Success(
                new Response(
                    entity.LookupValueId,
                    entity.LookupTypeId,
                    request.TypeCode.Trim().ToUpperInvariant(),
                    entity.Code,
                    entity.Name,
                    entity.SortOrder,
                    entity.IsActive,
                    entity.LookupTenantId));
        }
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/api/lookups",
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken)).ToHttpResult())
            .WithTags("Lookups")
            .WithName("CreateLookup")
            .RequireAuthorization();
    }
}
