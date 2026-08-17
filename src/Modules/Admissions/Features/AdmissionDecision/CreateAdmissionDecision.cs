using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Admissions.Contracts;
using SmartSchool.Modules.Admissions.Models;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features.AdmissionDecision;

public static class CreateAdmissionDecision
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<AdmissionDecisionResponse>>;

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
        IAdmissionDecisionQuery entityQuery,
        IAdmissionDecisionCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<AdmissionDecisionResponse>>
    {
        public async Task<Result<AdmissionDecisionResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<AdmissionDecisionResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<AdmissionDecisionResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(AdmissionDecision), request.Code)));
            }

            var entity = new AdmissionDecision
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<AdmissionDecisionResponse>.Success(AdmissionDecisionResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "admission-decision"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<AdmissionDecisionResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateAdmissionDecision")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
