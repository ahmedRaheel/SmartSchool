using SmartSchool.Modules.Activities.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Activities.Features.StudentActivity;

public static class DeleteStudentActivity
{
    public sealed record Command(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId,
        Guid Id);

    public interface IDeleteStudentActivityCommand
    {
        Task DeleteAsync(
                StudentActivityEntity entity,
                CancellationToken cancellationToken);

        Task<StudentActivityEntity?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken);

    }

    internal sealed class DeleteStudentActivityCommand(IActivitiesDbContext dbContext) : IDeleteStudentActivityCommand
    {
        public async Task DeleteAsync(
                StudentActivityEntity entity,
                CancellationToken cancellationToken)
            {
                dbContext.StudentActivities
                    .Remove(entity);

                await dbContext.SaveChangesAsync(cancellationToken);
            }

        public async Task<StudentActivityEntity?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken)
            {
                return await dbContext.StudentActivities
                    .FirstOrDefaultAsync(
                        x => x.TenantId == tenantId
                            && x.StudentActivityId == id,
                        cancellationToken);
            }
    }

    public sealed class Handler(IDeleteStudentActivityCommand command)
        : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Command request,
            CancellationToken cancellationToken)
        {
            var entity = await command.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(StudentActivityEntity))));
            }
            await command.DeleteAsync(entity, cancellationToken);
            return Result<Response>.Success(new Response(request.TenantId, request.Id));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student-activity"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Command(tenantId, id);
                    var result = await mediator.SendAsync<Command, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("DeleteStudentActivity")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
