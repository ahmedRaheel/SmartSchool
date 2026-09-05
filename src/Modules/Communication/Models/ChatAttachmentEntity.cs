using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Models;

/// <summary>Stores metadata for a file attached to a chat message.</summary>
public sealed class ChatAttachmentEntity : Entity
{
    /// <summary>Gets the entity-specific identifier.</summary>
    public Guid ChatAttachmentId { get; private set; } = Guid.NewGuid();
private ChatAttachmentEntity()
    {
    }

    /// <summary>Gets the message identifier.</summary>
    public Guid MessageId { get; private set; }

    /// <summary>Gets the original file name.</summary>
    public string FileName { get; private set; } = string.Empty;

    /// <summary>Gets the MIME content type.</summary>
    public string ContentType { get; private set; } = string.Empty;

    /// <summary>Gets the file size.</summary>
    public long FileSizeBytes { get; private set; }

    /// <summary>Gets the private storage key.</summary>
    public string StorageKey { get; private set; } = string.Empty;
}
