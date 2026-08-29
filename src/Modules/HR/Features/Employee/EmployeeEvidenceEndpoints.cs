using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.HR.Features.Employee;

/// <summary>Structured qualification and experience facts. Certificates themselves are stored by the Documents aggregate.</summary>
public static class EmployeeEvidenceEndpoints
{
    public sealed record EducationRequest(Guid? TenantId, string Qualification, string? Institute, string? FieldOfStudy, DateOnly? StartDate, DateOnly? EndDate, string? Grade, bool IsHighest);
    public sealed record ExperienceRequest(Guid? TenantId, string Employer, string JobTitle, DateOnly StartDate, DateOnly? EndDate, string? Responsibilities);

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/hr/employee/{employeeId:guid}/education", AddEducationAsync).WithTags("HR").RequireAuthorization();
        endpoints.MapPost("/api/hr/employee/{employeeId:guid}/experience", AddExperienceAsync).WithTags("HR").RequireAuthorization();
        return endpoints;
    }

    private static async Task<IResult> AddEducationAsync(Guid employeeId, EducationRequest request, ITenantScope scope, IDbConnectionFactory factory, CancellationToken ct)
    {
        var tenantId=scope.Resolve(request.TenantId); if(!tenantId.HasValue)return Results.BadRequest(new{message="Tenant is required."});
        if(string.IsNullOrWhiteSpace(request.Qualification))return Results.BadRequest(new{message="Qualification is required."});
        await using var c=await factory.OpenConnectionAsync(ct); var id=Guid.NewGuid();
        await c.ExecuteAsync(new CommandDefinition("INSERT INTO hr.employee_education(employee_education_id,tenant_id,employee_id,qualification,institute,field_of_study,start_date,end_date,grade,is_highest) VALUES(@Id,@TenantId,@EmployeeId,@Qualification,@Institute,@FieldOfStudy,@StartDate,@EndDate,@Grade,@IsHighest)",new{Id=id,TenantId=tenantId.Value,EmployeeId=employeeId,request.Qualification,request.Institute,request.FieldOfStudy,request.StartDate,request.EndDate,request.Grade,request.IsHighest},cancellationToken:ct));
        return Results.Created($"/api/hr/employee/{employeeId}/education/{id}",new{id});
    }
    private static async Task<IResult> AddExperienceAsync(Guid employeeId, ExperienceRequest request, ITenantScope scope, IDbConnectionFactory factory, CancellationToken ct)
    {
        var tenantId=scope.Resolve(request.TenantId); if(!tenantId.HasValue)return Results.BadRequest(new{message="Tenant is required."});
        if(string.IsNullOrWhiteSpace(request.Employer)||string.IsNullOrWhiteSpace(request.JobTitle))return Results.BadRequest(new{message="Employer and job title are required."});
        await using var c=await factory.OpenConnectionAsync(ct); var id=Guid.NewGuid();
        await c.ExecuteAsync(new CommandDefinition("INSERT INTO hr.employee_experience(employee_experience_id,tenant_id,employee_id,employer,job_title,start_date,end_date,responsibilities) VALUES(@Id,@TenantId,@EmployeeId,@Employer,@JobTitle,@StartDate,@EndDate,@Responsibilities)",new{Id=id,TenantId=tenantId.Value,EmployeeId=employeeId,request.Employer,request.JobTitle,request.StartDate,request.EndDate,request.Responsibilities},cancellationToken:ct));
        return Results.Created($"/api/hr/employee/{employeeId}/experience/{id}",new{id});
    }
}
