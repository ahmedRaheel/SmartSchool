using SmartSchool.Modules.Academics;
using FluentValidation;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.CourseOffering;

public static class UpdateCourseOffering
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
        ICourseOfferingQuery query,
        ICourseOfferingCommand command,
        IValidator<Request> validator)
    {
        public async Task<Result<CourseOffering>> HandleAsync(
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

                return Result<CourseOffering>.Failure(
                    Error.Validation(message));
            }

            var entity = await query.GetByIdAsync(
                request.TenantId,
                request.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<CourseOffering>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(CourseOffering))));
            }

            var duplicateCode = await query.ExistsByCodeAsync(
                request.TenantId,
                request.Code,
                request.Id,
                cancellationToken);

            if (duplicateCode)
            {
                return Result<CourseOffering>.Failure(
                    Error.Conflict(ErrorMessages.DuplicateCode(nameof(CourseOffering), request.Code)));
            }

            entity.Code = request.Code.Trim();
            entity.Name = request.Name.Trim();
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await command.UpdateAsync(
                entity,
                cancellationToken);

            return Result<CourseOffering>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/academics/course-offering/{id:guid}",
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
            .WithName("UpdateCourseOffering")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
