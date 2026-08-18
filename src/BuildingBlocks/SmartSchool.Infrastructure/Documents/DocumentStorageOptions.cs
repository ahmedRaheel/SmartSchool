namespace SmartSchool.Infrastructure.Documents;

/// <summary>
/// Configures file storage independently from relational document metadata.
/// </summary>
public sealed class DocumentStorageOptions
{
	public const string SectionName = "DocumentStorage";

	public string Provider { get; init; } = "Local";

	public string LocalRootPath { get; init; } = "App_Data/Documents";

	public long MaximumFileSizeBytes { get; init; } = 10 * 1024 * 1024;

	public string[] AllowedContentTypes { get; init; } =
	[
		"image/jpeg",
		"image/png",
		"application/pdf"
	];
}
