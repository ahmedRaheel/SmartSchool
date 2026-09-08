using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Features.TeachingAssignment;

public static class CreateTeachingAssignment
{
    public sealed record Request(
        Guid TenantId,
        Guid SchoolId,
        Guid CampusId,
        Guid EmployeeId,
        Guid ClassSectionId,
        Guid SubjectId,
        string Name,
        int? PeriodsPerWeek,
        bool IsClassTeacher,
        DateOnly? EffectiveFrom,
        DateOnly? EffectiveTo)
        : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TeacherTeachingAssignmentId,
        string Code,
        string Name);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(request => request.TenantId).NotEmpty();
            RuleFor(request => request.SchoolId).NotEmpty();
            RuleFor(request => request.CampusId).NotEmpty();
            RuleFor(request => request.EmployeeId).NotEmpty();
            RuleFor(request => request.ClassSectionId).NotEmpty();
            RuleFor(request => request.SubjectId).NotEmpty();
            RuleFor(request => request.Name).NotEmpty();
        }
    }

    public interface ICreateTeachingAssignmentCommand
    {
        Task<Response> ExecuteAsync(
            Request request,
            CancellationToken cancellationToken);
    }

    internal sealed class CreateTeachingAssignmentCommand(
        IHRDbContext dbContext,
        IBusinessNumberGenerator numberGenerator)
        : ICreateTeachingAssignmentCommand
    {
        public async Task<Response> ExecuteAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var code = await numberGenerator.NextAsync(
                "TeachingAssignment",
                "TAS",
                request.TenantId,
                5,
                cancellationToken);

            var entity = TeacherTeachingAssignmentEntity.Create(
                request.TenantId,
                request.SchoolId,
                request.CampusId,
                request.EmployeeId,
                request.ClassSectionId,
                request.SubjectId,
                code,
                request.Name,
                request.PeriodsPerWeek,
                request.IsClassTeacher,
                request.EffectiveFrom,
                request.EffectiveTo);

            await dbContext.TeacherTeachingAssignments.AddAsync(
                entity,
                cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new Response(
                entity.TeacherTeachingAssignmentId,
                entity.Code,
                entity.Name);
        }
    }

    public sealed class Handler(
        ICreateTeachingAssignmentCommand command)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var response = await command.ExecuteAsync(
                request,
                cancellationToken);

            return Result<Response>.Success(response);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/api/hr/teaching-assignment",
                async (
                    Request request,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<Response>>(
                        request,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("CreateTeachingAssignment")
            .WithTags("HR")
            .RequireAuthorization();

        return endpoints;
    }
}
