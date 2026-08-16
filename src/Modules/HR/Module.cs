using FluentValidation;
using SmartSchool.Modules.HR.Features.Candidate;
using SmartSchool.Modules.HR.Features.Employee;
using SmartSchool.Modules.HR.Features.EmploymentHistory;
using SmartSchool.Modules.HR.Features.Interview;
using SmartSchool.Modules.HR.Features.Job;
using SmartSchool.Modules.HR.Features.JobGrade;
using SmartSchool.Modules.HR.Features.LeaveRequest;
using SmartSchool.Modules.HR.Features.Position;
using SmartSchool.Modules.HR.Features.Resume;

namespace SmartSchool.Modules.HR;

public static class Module
{
    public static IServiceCollection AddHRModule(
        this IServiceCollection services)
    {
        services.AddScoped<CreateCandidate.Handler>();
        services.AddScoped<GetCandidateById.Handler>();
        services.AddScoped<GetCandidatePage.Handler>();
        services.AddScoped<UpdateCandidate.Handler>();
        services.AddScoped<DeleteCandidate.Handler>();
        services.AddScoped<IValidator<CreateCandidate.Request>, CreateCandidate.Validator>();
        services.AddScoped<IValidator<UpdateCandidate.Request>, UpdateCandidate.Validator>();
        services.AddScoped<CreateEmployee.Handler>();
        services.AddScoped<GetEmployeeById.Handler>();
        services.AddScoped<GetEmployeePage.Handler>();
        services.AddScoped<UpdateEmployee.Handler>();
        services.AddScoped<DeleteEmployee.Handler>();
        services.AddScoped<IValidator<CreateEmployee.Request>, CreateEmployee.Validator>();
        services.AddScoped<IValidator<UpdateEmployee.Request>, UpdateEmployee.Validator>();
        services.AddScoped<CreateEmploymentHistory.Handler>();
        services.AddScoped<GetEmploymentHistoryById.Handler>();
        services.AddScoped<GetEmploymentHistoryPage.Handler>();
        services.AddScoped<UpdateEmploymentHistory.Handler>();
        services.AddScoped<DeleteEmploymentHistory.Handler>();
        services.AddScoped<IValidator<CreateEmploymentHistory.Request>, CreateEmploymentHistory.Validator>();
        services.AddScoped<IValidator<UpdateEmploymentHistory.Request>, UpdateEmploymentHistory.Validator>();
        services.AddScoped<CreateInterview.Handler>();
        services.AddScoped<GetInterviewById.Handler>();
        services.AddScoped<GetInterviewPage.Handler>();
        services.AddScoped<UpdateInterview.Handler>();
        services.AddScoped<DeleteInterview.Handler>();
        services.AddScoped<IValidator<CreateInterview.Request>, CreateInterview.Validator>();
        services.AddScoped<IValidator<UpdateInterview.Request>, UpdateInterview.Validator>();
        services.AddScoped<CreateJob.Handler>();
        services.AddScoped<GetJobById.Handler>();
        services.AddScoped<GetJobPage.Handler>();
        services.AddScoped<UpdateJob.Handler>();
        services.AddScoped<DeleteJob.Handler>();
        services.AddScoped<IValidator<CreateJob.Request>, CreateJob.Validator>();
        services.AddScoped<IValidator<UpdateJob.Request>, UpdateJob.Validator>();
        services.AddScoped<CreateJobGrade.Handler>();
        services.AddScoped<GetJobGradeById.Handler>();
        services.AddScoped<GetJobGradePage.Handler>();
        services.AddScoped<UpdateJobGrade.Handler>();
        services.AddScoped<DeleteJobGrade.Handler>();
        services.AddScoped<IValidator<CreateJobGrade.Request>, CreateJobGrade.Validator>();
        services.AddScoped<IValidator<UpdateJobGrade.Request>, UpdateJobGrade.Validator>();
        services.AddScoped<CreateLeaveRequest.Handler>();
        services.AddScoped<GetLeaveRequestById.Handler>();
        services.AddScoped<GetLeaveRequestPage.Handler>();
        services.AddScoped<UpdateLeaveRequest.Handler>();
        services.AddScoped<DeleteLeaveRequest.Handler>();
        services.AddScoped<IValidator<CreateLeaveRequest.Request>, CreateLeaveRequest.Validator>();
        services.AddScoped<IValidator<UpdateLeaveRequest.Request>, UpdateLeaveRequest.Validator>();
        services.AddScoped<CreatePosition.Handler>();
        services.AddScoped<GetPositionById.Handler>();
        services.AddScoped<GetPositionPage.Handler>();
        services.AddScoped<UpdatePosition.Handler>();
        services.AddScoped<DeletePosition.Handler>();
        services.AddScoped<IValidator<CreatePosition.Request>, CreatePosition.Validator>();
        services.AddScoped<IValidator<UpdatePosition.Request>, UpdatePosition.Validator>();
        services.AddScoped<CreateResume.Handler>();
        services.AddScoped<GetResumeById.Handler>();
        services.AddScoped<GetResumePage.Handler>();
        services.AddScoped<UpdateResume.Handler>();
        services.AddScoped<DeleteResume.Handler>();
        services.AddScoped<IValidator<CreateResume.Request>, CreateResume.Validator>();
        services.AddScoped<IValidator<UpdateResume.Request>, UpdateResume.Validator>();

        return services;
    }

