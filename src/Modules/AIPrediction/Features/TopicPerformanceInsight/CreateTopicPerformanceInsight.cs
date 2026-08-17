using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Contracts;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.Modules.AIPrediction.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.TopicPerformanceInsight;

public static class CreateTopicPerformanceInsight
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<TopicPerformanceInsightResponse>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }

    public sealed class Handler(
        ITopicPerformanceInsightQuery entityQuery,
        ITopicPerformanceInsightCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<TopicPerformanceInsightResponse>>
    {
        public async Task<Result<TopicPerformanceInsightResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<TopicPerformanceInsightResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<TopicPerformanceInsightResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(TopicPerformanceInsight), request.Code)));
            }

            var entity = new TopicPerformanceInsight
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<TopicPerformanceInsightResponse>.Success(TopicPerformanceInsightResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "topic-performance-insight"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<TopicPerformanceInsightResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateTopicPerformanceInsight")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
