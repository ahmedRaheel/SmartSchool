using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Features.Student;

public static class StrikeOffStudent
{
    public sealed record Request(Guid TenantId, Guid StudentId, string Reason) : IRequest<Result<Response>>;
    public sealed record Response(Guid StudentId, string Status);

    public sealed class Handler(IStudentQuery query, IStudentCommand command, IIdentityAccountService accounts)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var student = await query.GetByIdAsync(request.TenantId, request.StudentId, cancellationToken);
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
