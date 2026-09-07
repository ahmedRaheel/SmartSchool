namespace SmartSchool.Modules.AICore.Features.KnowledgeDocument;
public sealed class RagKnowledgeChunkWriteEntity
{
    private RagKnowledgeChunkWriteEntity() { }
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid TenantId { get; private set; }
    public string Collection { get; private set; } = string.Empty;
    public string DocumentName { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public float[] Embedding { get; private set; } = [];
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public bool IsActive { get; private set; } = true;
    public static RagKnowledgeChunkWriteEntity Create(Guid tenantId,string collection,string documentName,string content,float[] embedding)=>new(){TenantId=tenantId,Collection=collection,DocumentName=documentName,Content=content,Embedding=embedding};
}
