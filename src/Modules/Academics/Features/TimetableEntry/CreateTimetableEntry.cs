using SmartSchool.Modules.Academics;
using FluentValidation;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.TimetableEntry;

public static class CreateTimetableEntry
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
        ITimetableEntryQuery query,
        ITimetableEntryCommand command,
        IValidator<Request> validator)
    {
        public async Task<Result<TimetableEntry>> HandleAsync(
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

                return Result<TimetableEntry>.Failure(
                    Error.Validation(message));
            }

            var codeExists = await query.ExistsByCodeAsync(
                request.TenantId,
                request.Code,
                excludingId: null,
                cancellationToken);

            if (codeExists)
            {
                return Result<TimetableEntry>.Failure(
                    Error.Conflict(ErrorMessages.DuplicateCode(nameof(TimetableEntry), request.Code)));
            }

            var entity = new TimetableEntry
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await command.AddAsync(
                entity,
                cancellationToken);

            return Result<TimetableEntry>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "timetable-entry"),
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
            .WithName("CreateTimetableEntry")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
