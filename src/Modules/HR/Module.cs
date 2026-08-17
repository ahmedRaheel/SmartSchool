
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Persistence;

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
        services.AddScoped<ICandidateQuery, CandidateQuery>();
        services.AddScoped<ICandidateCommand, CandidateCommand>();
        services.AddScoped<IEmployeeQuery, EmployeeQuery>();
        services.AddScoped<IEmployeeCommand, EmployeeCommand>();
        services.AddScoped<IEmploymentHistoryQuery, EmploymentHistoryQuery>();
        services.AddScoped<IEmploymentHistoryCommand, EmploymentHistoryCommand>();
        services.AddScoped<IInterviewQuery, InterviewQuery>();
        services.AddScoped<IInterviewCommand, InterviewCommand>();
        services.AddScoped<IJobQuery, JobQuery>();
        services.AddScoped<IJobCommand, JobCommand>();
        services.AddScoped<IJobGradeQuery, JobGradeQuery>();
        services.AddScoped<IJobGradeCommand, JobGradeCommand>();
        services.AddScoped<ILeaveRequestQuery, LeaveRequestQuery>();
        services.AddScoped<ILeaveRequestCommand, LeaveRequestCommand>();
        services.AddScoped<IPositionQuery, PositionQuery>();
        services.AddScoped<IPositionCommand, PositionCommand>();
        services.AddScoped<IResumeQuery, ResumeQuery>();
        services.AddScoped<IResumeCommand, ResumeCommand>();

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
