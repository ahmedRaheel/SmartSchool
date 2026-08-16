using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Models;

public sealed class TeacherAssignment : Entity
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public string? MetadataJson { get; set; }
}
