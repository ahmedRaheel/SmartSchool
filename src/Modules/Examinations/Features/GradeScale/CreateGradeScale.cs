using SmartSchool.Modules.Examinations;
using FluentValidation;
using SmartSchool.Modules.Examinations.Persistence;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Examinations.Features.GradeScale;

public static class CreateGradeScale
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
        IGradeScaleQuery query,
        IGradeScaleCommand command,
        IValidator<Request> validator)
    {
        public async Task<Result<GradeScale>> HandleAsync(
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

                return Result<GradeScale>.Failure(
                    Error.Validation(message));
            }

            var codeExists = await query.ExistsByCodeAsync(
                request.TenantId,
                request.Code,
                excludingId: null,
                cancellationToken);

            if (codeExists)
            {
                return Result<GradeScale>.Failure(
                    Error.Conflict(ErrorMessages.DuplicateCode(nameof(GradeScale), request.Code)));
            }

            var entity = new GradeScale
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await command.AddAsync(
                entity,
                cancellationToken);

            return Result<GradeScale>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "grade-scale"),
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
            .WithName("CreateGradeScale")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
