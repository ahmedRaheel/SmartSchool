using SmartSchool.Modules.AIInquiry.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIInquiry.Features.LeadCapture;

public static class CreateLeadCapture
{
    /// <summary>
    /// Represents the response returned by this LeadCaptureEntity feature.
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
        string Name,
        string? MetadataJson = null) : IRequest<Result<Response>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }

    public interface ICreateLeadCaptureCommand
    {
        Task AddAsync(
                LeadCaptureEntity entity,
                CancellationToken cancellationToken);
}

    internal sealed class CreateLeadCaptureCommand(IAIInquiryDbContext dbContext) : ICreateLeadCaptureCommand
    {
        public async Task AddAsync(
                LeadCaptureEntity entity,
                CancellationToken cancellationToken)
            {
                await dbContext.LeadCaptures
                    .AddAsync(entity, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
    }

    public sealed class Handler(ICreateLeadCaptureCommand command, IBusinessNumberGenerator numberGenerator)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var code = await numberGenerator.NextAsync("LeadCapture", "LC", request.TenantId, 3, cancellationToken);

            var entity = LeadCaptureEntity.Create(
                request.TenantId,
                code,
                request.Name,
                request.MetadataJson);

            await command.AddAsync(entity, cancellationToken);
            return Result<Response>.Success(MapResponse(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "lead-capture"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateLeadCapture")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }

    private static Response MapResponse(LeadCaptureEntity entity)
    {
        return new Response(
            entity.TenantId,
            entity.LeadCaptureId,
            entity.Code,
            entity.Name,
            entity.MetadataJson);
    }
}
