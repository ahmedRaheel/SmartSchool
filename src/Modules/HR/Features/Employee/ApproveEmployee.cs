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

    public sealed class Handler(
        IEmployeeQuery query,
        IEmployeeCommand command,
        IEmployeeOnboardingQuery onboardingQuery,
        IIdentityAccountService accounts,
        IBusinessNumberGenerator numberGenerator)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var employee = await query.GetByIdAsync(request.TenantId, request.EmployeeId, cancellationToken);
            if (employee is null) return Result<Response>.Failure(Error.NotFound("Employee was not found."));
            if (employee.UserId.HasValue) return Result<Response>.Failure(Error.Conflict("Employee already has a login account."));
            if (string.IsNullOrWhiteSpace(employee.Email)) return Result<Response>.Failure(Error.Validation("Employee email is required before approval."));

            var staffType = employee.StaffType.ToUpperInvariant();
            var missingDocuments = await onboardingQuery.GetMissingRequiredDocumentsAsync(
                request.TenantId,
                request.EmployeeId,
                staffType,
                cancellationToken);

            if (missingDocuments.Count > 0)
            {
                return Result<Response>.Failure(
                    Error.Validation($"Required employment documents are missing: {string.Join(", ", missingDocuments)}."));
            }

            if (staffType == "TEACHER" &&
                !await onboardingQuery.HasEducationAsync(request.TenantId, request.EmployeeId, cancellationToken))
            {
                return Result<Response>.Failure(
                    Error.Validation("At least one education/qualification record is required before a teacher can be hired."));
            }

            var branchCode = await query.GetBranchCodeAsync(
                request.TenantId,
                employee.BranchId,
                cancellationToken);
            if (string.IsNullOrWhiteSpace(branchCode)) return Result<Response>.Failure(Error.Validation("The employee's branch is invalid."));
            var marker = request.Roles.Any(r => r.Equals("Teacher", StringComparison.OrdinalIgnoreCase)) ? "T"
                : request.Roles.Any(r => r.Equals("Driver", StringComparison.OrdinalIgnoreCase)) ? "D" : "E";
            var employeeNumber = await numberGenerator.NextAsync(
                $"EMPLOYEE:{marker}:{employee.BranchId}", $"{branchCode}-{marker}-", request.TenantId, 7, cancellationToken);

            var accountType = request.Roles.Any(r => r.Equals("Teacher", StringComparison.OrdinalIgnoreCase)) ? "Teacher"
                : request.Roles.Any(r => r.Equals("Driver", StringComparison.OrdinalIgnoreCase)) ? "Driver"
                : request.Roles.Any(r => r.Equals("Examiner", StringComparison.OrdinalIgnoreCase)) ? "Examiner" : "Employee";
            var account = await accounts.CreateAccountAsync(
                request.TenantId, employee.EmployeeId, accountType, employee.Email, employee.FirstName, employee.LastName ?? string.Empty,
                employee.SchoolId, employee.BranchId, request.Roles, cancellationToken);

            employee.ApproveEmployment(account.UserId, employeeNumber);
            await command.UpdateAsync(employee, cancellationToken);
            return Result<Response>.Success(new Response(employee.EmployeeId, account.UserId, employee.EmployeeNumber!, employee.Status, request.Roles));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/hr/employee/{employeeId:guid}/approve", async (Guid employeeId, Request request, ITenantScope tenantScope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue) return Results.BadRequest(new { message = "Tenant is required for SuperAdmin." });
            var command = request with { TenantId = tenantId.Value, EmployeeId = employeeId };
            return (await mediator.SendAsync<Request, Result<Response>>(command, cancellationToken)).ToHttpResult();
        }).WithName("ApproveEmployee").WithTags("HR").RequireAuthorization();
        return endpoints;
    }
}
