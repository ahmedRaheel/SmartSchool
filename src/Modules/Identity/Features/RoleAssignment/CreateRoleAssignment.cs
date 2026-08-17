using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Identity.Contracts;
using SmartSchool.Modules.Identity.Models;
using SmartSchool.Modules.Identity.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Identity.Features.RoleAssignment;

public static class CreateRoleAssignment
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<RoleAssignmentResponse>>;

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
        IRoleAssignmentQuery entityQuery,
        IRoleAssignmentCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<RoleAssignmentResponse>>
    {
        public async Task<Result<RoleAssignmentResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<RoleAssignmentResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<RoleAssignmentResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(RoleAssignment), request.Code)));
            }

            var entity = new RoleAssignment
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<RoleAssignmentResponse>.Success(RoleAssignmentResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "role-assignment"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<RoleAssignmentResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateRoleAssignment")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
