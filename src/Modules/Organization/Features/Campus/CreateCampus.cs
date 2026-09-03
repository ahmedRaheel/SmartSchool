using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.Modules.Organization.Enums;
using SmartSchool.Modules.Organization.Features.School;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Campus;

public static class CreateCampus
{
    public sealed record Request(Guid? TenantId, Guid SchoolId, string Name, BranchType BranchType, Guid BranchGenderTypeId, Guid? AcademicSystemId, IReadOnlyCollection<Guid>? EducationLevelIds, string? Address, string? City, string? Province, string? Country, string? Phone, string? Fax, string? Mobile, string? Email, string? LogoUrl) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id, Guid SchoolId, string Code, string Name, BranchType BranchType, Guid BranchGenderTypeId, Guid? AcademicSystemId, IReadOnlyCollection<Guid>? EducationLevelIds, string? Address, string? City, string? Province, string? Country, string? Phone, string? Fax, string? Mobile, string? Email, string? LogoUrl);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.SchoolId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.BranchType).IsInEnum().WithMessage("A valid branch type is required.");
            RuleFor(x => x.BranchGenderTypeId).NotEmpty();
            
            RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        }
    }

    public sealed class Handler(ITenantScope tenantScope, ICampusCommand command, ISchoolQuery schoolQuery, IBranchPolicyCommand policyCommand, IBusinessNumberGenerator numberGenerator) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue) return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            if (await schoolQuery.GetByIdAsync(tenantId.Value, request.SchoolId, cancellationToken) is null) return Result<Response>.Failure(Error.NotFound("The selected school was not found in this tenant."));
            if (!await policyCommand.GenderTypeExistsAsync(request.BranchGenderTypeId, cancellationToken)) return Result<Response>.Failure(Error.Validation("Select a valid branch gender type."));
            var educationLevelIds = request.EducationLevelIds ?? Array.Empty<Guid>();
            if (educationLevelIds.Count > 0 && !await policyCommand.EducationLevelsExistAsync(educationLevelIds, cancellationToken)) return Result<Response>.Failure(Error.Validation("One or more education levels are invalid."));

            var code = await numberGenerator.NextAsync("BRANCH", "BR", tenantId.Value, 3, cancellationToken);
            var campus = CampusEntity.Create(tenantId.Value, request.SchoolId, code, request.Name, request.BranchType, request.BranchGenderTypeId, request.AcademicSystemId, request.Address, request.City, request.Province, request.Country, request.Phone, request.Fax, request.Mobile, request.Email, request.LogoUrl);
            await command.AddAsync(campus, cancellationToken);
            await policyCommand.SetEducationLevelsAsync(campus.CampusId, educationLevelIds, cancellationToken);
            return Result<Response>.Success(Map(campus, educationLevelIds));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "campus"), async (Request request, IMediator mediator, CancellationToken ct) => (await mediator.SendAsync<Request, Result<Response>>(request, ct)).ToHttpResult())
            .WithName("CreateCampus").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
        return endpoints;
    }

    private static Response Map(CampusEntity campus, IReadOnlyCollection<Guid> levels) => new(campus.CampusId, campus.SchoolId, campus.Code, campus.Name, campus.BranchType, campus.BranchGenderTypeId, campus.AcademicSystemId, levels, campus.Address, campus.City, campus.Province, campus.Country, campus.Phone, campus.Fax, campus.Mobile, campus.Email, campus.LogoUrl);
}
