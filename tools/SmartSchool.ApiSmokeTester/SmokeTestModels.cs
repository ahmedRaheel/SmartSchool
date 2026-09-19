using System.Text.Json;

namespace SmartSchool.ApiSmokeTester;

internal enum SmokeResultKind
{
    Passed,
    ValidationHandled,
    AuthenticationBlocked,
    FailedServerError,
    NetworkError,
    Timeout,
    Skipped
}

internal enum SmokeActorKind
{
    SuperAdmin,
    Owner,
    Admin,
    Principal,
    Teacher,
    Student,
    Parent,
    Driver,
    Accountant,
    HRManager,
    Librarian,
    Examiner
}

internal sealed record SmokeActor(
    SmokeActorKind Kind,
    Guid UserId,
    Guid? BusinessEntityId,
    string Email,
    string Password,
    string Token,
    Guid? TenantId,
    Guid? SchoolId,
    Guid? BranchId);

internal sealed class SmokeTestFixture
{
    public required string RunId { get; init; }
    public required Guid TenantId { get; init; }
    public Guid? SchoolId { get; set; }
    public Guid? CampusId { get; set; }
    public Guid? AcademicSystemId { get; set; }
    public Guid? AcademicYearId { get; set; }
    public Guid? GradeLevelId { get; set; }
    public Guid? ClassSectionId { get; set; }

    public Dictionary<SmokeActorKind, SmokeActor> Actors { get; } = [];

    public List<string> ProvisioningWarnings { get; } = [];

    public Dictionary<string, string> KnownValues { get; } =
        new(StringComparer.OrdinalIgnoreCase);

    public SmokeActor GetActor(SmokeActorKind kind) =>
        Actors.TryGetValue(kind, out var actor)
            ? actor
            : throw new InvalidOperationException(
                $"Smoke actor {kind} has not been provisioned.");

    public IReadOnlyDictionary<string, string> ValuesFor(SmokeActor actor)
    {
        var values = new Dictionary<string, string>(
            KnownValues,
            StringComparer.OrdinalIgnoreCase)
        {
            ["userId"] = actor.UserId.ToString()
        };

        if (actor.TenantId.HasValue)
        {
            values["tenantId"] = actor.TenantId.Value.ToString();
        }

        if (actor.SchoolId.HasValue)
        {
            values["schoolId"] = actor.SchoolId.Value.ToString();
        }

        if (actor.BranchId.HasValue)
        {
            values["branchId"] = actor.BranchId.Value.ToString();
            values["campusId"] = actor.BranchId.Value.ToString();
        }

        if (actor.BusinessEntityId.HasValue)
        {
            var value = actor.BusinessEntityId.Value.ToString();
            values["businessEntityId"] = value;

            switch (actor.Kind)
            {
                case SmokeActorKind.Student:
                    values["studentId"] = value;
                    break;
                case SmokeActorKind.Parent:
                    values["guardianId"] = value;
                    values["parentId"] = value;
                    break;
                case SmokeActorKind.Teacher:
                    values["teacherId"] = value;
                    values["employeeId"] = value;
                    break;
                case SmokeActorKind.Driver:
                    values["driverId"] = value;
                    values["employeeId"] = value;
                    break;
                case SmokeActorKind.Examiner:
                    values["examinerId"] = value;
                    values["employeeId"] = value;
                    break;
                case SmokeActorKind.SuperAdmin:
                    break;
                case SmokeActorKind.Owner:
                    break;
                case SmokeActorKind.Admin:
                    break;
                case SmokeActorKind.Principal:
                    break;
                case SmokeActorKind.Accountant:
                    break;
                case SmokeActorKind.HRManager:
                    break;
                case SmokeActorKind.Librarian:
                    break;
                default:
                    values["employeeId"] = value;
                    break;
            }
        }

        return values;
    }
}

internal sealed record OpenApiParameter(
    string Name,
    string Location,
    bool Required,
    JsonElement Schema);

internal sealed record OpenApiOperation(
    string Method,
    string Path,
    string OperationId,
    string Summary,
    IReadOnlyList<OpenApiParameter> Parameters,
    JsonElement? RequestBody);

internal sealed record SmokeResult(
    string Method,
    string Path,
    string RequestUri,
    string OperationId,
    string Actor,
    SmokeResultKind Result,
    int? StatusCode,
    long DurationMilliseconds,
    string Message,
    string ResponseBody);
