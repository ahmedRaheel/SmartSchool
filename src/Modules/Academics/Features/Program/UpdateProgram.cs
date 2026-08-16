using SmartSchool.Modules.Academics;
using FluentValidation;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.Program;

public static class UpdateProgram
{
    public sealed record Request(
        Guid TenantId,
        Guid Id,
        string Code,
        string Name,
        bool IsActive);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId)
                .NotEmpty();

            RuleFor(x => x.Id)
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
        IProgramQuery query,
        IProgramCommand command,
        IValidator<Request> validator)
    {
        public async Task<Result<Program>> HandleAsync(
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

                return Result<Program>.Failure(
                    Error.Validation(message));
            }

            var entity = await query.GetByIdAsync(
                request.TenantId,
                request.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<Program>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Program))));
            }

            var duplicateCode = await query.ExistsByCodeAsync(
                request.TenantId,
                request.Code,
                request.Id,
                cancellationToken);

            if (duplicateCode)
            {
                return Result<Program>.Failure(
                    Error.Conflict(ErrorMessages.DuplicateCode(nameof(Program), request.Code)));
            }

            entity.Code = request.Code.Trim();
            entity.Name = request.Name.Trim();
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await command.UpdateAsync(
                entity,
                cancellationToken);

            return Result<Program>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/academics/program/{id:guid}",
                async (
                    Guid id,
                    Request request,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var command = request with { Id = id };

                    var result = await handler.HandleAsync(
                        command,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("UpdateProgram")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
