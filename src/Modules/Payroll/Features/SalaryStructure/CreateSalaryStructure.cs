using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Payroll.Contracts;
using SmartSchool.Modules.Payroll.Models;
using SmartSchool.Modules.Payroll.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Payroll.Features.SalaryStructure;

public static class CreateSalaryStructure
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<SalaryStructureResponse>>;

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
        ISalaryStructureQuery entityQuery,
        ISalaryStructureCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<SalaryStructureResponse>>
    {
        public async Task<Result<SalaryStructureResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<SalaryStructureResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<SalaryStructureResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(SalaryStructure), request.Code)));
            }

            var entity = new SalaryStructure
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<SalaryStructureResponse>.Success(SalaryStructureResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "salary-structure"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<SalaryStructureResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateSalaryStructure")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
