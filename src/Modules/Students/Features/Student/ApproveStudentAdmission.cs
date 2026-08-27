using Dapper;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Student;

public static class ApproveStudentAdmission
{
    public sealed record Request(Guid TenantId, Guid StudentId, string Email) : IRequest<Result<Response>>;
    public sealed record Response(Guid StudentId, Guid UserId, string StudentNumber, string Status);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.StudentId).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }

    public sealed class Handler(IStudentQuery query, IStudentCommand command, IIdentityAccountService accounts, IBusinessNumberGenerator numberGenerator, IDbConnectionFactory connectionFactory)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var student = await query.GetByIdAsync(request.TenantId, request.StudentId, cancellationToken);
            if (student is null) return Result<Response>.Failure(Error.NotFound("Student was not found."));
            if (student.UserId.HasValue) return Result<Response>.Failure(Error.Conflict("Student already has a login account."));

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var branchCode = await connection.ExecuteScalarAsync<string>(new CommandDefinition(
                "SELECT code FROM org.campus WHERE tenant_id=@TenantId AND campus_id=@BranchId",
                new { request.TenantId, student.BranchId }, cancellationToken: cancellationToken));
            if (string.IsNullOrWhiteSpace(branchCode)) return Result<Response>.Failure(Error.Validation("The student's branch is invalid."));
            var studentNumber = await numberGenerator.NextAsync(
                $"STUDENT:{student.BranchId}", $"{branchCode}-", request.TenantId, 7, cancellationToken);

            var account = await accounts.CreateAccountAsync(
                request.TenantId, student.StudentId, "Student", request.Email, student.FirstName, student.LastName ?? string.Empty,
                student.SchoolId, student.BranchId, new[] { "Student" }, cancellationToken);

            student.ApproveAdmission(account.UserId, studentNumber);
            await command.UpdateAsync(student, cancellationToken);
            return Result<Response>.Success(new Response(student.StudentId, account.UserId, student.StudentNumber!, student.Status));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/students/student/{studentId:guid}/approve", async (Guid studentId, Request request, ITenantScope tenantScope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue) return Results.BadRequest(new { message = "Tenant is required for SuperAdmin." });
            var command = request with { TenantId = tenantId.Value, StudentId = studentId };
            return (await mediator.SendAsync<Request, Result<Response>>(command, cancellationToken)).ToHttpResult();
        }).WithName("ApproveStudentAdmission").WithTags("Students").RequireAuthorization();
        return endpoints;
    }
}
