using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Models;

/// <summary>
/// Represents a student enrolled in a SmartSchool tenant.
/// </summary>
public sealed class StudentEntity : Entity
{
	private StudentEntity()
	{
	}

	/// <summary>Gets the optional authenticated user identifier.</summary>
	public Guid? UserId { get; private set; }

	/// <summary>Gets the tenant-unique student number.</summary>
	public string StudentNumber { get; private set; } = string.Empty;

	/// <summary>Gets the student's first name.</summary>
	public string FirstName { get; private set; } = string.Empty;

	/// <summary>Gets the student's last name.</summary>
	public string? LastName { get; private set; }

	/// <summary>Gets the student's date of birth.</summary>
	public DateOnly? DateOfBirth { get; private set; }

	/// <summary>Gets the student's gender.</summary>
	public string? Gender { get; private set; }

	/// <summary>Gets the student photograph bytes.</summary>
	public byte[]? Photo { get; private set; }

	/// <summary>Gets the photograph MIME type.</summary>
	public string? PhotoContentType { get; private set; }

	/// <summary>Gets the photograph file name.</summary>
	public string? PhotoFileName { get; private set; }

	/// <summary>Gets the admission date.</summary>
	public DateOnly? AdmissionDate { get; private set; }

	/// <summary>Gets the current student status.</summary>
	public string Status { get; private set; } = "ACTIVE";

	/// <summary>Creates a student.</summary>
	public static StudentEntity Create(
		Guid tenantId,
		Guid? userId,
		string studentNumber,
		string firstName,
		string? lastName,
		DateOnly? dateOfBirth,
		string? gender,
		byte[]? photo,
		string? photoContentType,
		string? photoFileName,
		DateOnly? admissionDate,
		string status)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(studentNumber);
		ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
		ArgumentException.ThrowIfNullOrWhiteSpace(status);

		return new StudentEntity
		{
			TenantId = tenantId,
			UserId = userId,
			StudentNumber = studentNumber.Trim(),
			FirstName = firstName.Trim(),
			LastName = lastName?.Trim(),
			DateOfBirth = dateOfBirth,
			Gender = gender?.Trim(),
			Photo = photo,
			PhotoContentType = photoContentType?.Trim(),
			PhotoFileName = photoFileName?.Trim(),
			AdmissionDate = admissionDate,
			Status = status.Trim()
		};
	}

	/// <summary>Updates editable student details.</summary>
	public void UpdateDetails(
		string firstName,
		string? lastName,
		DateOnly? dateOfBirth,
		string? gender,
		DateOnly? admissionDate,
		string status)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
		ArgumentException.ThrowIfNullOrWhiteSpace(status);

		FirstName = firstName.Trim();
		LastName = lastName?.Trim();
		DateOfBirth = dateOfBirth;
		Gender = gender?.Trim();
		AdmissionDate = admissionDate;
		Status = status.Trim();
		MarkAsUpdated();
	}

	/// <summary>Updates the student photograph.</summary>
	public void UpdatePhoto(
		byte[]? photo,
		string? contentType,
		string? fileName)
	{
		Photo = photo;
		PhotoContentType = contentType?.Trim();
		PhotoFileName = fileName?.Trim();
		MarkAsUpdated();
	}
}
