using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Transport.Contracts;
using SmartSchool.Modules.Transport.Models;
using SmartSchool.Modules.Transport.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Transport.Features.StudentTransport;

public static class CreateStudentTransport
{
    public sealed record Request(
        Guid TenantId,
        string Code,
        string Name) : IRequest<Result<StudentTransportResponse>>;

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
        IStudentTransportQuery entityQuery,
        IStudentTransportCommand entityCommand,
        IValidator<Request> validator)
        : IRequestHandler<Request, Result<StudentTransportResponse>>
    {
        public async Task<Result<StudentTransportResponse>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var message = string.Join(
                    "; ",
                    validation.Errors.Select(error => error.ErrorMessage));
                return Result<StudentTransportResponse>.Failure(Error.Validation(message));
            }

            var exists = await entityQuery.ExistsByCodeAsync(
                request.TenantId, request.Code, null, cancellationToken);
            if (exists)
            {
                return Result<StudentTransportResponse>.Failure(
                    Error.Conflict(
                        ErrorMessages.DuplicateCode(nameof(StudentTransport), request.Code)));
            }

            var entity = new StudentTransport
            {
                TenantId = request.TenantId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await entityCommand.AddAsync(entity, cancellationToken);
            return Result<StudentTransportResponse>.Success(StudentTransportResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "student-transport"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<StudentTransportResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateStudentTransport")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
