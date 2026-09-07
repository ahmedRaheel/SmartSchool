using SmartSchool.Modules.Organization.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Program;

public static class DeleteProgram
{
    public sealed record Command(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId,
        Guid Id);

    public interface IDeleteProgramCommand
    {
        Task DeleteAsync(
                ProgramEntity entity,
                CancellationToken cancellationToken);

        Task<ProgramEntity?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken);

    }

    internal sealed class DeleteProgramCommand(IOrganizationDbContext dbContext) : IDeleteProgramCommand
    {
        public async Task DeleteAsync(
                ProgramEntity entity,
                CancellationToken cancellationToken)
            {
                dbContext
                    .Programs
                    .Remove(entity);

                await dbContext.SaveChangesAsync(cancellationToken);
            }

        public async Task<ProgramEntity?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken)
            {
                return await dbContext
                    .Programs
                    .FirstOrDefaultAsync(
                        x => x.TenantId == tenantId
                            && x.ProgramId == id,
                        cancellationToken);
            }
    }

    public sealed class Handler(IDeleteProgramCommand command)
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
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(ProgramEntity))));
            }
            await command.DeleteAsync(entity, cancellationToken);
            return Result<Response>.Success(new Response(request.TenantId, request.Id));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                ApiRoutes.EntityById("academics", "program"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Command(tenantId, id);
                    var result = await mediator.SendAsync<Command, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("DeleteProgram")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantTeacher);
        return endpoints;
    }
}
