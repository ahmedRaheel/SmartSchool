using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features;

public static class UpdateAdmissionCriteria
{
    public sealed record Request(Guid? TenantId, Guid Id, decimal MinimumMarks, decimal? EntranceTestMinimum, int? MinimumAge, int? MaximumAge, bool InterviewRequired, string? RequiredDocuments) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.MinimumMarks).InclusiveBetween(0, 100);
            RuleFor(x => x.EntranceTestMinimum).InclusiveBetween(0, 100).When(x => x.EntranceTestMinimum.HasValue);
            RuleFor(x => x.MinimumAge).GreaterThanOrEqualTo(0).When(x => x.MinimumAge.HasValue);
            RuleFor(x => x.MaximumAge).GreaterThanOrEqualTo(x => x.MinimumAge ?? 0).When(x => x.MaximumAge.HasValue);
            RuleFor(x => x.RequiredDocuments).MaximumLength(4000);
        }
    }

    public interface IUpdateAdmissionCriteria
    {
        Task<bool> ExecuteAsync(Guid tenantId, Request request, CancellationToken cancellationToken);
    }

    internal sealed class Command(IAdmissionsDbContext dbContext) : IUpdateAdmissionCriteria
    {
        public async Task<bool> ExecuteAsync(Guid tenantId, Request request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.AdmissionCriteria.SingleOrDefaultAsync(
                x => x.TenantId == tenantId && x.AdmissionCriteriaId == request.Id && x.Status == "ACTIVE",
                cancellationToken);
            if (entity is null)
            {
                return false;
            }

            entity.UpdateRules(request.MinimumMarks, request.EntranceTestMinimum, request.MinimumAge, request.MaximumAge, request.InterviewRequired, request.RequiredDocuments);
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    public sealed class Handler(ITenantScope tenantScope, IUpdateAdmissionCriteria command) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (tenantId is null)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            return await command.ExecuteAsync(tenantId.Value, request, cancellationToken)
                ? Result<Response>.Success(new Response(request.Id))
                : Result<Response>.Failure(Error.NotFound("Admission criteria were not found."));
        }
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/admissions/criteria/{id:guid}",
            async (Guid id, Request body, IMediator mediator, CancellationToken cancellationToken) =>
                (await mediator.SendAsync<Request, Result<Response>>(body with { Id = id }, cancellationToken)).ToHttpResult())
            .WithName("UpdateAdmissionCriteria")
            .WithTags("Admission Criteria")
            .RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);
    }
}
