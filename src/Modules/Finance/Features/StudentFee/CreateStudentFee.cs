using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Finance.Contracts;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.Modules.Finance.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Finance.Features.StudentFee;

public static class CreateStudentFee
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<StudentFeeResponse>>;

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
        IStudentFeeQuery entityQuery,
        IStudentFeeCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<StudentFeeResponse>>
    {
        public async Task<Result<StudentFeeResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<StudentFeeResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<StudentFeeResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(StudentFee), request.Code)));
            }

            var entity = new StudentFee
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<StudentFeeResponse>.Success(StudentFeeResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "student-fee"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<StudentFeeResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateStudentFee")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
