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

namespace SmartSchool.Modules.AIPrediction.Features.StudentPerformancePrediction;

public static class CreateStudentPerformancePrediction
{
    /// <summary>
    /// Represents the response returned by this StudentPerformancePredictionEntity feature.
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

    public interface ICreateStudentPerformancePredictionCommand
    {
        Task AddAsync(
                StudentPerformancePredictionEntity entity,
                CancellationToken cancellationToken);
}

    internal sealed class CreateStudentPerformancePredictionCommand(
        IAIPredictionDbContext dbContext) : ICreateStudentPerformancePredictionCommand
    {
        public async Task AddAsync(
                StudentPerformancePredictionEntity entity,
                CancellationToken cancellationToken)
            {
                await dbContext.StudentPerformancePredictions
                    .AddAsync(entity, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
}

    public sealed class Handler(ICreateStudentPerformancePredictionCommand command, IBusinessNumberGenerator numberGenerator)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var code = await numberGenerator.NextAsync("StudentPerformancePrediction", "SPP", request.TenantId, 3, cancellationToken);

            var entity = StudentPerformancePredictionEntity.Create(
                request.TenantId,
                code,
                request.Name);

            await command.AddAsync(entity, cancellationToken);
            return Result<Response>.Success(MapResponse(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "student-performance-prediction"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateStudentPerformancePrediction")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }

    private static Response MapResponse(StudentPerformancePredictionEntity entity)
    {
        return new Response(
            entity.TenantId,
            entity.StudentPerformancePredictionId,
            entity.Code,
            entity.Name,
            entity.MetadataJson);
    }
}
