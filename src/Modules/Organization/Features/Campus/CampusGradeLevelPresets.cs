namespace SmartSchool.Modules.Organization.Features.Campus;

internal static class CampusGradeLevelPresets
{
    internal sealed record Preset(string Code, string Name, int SortOrder);

    internal static IReadOnlyCollection<Preset> Resolve(string code, string name)
    {
        var key = $"{code} {name}".ToUpperInvariant();

        if (key.Contains("MIDDLE", StringComparison.Ordinal))
        {
            return RomanGrades(5);
        }

        if (key.Contains("MATRIC", StringComparison.Ordinal) ||
            key.Contains("SSC", StringComparison.Ordinal) ||
            key.Contains("SECONDARY", StringComparison.Ordinal))
        {
            return RomanGrades(10);
        }

        if (key.Contains("CAMBRIDGE", StringComparison.Ordinal) ||
            key.Contains("O LEVEL", StringComparison.Ordinal))
        {
            return RomanGrades(11);
        }

        return RomanGrades(12);
    }

    private static IReadOnlyCollection<Preset> RomanGrades(int count)
    {
        string[] romanNumerals =
        [
            "I", "II", "III", "IV", "V", "VI",
            "VII", "VIII", "IX", "X", "XI", "XII"
        ];

        return Enumerable.Range(0, count)
            .Select(index => new Preset(
                $"GRADE_{romanNumerals[index]}",
                $"Grade {romanNumerals[index]}",
                index + 1))
            .ToArray();
    }
}
