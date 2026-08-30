using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;
using SmartSchool.Modules.Students.Features.Student;
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

    public sealed class Handler(
        IStudentOnboardingQuery query,
        IStudentOnboardingCommand command)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var tenantId = request.TenantId!.Value;
            var valid = await query.StudentAndGuardianBelongToTenantAsync(
                tenantId,
                request.StudentId,
                request.GuardianId,
                cancellationToken);

            if (!valid)
            {
                return Result<Response>.Failure(
                    Error.Validation("Student and guardian must belong to the same tenant."));
            }

            var link = StudentGuardianEntity.Link(
                tenantId,
                request.StudentId,
                request.GuardianId,
                request.Relationship,
                request.IsPrimary);

            await command.AddGuardianLinkAsync(link, cancellationToken);

            return Result<Response>.Success(
                new Response(
                    link.StudentGuardianId,
                    link.StudentId,
                    link.GuardianId,
                    link.Relationship,
                    link.IsPrimary));
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
