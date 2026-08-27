using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Campus;

public static class UpdateCampus
{
    private static readonly string[] BranchTypes = ["HEAD_OFFICE", "REGIONAL_HEAD_OFFICE", "REGIONAL_BRANCH"];

    public sealed record Request(
        Guid TenantId, Guid Id, Guid SchoolId, string Code, string Name, string BranchType,
        string? Address, string? City, string? Province, string? Phone, string? Fax,
        string? Mobile, string? Email, string? LogoUrl) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId, Guid Id, Guid SchoolId, string Code, string Name, string BranchType,
        string? Address, string? City, string? Province, string? Phone, string? Fax,
        string? Mobile, string? Email, string? LogoUrl);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.SchoolId).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.BranchType).Must(value => BranchTypes.Contains(value));
            RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        }
    }

    public sealed class Handler(ICampusQuery query, ICampusCommand command, ISchoolQuery schoolQuery)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var campus = await query.GetByIdAsync(request.TenantId, request.Id, cancellationToken);
            if (campus is null)
            {
                return Result<Response>.Failure(Error.NotFound(ErrorMessages.EntityNotFound(nameof(CampusEntity))));
            }

            if (await schoolQuery.GetByIdAsync(request.TenantId, request.SchoolId, cancellationToken) is null)
            {
                return Result<Response>.Failure(Error.NotFound("The selected school was not found in this tenant."));
            }

            if (await query.ExistsByCodeAsync(request.TenantId, request.Code, request.Id, cancellationToken))
            {
                return Result<Response>.Failure(Error.Conflict(ErrorMessages.DuplicateCode(nameof(CampusEntity), request.Code)));
            }

            campus.UpdateDetails(request.SchoolId, request.Code, request.Name, request.BranchType, request.Address,
                request.City, request.Province, request.Phone, request.Fax, request.Mobile, request.Email, request.LogoUrl);
            await command.UpdateAsync(campus, cancellationToken);
            return Result<Response>.Success(Map(campus));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(ApiRoutes.EntityById(ModuleConstants.RouteSegment, "campus"),
            async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
                (await mediator.SendAsync<Request, Result<Response>>(request with { Id = id }, cancellationToken)).ToHttpResult())
            .WithName("UpdateCampus").WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
        return endpoints;
    }

    private static Response Map(CampusEntity campus) => new(
        campus.TenantId, campus.CampusId, campus.SchoolId, campus.Code, campus.Name, campus.BranchType,
        campus.Address, campus.City, campus.Province, campus.Phone, campus.Fax, campus.Mobile, campus.Email, campus.LogoUrl);
}
