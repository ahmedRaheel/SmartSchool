using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="WorkflowDefinitionEntity"/>.
/// </summary>
public sealed class WorkflowDefinitionEntityConfiguration
	: IEntityTypeConfiguration<WorkflowDefinitionEntity>
{
	public void Configure(EntityTypeBuilder<WorkflowDefinitionEntity> builder)
	{
		builder.ToTable("WorkflowDefinition");

		builder.HasKey(entity => entity.Id);

		builder
			.Property(entity => entity.TenantId)
			.IsRequired();

		builder
			.Property(entity => entity.IsActive)
			.IsRequired();

		builder
			.Property(entity => entity.RowVersion)
			.IsConcurrencyToken();

		builder.HasIndex(entity => entity.TenantId);

		builder
			.Property(entity => entity.Code)
			.HasMaxLength(100)
			.IsRequired();

		builder
			.HasIndex(entity => new { entity.TenantId, entity.Code })
			.IsUnique();

		builder
			.Property(entity => entity.Name)
			.HasMaxLength(250)
			.IsRequired();

	}
}