    public static IEndpointRouteBuilder MapHREndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateCandidate.MapEndpoint(endpoints);
        GetCandidateById.MapEndpoint(endpoints);
        GetCandidatePage.MapEndpoint(endpoints);
        UpdateCandidate.MapEndpoint(endpoints);
        DeleteCandidate.MapEndpoint(endpoints);
        CreateEmployee.MapEndpoint(endpoints);
        GetEmployeeById.MapEndpoint(endpoints);
        GetEmployeePage.MapEndpoint(endpoints);
        UpdateEmployee.MapEndpoint(endpoints);
        DeleteEmployee.MapEndpoint(endpoints);
        CreateEmploymentHistory.MapEndpoint(endpoints);
        GetEmploymentHistoryById.MapEndpoint(endpoints);
        GetEmploymentHistoryPage.MapEndpoint(endpoints);
        UpdateEmploymentHistory.MapEndpoint(endpoints);
        DeleteEmploymentHistory.MapEndpoint(endpoints);
        CreateInterview.MapEndpoint(endpoints);
        GetInterviewById.MapEndpoint(endpoints);
        GetInterviewPage.MapEndpoint(endpoints);
        UpdateInterview.MapEndpoint(endpoints);
        DeleteInterview.MapEndpoint(endpoints);
        CreateJob.MapEndpoint(endpoints);
        GetJobById.MapEndpoint(endpoints);
        GetJobPage.MapEndpoint(endpoints);
        UpdateJob.MapEndpoint(endpoints);
        DeleteJob.MapEndpoint(endpoints);
        CreateJobGrade.MapEndpoint(endpoints);
        GetJobGradeById.MapEndpoint(endpoints);
        GetJobGradePage.MapEndpoint(endpoints);
        UpdateJobGrade.MapEndpoint(endpoints);
        DeleteJobGrade.MapEndpoint(endpoints);
        CreateLeaveRequest.MapEndpoint(endpoints);
        GetLeaveRequestById.MapEndpoint(endpoints);
        GetLeaveRequestPage.MapEndpoint(endpoints);
        UpdateLeaveRequest.MapEndpoint(endpoints);
        DeleteLeaveRequest.MapEndpoint(endpoints);
        CreatePosition.MapEndpoint(endpoints);
        GetPositionById.MapEndpoint(endpoints);
        GetPositionPage.MapEndpoint(endpoints);
        UpdatePosition.MapEndpoint(endpoints);
        DeletePosition.MapEndpoint(endpoints);
        CreateResume.MapEndpoint(endpoints);
        GetResumeById.MapEndpoint(endpoints);
        GetResumePage.MapEndpoint(endpoints);
        UpdateResume.MapEndpoint(endpoints);
        DeleteResume.MapEndpoint(endpoints);

        return endpoints;
    }
}
