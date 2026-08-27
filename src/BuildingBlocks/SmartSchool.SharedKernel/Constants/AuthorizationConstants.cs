namespace SmartSchool.SharedKernel.Constants;

public static class SmartSchoolRoles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string SchoolAdmin = "SchoolAdmin";
    public const string Admin = "Admin";
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
    public const string UserAdministration = "UserAdministration";
    public const string Impersonation = "Impersonation";
    public const string WorkflowAdministration = "WorkflowAdministration";
    public const string SchoolAdministration = "SchoolAdministration";
    public const string AcademicManagement = "AcademicManagement";
    public const string TeacherWorkspace = "TeacherWorkspace";
    public const string StudentSelfService = "StudentSelfService";
    public const string ParentSelfService = "ParentSelfService";
    public const string DriverWorkspace = "DriverWorkspace";
    public const string ExaminationManagement = "ExaminationManagement";
    public const string FinanceManagement = "FinanceManagement";
    public const string HumanResourcesManagement = "HumanResourcesManagement";

    // Actor-composition policies. Names intentionally describe every role allowed.
    public const string SuperAdminOnly = "SuperAdminOnly";
    public const string SuperAdminTenantOnly = "SuperAdminTenantOnly";
    public const string SuperAdminTenantTeacher = "SuperAdminTenantTeacher";
    public const string SuperAdminTenantStudent = "SuperAdminTenantStudent";
    public const string SuperAdminTenantParent = "SuperAdminTenantParent";
    public const string SuperAdminTenantAdmin = "SuperAdminTenantAdmin";
    public const string SuperAdminTenantDriver = "SuperAdminTenantDriver";
    public const string AllAuthenticatedActors = "AllAuthenticatedActors";
}

public static class SmartSchoolClaims
{
    public const string TenantId = "tenant_id";
    public const string SchoolId = "school_id";
    public const string UserId = "sub";
    public const string Role = "role";
    public const string BranchId = "branch_id";
    public const string StudentId = "student_id";
    public const string TeacherId = "teacher_id";
    public const string DriverId = "driver_id";
    public const string ExaminerId = "examiner_id";
    public const string EmployeeId = "employee_id";
    public const string FirstName = "given_name";
    public const string LastName = "family_name";
    public const string MustChangePassword = "must_change_password";
    public const string Impersonated = "impersonated";
    public const string ImpersonatorSubject = "impersonator_sub";
}
