using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Api.Features;

public static class WorkflowCatalogEndpoints
{
    public sealed record WorkflowDefinitionDto(
        string Code, string Name, string[] Initiators, string[] Approvers, string[] Steps);

    public static IEndpointRouteBuilder MapWorkflowCatalogEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/workflows/catalog", (HttpContext http) => Result<WorkflowDefinitionDto[]>.Success(Catalog))
            .WithTags("Workflow")
            .RequireAuthorization();
        return endpoints;
    }

    private static readonly WorkflowDefinitionDto[] Catalog =
    [
        new("STUDENT_ADMISSION","Student Admission",["SchoolAdmin"],["SchoolAdmin","Principal"],
            ["Application","DocumentVerification","Eligibility","Assessment","Approval","AccountCreation","Enrollment","ClassSectionPlacement","ParentLink","Notify"]),
        new("CLASS_ASSIGNMENT","Class / Section Assignment",["SchoolAdmin","Principal"],["SchoolAdmin","Principal"],
            ["ValidateAcademicContext","CapacityCheck","ConflictCheck","Approval","Assign","Notify"]),
        new("SECTION_CHANGE","Student Section Change",["SchoolAdmin","Principal"],["SchoolAdmin","Principal"],
            ["Request","CapacityCheck","CompatibilityCheck","Approval","Transfer","Notify"]),
        new("CLASS_TEST","Class Test",["Teacher"],["Teacher"],
            ["Draft","AssignmentValidation","ScheduleCheck","Publish","Submission","Marking","PublishResult","PredictionRefresh"]),
        new("STUDENT_LEAVE","Student Leave",["Student","Parent"],["Teacher","SchoolAdmin"],
            ["Request","Validate","Approval","AttendanceUpdate","TransportUpdate","Notify"]),
        new("TEACHER_LEAVE","Teacher Leave",["Teacher"],["SchoolAdmin","Principal"],
            ["Request","ImpactAnalysis","SubstituteRecommendation","Approval","TimetableUpdate","Notify"]),
        new("TEACHER_SUBJECT_ASSIGNMENT","Teacher Subject Assignment",["SchoolAdmin","Principal"],["SchoolAdmin","Principal"],
            ["Need","Eligibility","WorkloadCalculation","ConflictCheck","CandidateRanking","Approval","Assignment","TimetableUpdate","Notify"]),
        new("EXAM_LIFECYCLE","Exam Lifecycle",["Examiner","SchoolAdmin"],["SchoolAdmin","Principal"],
            ["Draft","ConflictValidation","ResourceAssignment","Approval","Publish","Marks","Moderation","ResultApproval","PublishResult"]),
        new("FEE_CONCESSION","Fee Concession / Waiver",["SchoolAdmin","Parent"],["SchoolAdmin","Accountant"],
            ["Request","Eligibility","FinanceApproval","LedgerAdjustment","Audit","Notify"]),
        new("STAFF_HIRING","Staff Hiring",["SchoolAdmin","HRManager"],["SchoolAdmin","HRManager"],
            ["Candidate","Screening","Interview","Decision","Offer","EmployeeAccount","Onboarding"]),
        new("STUDENT_WITHDRAWAL","Student Withdrawal / Transfer",["Parent","SchoolAdmin"],["SchoolAdmin","Principal"],
            ["Request","Clearance","Approval","Documents","CloseEnrollment","AccountUpdate","Notify"]),
        new("NOTICE_PUBLICATION","Document / Notice Publication",["Teacher","SchoolAdmin","Principal"],["SchoolAdmin","Principal"],
            ["Draft","Audience","Approval","Publish","NotificationFanout","ReadReceipt"]),
        new("TIMETABLE_CHANGE","Timetable Change",["Teacher","SchoolAdmin"],["SchoolAdmin","Principal"],
            ["Request","ConflictCheck","WorkloadValidation","Approval","Apply","Notify"]),
        new("TRANSPORT_ASSIGNMENT","Transport Assignment / Change",["Parent","SchoolAdmin"],["SchoolAdmin","TransportManager"],
            ["Request","Capacity","DriverRouteValidation","Confirmation","Assign","Notify"]),
        new("ROLE_CHANGE","Role / Permission Change",["SchoolAdmin"],["SchoolAdmin"],
            ["Request","BoundaryValidation","Approval","IdentityUpdate","SessionRevocation","Audit"])
    ];
}
