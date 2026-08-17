using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Contracts;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.ClassSection;

public static class CreateClassSection
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<ClassSectionResponse>>;

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
        IClassSectionQuery entityQuery,
        IClassSectionCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<ClassSectionResponse>>
    {
        public async Task<Result<ClassSectionResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<ClassSectionResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<ClassSectionResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(ClassSection), request.Code)));
            }

            var entity = new ClassSection
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<ClassSectionResponse>.Success(ClassSectionResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "class-section"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<ClassSectionResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateClassSection")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
