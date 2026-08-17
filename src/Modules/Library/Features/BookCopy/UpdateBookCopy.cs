using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Library.Contracts;
using SmartSchool.Modules.Library.Models;
using SmartSchool.Modules.Library.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Library.Features.BookCopy;

public static class UpdateBookCopy
{
    public sealed record Request(
        Guid TenantId,
        Guid Id,
        string Code,
        string Name,
        bool IsActive) : IRequest<Result<BookCopyResponse>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }

    public sealed class Handler(
        IBookCopyQuery entityQuery,
        IBookCopyCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<BookCopyResponse>>
    {
        public async Task<Result<BookCopyResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<BookCopyResponse>.Failure(Error.Validation(message));
            }

            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<BookCopyResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(BookCopy))));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, request.Id, cancellationToken);
            if (exists)
            {
                return Result<BookCopyResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(BookCopy), request.Code)));
            }

            entity.Code = request.Code.Trim();
            entity.Name = request.Name.Trim();
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            await entityCommand.UpdateAsync(entity, cancellationToken);
            return Result<BookCopyResponse>.Success(BookCopyResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "book-copy"),
                async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var command = request with { Id = id };
                    var result = await mediator.SendAsync<Request, Result<BookCopyResponse>>(
                        command, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("UpdateBookCopy")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
