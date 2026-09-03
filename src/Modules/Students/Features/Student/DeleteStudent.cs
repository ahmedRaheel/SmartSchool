using SmartSchool.Modules.Students.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Student;

public static class DeleteStudent
{
    public sealed record Command(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId,
        Guid Id);

    public interface IDeleteStudent
    {
        Task DeleteAsync(
                StudentEntity entity,
                CancellationToken cancellationToken);

        Task<StudentEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);

    }

    internal sealed class DeleteStudentPersistence(
        IStudentsDbContext dbContext) : IDeleteStudent
    {
        public async Task DeleteAsync(
                StudentEntity entity,
                CancellationToken cancellationToken)
            {
                dbContext.Students
                    .Remove(entity);

                await dbContext.SaveChangesAsync(cancellationToken);
            }

        public Task<StudentEntity?> GetByIdAsync(
            Guid tenantId, Guid id, CancellationToken cancellationToken)
        {
            return dbContext.Students
                .SingleOrDefaultAsync(x => x.TenantId == tenantId && x.StudentId == id, cancellationToken);
        }
}

    public sealed class Handler(IDeleteStudent dataAccess)
        : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Command request,
            CancellationToken cancellationToken)
        {
            var entity = await dataAccess.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(StudentEntity))));
            }
            await dataAccess.DeleteAsync(entity, cancellationToken);
            return Result<Response>.Success(new Response(request.TenantId, request.Id));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Command(tenantId, id);
                    var result = await mediator.SendAsync<Command, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("DeleteStudent")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantStudent);
        return endpoints;
    }
}
