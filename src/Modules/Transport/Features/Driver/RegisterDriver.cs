using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Transport.Models;
using SmartSchool.Modules.Transport.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Transport.Features.Driver;

public static class RegisterDriver
{
    public sealed record Request(Guid TenantId, Guid EmployeeId, DateOnly DateOfBirth,
        string LicenseNumber, string LicenseCategory, DateOnly LicenseExpiry) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id, string Name);
    public sealed record Employee(string Number, string FirstName, string? LastName, string Cnic, string? Phone, DateOnly HireDate);
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty(); RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.DateOfBirth).NotEmpty().LessThan(DateOnly.FromDateTime(DateTime.UtcNow));
            RuleFor(x => x.LicenseNumber).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LicenseCategory).NotEmpty().MaximumLength(40);
            RuleFor(x => x.LicenseExpiry).GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow));
        }
    }
    public interface IRegisterDriverQuery { Task<Employee?> GetEmployeeAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class RegisterDriverQuery(IDbConnectionFactory factory, ICurrentUser user) : IRegisterDriverQuery
    {
        public async Task<Employee?> GetEmployeeAsync(Request request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT employee_number AS "Number", first_name AS "FirstName", last_name AS "LastName",
                    cnic_number AS "Cnic", phone AS "Phone", hire_date AS "HireDate"
                FROM hr.employee WHERE tenant_id = @TenantId AND employee_id = @EmployeeId AND is_active
                    AND upper(staff_type) = 'DRIVER' AND status IN ('ACTIVE', 'APPROVED')
                    AND (@CampusId IS NULL OR branch_id = @CampusId);
                """;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<Employee>(new CommandDefinition(sql,
                new { request.TenantId, request.EmployeeId, CampusId = user.BranchId }, cancellationToken: cancellationToken));
        }
    }
    public interface IRegisterDriverCommand { Task<Result<Response>> AddAsync(DriverEntity entity, CancellationToken cancellationToken); }
    internal sealed class RegisterDriverCommand(ITransportDbContext db) : IRegisterDriverCommand
    {
        public async Task<Result<Response>> AddAsync(DriverEntity entity, CancellationToken cancellationToken)
        {
            if (await db.Drivers.AnyAsync(x => x.TenantId == entity.TenantId && x.EmployeeId == entity.EmployeeId && x.IsActive, cancellationToken))
                return Result<Response>.Failure(Error.Conflict("A driver profile already exists for this employee."));
            db.Drivers.Add(entity); await db.SaveChangesAsync(cancellationToken);
            return Result<Response>.Success(new(entity.DriverId, entity.FullName));
        }
    }
    public sealed class Handler(IRegisterDriverQuery query, IRegisterDriverCommand command) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var employee = await query.GetEmployeeAsync(request, cancellationToken);
            if (employee is null) return Result<Response>.Failure(Error.Validation("Select an approved driver employee from this campus."));
            return await command.AddAsync(DriverEntity.Register(request.TenantId, request.EmployeeId, employee.Number, employee.FirstName,
                employee.LastName, employee.Cnic, employee.Phone, request.DateOfBirth, employee.HireDate,
                request.LicenseNumber, request.LicenseCategory, request.LicenseExpiry), cancellationToken);
        }
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/transport/operations/drivers", async (Request request, ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(request.TenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Request, Result<Response>>(request with { TenantId = tenant.Value }, cancellationToken)).ToHttpResult();
        }).WithName("RegisterDriver").WithTags("Transport").RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);
        return endpoints;
    }
}
