#pragma warning disable DAP005 // These integration fixtures intentionally use classic Dapper.
using System.Data.Common;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Dapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;
using SmartSchool.Infrastructure.Identity;
using SmartSchool.Infrastructure.Persistence;
using SmartSchool.Modules.Admissions;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.Modules.Documents;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.Modules.Examinations;
using SmartSchool.Modules.Examinations.Persistence;
using SmartSchool.Modules.HR;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.Modules.Learning;
using SmartSchool.Modules.Learning.Persistence;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.Modules.Students;
using SmartSchool.Modules.Students.Persistence;
using SmartSchool.Modules.Transport;
using SmartSchool.Modules.Transport.Persistence;

internal static class WorkflowChecks
{
    internal static readonly Guid Tenant = Guid.Parse("91000000-0000-0000-0000-000000000001");
    internal static readonly Guid OtherTenant = Guid.Parse("91000000-0000-0000-0000-000000000002");
    internal static readonly Guid Campus = Guid.Parse("92000000-0000-0000-0000-000000000001");
    internal static readonly Guid School = Guid.Parse("93000000-0000-0000-0000-000000000001");
    internal static readonly Guid Year = Guid.Parse("94000000-0000-0000-0000-000000000001");
    internal static readonly Guid Subject = Guid.Parse("95000000-0000-0000-0000-000000000001");
    internal static readonly Guid Teacher = Guid.Parse("96000000-0000-0000-0000-000000000001");
    internal static readonly Guid DriverEmployee = Guid.Parse("96000000-0000-0000-0000-000000000002");
    internal static readonly Guid Student = Guid.Parse("97000000-0000-0000-0000-000000000001");
    internal static readonly Guid User = Guid.Parse("98000000-0000-0000-0000-000000000001");
    internal static Guid UserFor(string role) => Guid.Parse("98000000-0000-0000-0000-00000000000" + (role switch { "Teacher" => "2", "Driver" => "3", "Student" => "4", "Parent" => "5", _ => "1" }));
    private static int s_checks;
    private static readonly List<string> Passed = [];
    private static string ConnectionString => Environment.GetEnvironmentVariable("SMARTSCHOOL_TEST_DATABASE")
        ?? "Host=127.0.0.1;Port=5433;Database=postgres;Username=postgres;Password=postgres;SSL Mode=Disable;Pooling=false;Include Error Detail=true";

