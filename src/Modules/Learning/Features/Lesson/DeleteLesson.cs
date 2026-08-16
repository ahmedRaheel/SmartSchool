using SmartSchool.Modules.Learning;
using SmartSchool.Modules.Learning.Persistence;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Learning.Features.Lesson;

public static class DeleteLesson
{
    public sealed record Command(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        ILessonQuery query,
        ILessonCommand command)
    {
        public async Task<Result<bool>> HandleAsync(
            Command command,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                command.TenantId,
                command.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<bool>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Lesson))));
            }

            await command.DeleteAsync(
                entity,
                cancellationToken);

            return Result<bool>.Success(true);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                "/api/learning/lesson/{id:guid}",
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
            .WithName("DeleteLesson")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
