using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.AICore.Features.KnowledgeDocument;
namespace SmartSchool.Modules.AICore.Persistence.Configurations;
public sealed class RagKnowledgeChunkWriteEntityConfiguration:IEntityTypeConfiguration<RagKnowledgeChunkWriteEntity>
{
    public void Configure(EntityTypeBuilder<RagKnowledgeChunkWriteEntity> builder)
    {
        builder.ToTable("rag_knowledge_chunk", "ai_core");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.TenantId).HasColumnName("tenant_id");
        builder.Property(x => x.Collection).HasColumnName("collection").HasMaxLength(80);
        builder.Property(x => x.DocumentName).HasColumnName("document_name").HasMaxLength(250);
        builder.Property(x => x.Content).HasColumnName("content");
        builder.Property(x => x.Embedding).HasColumnName("embedding").HasColumnType("vector(768)");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.IsActive).HasColumnName("is_active");
    }
}
