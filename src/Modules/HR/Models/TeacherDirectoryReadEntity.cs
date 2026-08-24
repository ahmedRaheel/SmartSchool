using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Models;

/// <summary>
/// Denormalized read table optimized for directory/list queries.
/// It is rebuilt from normalized transactional tables and is not the system of record.
/// </summary>
public sealed class TeacherDirectoryReadEntity : Entity
{
<<<<<<< HEAD
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid TeacherDirectoryReadId { get; private set; } = Guid.NewGuid();
private TeacherDirectoryReadEntity()
=======
	/// <summary>Gets the persisted entity identifier.</summary>
	public Guid Id
	{
		get => Id;
		private set => Id = value;
	}

	private TeacherDirectoryReadEntity()
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0
	{
	}

	/// <summary>Gets the source aggregate identifier.</summary>
	public Guid TeacherId { get; private set; }

	/// <summary>Gets the materialized EmployeeNumber value.</summary>
	public string EmployeeNumber { get; private set; } = string.Empty;
	/// <summary>Gets the materialized TeacherName value.</summary>
	public string TeacherName { get; private set; } = string.Empty;
	/// <summary>Gets the materialized JobTitle value.</summary>
	public string? JobTitle { get; private set; }
	/// <summary>Gets the materialized JobGrade value.</summary>
	public string? JobGrade { get; private set; }
	/// <summary>Gets the materialized DepartmentName value.</summary>
	public string? DepartmentName { get; private set; }
	/// <summary>Gets the materialized MobileNumber value.</summary>
	public string? MobileNumber { get; private set; }
	/// <summary>Gets the materialized ActiveClassAssignments value.</summary>
	public int ActiveClassAssignments { get; private set; }
	/// <summary>Gets the materialized DocumentCount value.</summary>
	public int DocumentCount { get; private set; }
	/// <summary>Gets the materialized VerifiedDocumentCount value.</summary>
	public int VerifiedDocumentCount { get; private set; }

	/// <summary>Creates or replaces a materialized read row.</summary>
	public static TeacherDirectoryReadEntity Create(
		Guid tenantId,
		Guid teacherId,
		string employeeNumber,
		string teacherName,
		string? jobTitle,
		string? jobGrade,
		string? departmentName,
		string? mobileNumber,
		int activeClassAssignments,
		int documentCount,
		int verifiedDocumentCount)
	{
		return new TeacherDirectoryReadEntity
		{
			TenantId = tenantId,
			TeacherId = teacherId,
			EmployeeNumber = employeeNumber,
			TeacherName = teacherName,
			JobTitle = jobTitle,
			JobGrade = jobGrade,
			DepartmentName = departmentName,
			MobileNumber = mobileNumber,
			ActiveClassAssignments = activeClassAssignments,
			DocumentCount = documentCount,
			VerifiedDocumentCount = verifiedDocumentCount,
		};
	}
}
