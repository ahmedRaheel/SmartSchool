using SmartSchool.Modules.AIPrediction.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionEvidence;

public static class CreatePredictionEvidence
{
    /// <summary>
    /// Represents the response returned by this PredictionEvidenceEntity feature.
    /// </summary>
    /// <param name="TenantId">The owning tenant identifier.</param>
    /// <param name="Id">The entity identifier.</param>
    /// <param name="Code">The business code.</param>
    /// <param name="Name">The display name.</param>
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    string Code,
    string Name,
    string? MetadataJson);

    public sealed record Request(
        Guid TenantId,
        string Name) : IRequest<Result<Response>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }

    public interface ICreatePredictionEvidence
    {
        Task AddAsync(
                PredictionEvidenceEntity entity,
                CancellationToken cancellationToken);
}

    internal sealed class CreatePredictionEvidencePersistence(IAIPredictionDbContext dbContext) : ICreatePredictionEvidence
    {
        public async Task AddAsync(
                PredictionEvidenceEntity entity,
                CancellationToken cancellationToken)
            {
                await dbContext.PredictionEvidences
                    .AddAsync(entity, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
    }

    public sealed class Handler(ICreatePredictionEvidence dataAccess)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {


            var entity = PredictionEvidenceEntity.Create(
                request.TenantId,
                Guid.NewGuid().ToString("N").ToUpperInvariant(),
                request.Name);

            await dataAccess.AddAsync(entity, cancellationToken);
            return Result<Response>.Success(MapResponse(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "prediction-evidence"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreatePredictionEvidence")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }

    private static Response MapResponse(PredictionEvidenceEntity entity)
    {
        return new Response(
            entity.TenantId,
            entity.PredictionEvidenceId,
            entity.Code,
            entity.Name,
            entity.MetadataJson);
    }
}
