using SmartSchool.Modules.Students.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Modules.Students.Models;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;

using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Features.Student;

public static class StrikeOffStudent
{
    public sealed record Request(Guid TenantId, Guid StudentId, string Reason) : IRequest<Result<Response>>;
    public sealed record Response(Guid StudentId, string Status);

    public sealed class Handler(StrikeOffStudentStudentCommand command, IIdentityAccountService accounts)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var student = await command.GetByIdAsync(request.TenantId, request.StudentId, cancellationToken);
            if (student is null) return Result<Response>.Failure(Error.NotFound("Student was not found."));
            if (student.UserId.HasValue) await accounts.DeactivateAccountAsync(student.UserId.Value, cancellationToken);
            student.StrikeOff();
            await command.UpdateAsync(student, cancellationToken);
            return Result<Response>.Success(new Response(student.StudentId, student.Status));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/students/student/{studentId:guid}/strike-off", async (Guid studentId, Request request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = request with { StudentId = studentId };
            return (await mediator.SendAsync<Request, Result<Response>>(command, cancellationToken)).ToHttpResult();
        }).WithName("StrikeOffStudent").WithTags("Students").RequireAuthorization();
        return endpoints;
    }
}

/// <summary>
/// Feature-owned data access for StrikeOffStudent. Do not share across slices.
/// </summary>
public sealed class StrikeOffStudentStudentCommand(IStudentsDbContext dbContext)
{
    public Task<StudentEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
    {
        return dbContext.Students.SingleOrDefaultAsync(
            entity => entity.TenantId == tenantId && entity.StudentId == id, cancellationToken);
    }


    public async Task UpdateAsync(
        StudentEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Students
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}


