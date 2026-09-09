using SmartSchool.Application.Messaging;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Identity;
using SmartSchool.Modules.HR.Models;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Teacher.ApplyTeacherLeave;

public static class ApplyTeacherLeave
{
    public sealed record Request(
        Guid? TenantId,
        Guid BranchId,
        Guid EmployeeId,
        DateOnly FromDate,
        DateOnly ToDate,
        string LeaveType,
        string Reason) : IRequest<Response>;

    public sealed record Response(Guid LeaveRequestId, string Status);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(request => request.LeaveType).NotEmpty();
            RuleFor(request => request.Reason).NotEmpty();
            RuleFor(request => request.ToDate)
                .GreaterThanOrEqualTo(request => request.FromDate);
        }
    }

    public interface IApplyTeacherLeaveCommand
    {
        Task<LeaveRequestEntity> ExecuteAsync(
            Guid tenantId,
            Guid branchId,
            Guid employeeId,
            Request request,
            CancellationToken cancellationToken);
    }

    internal sealed class ApplyTeacherLeaveCommand(IHRDbContext dbContext) : IApplyTeacherLeaveCommand
    {
        public async Task<LeaveRequestEntity> ExecuteAsync(
            Guid tenantId,
            Guid branchId,
            Guid employeeId,
            Request request,
            CancellationToken cancellationToken)
        {
            var entity = LeaveRequestEntity.CreateTeacherLeave(
                tenantId,
                branchId,
                employeeId,
                request.LeaveType,
                request.FromDate,
                request.ToDate,
                request.Reason,
                LifecycleStatuses.Pending);

            await dbContext.LeaveRequests.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }

    public sealed class Handler(IApplyTeacherLeaveCommand command) : IRequestHandler<Request, Response>
    {
        public async Task<Response> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var entity = await command.ExecuteAsync(request.TenantId!.Value, request.BranchId, request.EmployeeId, request, cancellationToken);
            return new Response(entity.LeaveRequestId, entity.Status);
        }
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("/{employeeId:guid}/leave", HandleAsync)
            .RequireAuthorization(SmartSchoolPolicies.TeacherWorkspace);
    }

    private static async Task<IResult> HandleAsync(
        Guid employeeId,
        Request request,
        ITenantScope tenantScope,
        ICurrentUser currentUser,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var tenantId = tenantScope.IsSuperAdmin
            ? request.TenantId
            : tenantScope.Resolve(request.TenantId);

        if (!tenantId.HasValue)
        {
            return Results.BadRequest(new { message = "Tenant is required." });
        }

        if (currentUser.BranchId is not Guid branchId)
        {
            return Results.Forbid();
        }

        var response = await mediator.SendAsync<Request, Response>(
            request with { TenantId = tenantId.Value, BranchId = branchId, EmployeeId = employeeId },
            cancellationToken);

        return Results.Accepted($"/api/teachers/{employeeId}/leave/{response.LeaveRequestId}", response);
    }
}
