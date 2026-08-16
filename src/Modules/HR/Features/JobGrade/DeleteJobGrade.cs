using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Features.JobGrade;

public static class DeleteJobGrade
{
    public sealed record Command(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<JobGrade> repository)
    {
        public async Task<Result<bool>> HandleAsync(
            Command command,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                command.TenantId,
                command.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<bool>.Failure(
                    Error.NotFound("JobGrade was not found."));
            }

            repository.Remove(entity);

            await repository.SaveChangesAsync(
                cancellationToken);

            return Result<bool>.Success(true);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                "/api/hr/job-grade/{id:guid}",
                async (
                    Guid id,
                    Guid tenantId,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var command = new Command(
                        tenantId,
                        id);

                    var result = await handler.HandleAsync(
                        command,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("DeleteJobGrade")
            .WithTags("HR")
            .RequireAuthorization();

        return endpoints;
    }
}
