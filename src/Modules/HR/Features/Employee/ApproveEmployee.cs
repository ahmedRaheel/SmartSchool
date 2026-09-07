using Dapper;
using SmartSchool.Modules.HR.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Modules.HR.Models;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;

using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

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
        ApproveEmployeeEmployeeQuery query,
        ApproveEmployeeEmployeeCommand command,
        ApproveEmployeeEmployeeOnboardingQuery onboardingQuery,
        IIdentityAccountService accounts,
        IBusinessNumberGenerator numberGenerator)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var employee = await command.GetByIdAsync(request.TenantId, request.EmployeeId, cancellationToken);
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
            var marker = request.Roles.Any(r => r.Equals(SmartSchoolRoles.Teacher, StringComparison.OrdinalIgnoreCase)) ? "T"
                : request.Roles.Any(r => r.Equals(SmartSchoolRoles.Driver, StringComparison.OrdinalIgnoreCase)) ? "D" : "E";
            var employeeNumber = await numberGenerator.NextAsync(
                $"EMPLOYEE:{marker}:{employee.BranchId}", $"{branchCode}-{marker}-", request.TenantId, 7, cancellationToken);

            var accountType = request.Roles.Any(r => r.Equals(SmartSchoolRoles.Teacher, StringComparison.OrdinalIgnoreCase)) ? SmartSchoolRoles.Teacher
                : request.Roles.Any(r => r.Equals(SmartSchoolRoles.Driver, StringComparison.OrdinalIgnoreCase)) ? SmartSchoolRoles.Driver
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

/// <summary>
/// Feature-owned data access for ApproveEmployee. Do not share across slices.
/// </summary>
public sealed class ApproveEmployeeEmployeeCommand(IHRDbContext dbContext)
{
    public Task<EmployeeEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
    {
        return dbContext.Employees.SingleOrDefaultAsync(
            entity => entity.TenantId == tenantId && entity.EmployeeId == id, cancellationToken);
    }


    public async Task UpdateAsync(
        EmployeeEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Employees
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

/// <summary>
/// Feature-owned data access for ApproveEmployee. Do not share across slices.
/// </summary>
public sealed class ApproveEmployeeEmployeeQuery(IDbConnectionFactory connectionFactory)
{
        public async Task<string?> GetBranchCodeAsync(
        Guid tenantId,
        Guid branchId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT code
            FROM org.campus
            WHERE tenant_id = @TenantId
              AND campus_id = @BranchId
              AND is_active = TRUE;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<string?>(
            new CommandDefinition(sql, new { TenantId = tenantId, BranchId = branchId }, cancellationToken: cancellationToken));
    }
}

/// <summary>
/// Feature-owned data access for ApproveEmployee. Do not share across slices.
/// </summary>
public sealed class ApproveEmployeeEmployeeOnboardingQuery(IDbConnectionFactory connectionFactory)
{

    public async Task<IReadOnlyList<string>> GetMissingRequiredDocumentsAsync(Guid tenantId, Guid employeeId, string staffType, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT r.display_name
            FROM document.required_document r
            WHERE r.is_active=true AND r.is_required=true AND r.actor_type='EMPLOYEE'
              AND (r.tenant_id IS NULL OR r.tenant_id=@TenantId)
              AND (r.staff_type IS NULL OR r.staff_type=@StaffType)
              AND (r.condition_code IS NULL OR (r.condition_code='EXPERIENCE_PRESENT' AND EXISTS(
                    SELECT 1 FROM hr.employee_experience x WHERE x.tenant_id=@TenantId AND x.employee_id=@EmployeeId)))
              AND NOT EXISTS (
                    SELECT 1 FROM document.document d
                    LEFT JOIN document.teacher_document td ON td.document_id=d.document_id AND td.teacher_id=@EmployeeId
                    LEFT JOIN document.admin_officer_document ad ON ad.document_id=d.document_id AND ad.employee_id=@EmployeeId
                    LEFT JOIN document.staff_document sd ON sd.document_id=d.document_id AND sd.employee_id=@EmployeeId
                    WHERE d.tenant_id=@TenantId AND d.document_type=r.document_type AND d.status='ACTIVE'
                      AND (td.document_id IS NOT NULL OR ad.document_id IS NOT NULL OR sd.document_id IS NOT NULL));
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<string>(new CommandDefinition(sql, new { TenantId=tenantId, EmployeeId=employeeId, StaffType=staffType }, cancellationToken:cancellationToken));
        return rows.AsList();
    }


    public async Task<bool> HasEducationAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM hr.employee_education WHERE tenant_id=@TenantId AND employee_id=@EmployeeId);";
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId=tenantId, EmployeeId=employeeId }, cancellationToken:cancellationToken));
    }
}
