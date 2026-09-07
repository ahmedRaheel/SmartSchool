using SmartSchool.Modules.Organization.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Persistence;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Models;

using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.School;

public static class UpdateSchool
{
    public sealed record Request(
        Guid TenantId, Guid Id, string Name, string? RegistrationNumber,
        string? Email, string? Phone, string? Fax, string? Website, string? Address,
        string? City, string? Province, string? Country, string? LogoUrl) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId, Guid Id, string Name, string? RegistrationNumber,
        string? Email, string? Phone, string? Fax, string? Website, string? Address,
        string? City, string? Province, string? Country, string? LogoUrl);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Id).NotEmpty();
                        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        }
    }

    public sealed class Handler(UpdateSchoolSchoolQuery query, UpdateSchoolSchoolCommand command) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var school = await query.GetByIdAsync(request.TenantId, request.Id, cancellationToken);
            if (school is null)
            {
                return Result<Response>.Failure(Error.NotFound(ErrorMessages.EntityNotFound(nameof(SchoolEntity))));
            }



            school.UpdateDetails(school.Code, request.Name, request.RegistrationNumber, request.Email, request.Phone,
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
        TenantId: school.TenantId,
        Id: school.SchoolId,
        Name: school.Name,
        RegistrationNumber: school.RegistrationNumber,
        Email: school.Email,
        Phone: school.Phone,
        Fax: school.Fax,
        Website: school.Website,
        Address: school.Address,
        City: school.City,
        Province: school.Province,
        Country: school.Country,
        LogoUrl: school.LogoUrl
    );
}

/// <summary>
/// Feature-owned data access for UpdateSchool. Do not share across slices.
/// </summary>
public sealed class UpdateSchoolSchoolQuery(IDbConnectionFactory connectionFactory)
{
    public sealed record SchoolRow(Guid Id, string Code, string Name);

    public async Task<SchoolRow?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT school_id AS "Id", code AS "Code", name AS "Name"
            FROM org.school
            WHERE tenant_id = @TenantId
              AND school_id = @Id
              AND is_active = TRUE;
            """;

        await using var connection =
            await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

        return await connection.QuerySingleOrDefaultAsync<SchoolRow>(
            new CommandDefinition(
                sql,
                new
                {
                    TenantId = tenantId,
                    Id = id
                },
                cancellationToken: cancellationToken)).ConfigureAwait(false);
    }
}

/// <summary>
/// Feature-owned data access for UpdateSchool. Do not share across slices.
/// </summary>
public sealed class UpdateSchoolSchoolCommand(IOrganizationDbContext dbContext)
{

    public async Task UpdateAsync(
        SchoolEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Schools
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
