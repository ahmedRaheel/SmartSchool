using SmartSchool.Modules.AITutor;
using FluentValidation;
using SmartSchool.Modules.AITutor.Persistence;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AITutor.Features.TutorSession;

public static class CreateTutorSession
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId)
                .NotEmpty();

            RuleFor(x => x.Code)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(250);
        }
    }

    public sealed class Handler(
        ITutorSessionQuery query,
        ITutorSessionCommand command,
        IValidator<Request> validator)
    {
        public async Task<Result<TutorSession>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validationResult =
                await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validationResult.Errors.Select(error => error.ErrorMessage));

                return Result<TutorSession>.Failure(
                    Error.Validation(message));
            }

            var codeExists = await query.ExistsByCodeAsync(
                request.TenantId,
                request.Code,
                excludingId: null,
                cancellationToken);

            if (codeExists)
            {
                return Result<TutorSession>.Failure(
                    Error.Conflict(ErrorMessages.DuplicateCode(nameof(TutorSession), request.Code)));
            }

            var entity = new TutorSession
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await command.AddAsync(
                entity,
                cancellationToken);

            return Result<TutorSession>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "tutor-session"),
                async (
                    Request request,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var result = await handler.HandleAsync(
                        request,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("CreateTutorSession")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
