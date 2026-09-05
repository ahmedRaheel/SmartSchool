using SmartSchool.Modules.Examinations.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Examinations.Features.StudentExamResult;

public static class DeleteStudentExamResult
{
    public sealed record Command(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId,
        Guid Id);

    public interface IDeleteStudentExamResult
    {
        Task DeleteAsync(
                StudentExamResultEntity entity,
                CancellationToken cancellationToken);

        Task<StudentExamResultEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);

    }

    internal sealed class DeleteStudentExamResultPersistence(
        IExaminationsDbContext dbContext) : IDeleteStudentExamResult
    {
        public async Task DeleteAsync(
                StudentExamResultEntity entity,
                CancellationToken cancellationToken)
            {
                dbContext.StudentExamResults
                    .Remove(entity);

                await dbContext.SaveChangesAsync(cancellationToken);
            }

        public Task<StudentExamResultEntity?> GetByIdAsync(
            Guid tenantId, Guid id, CancellationToken cancellationToken)
        {
            return dbContext.StudentExamResults
                .SingleOrDefaultAsync(x => x.TenantId == tenantId && x.StudentExamResultId == id, cancellationToken);
        }
}

    public sealed class Handler(IDeleteStudentExamResult dataAccess)
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
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(StudentExamResultEntity))));
            }
            await dataAccess.DeleteAsync(entity, cancellationToken);
            return Result<Response>.Success(new Response(request.TenantId, request.Id));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student-exam-result"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Command(tenantId, id);
                    var result = await mediator.SendAsync<Command, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("DeleteStudentExamResult")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
