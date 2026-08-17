using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AITutor.Contracts;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.Modules.AITutor.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AITutor.Features.GeneratedQuiz;

public static class CreateGeneratedQuiz
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<GeneratedQuizResponse>>;

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
        IGeneratedQuizQuery entityQuery,
        IGeneratedQuizCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<GeneratedQuizResponse>>
    {
        public async Task<Result<GeneratedQuizResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<GeneratedQuizResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<GeneratedQuizResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(GeneratedQuiz), request.Code)));
            }

            var entity = new GeneratedQuiz
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<GeneratedQuizResponse>.Success(GeneratedQuizResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "generated-quiz"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<GeneratedQuizResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateGeneratedQuiz")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
