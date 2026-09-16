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

namespace SmartSchool.Modules.Transport.Features.Operations;

public static class AssignTransportStudent
{
    public sealed record Request(Guid TenantId, Guid StudentId, Guid RouteId, Guid StopId) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id, Guid StudentId, Guid RouteId, Guid StopId);
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator() { RuleFor(x => x.TenantId).NotEmpty(); RuleFor(x => x.StudentId).NotEmpty(); RuleFor(x => x.RouteId).NotEmpty(); RuleFor(x => x.StopId).NotEmpty(); }
    }
    public interface IAssignTransportStudentQuery { Task<bool> ValidateAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class AssignTransportStudentQuery(IDbConnectionFactory factory, ICurrentUser user) : IAssignTransportStudentQuery
    {
        public async Task<bool> ValidateAsync(Request request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT EXISTS (SELECT 1 FROM transport.route r JOIN transport.stop st ON st.route_id = r.route_id AND st.tenant_id = r.tenant_id AND st.is_active
                    JOIN student.student s ON s.tenant_id = r.tenant_id AND s.branch_id = r.campus_id AND s.is_active AND s.status = 'ACTIVE'
                    WHERE r.tenant_id = @TenantId AND r.route_id = @RouteId AND r.is_active AND st.stop_id = @StopId AND s.student_id = @StudentId
                        AND (@CampusId IS NULL OR r.campus_id = @CampusId));
                """;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { request.TenantId, request.StudentId, request.RouteId, request.StopId, CampusId = user.BranchId }, cancellationToken: cancellationToken));
        }
    }
    public interface IAssignTransportStudentCommand { Task<Result<Response>> SaveAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class AssignTransportStudentCommand(ITransportDbContext db) : IAssignTransportStudentCommand
    {
        public async Task<Result<Response>> SaveAsync(Request request, CancellationToken cancellationToken)
        {
            await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
            var route = await db.Routes.FromSqlInterpolated($"SELECT * FROM transport.route WHERE tenant_id = {request.TenantId} AND route_id = {request.RouteId} FOR UPDATE").SingleAsync(cancellationToken);
            var vehicle = await db.Vehicles.SingleAsync(x => x.TenantId == request.TenantId && x.VehicleId == route.VehicleId && x.IsActive, cancellationToken);
            var seats = await db.StudentTransports.CountAsync(x => x.TenantId == request.TenantId && x.RouteId == request.RouteId && x.StudentId != request.StudentId && x.IsActive, cancellationToken);
            if (!vehicle.Capacity.HasValue || seats >= vehicle.Capacity.Value) return Result<Response>.Failure(Error.Conflict("This route's vehicle is at capacity."));
            var entity = await db.StudentTransports.SingleOrDefaultAsync(x => x.TenantId == request.TenantId && x.StudentId == request.StudentId && x.IsActive, cancellationToken);
            if (entity is null) { entity = StudentTransportEntity.Assign(request.TenantId, request.StudentId, request.RouteId, request.StopId); db.StudentTransports.Add(entity); }
            else entity.MoveTo(request.RouteId, request.StopId);
            await db.SaveChangesAsync(cancellationToken); await transaction.CommitAsync(cancellationToken);
            return Result<Response>.Success(new(entity.StudentTransportId, request.StudentId, request.RouteId, request.StopId));
        }
    }
    public sealed class Handler(IAssignTransportStudentQuery query, IAssignTransportStudentCommand command) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken) =>
            await query.ValidateAsync(request, cancellationToken) ? await command.SaveAsync(request, cancellationToken)
                : Result<Response>.Failure(Error.Validation("Student, route and stop must belong to the same campus."));
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/transport/operations/assignments", async (Request request, ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(request.TenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Request, Result<Response>>(request with { TenantId = tenant.Value }, cancellationToken)).ToHttpResult();
        }).WithName("AssignTransportStudent").WithTags("Transport").RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);
        return endpoints;
    }
}
