using SmartSchool.Application.Messaging;
using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Teachers;

public static class Module
{
    public static IServiceCollection AddTeachersModule(this IServiceCollection services)
    {
        services.AddFeatureDataAccess(typeof(Module).Assembly);
        return services;
    }

    public static IEndpointRouteBuilder MapTeachersEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var g = endpoints.MapGroup("/api/teachers").WithTags("Teachers").RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantTeacher);
        g.MapGet("/me", GetMe).RequireAuthorization(SmartSchoolPolicies.TeacherWorkspace);
        g.MapGet("/{employeeId:guid}", GetTeacher);
        g.MapGet("/{employeeId:guid}/classes", GetClasses);
        g.MapGet("/{employeeId:guid}/students", GetStudents);
        g.MapGet("/{employeeId:guid}/timetable", GetTimetable);
        g.MapGet("/{employeeId:guid}/assignments", GetAssignments);
        g.MapGet("/{employeeId:guid}/workload", GetWorkload);
        g.MapGet("/{employeeId:guid}/dashboard", GetDashboard);
        g.MapPost("/{employeeId:guid}/assignments", CreateAssignment).RequireAuthorization(SmartSchoolPolicies.TeacherWorkspace);
        g.MapPut("/{employeeId:guid}/submissions/{submissionId:guid}/grade", GradeSubmission).RequireAuthorization(SmartSchoolPolicies.TeacherWorkspace);
        g.MapPost("/{employeeId:guid}/leave", ApplyLeave).RequireAuthorization(SmartSchoolPolicies.TeacherWorkspace);
        return endpoints;
    }

    private static async Task<IResult> GetMe(ITenantScope scope, IDbConnectionFactory factory, CancellationToken ct)
    {
        const string sql="""SELECT employee_id AS "EmployeeId",tenant_id AS "TenantId",user_id AS "UserId",employee_number AS "EmployeeNumber",first_name AS "FirstName",last_name AS "LastName",email AS "Email",phone AS "Phone",status AS "Status" FROM hr.employee WHERE tenant_id=@TenantId AND user_id=@UserId LIMIT 1;""";
        await using var c = await factory.OpenConnectionAsync(ct);
		var row=await c.QuerySingleOrDefaultAsync(new CommandDefinition(sql,
			new { tenantId= scope.TenantId,userId=scope.UserId},
			cancellationToken:ct));
		return row is null ? Results.NotFound( new
		{ message="Teacher profile is not linked to this user."
		}): Results.Ok(row);
    }
    private static async Task<IResult> GetTeacher(Guid employeeId,Guid? tenantId,ITenantScope scope,IDbConnectionFactory f,CancellationToken ct)=>await One("""SELECT e.employee_id AS "EmployeeId",e.tenant_id AS "TenantId",e.user_id AS "UserId",e.employee_number AS "EmployeeNumber",e.first_name AS "FirstName",e.last_name AS "LastName",e.email AS "Email",e.phone AS "Phone",e.hire_date AS "HireDate",e.employment_type_code AS "EmploymentType",e.status AS "Status",tp.qualification AS "Qualification",tp.specialization AS "Specialization",tp.teaching_experience_years AS "TeachingExperienceYears" FROM hr.employee e LEFT JOIN hr."TeacherProfile" tp ON tp."EmployeeId"=e.employee_id AND tp."TenantId"=e.tenant_id WHERE e.tenant_id=@TenantId AND e.employee_id=@EmployeeId;""",employeeId,Tenant(scope,tenantId),f,ct);
    private static async Task<IResult> GetClasses(Guid employeeId,Guid? tenantId,ITenantScope scope,IDbConnectionFactory f,CancellationToken ct)=>await Many("""SELECT a.teacher_course_assignment_id AS "AssignmentId",a.course_offering_id AS "CourseOfferingId",a.class_section_id AS "ClassSectionId",a.assignment_role AS "Role",a.periods_per_week AS "PeriodsPerWeek",a.effective_from AS "EffectiveFrom",a.effective_to AS "EffectiveTo" FROM academic.teacher_course_assignment a WHERE a.tenant_id=@TenantId AND a.employee_id=@EmployeeId AND (a.effective_to IS NULL OR a.effective_to>=CURRENT_DATE);""",employeeId,Tenant(scope,tenantId),f,ct);
    private static async Task<IResult> GetStudents(Guid employeeId,Guid? tenantId,ITenantScope scope,IDbConnectionFactory f,CancellationToken ct)=>await Many("""SELECT DISTINCT s.student_id AS "StudentId",s.student_number AS "StudentNumber",s.first_name AS "FirstName",s.last_name AS "LastName",s.status AS "Status",a.class_section_id AS "ClassSectionId" FROM academic.teacher_course_assignment a JOIN student.student_enrollment se ON se.class_section_id=a.class_section_id AND se.tenant_id=a.tenant_id JOIN student.student s ON s.student_id=se.student_id AND s.tenant_id=a.tenant_id WHERE a.tenant_id=@TenantId AND a.employee_id=@EmployeeId;""",employeeId,Tenant(scope,tenantId),f,ct);
    private static async Task<IResult> GetTimetable(Guid employeeId,Guid? tenantId,ITenantScope scope,IDbConnectionFactory f,CancellationToken ct)=>await Many("""SELECT te.timetable_entry_id AS "TimetableEntryId",te.day_of_week AS "DayOfWeek",p.name AS "Period",p.start_time AS "StartTime",p.end_time AS "EndTime",te.class_section_id AS "ClassSectionId",te.course_offering_id AS "CourseOfferingId",te.room_id AS "RoomId" FROM academic.teacher_course_assignment a JOIN academic.timetable_entry te ON te.teacher_course_assignment_id=a.teacher_course_assignment_id JOIN academic.timetable_period p ON p.timetable_period_id=te.timetable_period_id WHERE a.tenant_id=@TenantId AND a.employee_id=@EmployeeId ORDER BY te.day_of_week,p.start_time;""",employeeId,Tenant(scope,tenantId),f,ct);
    private static async Task<IResult> GetAssignments(Guid employeeId,Guid? tenantId,ITenantScope scope,IDbConnectionFactory f,CancellationToken ct)=>await Many("""SELECT academic_assignment_id AS "AssignmentId",title AS "Title",assignment_type_code AS "Type",assigned_at AS "AssignedAt",due_at AS "DueAt",total_marks AS "TotalMarks",status AS "Status",class_section_id AS "ClassSectionId" FROM lms.academic_assignment WHERE tenant_id=@TenantId AND teacher_employee_id=@EmployeeId ORDER BY assigned_at DESC;""",employeeId,Tenant(scope,tenantId),f,ct);
    private static async Task<IResult> GetWorkload(Guid employeeId,Guid? tenantId,ITenantScope scope,IDbConnectionFactory f,CancellationToken ct)=>await One("""SELECT @EmployeeId AS "EmployeeId",COUNT(*) AS "ActiveAssignments",COALESCE(SUM(periods_per_week),0) AS "PeriodsPerWeek",COUNT(DISTINCT class_section_id) AS "Classes" FROM academic.teacher_course_assignment WHERE tenant_id=@TenantId AND employee_id=@EmployeeId AND (effective_to IS NULL OR effective_to>=CURRENT_DATE);""",employeeId,Tenant(scope,tenantId),f,ct);
    private static async Task<IResult> GetDashboard(Guid employeeId,Guid? tenantId,ITenantScope scope,IDbConnectionFactory f,CancellationToken ct)=>await One("""SELECT @EmployeeId AS "EmployeeId",(SELECT COUNT(*) FROM academic.teacher_course_assignment WHERE tenant_id=@TenantId AND employee_id=@EmployeeId AND (effective_to IS NULL OR effective_to>=CURRENT_DATE)) AS "ActiveCourseAssignments",(SELECT COUNT(*) FROM lms.academic_assignment WHERE tenant_id=@TenantId AND teacher_employee_id=@EmployeeId AND status IN ('DRAFT','PUBLISHED','ACTIVE')) AS "Assignments",(SELECT COUNT(*) FROM lms.student_assignment_submission s JOIN lms.academic_assignment a ON a.academic_assignment_id=s.academic_assignment_id WHERE a.tenant_id=@TenantId AND a.teacher_employee_id=@EmployeeId AND s.status IN ('SUBMITTED','PENDING_REVIEW')) AS "SubmissionsToGrade",(SELECT COUNT(DISTINCT se.student_id) FROM academic.teacher_course_assignment a JOIN student.student_enrollment se ON se.class_section_id=a.class_section_id AND se.tenant_id=a.tenant_id WHERE a.tenant_id=@TenantId AND a.employee_id=@EmployeeId) AS "Students";""",employeeId,Tenant(scope,tenantId),f,ct);

    public sealed record CreateAssignmentRequest(Guid? TenantId,Guid CourseOfferingId,Guid? ClassSectionId,string Type,string Title,string? Description,string? Instructions,DateTimeOffset? DueAt,decimal? TotalMarks,bool AllowLateSubmission=false,int MaxAttempts=1);
    public sealed record GradeRequest(Guid? TenantId,decimal Marks,string? Feedback);
    public sealed record LeaveRequest(Guid? TenantId,DateOnly FromDate,DateOnly ToDate,string LeaveType,string Reason);
    private static async Task<IResult> CreateAssignment(Guid employeeId,CreateAssignmentRequest r,ITenantScope scope,IDbConnectionFactory f,CancellationToken ct){var tenant=Tenant(scope,r.TenantId);if(!tenant.HasValue)return Results.BadRequest(new{message="Tenant is required."});const string sql="""INSERT INTO lms.academic_assignment(academic_assignment_id,tenant_id,course_offering_id,class_section_id,teacher_employee_id,assignment_type_code,title,description,instructions,assigned_at,due_at,total_marks,allow_late_submission,max_attempts,status) VALUES(@Id,@TenantId,@CourseOfferingId,@ClassSectionId,@EmployeeId,@Type,@Title,@Description,@Instructions,CURRENT_TIMESTAMP,@DueAt,@TotalMarks,@AllowLateSubmission,@MaxAttempts,'PUBLISHED');""";var id=Guid.NewGuid();await using var c=await f.OpenConnectionAsync(ct);await c.ExecuteAsync(new CommandDefinition(sql,new{Id=id,TenantId=tenant.Value,r.CourseOfferingId,r.ClassSectionId,EmployeeId=employeeId,r.Type,r.Title,r.Description,r.Instructions,r.DueAt,r.TotalMarks,r.AllowLateSubmission,r.MaxAttempts},cancellationToken:ct));return Results.Created($"/api/teachers/{employeeId}/assignments/{id}",new{assignmentId=id});}
    private static async Task<IResult> GradeSubmission(Guid employeeId,Guid submissionId,GradeRequest r,ITenantScope scope,IDbConnectionFactory f,CancellationToken ct){var tenant=Tenant(scope,r.TenantId);if(!tenant.HasValue)return Results.BadRequest(new{message="Tenant is required."});const string sql="""UPDATE lms.student_assignment_submission s SET marks_obtained=@Marks,teacher_feedback=@Feedback,status='GRADED' FROM lms.academic_assignment a WHERE s.academic_assignment_id=a.academic_assignment_id AND s.submission_id=@SubmissionId AND a.tenant_id=@TenantId AND a.teacher_employee_id=@EmployeeId;""";await using var c=await f.OpenConnectionAsync(ct);var n=await c.ExecuteAsync(new CommandDefinition(sql,new{r.Marks,r.Feedback,SubmissionId=submissionId,TenantId=tenant.Value,EmployeeId=employeeId},cancellationToken:ct));return n==0?Results.NotFound():Results.Ok(new{submissionId,status="GRADED"});}
    private static async Task<IResult> ApplyLeave(Guid employeeId,LeaveRequest r,ITenantScope scope,IDbConnectionFactory f,CancellationToken ct){var tenant=Tenant(scope,r.TenantId);if(!tenant.HasValue)return Results.BadRequest(new{message="Tenant is required."});if(r.ToDate<r.FromDate)return Results.BadRequest(new{message="ToDate must be on or after FromDate."});const string sql="""INSERT INTO teacher.leave_request(leave_request_id,tenant_id,employee_id,leave_type,from_date,to_date,reason,status,created_at) VALUES(@Id,@TenantId,@EmployeeId,@LeaveType,@FromDate,@ToDate,@Reason,'PENDING',CURRENT_TIMESTAMP);""";var id=Guid.NewGuid();await using var c=await f.OpenConnectionAsync(ct);await c.ExecuteAsync(new CommandDefinition(sql,new{Id=id,TenantId=tenant.Value,EmployeeId=employeeId,r.LeaveType,r.FromDate,r.ToDate,r.Reason},cancellationToken:ct));return Results.Accepted($"/api/teachers/{employeeId}/leave/{id}",new{leaveRequestId=id,status=LifecycleStatuses.Pending});}
    private static Guid? Tenant(ITenantScope s,Guid? requested)=>s.IsSuperAdmin?requested:s.Resolve(requested);
    private static async Task<IResult> One(string sql,Guid employeeId,Guid? tenant,IDbConnectionFactory f,CancellationToken ct){if(!tenant.HasValue)return Results.BadRequest(new{message="Tenant is required."});await using var c=await f.OpenConnectionAsync(ct);var x=await c.QuerySingleOrDefaultAsync(new CommandDefinition(sql,new{TenantId=tenant.Value,EmployeeId=employeeId},cancellationToken:ct));return x is null?Results.NotFound():Results.Ok(x);}
    private static async Task<IResult> Many(string sql,Guid employeeId,Guid? tenant,IDbConnectionFactory f,CancellationToken ct){if(!tenant.HasValue)return Results.BadRequest(new{message="Tenant is required."});await using var c=await f.OpenConnectionAsync(ct);return Results.Ok(await c.QueryAsync(new CommandDefinition(sql,new{TenantId=tenant.Value,EmployeeId=employeeId},cancellationToken:ct)));}
}
