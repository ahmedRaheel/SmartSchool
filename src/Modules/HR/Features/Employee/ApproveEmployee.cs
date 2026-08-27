using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Features.Employee;

public static class ApproveEmployee
{
    public sealed record Request(Guid TenantId, Guid EmployeeId, IReadOnlyCollection<string> Roles) : IRequest<Result<Response>>;
    public sealed record Response(Guid EmployeeId, Guid UserId, string EmployeeNumber, string Status, IReadOnlyCollection<string> Roles);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.Roles).NotEmpty();
        }
    }

    public sealed class Handler(IEmployeeQuery query, IEmployeeCommand command, IIdentityAccountService accounts, IBusinessNumberGenerator numberGenerator)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var employee = await query.GetByIdAsync(request.TenantId, request.EmployeeId, cancellationToken);
            if (employee is null) return Result<Response>.Failure(Error.NotFound("Employee was not found."));
            if (employee.UserId.HasValue) return Result<Response>.Failure(Error.Conflict("Employee already has a login account."));
            if (string.IsNullOrWhiteSpace(employee.Email)) return Result<Response>.Failure(Error.Validation("Employee email is required before approval."));

            var employeeNumber = await numberGenerator.NextAsync(
                "EMPLOYEE", "EMP", request.TenantId, 6, cancellationToken);

            var account = await accounts.CreateAccountAsync(
                request.TenantId, employee.EmployeeId, "Employee", employee.Email, employee.FirstName, employee.LastName ?? string.Empty,
                request.Roles, cancellationToken);

            employee.ApproveEmployment(account.UserId, employeeNumber);
            await command.UpdateAsync(employee, cancellationToken);
            return Result<Response>.Success(new Response(employee.EmployeeId, account.UserId, employee.EmployeeNumber!, employee.Status, request.Roles));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/hr/employee/{employeeId:guid}/approve", async (Guid employeeId, Request request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = request with { EmployeeId = employeeId };
            return (await mediator.SendAsync<Request, Result<Response>>(command, cancellationToken)).ToHttpResult();
        }).WithName("ApproveEmployee").WithTags("HR").RequireAuthorization();
        return endpoints;
    }
}
