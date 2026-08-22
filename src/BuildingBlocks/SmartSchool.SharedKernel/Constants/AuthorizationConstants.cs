namespace SmartSchool.SharedKernel.Constants;

public static class SmartSchoolRoles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string SchoolAdmin = "SchoolAdmin";
    public const string Principal = "Principal";
    public const string Teacher = "Teacher";
    public const string Student = "Student";
    public const string StudentEntity = Student; // backward-compatible alias
    public const string Parent = "Parent";
    public const string Driver = "Driver";
    public const string Examiner = "Examiner";
    public const string Staff = "Staff";
    public const string Accountant = "Accountant";
    public const string HrManager = "HRManager";
    public const string Librarian = "Librarian";
    public const string TransportManager = "TransportManager";
    public const string AdmissionOfficer = "AdmissionOfficer";
}

public static class SmartSchoolPolicies
{
    public const string PlatformAdministration = "PlatformAdministration";
    public const string SchoolAdministration = "SchoolAdministration";
    public const string AcademicManagement = "AcademicManagement";
    public const string TeacherWorkspace = "TeacherWorkspace";
    public const string StudentSelfService = "StudentSelfService";
    public const string ParentSelfService = "ParentSelfService";
    public const string DriverWorkspace = "DriverWorkspace";
    public const string ExaminationManagement = "ExaminationManagement";
    public const string FinanceManagement = "FinanceManagement";
    public const string HumanResourcesManagement = "HumanResourcesManagement";
}

public static class SmartSchoolClaims
{
    public const string TenantId = "tenant_id";
    public const string SchoolId = "school_id";
    public const string UserId = "sub";
    public const string Role = "role";
    public const string MustChangePassword = "must_change_password";
    public const string Impersonated = "impersonated";
    public const string ImpersonatorSubject = "impersonator_sub";
}
