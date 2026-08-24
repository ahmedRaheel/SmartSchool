using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="KnowledgeChunkEntity"/>.
/// </summary>
public sealed class KnowledgeChunkEntityConfiguration
	: IEntityTypeConfiguration<KnowledgeChunkEntity>
{
	public void Configure(EntityTypeBuilder<KnowledgeChunkEntity> builder)
	{
		builder.ToTable("knowledge_chunk", schema: "ai_core");

		builder.HasKey(entity => entity.Id);

		builder
			.Property(entity => entity.TenantId)
			.IsRequired();

		builder
			.Property(entity => entity.IsActive)
			.IsRequired();

		builder.HasIndex(entity => entity.TenantId);

		builder.Property(entity => entity.CreatedAt).IsRequired();
		builder.Property(entity => entity.UpdatedAt);
		builder.Property(entity => entity.RowVersion).IsRequired().IsConcurrencyToken();

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


		// Canonical database mapping generated from SmartSchoolComplete.sql.
		builder.Property(entity => entity.Code).HasColumnName("code");
		builder.Property(entity => entity.Name).HasColumnName("name");
		builder.Property(entity => entity.MetadataJson).HasColumnName("metadata_json");
		builder.Property(entity => entity.Id).HasColumnName("knowledge_chunk_id");
		builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
		builder.Property(entity => entity.IsActive).HasColumnName("is_active");
		builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
		builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
		builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

		// Database columns synchronized from SmartSchoolComplete.sql.
		builder.Property(entity => entity.KnowledgeDocumentId).HasColumnName("knowledge_document_id");
		builder.Property(entity => entity.ChunkIndex).HasColumnName("chunk_index");
		builder.Property(entity => entity.Content).HasColumnName("content");
		builder.Property(entity => entity.Metadata).HasColumnName("metadata");
		builder.Property(entity => entity.EmbeddingReference).HasColumnName("embedding_reference");
		builder.Property(entity => entity.Embedding).HasColumnName("embedding");
	}
}
