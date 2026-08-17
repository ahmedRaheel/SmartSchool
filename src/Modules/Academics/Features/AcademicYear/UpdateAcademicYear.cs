using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Contracts;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.AcademicYear;

public static class UpdateAcademicYear
{
    public sealed record Request(
        Guid TenantId,
        Guid Id,
        string Code,
        string Name,
        bool IsActive) : IRequest<Result<AcademicYearResponse>>;

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
        IAcademicYearQuery entityQuery,
        IAcademicYearCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<AcademicYearResponse>>
    {
        public async Task<Result<AcademicYearResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<AcademicYearResponse>.Failure(Error.Validation(message));
            }

            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<AcademicYearResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(AcademicYear))));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, request.Id, cancellationToken);
            if (exists)
            {
                return Result<AcademicYearResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(AcademicYear), request.Code)));
            }

            entity.Code = request.Code.Trim();
            entity.Name = request.Name.Trim();
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            await entityCommand.UpdateAsync(entity, cancellationToken);
            return Result<AcademicYearResponse>.Success(AcademicYearResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "academic-year"),
                async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var command = request with { Id = id };
                    var result = await mediator.SendAsync<Request, Result<AcademicYearResponse>>(
                        command, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("UpdateAcademicYear")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
