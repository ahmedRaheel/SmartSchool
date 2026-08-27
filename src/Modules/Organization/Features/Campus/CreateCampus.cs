using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Campus;

public static class CreateCampus
{
    private static readonly string[] BranchTypes = ["HEAD_OFFICE", "REGIONAL_HEAD_OFFICE", "REGIONAL_BRANCH"];

    public sealed record Request(
        Guid TenantId, Guid SchoolId, string Name, string BranchType,
        string? Address, string? City, string? Province, string? Country, string? Phone, string? Fax,
        string? Mobile, string? Email, string? LogoUrl) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId, Guid Id, Guid SchoolId, string Code, string Name, string BranchType,
        string? Address, string? City, string? Province, string? Country, string? Phone, string? Fax,
        string? Mobile, string? Email, string? LogoUrl);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.SchoolId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.BranchType).Must(value => BranchTypes.Contains(value)).WithMessage("A valid branch type is required.");
            RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        }
    }

    public sealed class Handler(ICampusCommand command, ISchoolQuery schoolQuery, IBusinessNumberGenerator numberGenerator) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var school = await schoolQuery.GetByIdAsync(request.TenantId, request.SchoolId, cancellationToken);
            if (school is null)
            {
                return Result<Response>.Failure(Error.NotFound("The selected school was not found in this tenant."));
            }

            var code = await numberGenerator.NextAsync(
                "BRANCH", "BR", request.TenantId, 3, cancellationToken);

            var campus = CampusEntity.Create(
                request.TenantId, request.SchoolId, code, request.Name, request.BranchType,
                request.Address, request.City, request.Province, request.Country, request.Phone, request.Fax,
                request.Mobile, request.Email, request.LogoUrl);

            await command.AddAsync(campus, cancellationToken);
            return Result<Response>.Success(Map(campus));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "campus"),
            async (Request request, ITenantScope tenantScope, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var tenantId = tenantScope.Resolve(request.TenantId) ?? request.TenantId;
                var command = request with { TenantId = tenantId };
                return (await mediator.SendAsync<Request, Result<Response>>(command, cancellationToken)).ToHttpResult();
            })
            .WithName("CreateCampus")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);

        return endpoints;
    }

    private static Response Map(CampusEntity campus) => new(
        campus.TenantId, campus.CampusId, campus.SchoolId, campus.Code, campus.Name, campus.BranchType,
        campus.Address, campus.City, campus.Province, campus.Country, campus.Phone, campus.Fax, campus.Mobile, campus.Email, campus.LogoUrl);
}
