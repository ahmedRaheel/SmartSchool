using SmartSchool.Modules.Documents;
using FluentValidation;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Documents.Features.Certificate;

public static class CreateCertificate
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
        ICertificateQuery query,
        ICertificateCommand command,
        IValidator<Request> validator)
    {
        public async Task<Result<Certificate>> HandleAsync(
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

                return Result<Certificate>.Failure(
                    Error.Validation(message));
            }

            var codeExists = await query.ExistsByCodeAsync(
                request.TenantId,
                request.Code,
                excludingId: null,
                cancellationToken);

            if (codeExists)
            {
                return Result<Certificate>.Failure(
                    Error.Conflict(ErrorMessages.DuplicateCode(nameof(Certificate), request.Code)));
            }

            var entity = new Certificate
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await command.AddAsync(
                entity,
                cancellationToken);

            return Result<Certificate>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "certificate"),
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
            .WithName("CreateCertificate")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
