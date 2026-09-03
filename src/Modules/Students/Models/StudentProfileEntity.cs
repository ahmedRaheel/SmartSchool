using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Models;

/// <summary>
/// Stores the detailed personal, academic and emergency profile for a student.
/// </summary>
public sealed class StudentProfileEntity : Entity
{
    /// <summary>Gets the entity-specific identifier.</summary>
    public Guid StudentProfileId { get; private set; } = Guid.NewGuid();

    public Guid StudentId { get; private set; }
    public string AdmissionNumber { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string? MiddleName { get; private set; }
    public string LastName { get; private set; } = string.Empty;
    public DateOnly DateOfBirth { get; private set; }
    public string GenderCode { get; private set; } = string.Empty;
    public string? BFormNumber { get; private set; }
    public string? PassportNumber { get; private set; }
    public string? BloodGroupCode { get; private set; }
    public string? PrimaryLanguageCode { get; private set; }
    public string? MobileNumber { get; private set; }
    public string? EmailAddress { get; private set; }
    public string? AddressLine1 { get; private set; }
    public string? AddressLine2 { get; private set; }
    public string? City { get; private set; }
    public string? Province { get; private set; }
    public string? PostalCode { get; private set; }
    public string? CountryCode { get; private set; }
    public string? EmergencyContactName { get; private set; }
    public string? EmergencyContactPhone { get; private set; }
    public string? MedicalNotes { get; private set; }
    public string? Allergies { get; private set; }
    public DateOnly AdmissionDate { get; private set; }
    public Guid? CurrentClassId { get; private set; }
    public Guid? CurrentSectionId { get; private set; }

    private StudentProfileEntity() { }
}
