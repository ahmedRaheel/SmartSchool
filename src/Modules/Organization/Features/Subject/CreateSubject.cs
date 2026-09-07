using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Features.School;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using System.Threading.Tasks;

namespace SmartSchool.Modules.Organization.Features.Subject;

public static class CreateSubject
{
    /// <summary>
    /// Represents the response returned by this SubjectEntity feature.
    /// </summary>
    /// <param name="TenantId">The owning tenant identifier.</param>
    /// <param name="Id">The entity identifier.</param>
    /// <param name="Code">The business code.</param>
    /// <param name="Name">The display name.</param>
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    string Code,
    string Name
    );

    public sealed record Request(
        Guid TenantId,
        Guid BranchId,
        string Name) : IRequest<Result<Response>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.BranchId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }    public interface ICreateSubjectCommand
    {
        Task<Result<Response>> ExecuteAsync(
            Request request,
            CancellationToken cancellationToken);
    }



    internal sealed class CreateSubjectCommand(IOrganizationDbContext dbContext, IBusinessNumberGenerator numberGenerator) : ICreateSubjectCommand
    {
        public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken cancellationToken)
        {
            var code = await numberGenerator.NextAsync(
                "SUBJECT", "SUB", request.TenantId, 8, cancellationToken);
            var subjectId = Guid.NewGuid();
            var subject = SubjectEntity.Create(
                request.TenantId, subjectId, code, request.Name);

            await dbContext.Subjects.AddAsync(subject, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result<Response>.Success(new Response(request.TenantId, subjectId, code, request.Name));
        }
    }

    public sealed class Handler(ICreateSubjectCommand command)
        : IRequestHandler<Request, Result<Response>>
    {
        public Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            return command.ExecuteAsync(request, cancellationToken);
        }
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection("academics", "subject"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateSubject")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantTeacher);
        return endpoints;
    }

}
