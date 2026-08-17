using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Learning.Contracts;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.Modules.Learning.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Learning.Features.AssignmentSubmission;

public static class CreateAssignmentSubmission
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<AssignmentSubmissionResponse>>;

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
        IAssignmentSubmissionQuery entityQuery,
        IAssignmentSubmissionCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<AssignmentSubmissionResponse>>
    {
        public async Task<Result<AssignmentSubmissionResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<AssignmentSubmissionResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<AssignmentSubmissionResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(AssignmentSubmission), request.Code)));
            }

            var entity = new AssignmentSubmission
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<AssignmentSubmissionResponse>.Success(AssignmentSubmissionResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "assignment-submission"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<AssignmentSubmissionResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateAssignmentSubmission")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
