using Microsoft.EntityFrameworkCore;
using SmartSchool.Modules.Admissions.Persistence;

namespace SmartSchool.Modules.Admissions.Features;

public interface ICompleteAdmission
{
    Task ExecuteAsync(Guid tenantId, AdmissionApplicationDetails application, Guid studentId, Guid studentUserId, Guid guardianId, Guid guardianUserId, string studentNumber, string? notes, CancellationToken ct);
}

public sealed class CompleteAdmissionCommand(IAdmissionsDbContext db) : ICompleteAdmission
{
    public async Task ExecuteAsync(Guid t, AdmissionApplicationDetails a, Guid sid, Guid suid, Guid gid, Guid guid, string sn, string? notes, CancellationToken ct)
    {
        await using var tx = await db.Database.BeginTransactionAsync(ct);
        try
        {
            await db.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO student.student (student_id,tenant_id,user_id,school_id,branch_id,student_number,first_name,last_name,date_of_birth,gender,admission_date,status) VALUES ({sid},{t},{suid},{a.SchoolId},{a.BranchId},{sn},{a.FirstName},{a.LastName},{a.DateOfBirth},{a.Gender},CURRENT_DATE,'ACTIVE');", ct);
            await db.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO student.guardian (guardian_id,tenant_id,user_id,full_name,cnic_number,email,phone) VALUES ({gid},{t},{guid},{a.GuardianName},{a.GuardianCnic},{a.GuardianEmail},{a.GuardianPhone});", ct);
            var relationship = string.IsNullOrWhiteSpace(a.Relationship) ? "GUARDIAN" : a.Relationship.Trim();
            await db.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO student.student_guardian (student_id,guardian_id,relationship,is_primary,can_view_academics,can_view_finance,can_pickup) VALUES ({sid},{gid},{relationship},TRUE,TRUE,TRUE,FALSE);", ct);
            if (a.AcademicYearId.HasValue && a.SectionId.HasValue)
            {
                var enrollmentId = Guid.NewGuid();
                await db.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO student.student_enrollment (student_enrollment_id,tenant_id,student_id,enrollment_number,academic_year_id,class_section_id,enrollment_date,status) VALUES ({enrollmentId},{t},{sid},{sn},{a.AcademicYearId.Value},{a.SectionId.Value},CURRENT_DATE,'ACTIVE');", ct);
            }
            var accepted = AdmissionApplicationStatus.AdmissionAccepted.ToDatabaseValue();
            await db.Database.ExecuteSqlInterpolatedAsync($"UPDATE admission.student_application SET status={accepted},student_id={sid},decision_notes={notes},decided_at=NOW() WHERE application_id={a.Id} AND tenant_id={t};", ct);
            await tx.CommitAsync(ct);
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }
    }
}
