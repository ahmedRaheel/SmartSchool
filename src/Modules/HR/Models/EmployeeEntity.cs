using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Models;

/// <summary>
/// Represents a teaching or non-teaching employee.
/// </summary>
public sealed class EmployeeEntity : Entity
{
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid EmployeeId { get; private set; } = Guid.NewGuid();

	private EmployeeEntity()
	{
	}

	/// <summary>Gets the optional authenticated user identifier.</summary>
	public Guid? UserId { get; private set; }

	public Guid SchoolId { get; private set; }
	public Guid BranchId { get; private set; }
	public Guid? DepartmentId { get; private set; }
	public string StaffType { get; private set; } = "OTHER";

	/// <summary>Gets the employee business designation.</summary>
	public EmployeeDesignation Designation { get; private set; } = EmployeeDesignation.Other;

	/// <summary>Gets the tenant-unique employee number.</summary>
	public string? EmployeeNumber { get; private set; }

	/// <summary>Gets the employee first name.</summary>
	public string FirstName { get; private set; } = string.Empty;

	/// <summary>Gets the employee last name.</summary>
	public string? LastName { get; private set; }

	/// <summary>Gets the employee CNIC number.</summary>
	public string? CnicNumber { get; private set; }

	public DateOnly? DateOfBirth { get; private set; }
	public string? Gender { get; private set; }
	public string? JobTitle { get; private set; }

	/// <summary>Gets the employee photograph bytes.</summary>
	public byte[]? Photo { get; private set; }

	/// <summary>Gets the photograph MIME type.</summary>
	public string? PhotoContentType { get; private set; }

	/// <summary>Gets the photograph file name.</summary>
	public string? PhotoFileName { get; private set; }

	/// <summary>Gets the employee email address.</summary>
	public string? Email { get; private set; }

	/// <summary>Gets the employee phone number.</summary>
	public string? Phone { get; private set; }
	public string? AlternatePhone { get; private set; }
	public string? Address { get; private set; }
	public string? EmergencyContactName { get; private set; }
	public string? EmergencyContactPhone { get; private set; }

	/// <summary>Gets the employee hire date.</summary>
	public DateOnly HireDate { get; private set; }

	/// <summary>Gets the employment type code.</summary>
	public string EmploymentTypeCode { get; private set; } = string.Empty;

	/// <summary>Gets the employee status.</summary>
	public string Status { get; private set; } = LifecycleStatuses.Active;

	/// <summary>Gets the optional recruitment candidate identifier.</summary>
	public Guid? SourceCandidateId { get; private set; }

	/// <summary>Creates a new employee.</summary>
	/// <param name="tenantId">Owning tenant identifier.</param>
	/// <param name="userId">Optional authenticated user identifier.</param>
	/// <param name="employeeNumber">Tenant-unique employee number.</param>
	/// <param name="firstName">Employee first name.</param>
	/// <param name="lastName">Employee last name.</param>
	/// <param name="cnicNumber">Employee CNIC number.</param>
	/// <param name="photo">Employee photograph bytes.</param>
	/// <param name="photoContentType">Photograph MIME type.</param>
	/// <param name="photoFileName">Photograph file name.</param>
	/// <param name="email">Employee email.</param>
	/// <param name="phone">Employee phone.</param>
	/// <param name="hireDate">Employee hire date.</param>
	/// <param name="employmentTypeCode">Employment type code.</param>
	/// <param name="status">Employee status.</param>
	/// <param name="sourceCandidateId">Optional source candidate identifier.</param>
	/// <returns>A new employee entity.</returns>
	public static EmployeeEntity Create(
		Guid tenantId,
		Guid? userId,
		Guid schoolId,
		Guid branchId,
		Guid? departmentId,
		string staffType,
		string? employeeNumber,
		string firstName,
		string? lastName,
		string? cnicNumber,
		DateOnly? dateOfBirth,
		string? gender,
		string? jobTitle,
		byte[]? photo,
		string? photoContentType,
		string? photoFileName,
		string? email,
		string? phone,
		string? alternatePhone,
		string? address,
		string? emergencyContactName,
		string? emergencyContactPhone,
		DateOnly hireDate,
		string employmentTypeCode,
		string status,
		Guid? sourceCandidateId)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
		ArgumentException.ThrowIfNullOrWhiteSpace(employmentTypeCode);
		ArgumentException.ThrowIfNullOrWhiteSpace(status);

