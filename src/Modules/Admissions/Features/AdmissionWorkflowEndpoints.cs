using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Admissions.Features;

public static class AdmissionWorkflowEndpoints
{
    private static readonly string[] AllowedStatuses = ["SUBMITTED_APPLICATION", "ADMISSION_ACCEPTED", "ADMISSION_REJECTED", "WAITING_LIST"];

    public sealed record ApplicationRequest(Guid? TenantId, Guid SchoolId, Guid BranchId, Guid? AcademicYearId, Guid? ClassSectionId,
        string FirstName, string? LastName, DateOnly? DateOfBirth, string? Gender, string? Email, string? Phone, string? Address,
        string GuardianName, string? GuardianCnic, string? GuardianEmail, string? GuardianPhone, string? Relationship,
        string? PreviousSchool, decimal? PreviousMarks);
    public sealed record StatusRequest(Guid? TenantId, string Status, string? Notes);
    public sealed record CriteriaRequest(Guid? TenantId, Guid SchoolId, Guid BranchId, Guid AcademicYearId, Guid ClassSectionId,
        decimal MinimumMarks, decimal? EntranceTestMinimum, int? MinimumAge, int? MaximumAge, bool InterviewRequired, string? RequiredDocuments);

    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/admissions/workflow/applications", async (Guid? tenantId, ITenantScope scope, IDbConnectionFactory factory, CancellationToken ct) =>
        {
            var resolved = scope.Resolve(tenantId); if (!resolved.HasValue) return Results.BadRequest();
            await using var db = await factory.OpenConnectionAsync(ct);
            var rows = await db.QueryAsync(new CommandDefinition("SELECT application_id AS Id, school_id AS SchoolId, branch_id AS BranchId, academic_year_id AS AcademicYearId, class_section_id AS ClassSectionId, first_name AS FirstName, last_name AS LastName, date_of_birth AS DateOfBirth, gender AS Gender, email AS Email, phone AS Phone, guardian_name AS GuardianName, guardian_email AS GuardianEmail, guardian_phone AS GuardianPhone, previous_marks AS PreviousMarks, status AS Status, submitted_at AS SubmittedAt, decision_notes AS DecisionNotes, student_id AS StudentId FROM admission.student_application WHERE tenant_id=@TenantId AND is_active=true ORDER BY submitted_at DESC", new { TenantId=resolved.Value }, cancellationToken:ct));
            return Results.Ok(new { items = rows });
        }).RequireAuthorization();

        endpoints.MapPost("/api/admissions/workflow/applications", async (ApplicationRequest request, ITenantScope scope, IDbConnectionFactory factory, CancellationToken ct) =>
        {
            var tenantId = scope.Resolve(request.TenantId); if (!tenantId.HasValue) return Results.BadRequest();
            await using var db = await factory.OpenConnectionAsync(ct);
            var valid = await db.ExecuteScalarAsync<bool>(new CommandDefinition("SELECT EXISTS(SELECT 1 FROM org.campus WHERE tenant_id=@TenantId AND school_id=@SchoolId AND campus_id=@BranchId)", new { TenantId=tenantId.Value, request.SchoolId, request.BranchId }, cancellationToken:ct));
            if (!valid) return Results.BadRequest(new { message="Selected branch does not belong to the school." });
            var id=Guid.NewGuid();
            await db.ExecuteAsync(new CommandDefinition("INSERT INTO admission.student_application(application_id,tenant_id,school_id,branch_id,academic_year_id,class_section_id,first_name,last_name,date_of_birth,gender,email,phone,address,guardian_name,guardian_cnic,guardian_email,guardian_phone,relationship,previous_school,previous_marks,status) VALUES(@Id,@TenantId,@SchoolId,@BranchId,@AcademicYearId,@ClassSectionId,@FirstName,@LastName,@DateOfBirth,@Gender,@Email,@Phone,@Address,@GuardianName,@GuardianCnic,@GuardianEmail,@GuardianPhone,@Relationship,@PreviousSchool,@PreviousMarks,'SUBMITTED_APPLICATION')", new { Id=id, TenantId=tenantId.Value, request.SchoolId, request.BranchId, request.AcademicYearId, request.ClassSectionId, request.FirstName, request.LastName, request.DateOfBirth, request.Gender, request.Email, request.Phone, request.Address, request.GuardianName, request.GuardianCnic, request.GuardianEmail, request.GuardianPhone, request.Relationship, request.PreviousSchool, request.PreviousMarks }, cancellationToken:ct));
            return Results.Created($"/api/admissions/workflow/applications/{id}", new { id, status="SUBMITTED_APPLICATION" });
        }).RequireAuthorization();

        endpoints.MapPut("/api/admissions/workflow/applications/{id:guid}/status", async (Guid id, StatusRequest request, ITenantScope scope, IDbConnectionFactory factory, IIdentityAccountService accounts, IBusinessNumberGenerator numbers, CancellationToken ct) =>
        {
            if (!AllowedStatuses.Contains(request.Status)) return Results.BadRequest(new { message="Invalid admission status." });
            var tenantId=scope.Resolve(request.TenantId); if(!tenantId.HasValue) return Results.BadRequest();
            await using var db=await factory.OpenConnectionAsync(ct);
            if (request.Status == "ADMISSION_ACCEPTED")
            {
                var application = await db.QuerySingleOrDefaultAsync<dynamic>(new CommandDefinition("SELECT * FROM admission.student_application WHERE application_id=@Id AND tenant_id=@TenantId AND is_active=true", new { Id=id,TenantId=tenantId.Value }, cancellationToken:ct));
                if (application is null) return Results.NotFound();
                if (application.student_id is not null) return Results.Conflict(new { message="This application has already been admitted." });
                if (string.IsNullOrWhiteSpace((string?)application.email)) return Results.BadRequest(new { message="Student email is required before admission can be accepted." });
                if (string.IsNullOrWhiteSpace((string?)application.guardian_email)) return Results.BadRequest(new { message="Guardian email is required so the parent account can be created." });
                var branchCode = await db.ExecuteScalarAsync<string>(new CommandDefinition("SELECT code FROM org.campus WHERE tenant_id=@TenantId AND campus_id=@BranchId", new { TenantId=tenantId.Value, BranchId=(Guid)application.branch_id }, cancellationToken:ct));
                if (string.IsNullOrWhiteSpace(branchCode)) return Results.BadRequest(new { message="Application branch is invalid." });
                var studentId=Guid.NewGuid(); var guardianId=Guid.NewGuid();
                var studentNumber=await numbers.NextAsync($"STUDENT:{application.branch_id}",$"{branchCode}-",tenantId.Value,7,ct);
                var studentAccount=await accounts.CreateAccountAsync(tenantId.Value,studentId,"Student",(string)application.email,(string)application.first_name,(string?)application.last_name??string.Empty,(Guid)application.school_id,(Guid)application.branch_id,new[]{"Student"},ct);
                ProvisionedAccount? parentAccount=null;
                try
                {
                    parentAccount=await accounts.CreateAccountAsync(tenantId.Value,guardianId,"Parent",(string)application.guardian_email,(string)application.guardian_name,string.Empty,(Guid)application.school_id,(Guid)application.branch_id,new[]{"Parent"},ct);
                    using var tx=db.BeginTransaction();
                    await db.ExecuteAsync(new CommandDefinition("INSERT INTO student.student(student_id,tenant_id,user_id,school_id,branch_id,student_number,first_name,last_name,date_of_birth,gender,admission_date,status,is_active,created_at) VALUES(@StudentId,@TenantId,@UserId,@SchoolId,@BranchId,@StudentNumber,@FirstName,@LastName,@DateOfBirth,@Gender,CURRENT_DATE,'ACTIVE',true,now())",new{StudentId=studentId,TenantId=tenantId.Value,userId=studentAccount.UserId,SchoolId=(Guid)application.school_id,BranchId=(Guid)application.branch_id,StudentNumber=studentNumber,FirstName=(string)application.first_name,LastName=(string?)application.last_name,DateOfBirth=(DateOnly?)application.date_of_birth,Gender=(string?)application.gender},tx,cancellationToken:ct));
					await db.ExecuteAsync(new CommandDefinition("INSERT INTO student.guardian(guardian_id,tenant_id,user_id,full_name,cnic_number,email,phone,is_active,created_at) VALUES(@GuardianId,@TenantId,@UserId,@Name,@Cnic,@Email,@Phone,true,now())", new
					{
						GuardianId = guardianId,
						TenantId = tenantId.Value,
						userId = parentAccount.UserId,
						Name = (string)application.guardian_name,
						Cnic = (string?)application.guardian_cnic,
						Email = (string)application.guardian_email,
						Phone = (string?)application.guardian_phone
					}, tx, cancellationToken: ct));
                    await db.ExecuteAsync(new CommandDefinition("INSERT INTO student.student_guardian(student_id,guardian_id,relationship,is_primary,can_view_academics,can_view_finance,can_pickup) VALUES(@StudentId,@GuardianId,@Relationship,true,true,true,true)",new{StudentId=studentId,GuardianId=guardianId,Relationship=(string?)application.relationship??"Parent"},tx,cancellationToken:ct));
                    if (application.academic_year_id is not null && application.class_section_id is not null) { var enrollmentNumber=$"{studentNumber}-001"; await db.ExecuteAsync(new CommandDefinition("INSERT INTO student.student_enrollment(student_enrollment_id,tenant_id,student_id,academic_year_id,class_section_id,enrollment_number,enrollment_date,status,is_active,created_at) VALUES(@EnrollmentId,@TenantId,@StudentId,@AcademicYearId,@ClassSectionId,@EnrollmentNumber,CURRENT_DATE,'ACTIVE',true,now())",new{EnrollmentId=Guid.NewGuid(),TenantId=tenantId.Value,StudentId=studentId,AcademicYearId=(Guid)application.academic_year_id,ClassSectionId=(Guid)application.class_section_id,EnrollmentNumber=enrollmentNumber},tx,cancellationToken:ct)); }
                    await db.ExecuteAsync(new CommandDefinition("UPDATE admission.student_application SET status='ADMISSION_ACCEPTED',decision_notes=@Notes,decided_at=now(),student_id=@StudentId WHERE application_id=@Id AND tenant_id=@TenantId",new{request.Notes,StudentId=studentId,Id=id,TenantId=tenantId.Value},tx,cancellationToken:ct)); tx.Commit();
                }
                catch { await accounts.DeactivateAccountAsync(studentAccount.UserId,ct); if(parentAccount is not null) await accounts.DeactivateAccountAsync(parentAccount.UserId,ct); throw; }
                return Results.Ok(new { id, status=request.Status, studentNumber });
            }
            var changed=await db.ExecuteAsync(new CommandDefinition("UPDATE admission.student_application SET status=@Status, decision_notes=@Notes, decided_at=CASE WHEN @Status='SUBMITTED_APPLICATION' THEN NULL ELSE now() END WHERE application_id=@Id AND tenant_id=@TenantId", new { Id=id,TenantId=tenantId.Value,request.Status,request.Notes }, cancellationToken:ct));
            return changed==0 ? Results.NotFound() : Results.Ok(new { id, request.Status });
        }).RequireAuthorization();

        endpoints.MapGet("/api/admissions/criteria", async (Guid? tenantId, ITenantScope scope, IDbConnectionFactory factory, CancellationToken ct) => { var t=scope.Resolve(tenantId); if(!t.HasValue)return Results.BadRequest(); await using var db=await factory.OpenConnectionAsync(ct); var rows=await db.QueryAsync(new CommandDefinition("SELECT admission_criteria_id AS Id, school_id AS SchoolId, branch_id AS BranchId, academic_year_id AS AcademicYearId, class_section_id AS ClassSectionId, minimum_marks AS MinimumMarks, entrance_test_minimum AS EntranceTestMinimum, minimum_age AS MinimumAge, maximum_age AS MaximumAge, interview_required AS InterviewRequired, required_documents AS RequiredDocuments, status AS Status FROM admission.admission_criteria WHERE tenant_id=@TenantId ORDER BY created_at DESC",new{TenantId=t.Value},cancellationToken:ct)); return Results.Ok(new{items=rows}); }).RequireAuthorization();
        endpoints.MapPost("/api/admissions/criteria", async (CriteriaRequest request, ITenantScope scope, IDbConnectionFactory factory, CancellationToken ct) => { var t=scope.Resolve(request.TenantId);if(!t.HasValue)return Results.BadRequest(); await using var db=await factory.OpenConnectionAsync(ct); var id=Guid.NewGuid(); await db.ExecuteAsync(new CommandDefinition("INSERT INTO admission.admission_criteria(admission_criteria_id,tenant_id,school_id,branch_id,academic_year_id,class_section_id,minimum_marks,entrance_test_minimum,minimum_age,maximum_age,interview_required,required_documents) VALUES(@Id,@TenantId,@SchoolId,@BranchId,@AcademicYearId,@ClassSectionId,@MinimumMarks,@EntranceTestMinimum,@MinimumAge,@MaximumAge,@InterviewRequired,@RequiredDocuments)",new{Id=id,TenantId=t.Value,request.SchoolId,request.BranchId,request.AcademicYearId,request.ClassSectionId,request.MinimumMarks,request.EntranceTestMinimum,request.MinimumAge,request.MaximumAge,request.InterviewRequired,request.RequiredDocuments},cancellationToken:ct)); return Results.Created($"/api/admissions/criteria/{id}",new{id}); }).RequireAuthorization();
    }
}
