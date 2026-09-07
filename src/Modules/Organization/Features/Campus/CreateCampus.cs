using SmartSchool.Modules.Organization.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
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
    public sealed record Request(Guid TenantId, Guid SchoolId, string Name, BranchType BranchType, Guid BranchGenderTypeId, Guid? AcademicSystemId, IReadOnlyCollection<Guid>? EducationLevelIds, string? Address, string? City, string? Province, string? Country, string? Phone, string? Fax, string? Mobile, string? Email, string? LogoUrl) : IRequest<Result<Response>>;
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

    public sealed class Handler(ITenantScope tenantScope, CreateCampusCampusWriteData command, CreateCampusSchoolReadData schoolQuery, CreateCampusBranchPolicyWriteData policyCommand, IBusinessNumberGenerator numberGenerator) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            if (await schoolQuery.GetByIdAsync(tenantId.Value, request.SchoolId, cancellationToken) is null)
                return Result<Response>.Failure(Error.NotFound("The selected school was not found in this tenant."));
            if (!await policyCommand.GenderTypeExistsAsync(request.BranchGenderTypeId, cancellationToken))
                return Result<Response>.Failure(Error.Validation("Select a valid branch gender type."));
            var educationLevelIds = request.EducationLevelIds ?? Array.Empty<Guid>();
            if (educationLevelIds.Count > 0 && !await policyCommand.EducationLevelsExistAsync(educationLevelIds, cancellationToken))
                return Result<Response>.Failure(Error.Validation("One or more education levels are invalid."));

            var code = await numberGenerator.NextAsync("BRANCH", "BR", tenantId.Value, 3, cancellationToken);
            var campus = CampusEntity.Create(tenantId.Value, request.SchoolId, code, request.Name, request.BranchType, request.BranchGenderTypeId, request.AcademicSystemId, request.Address, request.City, request.Province, request.Country, request.Phone, request.Fax, request.Mobile, request.Email, request.LogoUrl);
            await command.AddAsync(campus, cancellationToken);
            await policyCommand.SetEducationLevelsAsync(tenantId.Value, campus.CampusId, educationLevelIds, cancellationToken);
            await command.SyncGradeLevelsAsync(tenantId.Value, campus.CampusId, request.AcademicSystemId, cancellationToken);
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

/// <summary>
/// Feature-owned data access for CreateCampus. Do not share across slices.
/// </summary>
public sealed class CreateCampusSchoolReadData(IDbConnectionFactory connectionFactory)
{
    public async Task<SchoolEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT *
            FROM org.school
            WHERE tenant_id = @TenantId
              AND school_id = @Id
              AND is_active = TRUE;
            """;

        await using var connection =
            await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

        return await connection.QuerySingleOrDefaultAsync<SchoolEntity>(
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
/// Feature-owned data access for CreateCampus. Do not share across slices.
/// </summary>
public sealed class CreateCampusBranchPolicyWriteData(IDbConnectionFactory connectionFactory)
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
        const string deleteSql = "DELETE FROM org.campus_education_level WHERE tenant_id=@TenantId AND campus_id=@CampusId;";
        const string insertSql = "INSERT INTO org.campus_education_level(tenant_id, campus_id, education_level_id) VALUES(@TenantId, @CampusId, @EducationLevelId);";
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(deleteSql, new { TenantId = tenantId, CampusId = branchId }, transaction, cancellationToken: cancellationToken));
        foreach (var levelId in educationLevelIds.Distinct())
            await connection.ExecuteAsync(new CommandDefinition(insertSql, new { TenantId = tenantId, CampusId = branchId, EducationLevelId = levelId }, transaction, cancellationToken: cancellationToken));
        await transaction.CommitAsync(cancellationToken);
    }
}

/// <summary>
/// Feature-owned data access for CreateCampus. Do not share across slices.
/// </summary>
public sealed class CreateCampusCampusWriteData(IOrganizationDbContext dbContext)
{
    public async Task AddAsync(
        CampusEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.Campuses
            .AddAsync(entity, cancellationToken);

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
