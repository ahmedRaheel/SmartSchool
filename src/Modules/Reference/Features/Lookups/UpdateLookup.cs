using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Reference.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Reference.Features.Lookups;

public static class UpdateLookup
{
    public sealed record Request(long Id, string Code, string Name, int SortOrder, bool IsActive, string? Metadata = null) : IRequest<Result<Response>>;
    public sealed record Response(long Id, long LookupTypeId, string TypeCode, string Code, string Name, int SortOrder, bool IsActive);
    public sealed class Validator : AbstractValidator<Request> { public Validator() { RuleFor(x => x.Id).GreaterThan(0); RuleFor(x => x.Code).NotEmpty(); RuleFor(x => x.Name).NotEmpty(); } }
    public interface IUpdateLookup { Task<LookupValueEntity?> GetByIdAsync(long id, CancellationToken cancellationToken); Task<string?> GetTypeCodeAsync(long typeId, CancellationToken cancellationToken); Task SaveAsync(CancellationToken cancellationToken); }
    internal sealed class UpdateLookupPersistence(IApplicationDbContext dbContext) : IUpdateLookup
    {
        public Task<LookupValueEntity?> GetByIdAsync(long id, CancellationToken cancellationToken) => dbContext.Set<LookupValueEntity>().SingleOrDefaultAsync(x => x.LookupValueId == id, cancellationToken);
        public Task<string?> GetTypeCodeAsync(long typeId, CancellationToken cancellationToken) => dbContext.Database.SqlQueryRaw<string?>("SELECT code AS \"Value\" FROM saas.lookup_type WHERE lookup_type_id = {0}", typeId).SingleOrDefaultAsync(cancellationToken);
        public Task SaveAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
    }
    public sealed class Handler(IUpdateLookup persistence) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken) { var entity = await persistence.GetByIdAsync(request.Id, cancellationToken); if (entity is null) return Result<Response>.Failure(Error.NotFound("Lookup value not found.")); entity.Update(request.Code, request.Name, request.SortOrder, request.IsActive, request.Metadata); await persistence.SaveAsync(cancellationToken); var typeCode = await persistence.GetTypeCodeAsync(entity.LookupTypeId, cancellationToken) ?? string.Empty; return Result<Response>.Success(new(entity.LookupValueId, entity.LookupTypeId, typeCode, entity.Code, entity.Name, entity.SortOrder, entity.IsActive)); }
    }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) { endpoints.MapPut("/api/lookups/{id:long}", async (long id, Request request, IMediator mediator, CancellationToken cancellationToken) => (await mediator.SendAsync<Request, Result<Response>>(request with { Id = id }, cancellationToken)).ToHttpResult()).WithTags("Lookups").WithName("UpdateLookup").RequireAuthorization(); }
}
