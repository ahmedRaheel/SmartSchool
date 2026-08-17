using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Contracts;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.Modules.AIPrediction.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.TeachingRecommendation;

public static class UpdateTeachingRecommendation
{
    public sealed record Request(
        Guid TenantId,
        Guid Id,
        string Code,
        string Name,
        bool IsActive) : IRequest<Result<TeachingRecommendationResponse>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }

    public sealed class Handler(
        ITeachingRecommendationQuery entityQuery,
        ITeachingRecommendationCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<TeachingRecommendationResponse>>
    {
        public async Task<Result<TeachingRecommendationResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<TeachingRecommendationResponse>.Failure(Error.Validation(message));
            }

            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<TeachingRecommendationResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(TeachingRecommendation))));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, request.Id, cancellationToken);
            if (exists)
            {
                return Result<TeachingRecommendationResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(TeachingRecommendation), request.Code)));
            }

            entity.Code = request.Code.Trim();
            entity.Name = request.Name.Trim();
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            await entityCommand.UpdateAsync(entity, cancellationToken);
            return Result<TeachingRecommendationResponse>.Success(TeachingRecommendationResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "teaching-recommendation"),
                async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var command = request with { Id = id };
                    var result = await mediator.SendAsync<Request, Result<TeachingRecommendationResponse>>(
                        command, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("UpdateTeachingRecommendation")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
