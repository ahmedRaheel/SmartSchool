using SmartSchool.Modules.Reference.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Reference.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Reference.Features.Lookups;

public static class DeleteLookup
{
    public sealed record Command(long Id) : IRequest<Result<Response>>;
    public sealed record Response(long Id);
    public interface IDeleteLookup { Task<LookupValueEntity?> GetByIdAsync(long id, CancellationToken cancellationToken); Task DeleteAsync(LookupValueEntity entity, CancellationToken cancellationToken); }
    internal sealed class DeleteLookupPersistence(IReferenceDbContext dbContext) : IDeleteLookup
    {
        public Task<LookupValueEntity?> GetByIdAsync(long id, CancellationToken cancellationToken) => dbContext.LookupValues.SingleOrDefaultAsync(x => x.LookupValueId == id, cancellationToken);
        public async Task DeleteAsync(LookupValueEntity entity, CancellationToken cancellationToken) { dbContext.LookupValues.Remove(entity); await dbContext.SaveChangesAsync(cancellationToken); }
    }
    public sealed class Handler(IDeleteLookup persistence) : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Command request, CancellationToken cancellationToken) { var entity = await persistence.GetByIdAsync(request.Id, cancellationToken); if (entity is null) return Result<Response>.Failure(Error.NotFound("Lookup value not found.")); await persistence.DeleteAsync(entity, cancellationToken); return Result<Response>.Success(new(request.Id)); }
    }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) { endpoints.MapDelete("/api/lookups/{id:long}", async (long id, IMediator mediator, CancellationToken cancellationToken) => (await mediator.SendAsync<Command, Result<Response>>(new(id), cancellationToken)).ToHttpResult()).WithTags("Lookups").WithName("DeleteLookup").RequireAuthorization(); }
}
