using SmartSchool.Modules.AICore.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Features.ModelConfiguration;

/// <summary>
/// Executes database writes for <see cref="ModelConfigurationEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ModelConfigurationCommand(IAICoreDbContext dbContext) : IModelConfigurationCommand
{
	public async Task AddAsync(
		ModelConfigurationEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.ModelConfigurations
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ModelConfigurationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.ModelConfigurations
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ModelConfigurationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.ModelConfigurations
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
