using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;

using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Features.Employee;

public static class TerminateEmployee
{
    public sealed record Request(Guid TenantId, Guid EmployeeId, string Reason) : IRequest<Result<Response>>;
    public sealed record Response(Guid EmployeeId, string Status);

    public sealed class Handler(IEmployeeQuery query, IEmployeeCommand command, IIdentityAccountService accounts)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var employee = await query.GetByIdAsync(request.TenantId, request.EmployeeId, cancellationToken);
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
