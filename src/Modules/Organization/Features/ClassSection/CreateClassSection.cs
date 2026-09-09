using SmartSchool.Modules.Organization.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Organization.Features.ClassSection;

public static class CreateClassSection
{
    /// <summary>
    /// Represents the response returned by this ClassSectionEntity feature.
    /// </summary>
    /// <param name="TenantId">The owning tenant identifier.</param>
    /// <param name="Id">The entity identifier.</param>
    /// <param name="Code">The business code.</param>
    /// <param name="Name">The display name.</param>
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    string Code,
    string Name,
    string? MetadataJson);

    public sealed record Request(
        Guid TenantId,
        Guid CampusId,
        Guid AcademicYearId,
        Guid GradeLevelId,
        string Name,
        int? Capacity = null,
        string? RoomNo = null) : IRequest<Result<Response>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.CampusId).NotEmpty();
            RuleFor(x => x.AcademicYearId).NotEmpty();
            RuleFor(x => x.GradeLevelId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }

    public interface ICreateClassSectionCommand
    {
        Task AddAsync(
            ClassSectionEntity entity,
            CancellationToken cancellationToken);
}

    internal sealed class CreateClassSectionCommand(IOrganizationDbContext dbContext) : ICreateClassSectionCommand
    {
        public async Task AddAsync(
            ClassSectionEntity entity,
            CancellationToken cancellationToken)
        {

            await dbContext.ClassSections.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public sealed class Handler(ICreateClassSectionCommand command, IBusinessNumberGenerator numberGenerator)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {

            var code = await numberGenerator.NextAsync("Section", "SEC", request.TenantId, 3, cancellationToken);

            var entity = ClassSectionEntity.Create(
                request.TenantId,
                request.CampusId,
                request.AcademicYearId,
                request.GradeLevelId,
                 code,
                request.Name,
                capacity: request.Capacity,
                roomNo: request.RoomNo);

            await command.AddAsync(entity, cancellationToken);
            return Result<Response>.Success(MapResponse(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection("academics", "class-section"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateClassSection")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantTeacher);
        return endpoints;
    }

    private static Response MapResponse(ClassSectionEntity entity)
    {
        return new Response(
            entity.TenantId,
            entity.ClassSectionId,
            entity.Code,
            entity.Name,
            entity.MetadataJson);
    }
}
