using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using Dapper;
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

    public sealed class Handler(IEmployeeQuery query, IEmployeeCommand command, IIdentityAccountService accounts, IBusinessNumberGenerator numberGenerator, IDbConnectionFactory connectionFactory)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var employee = await query.GetByIdAsync(request.TenantId, request.EmployeeId, cancellationToken);
            if (employee is null) return Result<Response>.Failure(Error.NotFound("Employee was not found."));
            if (employee.UserId.HasValue) return Result<Response>.Failure(Error.Conflict("Employee already has a login account."));
            if (string.IsNullOrWhiteSpace(employee.Email)) return Result<Response>.Failure(Error.Validation("Employee email is required before approval."));

            await using var complianceConnection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var staffType = employee.StaffType.ToUpperInvariant();
            var missingDocuments = (await complianceConnection.QueryAsync<string>(new CommandDefinition(
                """
                SELECT r.display_name
                FROM document.required_document r
                WHERE r.is_active=true AND r.is_required=true AND r.actor_type='EMPLOYEE'
                  AND (r.tenant_id IS NULL OR r.tenant_id=@TenantId)
                  AND (r.staff_type IS NULL OR r.staff_type=@StaffType)
                  AND (r.condition_code IS NULL OR (r.condition_code='EXPERIENCE_PRESENT' AND EXISTS(SELECT 1 FROM hr.employee_experience x WHERE x.tenant_id=@TenantId AND x.employee_id=@EmployeeId)))
                  AND NOT EXISTS (
                    SELECT 1 FROM document.document d JOIN document.document_link l ON l.document_id=d.document_id AND l.tenant_id=d.tenant_id
                    WHERE d.tenant_id=@TenantId AND l.entity_id=@EmployeeId AND l.entity_type IN ('EMPLOYEE',@StaffType)
                      AND d.document_type=r.document_type AND d.status='ACTIVE')
                """, new { request.TenantId, request.EmployeeId, StaffType=staffType }, cancellationToken:cancellationToken))).ToArray();
            if (missingDocuments.Length > 0)
                return Result<Response>.Failure(Error.Validation($"Required employment documents are missing: {string.Join(", ", missingDocuments)}."));

            if (staffType == "TEACHER")
            {
                var hasEducation = await complianceConnection.ExecuteScalarAsync<bool>(new CommandDefinition(
                    "SELECT EXISTS(SELECT 1 FROM hr.employee_education WHERE tenant_id=@TenantId AND employee_id=@EmployeeId)",
                    new { request.TenantId, request.EmployeeId }, cancellationToken:cancellationToken));
                if (!hasEducation) return Result<Response>.Failure(Error.Validation("At least one education/qualification record is required before a teacher can be hired."));
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
