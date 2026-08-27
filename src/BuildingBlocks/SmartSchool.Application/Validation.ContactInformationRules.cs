using System.Text.RegularExpressions;

namespace SmartSchool.Application.Validation;

/// <summary>
/// Defines the canonical contact and identity formats accepted by SmartSchool.
/// </summary>
public static partial class ContactInformationRules
{
    public const string CnicExample = "12345-1234567-1";
    public const string TelephoneExample = "(21)-(123)-(12345678)";
    public const string MobileExample = "(92)-(3000)-(1234567)";

    public static bool IsValidEmail(string? value)
    {
        return string.IsNullOrWhiteSpace(value) || EmailRegex().IsMatch(value.Trim());
    }

    public static bool IsValidCnic(string? value)
    {
        return string.IsNullOrWhiteSpace(value) || CnicRegex().IsMatch(value.Trim());
    }

    public static bool IsValidTelephone(string? value)
    {
        return string.IsNullOrWhiteSpace(value) || TelephoneRegex().IsMatch(value.Trim());
    }

    public static bool IsValidMobile(string? value)
    {
        return string.IsNullOrWhiteSpace(value) || MobileRegex().IsMatch(value.Trim());
    }

    [GeneratedRegex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.CultureInvariant)]
    private static partial Regex EmailRegex();

    [GeneratedRegex(@"^\d{5}-\d{7}-\d$", RegexOptions.CultureInvariant)]
    private static partial Regex CnicRegex();

    [GeneratedRegex(@"^\(\d{2}\)-\(\d{3}\)-\(\d{8}\)$", RegexOptions.CultureInvariant)]
    private static partial Regex TelephoneRegex();

    [GeneratedRegex(@"^\(\d{2}\)-\(\d{4}\)-\(\d{7}\)$", RegexOptions.CultureInvariant)]
    private static partial Regex MobileRegex();
}
