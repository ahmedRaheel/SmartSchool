using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Api.Seed;

public sealed record SampleActor(
    Guid UserId,
    Guid TenantId,
    Guid SchoolId,
    string UserName,
    string Email,
    string FirstName,
    string LastName,
    string Role,
    string ReferenceNumber);

public static class SampleActors
{
    public static readonly Guid TenantId =
        Guid.Parse("11111111-1111-1111-1111-111111111111");

    public static readonly Guid SchoolId =
        Guid.Parse("22222222-2222-2222-2222-222222222222");

    public static IReadOnlyCollection<SampleActor> All { get; } =
        new[]
        {
            Create(
                "10000000-0000-0000-0000-000000000001",
                "admin.smartschool",
                "admin@smartschool.local",
                "System",
                "Administrator",
                SmartSchoolRoles.SchoolAdmin,
                "ADM-ADMIN-001"),

            Create(
                "10000000-0000-0000-0000-000000000002",
                "sara.teacher",
                "sara.teacher@smartschool.local",
                "Sara",
                "Ali",
                SmartSchoolRoles.Teacher,
                "TCH-2026-001"),

            Create(
                "10000000-0000-0000-0000-000000000003",
                "ahmed.student",
                "ahmed.student@smartschool.local",
                "Ahmed",
                "Khan",
                SmartSchoolRoles.Student,
                "STD-2026-0001"),

            Create(
                "10000000-0000-0000-0000-000000000004",
                "imran.parent",
                "imran.parent@smartschool.local",
                "Imran",
                "Khan",
                SmartSchoolRoles.Parent,
                "PAR-2026-0001")
        };

    private static SampleActor Create(
        string userId,
        string userName,
        string email,
        string firstName,
        string lastName,
        string role,
        string referenceNumber)
    {
        return new SampleActor(
            Guid.Parse(userId),
            TenantId,
            SchoolId,
            userName,
            email,
            firstName,
            lastName,
            role,
            referenceNumber);
    }
}
