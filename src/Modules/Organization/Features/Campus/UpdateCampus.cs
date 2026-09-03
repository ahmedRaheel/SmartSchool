using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.Modules.Organization.Enums;
using SmartSchool.Modules.Organization.Features.School;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Campus;

public static class UpdateCampus
{
    public sealed record Request(Guid? TenantId,Guid CampusId,  Guid SchoolId, string Name, BranchType BranchType, Guid BranchGenderTypeId, Guid? AcademicSystemId, IReadOnlyCollection<Guid>? EducationLevelIds, string? Address, string? City, string? Province, string? Country, string? Phone, string? Fax, string? Mobile, string? Email, string? LogoUrl) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id, Guid SchoolId, string Name, BranchType BranchType, Guid BranchGenderTypeId, Guid? AcademicSystemId, IReadOnlyCollection<Guid>? EducationLevelIds, string? Address, string? City, string? Province, string? Country, string? Phone, string? Fax, string? Mobile, string? Email, string? LogoUrl);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
           
            RuleFor(x => x.SchoolId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.BranchType).IsInEnum();
            RuleFor(x => x.BranchGenderTypeId).NotEmpty();
            
            RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        }
    }

    public sealed class Handler(ITenantScope tenantScope, ICampusQuery query, ICampusCommand command, ISchoolQuery schoolQuery, IBranchPolicyCommand policyCommand) : IRequestHandler<Request, Result<Response>>
    {
		public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
			if (!tenantId.HasValue)
				return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            var campus = await query.GetByIdAsync(tenantId.Value, request.CampusId, cancellationToken);
			if (campus is null)
				return Result<Response>.Failure(Error.NotFound("Branch was not found."));
			if (await schoolQuery.GetByIdAsync(tenantId.Value, request.SchoolId, cancellationToken) is null)
				return Result<Response>.Failure(Error.NotFound("The selected school was not found in this tenant."));
			if (!await policyCommand.GenderTypeExistsAsync(request.BranchGenderTypeId, cancellationToken))
				return Result<Response>.Failure(Error.Validation("Select a valid branch gender type."));
			var educationLevelIds = request.EducationLevelIds ?? Array.Empty<Guid>();
			if (educationLevelIds.Count > 0 && !await policyCommand.EducationLevelsExistAsync(educationLevelIds, cancellationToken))
				return Result<Response>.Failure(Error.Validation("One or more education levels are invalid."));
            campus.UpdateDetails(campus.Code, request.Name, request.BranchType, request.BranchGenderTypeId, request.AcademicSystemId, request.Address, request.City, request.Province, request.Country, request.Phone, request.Fax, request.Mobile, request.Email, request.LogoUrl);
            await command.UpdateAsync(campus, cancellationToken);
            await policyCommand.SetEducationLevelsAsync(campus.CampusId, educationLevelIds, cancellationToken);
			return Result<Response>.Success(new Response(campus.CampusId, campus.SchoolId, campus.Name, campus.BranchType, campus.BranchGenderTypeId, campus.AcademicSystemId, educationLevelIds, campus.Address, campus.City, campus.Province, campus.Country, campus.Phone, campus.Fax, campus.Mobile, campus.Email, campus.LogoUrl));
        }
	}

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(ApiRoutes.EntityById(ModuleConstants.RouteSegment, "campus"), async (Guid id, Request request, IMediator mediator, CancellationToken ct) => (await mediator.SendAsync<Request, Result<Response>>(request with { CampusId = id }, ct)).ToHttpResult())
            .WithName("UpdateCampus").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
        return endpoints;
    }
}
