using FluentValidation; using SmartSchool.Application.Http; using SmartSchool.Application.Identity; using SmartSchool.Application.Messaging; using SmartSchool.Modules.HR.Persistence; using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
namespace SmartSchool.Modules.HR.Features.Employee;

public static class UpdateEmploymentStatus
{
	public sealed record Request(Guid? TenantId, Guid EmployeeId, string Status) : IRequest<Result<Response>>; public sealed record Response(Guid EmployeeId, string Status); public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.EmployeeId).NotEmpty();
			RuleFor(x => x.Status).Must(x => x is LifecycleStatuses.Submitted or LifecycleStatuses.Rejected or LifecycleStatuses.WaitingList);
		}
	}
	public sealed class Handler(IEmployeeQuery q, IEmployeeCommand c) : IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Request r, CancellationToken cancellationToken)
		{
			var e = await q.GetByIdAsync(r.TenantId!.Value, r.EmployeeId, cancellationToken);
			if (e is null)
				return Result<Response>.Failure(Error.NotFound("Employee was not found."));
			if (e.UserId.HasValue)
				return Result<Response>.Failure(Error.Conflict("An active account already exists; use termination instead."));
			e.SetRecruitmentStatus(r.Status);
			await c.UpdateAsync(e, cancellationToken);
			return Result<Response>.Success(new(e.EmployeeId, e.Status));
		}
	}
	public static void MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPut("/api/hr/employee/{employeeId:guid}/recruitment-status", async (Guid employeeId, Request request, ITenantScope scope, IMediator mediator, CancellationToken ct) => { var t = scope.Resolve(request.TenantId); if (!t.HasValue) return Results.BadRequest(); return (await mediator.SendAsync<Request, Result<Response>>(request with { TenantId = t.Value, EmployeeId = employeeId }, ct)).ToHttpResult(); }).RequireAuthorization();
	}
}
