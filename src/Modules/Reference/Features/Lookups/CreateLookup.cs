using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Reference.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Reference.Features.Lookups;

public static class CreateLookup
{
    public sealed record Request(string TypeCode, string Code, string Name, int SortOrder = 0, string? Metadata = null) : IRequest<Result<Response>>;
    public sealed record Response(long Id, long LookupTypeId, string TypeCode, string Code, string Name, int SortOrder, bool IsActive);
    public sealed class Validator : AbstractValidator<Request> { public Validator() { RuleFor(x => x.TypeCode).NotEmpty(); RuleFor(x => x.Code).NotEmpty(); RuleFor(x => x.Name).NotEmpty(); } }
    public interface ICreateLookup { Task<long?> GetTypeIdAsync(string typeCode, CancellationToken cancellationToken); Task AddAsync(LookupValueEntity entity, CancellationToken cancellationToken); }
    internal sealed class CreateLookupPersistence(IApplicationDbContext dbContext) : ICreateLookup
    {
        public Task<long?> GetTypeIdAsync(string typeCode, CancellationToken cancellationToken) => dbContext.Database.SqlQueryRaw<long?>("SELECT lookup_type_id AS \"Value\" FROM saas.lookup_type WHERE code = {0}", typeCode.Trim().ToUpperInvariant()).SingleOrDefaultAsync(cancellationToken);
        public async Task AddAsync(LookupValueEntity entity, CancellationToken cancellationToken) { await dbContext.Set<LookupValueEntity>().AddAsync(entity, cancellationToken); await dbContext.SaveChangesAsync(cancellationToken); }
    }
    public sealed class Handler(ICreateLookup persistence) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken) { var typeId = await persistence.GetTypeIdAsync(request.TypeCode, cancellationToken); if (!typeId.HasValue) return Result<Response>.Failure(Error.Validation("Unknown lookup type.")); var entity = LookupValueEntity.Create(typeId.Value, request.Code, request.Name, request.SortOrder, request.Metadata); await persistence.AddAsync(entity, cancellationToken); return Result<Response>.Success(new(entity.LookupValueId, entity.LookupTypeId, request.TypeCode.Trim().ToUpperInvariant(), entity.Code, entity.Name, entity.SortOrder, entity.IsActive)); }
    }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) { endpoints.MapPost("/api/lookups", async (Request request, IMediator mediator, CancellationToken cancellationToken) => (await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken)).ToHttpResult()).WithTags("Lookups").WithName("CreateLookup").RequireAuthorization(); }
}
