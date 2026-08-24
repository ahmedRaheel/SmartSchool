using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Models;

/// <summary>
/// Denormalized read table optimized for directory/list queries.
/// It is rebuilt from normalized transactional tables and is not the system of record.
/// </summary>
public sealed class StudentDirectoryReadEntity : Entity
{
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid StudentDirectoryReadId { get; private set; } = Guid.NewGuid();
private StudentDirectoryReadEntity()
	{
	}

	/// <summary>Gets the source aggregate identifier.</summary>
	public Guid StudentId { get; private set; }

	/// <summary>Gets the materialized AdmissionNumber value.</summary>
	public string AdmissionNumber { get; private set; } = string.Empty;
	/// <summary>Gets the materialized StudentName value.</summary>
	public string StudentName { get; private set; } = string.Empty;
	/// <summary>Gets the materialized ProgramName value.</summary>
	public string? ProgramName { get; private set; }
	/// <summary>Gets the materialized ClassName value.</summary>
	public string? ClassName { get; private set; }
	/// <summary>Gets the materialized SectionName value.</summary>
	public string? SectionName { get; private set; }
	/// <summary>Gets the materialized PrimaryGuardianName value.</summary>
	public string? PrimaryGuardianName { get; private set; }
	/// <summary>Gets the materialized PrimaryGuardianMobile value.</summary>
	public string? PrimaryGuardianMobile { get; private set; }
	/// <summary>Gets the materialized AttendancePercentage value.</summary>
	public decimal? AttendancePercentage { get; private set; }
	/// <summary>Gets the materialized LatestExamPercentage value.</summary>
	public decimal? LatestExamPercentage { get; private set; }
	/// <summary>Gets the materialized OutstandingBalance value.</summary>
	public decimal OutstandingBalance { get; private set; }
	/// <summary>Gets the materialized DocumentCount value.</summary>
	public int DocumentCount { get; private set; }
	/// <summary>Gets the materialized VerifiedDocumentCount value.</summary>
	public int VerifiedDocumentCount { get; private set; }

	/// <summary>Creates or replaces a materialized read row.</summary>
	public static StudentDirectoryReadEntity Create(
		Guid tenantId,
		Guid studentId,
		string admissionNumber,
		string studentName,
		string? programName,
		string? className,
		string? sectionName,
		string? primaryGuardianName,
		string? primaryGuardianMobile,
		decimal? attendancePercentage,
		decimal? latestExamPercentage,
		decimal outstandingBalance,
		int documentCount,
		int verifiedDocumentCount)
	{
		return new StudentDirectoryReadEntity
		{
			TenantId = tenantId,
			StudentId = studentId,
			AdmissionNumber = admissionNumber,
			StudentName = studentName,
			ProgramName = programName,
			ClassName = className,
			SectionName = sectionName,
			PrimaryGuardianName = primaryGuardianName,
			PrimaryGuardianMobile = primaryGuardianMobile,
			AttendancePercentage = attendancePercentage,
			LatestExamPercentage = latestExamPercentage,
			OutstandingBalance = outstandingBalance,
			DocumentCount = documentCount,
			VerifiedDocumentCount = verifiedDocumentCount,
		};
	}
}
