using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.School;

public static class UpdateSchool
{
    public sealed record Request(
        Guid TenantId, Guid Id, string Code, string Name, string? RegistrationNumber,
        string? Email, string? Phone, string? Fax, string? Website, string? Address,
        string? City, string? Province, string? Country, string? LogoUrl) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId, Guid Id, string Code, string Name, string? RegistrationNumber,
        string? Email, string? Phone, string? Fax, string? Website, string? Address,
        string? City, string? Province, string? Country, string? LogoUrl);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        }
    }

    public sealed class Handler(ISchoolQuery query, ISchoolCommand command) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var school = await query.GetByIdAsync(request.TenantId, request.Id, cancellationToken);
            if (school is null)
            {
                return Result<Response>.Failure(Error.NotFound(ErrorMessages.EntityNotFound(nameof(SchoolEntity))));
            }

            if (await query.ExistsByCodeAsync(request.TenantId, request.Code, request.Id, cancellationToken))
            {
                return Result<Response>.Failure(Error.Conflict(ErrorMessages.DuplicateCode(nameof(SchoolEntity), request.Code)));
            }

            school.UpdateDetails(request.Code, request.Name, request.RegistrationNumber, request.Email, request.Phone,
                request.Fax, request.Website, request.Address, request.City, request.Province, request.Country, request.LogoUrl);
            await command.UpdateAsync(school, cancellationToken);
            return Result<Response>.Success(Map(school));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(ApiRoutes.EntityById(ModuleConstants.RouteSegment, "school"),
            async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
                (await mediator.SendAsync<Request, Result<Response>>(request with { Id = id }, cancellationToken)).ToHttpResult())
            .WithName("UpdateSchool").WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
        return endpoints;
    }

    private static Response Map(SchoolEntity school) => new(
        school.TenantId, school.SchoolId, school.Code, school.Name, school.RegistrationNumber,
        school.Email, school.Phone, school.Fax, school.Website, school.Address, school.City,
        school.Province, school.Country, school.LogoUrl);
}
