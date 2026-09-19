namespace SmartSchool.ApiSmokeTester;

internal static class SmokeActorResolver
{
    private static readonly SmokeActorKind[] DefaultOrder =
    [
        SmokeActorKind.SuperAdmin,
        SmokeActorKind.Owner,
        SmokeActorKind.Admin,
        SmokeActorKind.Principal,
        SmokeActorKind.Teacher,
        SmokeActorKind.Student,
        SmokeActorKind.Parent,
        SmokeActorKind.Driver,
        SmokeActorKind.Accountant,
        SmokeActorKind.HRManager,
        SmokeActorKind.Librarian,
        SmokeActorKind.Examiner
    ];

    public static IReadOnlyList<SmokeActorKind> GetCandidates(
        OpenApiOperation operation)
    {
        var path = operation.Path.ToLowerInvariant();
        var preferred = new List<SmokeActorKind>();

        void Prefer(params SmokeActorKind[] kinds)
        {
            foreach (var kind in kinds)
            {
                if (!preferred.Contains(kind))
                {
                    preferred.Add(kind);
                }
            }
        }

        if (path.Contains("/students/self")
            || path.Contains("/students/me")
            || path.Contains("/student/self")
            || path.Contains("/student/me"))
        {
            Prefer(SmokeActorKind.Student);
        }
        else if (path.Contains("/parent")
                 || path.Contains("/parents"))
        {
            Prefer(SmokeActorKind.Parent, SmokeActorKind.Owner);
        }
        else if (path.Contains("/teachers")
                 || path.Contains("/teacher"))
        {
            Prefer(SmokeActorKind.Teacher, SmokeActorKind.Owner);
        }
        else if (path.StartsWith("/api/transport", StringComparison.Ordinal))
        {
            Prefer(SmokeActorKind.Driver, SmokeActorKind.Admin, SmokeActorKind.Owner);
        }
        else if (path.StartsWith("/api/finance", StringComparison.Ordinal))
        {
            Prefer(SmokeActorKind.Accountant, SmokeActorKind.Owner);
        }
        else if (path.StartsWith("/api/payroll", StringComparison.Ordinal))
        {
            Prefer(
                SmokeActorKind.HRManager,
                SmokeActorKind.Accountant,
                SmokeActorKind.Owner);
        }
        else if (path.StartsWith("/api/hr", StringComparison.Ordinal))
        {
            Prefer(SmokeActorKind.HRManager, SmokeActorKind.Owner);
        }
        else if (path.StartsWith("/api/library", StringComparison.Ordinal))
        {
            Prefer(SmokeActorKind.Librarian, SmokeActorKind.Owner);
        }
        else if (path.StartsWith("/api/examinations", StringComparison.Ordinal)
                 || path.StartsWith("/api/exam", StringComparison.Ordinal))
        {
            Prefer(
                SmokeActorKind.Examiner,
                SmokeActorKind.Teacher,
                SmokeActorKind.Owner);
        }
        else if (path.StartsWith("/api/aitutor", StringComparison.Ordinal))
        {
            Prefer(
                SmokeActorKind.Student,
                SmokeActorKind.Teacher,
                SmokeActorKind.Owner);
        }
        else if (path.StartsWith("/api/ai", StringComparison.Ordinal)
                 || path.StartsWith("/api/aicore", StringComparison.Ordinal))
        {
            Prefer(
                SmokeActorKind.Teacher,
                SmokeActorKind.Owner,
                SmokeActorKind.SuperAdmin);
        }
        else if (path.StartsWith("/api/workflow", StringComparison.Ordinal)
                 || path.StartsWith("/api/organization", StringComparison.Ordinal)
                 || path.StartsWith("/api/academics", StringComparison.Ordinal)
                 || path.StartsWith("/api/admissions", StringComparison.Ordinal)
                 || path.StartsWith("/api/documents", StringComparison.Ordinal)
                 || path.StartsWith("/api/inventory", StringComparison.Ordinal)
                 || path.StartsWith("/api/activities", StringComparison.Ordinal))
        {
            Prefer(
                SmokeActorKind.Owner,
                SmokeActorKind.Admin,
                SmokeActorKind.Principal,
                SmokeActorKind.SuperAdmin);
        }
        else
        {
            Prefer(SmokeActorKind.SuperAdmin, SmokeActorKind.Owner);
        }

        Prefer(DefaultOrder);
        return preferred;
    }
}
