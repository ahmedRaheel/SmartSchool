namespace SmartSchool.SharedKernel.Constants;

public static class SmartSchoolRoles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string SchoolAdmin = "SchoolAdmin";
    public const string Principal = "Principal";
    public const string Teacher = "Teacher";
    public const string StudentEntity = "StudentEntity";
    public const string Parent = "Parent";
    public const string Accountant = "Accountant";
    public const string HrManager = "HRManager";
    public const string Librarian = "Librarian";
    public const string TransportManager = "TransportManager";
    public const string AdmissionOfficer = "AdmissionOfficer";
}

public static class SmartSchoolPolicies
{
    public const string SchoolAdministration = "SchoolAdministration";
    public const string AcademicManagement = "AcademicManagement";
    public const string StudentSelfService = "StudentSelfService";
    public const string ParentSelfService = "ParentSelfService";
    public const string FinanceManagement = "FinanceManagement";
    public const string HumanResourcesManagement = "HumanResourcesManagement";
}

public static class SmartSchoolClaims
{
    public const string TenantId = "tenant_id";
    public const string SchoolId = "school_id";
    public const string UserId = "sub";
    public const string Role = "role";
}
