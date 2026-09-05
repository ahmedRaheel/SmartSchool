namespace SmartSchool.SharedKernel.Documents;

/// <summary>
/// Defines document classification and lifecycle constants shared by modules.
/// </summary>
public static class DocumentConstants
{
    public const int MaximumOriginalFileNameLength = 255;
    public const int MaximumContentTypeLength = 150;
    public const int MaximumStorageProviderLength = 50;
    public const int MaximumStorageKeyLength = 500;
    public const int MaximumDocumentNumberLength = 100;
    public const int MaximumNotesLength = 1000;
    public const int Sha256Length = 64;
}

/// <summary>
/// Well-known document type codes.
/// </summary>
public static class DocumentTypeCodes
{
    public const string ProfilePicture = "PROFILE_PICTURE";
    public const string BirthCertificate = "BIRTH_CERTIFICATE";
    public const string BForm = "B_FORM";
    public const string CnicFront = "CNIC_FRONT";
    public const string CnicBack = "CNIC_BACK";
    public const string Passport = "PASSPORT";
    public const string AcademicCertificate = "ACADEMIC_CERTIFICATE";
    public const string Degree = "DEGREE";
    public const string ExperienceCertificate = "EXPERIENCE_CERTIFICATE";
    public const string DrivingLicense = "DRIVING_LICENSE";
    public const string PoliceVerification = "POLICE_VERIFICATION";
    public const string MedicalCertificate = "MEDICAL_CERTIFICATE";
    public const string Resume = "RESUME";
    public const string EmploymentContract = "EMPLOYMENT_CONTRACT";
    public const string Other = "OTHER";
}