    public static async Task RunAsync()
    {
        DateTimeTypeHandlers.Register();
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = "Testing" });
        builder.Logging.ClearProviders();
        builder.WebHost.UseUrls("http://127.0.0.1:5487");
        var services = builder.Services;
        services.ConfigureHttpJsonOptions(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthentication>("Test", _ => { });
        services.AddSmartSchoolAuthorization();
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<IDbConnectionFactory>(new TestConnectionFactory(ConnectionString));
        services.AddScoped<IBusinessNumberGenerator, BusinessNumberGenerator>();
        services.AddSingleton<IIdentityAccountService, TestAccounts>();
        services.AddDbContext<OrganizationDbContext>(o => o.UseNpgsql(ConnectionString));
        services.AddDbContext<StudentsDbContext>(o => o.UseNpgsql(ConnectionString));
        services.AddDbContext<AdmissionsDbContext>(o => o.UseNpgsql(ConnectionString));
        services.AddDbContext<HRDbContext>(o => o.UseNpgsql(ConnectionString));
        services.AddDbContext<DocumentsDbContext>(o => o.UseNpgsql(ConnectionString));
        services.AddDbContext<LearningDbContext>(o => o.UseNpgsql(ConnectionString));
        services.AddDbContext<ExaminationsDbContext>(o => o.UseNpgsql(ConnectionString));
        services.AddDbContext<TransportDbContext>(o => o.UseNpgsql(ConnectionString));
        services.AddOrganizationModule(); services.AddStudentsModule(); services.AddAdmissionsModule(); services.AddHRModule();
        services.AddDocumentsModule(); services.AddLearningModule(); services.AddExaminationsModule(); services.AddTransportModule();
        await using var app = builder.Build();
        app.Use(async (context, next) =>
        {
            try { await next(context); }
            catch (Exception ex)
            {
                context.Response.StatusCode = ex is ValidationException ? 400 : ex is UnauthorizedAccessException ? 403 : 500;
                await context.Response.WriteAsJsonAsync(new { error = ex.ToString() });
            }
        });
        app.UseAuthentication(); app.UseAuthorization();
        app.MapOrganizationEndpoints(); app.MapStudentsEndpoints(); app.MapAdmissionsEndpoints(); app.MapHREndpoints();
        app.MapDocumentsEndpoints(); app.MapLearningEndpoints(); app.MapExaminationsEndpoints(); app.MapTransportEndpoints();
        app.MapTeachersEndpoints();
        await SeedAsync();
        await app.StartAsync();
        using var client = new HttpClient { BaseAddress = new Uri("http://127.0.0.1:5487"), Timeout = TimeSpan.FromSeconds(25) };
        void Actor(string role) { client.DefaultRequestHeaders.Clear(); client.DefaultRequestHeaders.Add("X-Test-Role", role); }
        async Task<JsonNode> Call(string method, string path, object? body = null, int status = 200, string? name = null)
        {
            using var req = new HttpRequestMessage(new HttpMethod(method), path);
            if (body is HttpContent content) req.Content = content; else if (body is not null) req.Content = JsonContent.Create(body);
            using var result = await client.SendAsync(req);
            var raw = await result.Content.ReadAsStringAsync();
            if ((int)result.StatusCode != status) throw new InvalidOperationException($"{name ?? path}: expected {status}, received {(int)result.StatusCode}: {raw}");
            s_checks++; Passed.Add(name ?? method + " " + path); Console.WriteLine($"PASS {s_checks}: {name ?? method + " " + path}");
            if (string.IsNullOrWhiteSpace(raw)) return new JsonObject();
            var node = JsonNode.Parse(raw)!; return node is JsonObject obj ? obj["value"] ?? obj : node;
        }
        void Assert(bool success, string label) { if (!success) throw new InvalidOperationException(label); s_checks++; Passed.Add(label); Console.WriteLine($"PASS {s_checks}: {label}"); }
        var tid = Tenant.ToString(); var q = $"tenantId={tid}";
        Actor("Admin");
        var grade = await Call("POST", "/api/academics/grade-level", new { tenantId = Tenant, campusId = Campus, name = "Grade 5", educationLevelId = "20000000-0000-0000-0000-000000000002", academicYearId = Year, sections = new[] { new { name = "A", capacity = 30, roomNo = "101" } } }, name: "Create grade and section atomically");
        var setup = await Call("GET", $"/api/academics/teaching-allocations?{q}&campusId={Campus}");
        var sectionId = setup["sections"]![0]!["id"]!.GetValue<string>();
        await Call("GET", $"/api/academics/academic-year?{q}&campusId={Campus}&page=1&pageSize=100");
        await Call("GET", $"/api/academics/grade-level?{q}&page=1&pageSize=100");
        await Call("GET", $"/api/academics/class-section?{q}&page=1&pageSize=100");
        var allocation = await Call("POST", "/api/academics/teaching-allocations", new { tenantId = Tenant, campusId = Campus, classSectionId = sectionId, subjectId = Subject, employeeId = Teacher, periodsPerWeek = 5 });
        var courseId = allocation["courseOfferingId"]!.GetValue<string>();
        await using (var db = new NpgsqlConnection(ConnectionString))
        {
            await db.OpenAsync();
            await db.ExecuteAsync("INSERT INTO student.student_enrollment(tenant_id,student_id,academic_year_id,class_section_id,enrollment_number) VALUES(@Tenant,@Student,@Year,@Section,'CHECK-001')", new { Tenant, Student, Year, Section = Guid.Parse(sectionId) });
        }
        await Call("POST", "/api/academics/teaching-allocations", new { tenantId = Tenant, campusId = Campus, classSectionId = sectionId, subjectId = Subject, employeeId = Teacher, periodsPerWeek = 5 }, 409, "Duplicate allocation rejected");
        Actor("Teacher");
        await Call("GET", $"/api/teachers/{Teacher}/classes?{q}", name:"Teacher class workspace uses saved allocation");
        await Call("GET", $"/api/teachers/{DriverEmployee}/classes?{q}", status:403, name:"Teacher cannot read another teacher workspace");
        await Call("GET", $"/api/teachers/{Teacher}/timetable?{q}");
        var teacherStudents = await Call("GET", $"/api/teachers/{Teacher}/students?{q}", name:"Teacher reads saved class roster");
        Assert(teacherStudents.AsArray().Count == 1 && teacherStudents[0]!["id"]!.GetValue<string>() == Student.ToString(), "Teacher roster matches enrolled student");
        var options = await Call("GET", $"/api/learning/assignment-options?{q}", name: "Teacher reads saved allocations");
        var assignment = await Call("POST", "/api/learning/assignment", new { tenantId = Tenant, name = "Fractions", courseOfferingId = courseId, classSectionId = sectionId, teacherEmployeeId = Teacher, assignmentTypeCode = "HOMEWORK", totalMarks = 20, dueAt = DateTimeOffset.UtcNow.AddDays(1), maxAttempts = 1 });
        var assignmentId = assignment["id"]!.GetValue<string>();
        await Call("GET", $"/api/learning/assignment?{q}&page=1&pageSize=25");
        await Call("GET", $"/api/learning/assignment?tenantId={OtherTenant}&page=1&pageSize=25", status:403, name:"Cross-tenant request rejected");
        Actor("Student");
        await Call("POST", "/api/learning/assignment", new { tenantId = Tenant, name = "Unauthorized" }, 403, "Student cannot create assignments");
        var studentAssignments = await Call("GET", $"/api/learning/assignment?{q}&page=1&pageSize=25");
        Assert(studentAssignments["items"]!.AsArray().Count == 1, "Student sees only enrolled class assignments");
        MultipartFormDataContent Submission() { var form = new MultipartFormDataContent(); form.Add(new StringContent(tid), "tenantId"); form.Add(new StringContent(assignmentId), "assignmentId"); form.Add(new StringContent("Two halves make one whole."), "comment"); form.Add(new ByteArrayContent("verified-content"u8.ToArray()), "file", "answer.txt"); return form; }
        var submitted = await Call("POST", "/api/learning/assignment-submission", Submission(), name:"Student uploads actual assignment file");
        var submissionId = submitted["id"]!.GetValue<string>();
        await Call("POST", "/api/learning/assignment-submission", Submission(), 409, "Submission attempt limit enforced");
        Actor("Teacher");
        var roster = await Call("GET", $"/api/learning/assignment-submission?{q}&assignmentId={assignmentId}");
        await Call("PUT", $"/api/learning/assignment-submission/{submissionId}/grade", new { tenantId = Tenant, marks = 21, feedback = "Invalid" }, 400, "Assignment marks cannot exceed total");
        await Call("PUT", $"/api/learning/assignment-submission/{submissionId}/grade", new { tenantId = Tenant, marks = 18, feedback = "Well explained." });
        Actor("Student");
        var graded = await Call("GET", $"/api/learning/assignment?{q}&page=1&pageSize=25");
        Assert(graded.ToJsonString().Contains("18"), "Student reads persisted assignment grade");
        using (var file = await client.GetAsync($"/api/learning/assignment-submission/{submissionId}/file?{q}")) Assert(file.IsSuccessStatusCode && await file.Content.ReadAsStringAsync() == "verified-content", "Submitted file downloads byte-for-byte");
        Actor("Admin");
        await Call("POST", "/api/examinations/grade-scale", new { tenantId = Tenant, campusId = Campus, name = "A", minimumPercentage = 80, maximumPercentage = 100 });
        await Call("GET", $"/api/examinations/grade-scale?{q}&page=1&pageSize=100");
        var exam = await Call("POST", "/api/examinations/exam", new { tenantId = Tenant, name = "Term test", classSectionId = sectionId, examTypeCode = "MID_TERM", startDate = "2026-09-16", endDate = "2026-09-17", subjects = new[] { new { courseOfferingId = courseId, totalMarks = 100, passingMarks = 40, examDate = "2026-09-16" } } });
        var examId = exam["id"]!.GetValue<string>();
        await Call("POST", $"/api/examinations/exam/{examId}/publish", new { tenantId = Tenant }, 400, "Incomplete exam cannot be published");
        var resultGrid = await Call("GET", $"/api/examinations/exam/{examId}/results?{q}");
        var subjectId = resultGrid["subjects"]![0]!["id"]!.GetValue<string>();
        var resultsBody = new { tenantId = Tenant, rows = new[] { new { studentId = Student, examSubjectId = subjectId, marksObtained = 85, isAbsent = false, remarks = "Good" } } };
        await Call("PUT", $"/api/examinations/exam/{examId}/results", resultsBody);
        Actor("Student");
        var hidden = await Call("GET", $"/api/examinations/exam/{examId}/results?{q}", name:"Unpublished exam query");
        Assert(hidden["subjects"]!.AsArray().Count == 0 && hidden["rows"]!.AsArray().Count == 0, "Unpublished exam is private");
        Actor("Admin");
        await Call("POST", $"/api/examinations/exam/{examId}/publish", new { tenantId = Tenant });
        await Call("PUT", $"/api/examinations/exam/{examId}/results", resultsBody, 409, "Published marks are locked");
        Actor("Student");
        var published = await Call("GET", $"/api/examinations/exam/{examId}/results?{q}");
        Assert(published["rows"]![0]!["grade"]!.GetValue<string>() == "A", "Published result uses saved campus grade scale");
        Actor("Parent");
        var parentResults = await Call("GET", $"/api/examinations/exam/{examId}/results?{q}", name:"Parent reads published results for linked child");
        Assert(parentResults["rows"]!.AsArray().Count == 1 && parentResults["rows"]![0]!["studentId"]!.GetValue<string>() == Student.ToString(), "Parent result scope matches guardian link");
        await Call("GET", $"/api/learning/assignment?{q}&page=1&pageSize=25", name:"Parent reads linked child's assignments");
        Actor("Admin");
        var driver = await Call("POST", "/api/transport/operations/drivers", new { tenantId = Tenant, employeeId = DriverEmployee, dateOfBirth = "1985-01-01", licenseNumber = "CHECK-LICENSE", licenseCategory = "HTV", licenseExpiry = "2035-01-01" });
        var vehicle = await Call("POST", "/api/transport/vehicle", new { tenantId = Tenant, campusId = Campus, name = "School bus", registrationNo = "CHECK-101", capacity = 1 });
        var route = await Call("POST", "/api/transport/operations/routes", new { tenantId = Tenant, name = "North route", campusId = Campus, vehicleId = vehicle["id"]!.GetValue<string>(), driverId = driver["id"]!.GetValue<string>(), startTime = "07:00:00", arrivalTime = "08:00:00", dismissalTime = "14:00:00", stops = new[] { new { name = "Library stop", pickupTime = "07:30:00", dropoffTime = "14:30:00" } } });
        var routeId = route["id"]!.GetValue<string>();
        var operations = await Call("GET", $"/api/transport/operations?{q}");
        var stopId = operations["stops"]![0]!["id"]!.GetValue<string>();
        await Call("POST", "/api/transport/operations/assignments", new { tenantId = Tenant, studentId = Student, routeId, stopId });
        Actor("Driver");
        var workspace = await Call("GET", $"/api/transport/driver/workspace?{q}&direction=PICKUP");
        Assert(workspace["students"]!.AsArray().Count == 1, "Driver roster comes from saved transport assignment");
        var date = DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyy-MM-dd");
        await Call("PUT", "/api/transport/driver/trip-status", new { tenantId = Tenant, routeId, studentId = Student, serviceDate = date, direction = "PICKUP", status = "DROPPED_OFF" }, 400, "Drop-off requires boarding first");
        await Call("PUT", "/api/transport/driver/trip-status", new { tenantId = Tenant, routeId, studentId = Student, serviceDate = date, direction = "PICKUP", status = "BOARDED" });
        var boarded = await Call("GET", $"/api/transport/driver/workspace?{q}&direction=PICKUP");
        Assert(boarded["students"]![0]!["status"]!.GetValue<string>() == "BOARDED", "Trip status survives a fresh request");
        await Call("POST", "/api/transport/driver/notices", new { tenantId = Tenant, routeId, serviceDate = date, delayMinutes = 10, message = "Traffic delay" });
        Actor("Student");
        await Call("GET", $"/api/transport/operations?{q}", status:403, name:"Student cannot read transport management roster");
        Actor("Admin");
        var type = await Call("POST", "/api/documents/document-types", new { tenantId = Tenant, ownerType = "StudentDocument", name = "Birth certificate" });
        var requiredType = await Call("POST", "/api/documents/required-document-types", new { tenantId = Tenant, name = "Birth certificate" });
        await Call("POST", "/api/documents/required-documents", new { tenantId = Tenant, userRole = "STUDENT", isMandatory = true, requiredDocumentTypeId = requiredType["requiredDocumentTypeId"]!.GetValue<string>() });
        await Call("GET", $"/api/documents/setup?{q}");
        await Call("GET", $"/api/documents/compliance?{q}");
        var upload = new MultipartFormDataContent(); upload.Add(new StringContent("StudentDocument"), "ownerType"); upload.Add(new StringContent(Student.ToString()), "ownerId"); upload.Add(new StringContent(type["documentTypeId"]!.GetValue<string>()), "documentTypeId"); upload.Add(new StringContent(requiredType["requiredDocumentTypeId"]!.GetValue<string>()), "requiredDocumentTypeId"); var bytes = new ByteArrayContent("document-check"u8.ToArray()); bytes.Headers.ContentType = new("text/plain"); upload.Add(bytes, "file", "birth-certificate.txt");
        var doc = await Call("POST", $"/api/documents?{q}", upload, name:"Upload real document with required type");
        var docs = await Call("GET", $"/api/documents?{q}&page=1&pageSize=25");
        Assert(docs["items"]!.AsArray().Count == 1, "Uploaded document appears in authenticated library");
        Actor("Parent");
        var parentDocuments = await Call("GET", $"/api/documents?{q}&page=1&pageSize=25", status:403, name:"Parent cannot read tenant-wide document library");
        Actor("Admin");
        var compliant = await Call("GET", $"/api/documents/compliance?{q}");
        Assert(compliant["items"]!.AsArray().Any(x => x!["type"]!.GetValue<string>() == "STUDENT" && x["compliant"]!.GetValue<int>() == 1), "Compliance recalculates from uploaded evidence");
        await CheckAdmissions(Call, grade["id"]!.GetValue<string>(), sectionId, type["documentTypeId"]!.GetValue<string>(), requiredType["requiredDocumentTypeId"]!.GetValue<string>());
        var output = Environment.GetEnvironmentVariable("SMARTSCHOOL_TEST_REPORT");
        if (!string.IsNullOrEmpty(output)) await File.WriteAllTextAsync(output, System.Text.Json.JsonSerializer.Serialize(new { passed = s_checks, checks = Passed }, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
        Console.WriteLine($"VERIFIED: {s_checks} checks passed.");
        await app.StopAsync();
    }

    private static async Task CheckAdmissions(Func<string,string,object?,int,string?,Task<JsonNode>> call, string gradeId, string sectionId, string typeId, string requiredTypeId)
    {
        var created = await call("POST", "/api/admissions/workflow/applications", new { tenantId = Tenant, schoolId = School, branchId = Campus, academicYearId = Year, classId = gradeId, classSectionId = sectionId, firstName = "New", lastName = "Pupil", dateOfBirth = "2016-01-01", gender = "Male", email = "student-check@example.test", guardianName = "Test Guardian", guardianEmail = "parent-check@example.test", guardianPhone = "+923001234567", previousMarks = 85 }, 200, "Save admission application and academic placement");
        var id = created["id"]!.GetValue<string>();
        await call("PUT", $"/api/admissions/workflow/applications/{id}/status", new { tenantId = Tenant, status = "ADMISSION_ACCEPTED" }, 400, "Admission blocked while mandatory documents are missing");
        await call("PUT", $"/api/admissions/workflow/applications/{id}/status", new { tenantId = Tenant, status = "WAITING_LIST", notes = "Review next week" }, 200, "Admission decision and notes persist");
        await call("GET", $"/api/admissions/workflow/applications?tenantId={Tenant}", null, 200, "Admission applications read back from database");
        var upload = new MultipartFormDataContent(); upload.Add(new StringContent("AdmissionDocument"), "ownerType"); upload.Add(new StringContent(id), "ownerId"); upload.Add(new StringContent(typeId), "documentTypeId"); upload.Add(new StringContent(requiredTypeId), "requiredDocumentTypeId"); var bytes = new ByteArrayContent("application-evidence"u8.ToArray()); bytes.Headers.ContentType = new("text/plain"); upload.Add(bytes, "file", "birth-certificate.txt");
        await call("POST", $"/api/documents?tenantId={Tenant}", upload, 200, "Upload applicant document before enrollment");
        await call("POST", "/api/admissions/criteria", new { tenantId = Tenant, schoolId = School, branchId = Campus, academicYearId = Year, classId = gradeId, minimumMarks = 50, entranceTestMinimum = 60, interviewRequired = true, minimumAge = 8, maximumAge = 12 }, 200, "Save actual admission criteria");
        await call("GET", $"/api/admissions/criteria?tenantId={Tenant}", null, 200, "Read saved admission criteria");
        await call("PUT", $"/api/admissions/workflow/applications/{id}/status", new { tenantId = Tenant, status = "ADMISSION_ACCEPTED", entranceTestMarks = 50, interviewPassed = true }, 400, "Entrance test minimum enforced");
        await call("PUT", $"/api/admissions/workflow/applications/{id}/status", new { tenantId = Tenant, status = "ADMISSION_ACCEPTED", entranceTestMarks = 80, interviewPassed = false }, 400, "Required interview enforced");
        await call("PUT", $"/api/admissions/workflow/applications/{id}/status", new { tenantId = Tenant, status = "ADMISSION_ACCEPTED", entranceTestMarks = 80, interviewPassed = true, notes = "Approved after review" }, 200, "Accept eligible applicant and save enrollment");
        var applications = await call("GET", $"/api/admissions/workflow/applications?tenantId={Tenant}", null, 200, "Accepted application read back");
        var studentId = applications.AsArray().Single(x => x!["id"]!.GetValue<string>() == id)!["studentId"]!.GetValue<string>();
        var documents = await call("GET", $"/api/documents?tenantId={Tenant}&ownerType=StudentDocument&ownerId={studentId}&page=1&pageSize=25", null, 200, "Admitted student inherits application documents");
        if (documents["items"]!.AsArray().Count != 1) throw new InvalidOperationException("Admission document transfer failed.");
        await call("PUT", $"/api/admissions/workflow/applications/{id}/status", new { tenantId = Tenant, status = "ADMISSION_ACCEPTED", entranceTestMarks = 80, interviewPassed = true }, 409, "Duplicate admission rejected");
        await call("PUT", $"/api/admissions/workflow/applications/{id}/status", new { tenantId = Tenant, status = "ADMISSION_REJECTED" }, 409, "Accepted admission cannot be rejected later");

    }

    private static async Task SeedAsync()
    {
        await using var db = new NpgsqlConnection(ConnectionString); await db.OpenAsync();
        await db.ExecuteAsync("""
            INSERT INTO saas.tenant(tenant_id,code,name) VALUES(@Tenant,'CHECK','Integration school'),(@OtherTenant,'CHECK-OTHER','Other tenant');
            INSERT INTO org.school(school_id,tenant_id,code,name) VALUES(@School,@Tenant,'SCH','Test school');
            INSERT INTO academic.academic_system(academic_system_id,tenant_id,code,name,system_type_code) VALUES('99000000-0000-0000-0000-000000000001',@Tenant,'CHECK','Standard','NATIONAL');
            INSERT INTO org.campus(campus_id,tenant_id,school_id,code,name,branch_gender_type_id,branch_type,academic_system_id) VALUES(@Campus,@Tenant,@School,'MAIN','Main campus',(SELECT branch_gender_type_id FROM reference.branch_gender_type WHERE code='CO_EDUCATION'),3,'99000000-0000-0000-0000-000000000001');
            INSERT INTO org.campus_education_level(tenant_id,campus_id,education_level_id) VALUES(@Tenant,@Campus,'20000000-0000-0000-0000-000000000002');
            INSERT INTO academic.academic_year(academic_year_id,tenant_id,campus_id,code,name,start_date,end_date,is_current) VALUES(@Year,@Tenant,@Campus,'AY26','2026/27','2026-08-01','2027-07-31',true);
            INSERT INTO org.department(department_id,tenant_id,campus_id,code,name) VALUES('99000000-0000-0000-0000-000000000002',@Tenant,@Campus,'MATH','Mathematics');
            INSERT INTO academic.subject(subject_id,tenant_id,department_id,code,name) VALUES(@Subject,@Tenant,'99000000-0000-0000-0000-000000000002','MATH5','Mathematics');
            INSERT INTO hr.employee(employee_id,tenant_id,branch_id,school_id,user_id,employee_number,first_name,last_name,hire_date,employment_type_code,staff_type,cnic_number)
                VALUES(@Teacher,@Tenant,@Campus,@School,@TeacherUser,'T-001','Test','Teacher','2020-01-01','FULL_TIME','TEACHER','11111-1111111-1'),
                (@DriverEmployee,@Tenant,@Campus,@School,@DriverUser,'D-001','Test','Driver','2020-01-01','FULL_TIME','DRIVER','22222-2222222-2');
            INSERT INTO student.student(student_id,tenant_id,branch_id,school_id,user_id,student_number,first_name,last_name) VALUES(@Student,@Tenant,@Campus,@School,@StudentUser,'CHECK-STU-001','Test','Student');
            INSERT INTO student.guardian(guardian_id,tenant_id,user_id,full_name) VALUES('99000000-0000-0000-0000-000000000005',@Tenant,@ParentUser,'Test parent');
            INSERT INTO student.student_guardian(tenant_id,student_id,guardian_id,relationship,can_view_academics) VALUES(@Tenant,@Student,'99000000-0000-0000-0000-000000000005','Father',true);
            """, new { Tenant, OtherTenant, Campus, School, Year, Subject, Teacher, DriverEmployee, Student, User,
                TeacherUser = UserFor("Teacher"), DriverUser = UserFor("Driver"), StudentUser = UserFor("Student"), ParentUser = UserFor("Parent") });
    }
}

internal sealed class TestConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public async Task<DbConnection> OpenConnectionAsync(CancellationToken cancellationToken = default) { var connection = new NpgsqlConnection(connectionString); await connection.OpenAsync(cancellationToken); return connection; }
}
internal sealed class TestAccounts : IIdentityAccountService
{
    public Task<ProvisionedAccount> CreateAccountAsync(Guid tenantId, Guid businessEntityId, string accountType, string email, string firstName, string lastName, Guid? schoolId, Guid? branchId, IReadOnlyCollection<string> roles, CancellationToken cancellationToken) => Task.FromResult(new ProvisionedAccount(Guid.NewGuid(), email, "TEST-ONLY", true));
    public Task DeleteAccountAsync(Guid userId, CancellationToken cancellationToken) => Task.CompletedTask;
    public Task DeactivateAccountAsync(Guid userId, CancellationToken cancellationToken) => Task.CompletedTask;
}
internal sealed class TestAuthentication(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var role = Request.Headers["X-Test-Role"].ToString();
        if (string.IsNullOrEmpty(role)) return Task.FromResult(AuthenticateResult.NoResult());
        var claims = new List<Claim> { new("role",role),new("sub",WorkflowChecks.UserFor(role).ToString()),new("tenant_id",WorkflowChecks.Tenant.ToString()),new("branch_id",WorkflowChecks.Campus.ToString()) };
        if (role == "Teacher") claims.Add(new("employee_id",WorkflowChecks.Teacher.ToString()));
        if (role == "Driver") claims.Add(new("employee_id",WorkflowChecks.DriverEmployee.ToString()));
        if (role == "Student") claims.Add(new("student_id",WorkflowChecks.Student.ToString()));
        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(claims,"Test","sub","role")),"Test")));
    }
}
