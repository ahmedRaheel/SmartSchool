using Dapper;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Features.StudentGuardian;

/// <summary>Links a real guardian record to a student. This relationship is mandatory before admission approval.</summary>
public static class LinkStudentGuardian
{
    public sealed record Request(Guid? TenantId, Guid StudentId, Guid GuardianId, string Relationship, bool IsPrimary = true) : IRequest<Result<Response>>;
    public sealed record Response(Guid StudentGuardianId, Guid StudentId, Guid GuardianId, string Relationship, bool IsPrimary);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.StudentId).NotEmpty();
            RuleFor(x => x.GuardianId).NotEmpty();
            RuleFor(x => x.Relationship).NotEmpty().MaximumLength(30);
        }
    }

    public sealed class Handler(IDbConnectionFactory factory) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            var valid = await connection.ExecuteScalarAsync<bool>(new CommandDefinition(
                "SELECT EXISTS(SELECT 1 FROM student.student s JOIN student.guardian g ON g.tenant_id=s.tenant_id WHERE s.tenant_id=@TenantId AND s.student_id=@StudentId AND g.guardian_id=@GuardianId)",
                new { TenantId=request.TenantId!.Value, request.StudentId, request.GuardianId }, cancellationToken:cancellationToken));
            if (!valid) return Result<Response>.Failure(Error.Validation("Student and guardian must belong to the same tenant."));

            var id = Guid.NewGuid();
            await connection.ExecuteAsync(new CommandDefinition(
                """INSERT INTO student.student_guardian(student_guardian_id,tenant_id,student_id,guardian_id,relationship,is_primary,can_view_academics,can_view_finance,can_pickup,is_active,created_at,row_version)
                    VALUES(@Id,@TenantId,@StudentId,@GuardianId,@Relationship,@IsPrimary,true,true,true,true,now(),0)""",
                new { Id=id, TenantId=request.TenantId.Value, request.StudentId, request.GuardianId, Relationship=request.Relationship.ToUpperInvariant(), request.IsPrimary }, cancellationToken:cancellationToken));
            return Result<Response>.Success(new Response(id, request.StudentId, request.GuardianId, request.Relationship.ToUpperInvariant(), request.IsPrimary));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/students/student-guardian/link", async (Request request, ITenantScope scope, IMediator mediator, CancellationToken ct) =>
        {
            var tenantId = scope.Resolve(request.TenantId);
            if (!tenantId.HasValue) return Results.BadRequest(new { message = "Tenant is required for SuperAdmin." });
            return (await mediator.SendAsync<Request, Result<Response>>(request with { TenantId=tenantId.Value }, ct)).ToHttpResult();
        }).WithName("LinkStudentGuardian").WithTags("Students").RequireAuthorization();
        return endpoints;
    }
}
