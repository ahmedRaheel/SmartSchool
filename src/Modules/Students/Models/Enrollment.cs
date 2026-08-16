using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Models;

public sealed class Enrollment : Entity
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public string? MetadataJson { get; set; }
}
