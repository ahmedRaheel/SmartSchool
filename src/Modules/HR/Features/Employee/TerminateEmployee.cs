using Dapper;
using SmartSchool.Modules.HR.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;

using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Features.Employee;

public static class TerminateEmployee
{
    public sealed record Request(Guid TenantId, Guid EmployeeId, string Reason) : IRequest<Result<Response>>;
    public sealed record Response(Guid EmployeeId, string Status);

    public sealed class Handler(TerminateEmployeeEmployeeQuery query, TerminateEmployeeEmployeeCommand command, IIdentityAccountService accounts)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var employee = await command.GetByIdAsync(request.TenantId, request.EmployeeId, cancellationToken);
            if (employee is null) return Result<Response>.Failure(Error.NotFound("Employee was not found."));
            if (employee.UserId.HasValue) await accounts.DeactivateAccountAsync(employee.UserId.Value, cancellationToken);
            employee.Terminate();
            await command.UpdateAsync(employee, cancellationToken);
            return Result<Response>.Success(new Response(employee.EmployeeId, employee.Status));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/hr/employee/{employeeId:guid}/terminate", async (Guid employeeId, Request request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = request with { EmployeeId = employeeId };
            return (await mediator.SendAsync<Request, Result<Response>>(command, cancellationToken)).ToHttpResult();
        }).WithName("TerminateEmployee").WithTags("HR").RequireAuthorization();
        return endpoints;
    }
}

/// <summary>
/// Feature-owned data access for TerminateEmployee. Do not share across slices.
/// </summary>
public sealed class TerminateEmployeeEmployeeCommand(IHRDbContext dbContext)
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
/// Feature-owned data access for TerminateEmployee. Do not share across slices.
/// </summary>
public sealed class TerminateEmployeeEmployeeQuery(IDbConnectionFactory connectionFactory)
{
    
