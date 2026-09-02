namespace SmartSchool.SharedKernel.Constants;

public static class SmartSchoolRoles
{
    public const string SuperAdmin = nameof(Role.SuperAdmin);
    public const string Tenant = nameof(Role.Tenant);
    public const string Principal = nameof(Role.Principal);
    public const string Admin = nameof(Role.Admin);
    public const string Teacher = nameof(Role.Teacher);
    public const string Student = nameof(Role.Student);
    public const string StudentEntity = Student;
    public const string Parent = nameof(Role.Parent);
    public const string Driver = nameof(Role.Driver);
    public const string Accountant = nameof(Role.Accountant);
    public const string HrManager = nameof(Role.HRManager);
    public const string Librarian = nameof(Role.Librarian);
    public const string Examiner = nameof(Role.Examiner);

    // Compatibility aliases. New code must use the canonical Role enum.
    public const string SchoolAdmin = Tenant;
    public const string Staff = Admin;
    public const string TransportManager = Admin;
    public const string AdmissionOfficer = Admin;
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
    public const string DisplayName = "name";
    public const string Email = "email";
    public const string AccountType = "account_type";
    public const string MustChangePassword = "must_change_password";
    public const string Impersonated = "impersonated";
    public const string ImpersonatorSubject = "impersonator_sub";
}
