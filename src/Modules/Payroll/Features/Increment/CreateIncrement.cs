using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Payroll.Contracts;
using SmartSchool.Modules.Payroll.Models;
using SmartSchool.Modules.Payroll.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Payroll.Features.Increment;

public static class CreateIncrement
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<IncrementResponse>>;

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
        IIncrementQuery entityQuery,
        IIncrementCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<IncrementResponse>>
    {
        public async Task<Result<IncrementResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<IncrementResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<IncrementResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(Increment), request.Code)));
            }

            var entity = new Increment
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<IncrementResponse>.Success(IncrementResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "increment"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<IncrementResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateIncrement")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
