using SmartSchool.Modules.Organization.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Persistence;
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
    public sealed record Request(Guid TenantId,Guid CampusId,  Guid SchoolId, string Name, BranchType BranchType, Guid BranchGenderTypeId, Guid? AcademicSystemId, IReadOnlyCollection<Guid>? EducationLevelIds, string? Address, string? City, string? Province, string? Country, string? Phone, string? Fax, string? Mobile, string? Email, string? LogoUrl) : IRequest<Result<Response>>;
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

    public sealed class Handler(ITenantScope tenantScope, UpdateCampusCampusCommand command, UpdateCampusSchoolQuery schoolQuery, UpdateCampusBranchPolicyCommand policyCommand) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            var campus = await command.GetByIdAsync(tenantId.Value, request.CampusId, cancellationToken);
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
            await policyCommand.SetEducationLevelsAsync(tenantId.Value, campus.CampusId, educationLevelIds, cancellationToken);
            await command.SyncGradeLevelsAsync(tenantId.Value, campus.CampusId, request.AcademicSystemId, cancellationToken);
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

/// <summary>
/// Feature-owned data access for UpdateCampus. Do not share across slices.
/// </summary>
public sealed class UpdateCampusSchoolQuery(IDbConnectionFactory connectionFactory)
{
    public sealed record SchoolRow(Guid Id);

    public async Task<SchoolRow?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT school_id AS "Id"
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
/// Feature-owned data access for UpdateCampus. Do not share across slices.
/// </summary>
public sealed class UpdateCampusBranchPolicyCommand(IDbConnectionFactory connectionFactory, OrganizationDbContext dbContext)
{
    public async Task<bool> GenderTypeExistsAsync(Guid genderTypeId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM reference.branch_gender_type WHERE branch_gender_type_id=@Id AND is_active=TRUE);";
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { Id = genderTypeId }, cancellationToken: cancellationToken));
    }


    public async Task<bool> EducationLevelsExistAsync(IReadOnlyCollection<Guid> educationLevelIds, CancellationToken cancellationToken)
    {
        if (educationLevelIds.Count == 0) return false;
        const string sql = "SELECT COUNT(*) FROM reference.education_level WHERE education_level_id = ANY(@Ids) AND is_active=TRUE;";
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var count = await connection.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { Ids = educationLevelIds.ToArray() }, cancellationToken: cancellationToken));
        return count == educationLevelIds.Distinct().Count();
    }


    public async Task SetEducationLevelsAsync(Guid tenantId, Guid branchId, IReadOnlyCollection<Guid> educationLevelIds, CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        await dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"DELETE FROM org.campus_education_level WHERE tenant_id={tenantId} AND campus_id={branchId};", cancellationToken);
        foreach (var levelId in educationLevelIds.Distinct())
        {
            await dbContext.Database.ExecuteSqlInterpolatedAsync(
                $"INSERT INTO org.campus_education_level(tenant_id, campus_id, education_level_id) VALUES({tenantId}, {branchId}, {levelId});", cancellationToken);
        }
        await transaction.CommitAsync(cancellationToken);
    }
}

/// <summary>
/// Feature-owned data access for UpdateCampus. Do not share across slices.
/// </summary>
/// <summary>
/// Feature-owned data access for UpdateCampus. Do not share across slices.
/// </summary>
public sealed class UpdateCampusCampusCommand(IOrganizationDbContext dbContext)
{
    public Task<CampusEntity?> GetByIdAsync(Guid? tenantId, Guid id, CancellationToken cancellationToken)
    {
        return dbContext.Campuses.SingleOrDefaultAsync(
            entity => (!tenantId.HasValue || entity.TenantId == tenantId.Value) && entity.CampusId == id, cancellationToken);
    }


    public async Task UpdateAsync(
        CampusEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Campuses
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async Task SyncGradeLevelsAsync(
        Guid tenantId,
        Guid campusId,
        Guid? academicSystemId,
        CancellationToken cancellationToken)
    {
        if (!academicSystemId.HasValue)
        {
            return;
        }

        var academicSystem = await dbContext.AcademicSystems
            .AsNoTracking()
            .SingleOrDefaultAsync(
                x => x.TenantId == tenantId && x.AcademicSystemId == academicSystemId.Value,
                cancellationToken);

        if (academicSystem is null)
        {
            throw new InvalidOperationException("The selected academic system does not belong to the tenant.");
        }

        var presets = CampusGradeLevelPresets.Resolve(academicSystem.Code, academicSystem.Name);
        var existingCodes = await dbContext.GradeLevels
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && x.CampusId == campusId)
            .Select(x => x.Code)
            .ToListAsync(cancellationToken);

        var existing = existingCodes.ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var preset in presets.Where(x => !existing.Contains(x.Code)))
        {
            await dbContext.GradeLevels.AddAsync(
                GradeLevelEntity.Create(
                    tenantId,
                    campusId,
                    academicSystemId,
                    preset.Code,
                    preset.Name,
                    preset.SortOrder,
                    "{\"source\":\"campus-academic-system\"}"),
                cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
