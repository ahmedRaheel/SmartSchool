using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Models;

public sealed class TutorMessage : Entity
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public string? MetadataJson { get; set; }
}
