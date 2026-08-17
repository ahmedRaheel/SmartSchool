using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Contracts;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.GradeLevel;

public static class CreateGradeLevel
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<GradeLevelResponse>>;

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
        IGradeLevelQuery entityQuery,
        IGradeLevelCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<GradeLevelResponse>>
    {
        public async Task<Result<GradeLevelResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<GradeLevelResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<GradeLevelResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(GradeLevel), request.Code)));
            }

            var entity = new GradeLevel
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<GradeLevelResponse>.Success(GradeLevelResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "grade-level"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<GradeLevelResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateGradeLevel")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