		return new EmployeeEntity
		{
			TenantId = tenantId,
			UserId = userId,
			SchoolId = schoolId,
			BranchId = branchId,
			DepartmentId = departmentId,
			StaffType = staffType.Trim(),
			Designation = ParseDesignation(staffType),
			EmployeeNumber = employeeNumber?.Trim(),
			FirstName = firstName.Trim(),
			LastName = lastName?.Trim(),
			CnicNumber = cnicNumber?.Trim(),
			DateOfBirth = dateOfBirth,
			Gender = gender?.Trim(),
			JobTitle = jobTitle?.Trim(),
			Photo = photo,
			PhotoContentType = photoContentType?.Trim(),
			PhotoFileName = photoFileName?.Trim(),
			Email = email?.Trim(),
			Phone = phone?.Trim(),
			AlternatePhone = alternatePhone?.Trim(),
			Address = address?.Trim(),
			EmergencyContactName = emergencyContactName?.Trim(),
			EmergencyContactPhone = emergencyContactPhone?.Trim(),
			HireDate = hireDate,
			EmploymentTypeCode = employmentTypeCode.Trim(),
			Status = status.Trim(),
			SourceCandidateId = sourceCandidateId
		};
	}

	/// <summary>Approves employment and links the provisioned Identity account.</summary>
	public void ApproveEmployment(Guid userId, string employeeNumber)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(employeeNumber);
		if (userId == Guid.Empty) throw new ArgumentException("User id is required.", nameof(userId));
		UserId = userId;
		EmployeeNumber = employeeNumber.Trim();
		Status = LifecycleStatuses.Hired;
		MarkAsUpdated();
	}

	public void SetRecruitmentStatus(string status)
	{
		if (status is not (LifecycleStatuses.Submitted or LifecycleStatuses.Rejected or LifecycleStatuses.WaitingList))
			throw new ArgumentException("Invalid recruitment status.", nameof(status));
		Status = status;
		MarkAsUpdated();
	}

	/// <summary>Terminates employment while preserving HR and payroll history.</summary>
	public void Terminate()
	{
		Status = "TERMINATED";
		MarkAsUpdated();
	}

	/// <summary>Updates editable employee business details.</summary>
	/// <param name="firstName">Employee first name.</param>
	/// <param name="lastName">Employee last name.</param>
	/// <param name="cnicNumber">Employee CNIC number.</param>
	/// <param name="email">Employee email.</param>
	/// <param name="phone">Employee phone.</param>
	/// <param name="hireDate">Employee hire date.</param>
	/// <param name="employmentTypeCode">Employment type code.</param>
	/// <param name="status">Employee status.</param>
	public void UpdateDetails(
		string firstName,
		string? lastName,
		string? cnicNumber,
		string? email,
		string? phone,
		
		DateOnly hireDate,
		string employmentTypeCode,
		string status)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
		ArgumentException.ThrowIfNullOrWhiteSpace(employmentTypeCode);
		ArgumentException.ThrowIfNullOrWhiteSpace(status);

		FirstName = firstName.Trim();
		LastName = lastName?.Trim();
		CnicNumber = cnicNumber?.Trim();
		Email = email?.Trim();
		Phone = phone?.Trim();
		HireDate = hireDate;
		EmploymentTypeCode = employmentTypeCode.Trim();
		Status = status.Trim();
		MarkAsUpdated();
	}

	/// <summary>Updates the employee photograph.</summary>
	/// <param name="photo">Photograph bytes.</param>
	/// <param name="contentType">Photograph MIME type.</param>
	/// <param name="fileName">Photograph file name.</param>
	public void UpdatePhoto(byte[]? photo, string? contentType, string? fileName)
	{
		Photo = photo;
		PhotoContentType = contentType?.Trim();
		PhotoFileName = fileName?.Trim();
		MarkAsUpdated();
	}
	private static EmployeeDesignation ParseDesignation(string? value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return EmployeeDesignation.Other;
		}

		var normalized = value.Replace("_", string.Empty, StringComparison.Ordinal)
			.Replace("-", string.Empty, StringComparison.Ordinal)
			.Replace(" ", string.Empty, StringComparison.Ordinal);

		return Enum.TryParse<EmployeeDesignation>(normalized, true, out var designation)
			? designation
			: EmployeeDesignation.Other;
	}

}
