using Dapper;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features;

public interface ICreateAdmissionApplicationQuery
{
    Task<bool> BranchBelongsToSchoolAsync(Guid tenantId, Guid schoolId, Guid branchId, CancellationToken ct);
    Task<string?> GetBranchGenderPolicyAsync(Guid tenantId, Guid branchId, CancellationToken ct);
    Task<bool> ClassIsEligibleForBranchAsync(Guid tenantId, Guid branchId, Guid classId, CancellationToken ct);
    Task<bool> AcademicYearBelongsToBranchAsync(Guid tenantId, Guid branchId, Guid academicYearId, CancellationToken ct);
}
public sealed class CreateAdmissionApplicationQuery(IDbConnectionFactory factory) : ICreateAdmissionApplicationQuery
{
    public Task<bool> BranchBelongsToSchoolAsync(Guid tenantId, Guid schoolId, Guid branchId, CancellationToken ct)=>Exists("SELECT EXISTS(SELECT 1 FROM org.campus WHERE tenant_id=@T AND school_id=@S AND campus_id=@B AND is_active=TRUE);",new { T = tenantId, S = schoolId, B = branchId },ct);
    public async Task<string?> GetBranchGenderPolicyAsync(Guid tenantId, Guid branchId, CancellationToken ct){const string sql="SELECT g.code FROM org.campus c INNER JOIN reference.branch_gender_type g ON g.branch_gender_type_id=c.branch_gender_type_id WHERE c.tenant_id=@T AND c.campus_id=@B AND c.is_active=TRUE;";await using var c=await factory.OpenConnectionAsync(ct);return await c.ExecuteScalarAsync<string?>(new CommandDefinition(sql,new { T = tenantId, B = branchId },cancellationToken:ct));}
    public Task<bool> ClassIsEligibleForBranchAsync(Guid tenantId, Guid branchId, Guid classId, CancellationToken ct)=>Exists("SELECT EXISTS(SELECT 1 FROM academic.class c INNER JOIN org.campus_education_level bel ON bel.campus_id=c.branch_id AND bel.education_level_id=c.education_level_id WHERE c.tenant_id=@T AND c.branch_id=@B AND c.class_id=@Id AND c.is_active=TRUE);",new { T = tenantId, B = branchId, Id = classId },ct);
    public Task<bool> AcademicYearBelongsToBranchAsync(Guid tenantId, Guid branchId, Guid academicYearId, CancellationToken ct)=>Exists("SELECT EXISTS(SELECT 1 FROM academic.academic_year WHERE tenant_id=@T AND branch_id=@B AND academic_year_id=@Id AND is_active=TRUE);",new { T = tenantId, B = branchId, Id = classId },ct);
    private async Task<bool> Exists(string sql,object p,CancellationToken ct){await using var c=await factory.OpenConnectionAsync(ct);return await c.ExecuteScalarAsync<bool>(new CommandDefinition(sql,p,cancellationToken:ct));}
}
public interface ICreateAdmissionApplication { Task<Guid> CreateApplicationAsync(Guid tenantId, CreateAdmissionApplication.Request request, CancellationToken ct); }
public sealed class CreateAdmissionApplicationCommand(IAdmissionsDbContext db) : ICreateAdmissionApplication
{
    public async Task<Guid> CreateApplicationAsync(Guid tenantId, CreateAdmissionApplication.Request request, CancellationToken ct){var id = Guid.NewGuid(); var st =AdmissionApplicationStatus.SubmittedApplication.ToDatabaseValue();await db.Database.ExecuteSqlInterpolatedAsync($"""INSERT INTO admission.student_application (application_id,tenant_id,school_id,branch_id,academic_year_id,class_id,section_id,first_name,last_name,date_of_birth,gender,email,phone,address,guardian_name,guardian_cnic,guardian_email,guardian_phone,relationship,previous_school,previous_marks,status) VALUES ({id},{tenantId},{request.SchoolId},{request.BranchId},{request.AcademicYearId},{request.ClassId},{request.SectionId},{request.FirstName},{request.LastName},{request.DateOfBirth},{request.Gender},{request.Email},{request.Phone},{request.Address},{request.GuardianName},{request.GuardianCnic},{request.GuardianEmail},{request.GuardianPhone},{request.Relationship},{request.PreviousSchool},{request.PreviousMarks},{st});""",ct);return id;}
}

public static class CreateAdmissionApplication
{
    public sealed record Request(
        Guid? TenantId,
        Guid SchoolId,
        Guid BranchId,
        Guid? AcademicYearId,
        Guid? ClassId,
        Guid? SectionId,
        string FirstName,
        string? LastName,
        DateOnly? DateOfBirth,
        string? Gender,
        string? Email,
        string? Phone,
        string? Address,
        string GuardianName,
        string? GuardianCnic,
        string? GuardianEmail,
        string? GuardianPhone,
        string? Relationship,
        string? PreviousSchool,
        decimal? PreviousMarks)
        : IRequest<Result<Response>>;

    public sealed record Response(Guid Id, string Status);

    public sealed class Handler(
        ITenantScope tenantScope,
        ICreateAdmissionApplicationQuery query,
        ICreateAdmissionApplication command)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            var branchIsValid = await query.BranchBelongsToSchoolAsync(
                tenantId.Value,
                request.SchoolId,
                request.BranchId,
                cancellationToken);

            if (!branchIsValid)
            {
                return Result<Response>.Failure(
                    Error.Validation("Selected branch does not belong to the selected school."));
            }

            if (string.IsNullOrWhiteSpace(request.Gender))
            {
                return Result<Response>.Failure(Error.Validation("Applicant gender is required."));
            }

            var branchGenderPolicy = await query.GetBranchGenderPolicyAsync(
                tenantId.Value,
                request.BranchId,
                cancellationToken);

            if (!GenderIsAllowed(branchGenderPolicy, request.Gender))
            {
                return Result<Response>.Failure(
                    Error.Validation("Applicant gender is not eligible for the selected branch."));
            }

            if (request.ClassId.HasValue)
            {
                var classIsEligible = await query.ClassIsEligibleForBranchAsync(
                    tenantId.Value,
                    request.BranchId,
                    request.ClassId.Value,
                    cancellationToken);

                if (!classIsEligible)
                {
                    return Result<Response>.Failure(
                        Error.Validation("The selected class is not available for this branch education level."));
                }
            }

            if (request.AcademicYearId.HasValue)
            {
                var academicYearIsValid = await query.AcademicYearBelongsToBranchAsync(
                    tenantId.Value,
                    request.BranchId,
                    request.AcademicYearId.Value,
                    cancellationToken);

                if (!academicYearIsValid)
                {
                    return Result<Response>.Failure(
                        Error.Validation("Academic year is not available for the selected branch."));
                }
            }

            var applicationId = await command.CreateApplicationAsync(
                tenantId.Value,
                request,
                cancellationToken);

            return Result<Response>.Success(
                new Response(
                    applicationId,
                    AdmissionApplicationStatus.SubmittedApplication.ToDatabaseValue()));
        }

        private static bool GenderIsAllowed(string? branchPolicy, string applicantGender)
        {
            if (string.Equals(branchPolicy, "CO_EDUCATION", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(branchPolicy, "BOYS_ONLY", StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(applicantGender, "MALE", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(applicantGender, "BOY", StringComparison.OrdinalIgnoreCase);
            }

            if (string.Equals(branchPolicy, "GIRLS_ONLY", StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(applicantGender, "FEMALE", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(applicantGender, "GIRL", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}
