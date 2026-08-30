using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Models;

using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.School;

public static class CreateSchool
{
    public sealed record Request(
        Guid TenantId,
        string Name,
        string? RegistrationNumber,
        string? Email,
        string? Phone,
        string? Fax,
        string? Website,
        string? Address,
        string? City,
        string? Province,
        string? Country,
        string? LogoUrl) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId, Guid Id, string Code, string Name, string? RegistrationNumber,
        string? Email, string? Phone, string? Fax, string? Website, string? Address,
        string? City, string? Province, string? Country, string? LogoUrl);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
            RuleFor(x => x.Phone).MaximumLength(50);
            RuleFor(x => x.Fax).MaximumLength(50);
            RuleFor(x => x.City).MaximumLength(120);
            RuleFor(x => x.Province).MaximumLength(120);
            RuleFor(x => x.Website).MaximumLength(300);
        }
    }

    public sealed class Handler(ISchoolCommand command, IBusinessNumberGenerator numberGenerator) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var code = await numberGenerator.NextAsync(
                "SCHOOL", "SCH", request.TenantId, 3, cancellationToken);

            var school = SchoolEntity.Create(
                request.TenantId, code, request.Name, request.RegistrationNumber, request.Email,
                request.Phone, request.Fax, request.Website, request.Address, request.City, request.Province,
                request.Country, request.LogoUrl);

            await command.AddAsync(school, cancellationToken);
            return Result<Response>.Success(Map(school));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "school"),
            async (Request request, ITenantScope tenantScope, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var tenantId = tenantScope.Resolve(request.TenantId) ?? request.TenantId;
                var command = request with { TenantId = tenantId };
                return (await mediator.SendAsync<Request, Result<Response>>(command, cancellationToken)).ToHttpResult();
            })
            .WithName("CreateSchool")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);

        return endpoints;
    }

    private static Response Map(SchoolEntity school) => new(
        school.TenantId, school.SchoolId, school.Code, school.Name, school.RegistrationNumber,
        school.Email, school.Phone, school.Fax, school.Website, school.Address, school.City,
        school.Province, school.Country, school.LogoUrl);
}
