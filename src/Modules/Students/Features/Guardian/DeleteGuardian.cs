using SmartSchool.Modules.Students.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Guardian;

public static class DeleteGuardian
{
    public sealed record Command(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId,
        Guid Id);

    public interface IDeleteGuardianCommand
    {
        Task DeleteAsync(
                GuardianEntity entity,
                CancellationToken cancellationToken);

        Task<GuardianEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);

    }

    internal sealed class DeleteGuardianCommand(
        IStudentsDbContext dbContext) : IDeleteGuardianCommand
    {
        public async Task DeleteAsync(
                GuardianEntity entity,
                CancellationToken cancellationToken)
            {
                dbContext.Guardians
                    .Remove(entity);

                await dbContext.SaveChangesAsync(cancellationToken);
            }

        public Task<GuardianEntity?> GetByIdAsync(
            Guid tenantId, Guid id, CancellationToken cancellationToken)
        {
            return dbContext.Guardians
                .SingleOrDefaultAsync(x => x.TenantId == tenantId && x.GuardianId == id, cancellationToken);
        }
}

    public sealed class Handler(IDeleteGuardianCommand command)
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
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(GuardianEntity))));
            }
            await command.DeleteAsync(entity, cancellationToken);
            return Result<Response>.Success(new Response(request.TenantId, request.Id));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "guardian"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Command(tenantId, id);
                    var result = await mediator.SendAsync<Command, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("DeleteGuardian")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantStudent);
        return endpoints;
    }
}
