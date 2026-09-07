using SmartSchool.Modules.Organization.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Features.Campus;

/// <summary>
/// Executes database writes for <see cref="CampusEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class CampusWriter(IOrganizationDbContext dbContext)
{
    public async Task AddAsync(
        CampusEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.Campuses
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        CampusEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Campuses
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        CampusEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.Campuses
            .Remove(entity);

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
