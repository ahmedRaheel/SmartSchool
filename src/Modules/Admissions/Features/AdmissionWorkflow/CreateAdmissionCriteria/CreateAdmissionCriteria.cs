using Dapper;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features;

public interface ICreateAdmissionCriteriaQuery { Task<bool> CriteriaContextIsValidAsync(Guid tenantId,Guid schoolId,Guid branchId,Guid academicYearId,Guid classId,CancellationToken ct); }
public sealed class CreateAdmissionCriteriaQuery(IDbConnectionFactory factory) : ICreateAdmissionCriteriaQuery
{
    public async Task<bool> CriteriaContextIsValidAsync(Guid t,Guid s,Guid b,Guid y,Guid c,CancellationToken ct){const string sql="""SELECT EXISTS(SELECT 1 FROM academic.class c INNER JOIN academic.academic_year y ON y.branch_id=c.branch_id AND y.tenant_id=c.tenant_id INNER JOIN org.campus b ON b.campus_id=c.branch_id AND b.tenant_id=c.tenant_id INNER JOIN org.campus_education_level bel ON bel.campus_id=c.branch_id AND bel.education_level_id=c.education_level_id WHERE c.tenant_id=@T AND b.school_id=@S AND c.branch_id=@B AND c.class_id=@C AND y.academic_year_id=@Y AND c.is_active=TRUE AND y.is_active=TRUE AND b.is_active=TRUE);""";await using var cn=await factory.OpenConnectionAsync(ct);return await cn.ExecuteScalarAsync<bool>(new CommandDefinition(sql,new{T=t,S=s,B=b,Y=y,C=c},cancellationToken:ct));}
}
public interface ICreateAdmissionCriteria { Task<Guid> CreateCriteriaAsync(Guid tenantId,CreateAdmissionCriteria.Request request,CancellationToken ct); }
public sealed class CreateAdmissionCriteriaCommand(IAdmissionsDbContext db) : ICreateAdmissionCriteria
{
    public async Task<Guid> CreateCriteriaAsync(Guid t,CreateAdmissionCriteria.Request r,CancellationToken ct){var id=Guid.NewGuid();await db.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO admission.admission_criteria (admission_criteria_id,tenant_id,school_id,branch_id,academic_year_id,class_id,minimum_marks,entrance_test_minimum,minimum_age,maximum_age,interview_required,required_documents) VALUES ({id},{t},{r.SchoolId},{r.BranchId},{r.AcademicYearId},{r.ClassId},{r.MinimumMarks},{r.EntranceTestMinimum},{r.MinimumAge},{r.MaximumAge},{r.InterviewRequired},{r.RequiredDocuments});",ct);return id;}
}

public static class CreateAdmissionCriteria
{
    public sealed record Request(
        Guid? TenantId,
        Guid SchoolId,
        Guid BranchId,
        Guid AcademicYearId,
        Guid ClassId,
        decimal MinimumMarks,
        decimal? EntranceTestMinimum,
        int? MinimumAge,
        int? MaximumAge,
        bool InterviewRequired,
        string? RequiredDocuments)
        : IRequest<Result<Response>>;

    public sealed record Response(Guid Id);

    public sealed class Handler(
        ITenantScope tenantScope,
        ICreateAdmissionCriteriaQuery query,
        ICreateAdmissionCriteria command)
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

            var contextIsValid = await query.CriteriaContextIsValidAsync(
                tenantId.Value,
                request.SchoolId,
                request.BranchId,
                request.AcademicYearId,
                request.ClassId,
                cancellationToken);

            if (!contextIsValid)
            {
                return Result<Response>.Failure(
                    Error.Validation(
                        "School, branch, academic year and class must belong to the same tenant context."));
            }

            var criteriaId = await command.CreateCriteriaAsync(
                tenantId.Value,
                request,
                cancellationToken);

            return Result<Response>.Success(new Response(criteriaId));
        }
    }
}
