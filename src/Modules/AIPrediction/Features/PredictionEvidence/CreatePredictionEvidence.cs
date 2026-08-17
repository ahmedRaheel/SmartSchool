using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Contracts;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.Modules.AIPrediction.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionEvidence;

public static class CreatePredictionEvidence
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<PredictionEvidenceResponse>>;

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
        IPredictionEvidenceQuery entityQuery,
        IPredictionEvidenceCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<PredictionEvidenceResponse>>
    {
        public async Task<Result<PredictionEvidenceResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<PredictionEvidenceResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<PredictionEvidenceResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(PredictionEvidence), request.Code)));
            }

            var entity = new PredictionEvidence
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<PredictionEvidenceResponse>.Success(PredictionEvidenceResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "prediction-evidence"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<PredictionEvidenceResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreatePredictionEvidence")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
