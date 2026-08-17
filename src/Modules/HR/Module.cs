using SmartSchool.Modules.HR.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Persistence;
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
        services.AddScoped<IValidator<CreateCandidate.Request>, CreateCandidate.Validator>();
        services.AddScoped<IValidator<UpdateCandidate.Request>, UpdateCandidate.Validator>();
        services.AddScoped<IValidator<CreateEmployee.Request>, CreateEmployee.Validator>();
        services.AddScoped<IValidator<UpdateEmployee.Request>, UpdateEmployee.Validator>();
        services.AddScoped<IValidator<CreateEmploymentHistory.Request>, CreateEmploymentHistory.Validator>();
        services.AddScoped<IValidator<UpdateEmploymentHistory.Request>, UpdateEmploymentHistory.Validator>();
        services.AddScoped<IValidator<CreateInterview.Request>, CreateInterview.Validator>();
        services.AddScoped<IValidator<UpdateInterview.Request>, UpdateInterview.Validator>();
        services.AddScoped<IValidator<CreateJob.Request>, CreateJob.Validator>();
        services.AddScoped<IValidator<UpdateJob.Request>, UpdateJob.Validator>();
        services.AddScoped<IValidator<CreateJobGrade.Request>, CreateJobGrade.Validator>();
        services.AddScoped<IValidator<UpdateJobGrade.Request>, UpdateJobGrade.Validator>();
        services.AddScoped<IValidator<CreateLeaveRequest.Request>, CreateLeaveRequest.Validator>();
        services.AddScoped<IValidator<UpdateLeaveRequest.Request>, UpdateLeaveRequest.Validator>();
        services.AddScoped<IValidator<CreatePosition.Request>, CreatePosition.Validator>();
        services.AddScoped<IValidator<UpdatePosition.Request>, UpdatePosition.Validator>();
        services.AddScoped<IValidator<CreateResume.Request>, CreateResume.Validator>();
        services.AddScoped<IValidator<UpdateResume.Request>, UpdateResume.Validator>();


        services.AddScoped<IRequestHandler<CreateCandidate.Request, Result<CandidateResponse>>, CreateCandidate.Handler>();
        services.AddScoped<IRequestHandler<GetCandidateById.Query, Result<CandidateResponse>>, GetCandidateById.Handler>();
        services.AddScoped<IRequestHandler<GetCandidatePage.Query, Result<PagedResult<CandidateResponse>>>, GetCandidatePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateCandidate.Request, Result<CandidateResponse>>, UpdateCandidate.Handler>();
        services.AddScoped<IRequestHandler<DeleteCandidate.Command, Result<DeleteCandidate.Response>>, DeleteCandidate.Handler>();
        services.AddScoped<IRequestHandler<CreateEmployee.Request, Result<EmployeeResponse>>, CreateEmployee.Handler>();
        services.AddScoped<IRequestHandler<GetEmployeeById.Query, Result<EmployeeResponse>>, GetEmployeeById.Handler>();
        services.AddScoped<IRequestHandler<GetEmployeePage.Query, Result<PagedResult<EmployeeResponse>>>, GetEmployeePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateEmployee.Request, Result<EmployeeResponse>>, UpdateEmployee.Handler>();
        services.AddScoped<IRequestHandler<DeleteEmployee.Command, Result<DeleteEmployee.Response>>, DeleteEmployee.Handler>();
        services.AddScoped<IRequestHandler<CreateEmploymentHistory.Request, Result<EmploymentHistoryResponse>>, CreateEmploymentHistory.Handler>();
        services.AddScoped<IRequestHandler<GetEmploymentHistoryById.Query, Result<EmploymentHistoryResponse>>, GetEmploymentHistoryById.Handler>();
        services.AddScoped<IRequestHandler<GetEmploymentHistoryPage.Query, Result<PagedResult<EmploymentHistoryResponse>>>, GetEmploymentHistoryPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateEmploymentHistory.Request, Result<EmploymentHistoryResponse>>, UpdateEmploymentHistory.Handler>();
        services.AddScoped<IRequestHandler<DeleteEmploymentHistory.Command, Result<DeleteEmploymentHistory.Response>>, DeleteEmploymentHistory.Handler>();
        services.AddScoped<IRequestHandler<CreateInterview.Request, Result<InterviewResponse>>, CreateInterview.Handler>();
        services.AddScoped<IRequestHandler<GetInterviewById.Query, Result<InterviewResponse>>, GetInterviewById.Handler>();
        services.AddScoped<IRequestHandler<GetInterviewPage.Query, Result<PagedResult<InterviewResponse>>>, GetInterviewPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateInterview.Request, Result<InterviewResponse>>, UpdateInterview.Handler>();
        services.AddScoped<IRequestHandler<DeleteInterview.Command, Result<DeleteInterview.Response>>, DeleteInterview.Handler>();
        services.AddScoped<IRequestHandler<CreateJob.Request, Result<JobResponse>>, CreateJob.Handler>();
        services.AddScoped<IRequestHandler<GetJobById.Query, Result<JobResponse>>, GetJobById.Handler>();
        services.AddScoped<IRequestHandler<GetJobPage.Query, Result<PagedResult<JobResponse>>>, GetJobPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateJob.Request, Result<JobResponse>>, UpdateJob.Handler>();
        services.AddScoped<IRequestHandler<DeleteJob.Command, Result<DeleteJob.Response>>, DeleteJob.Handler>();
        services.AddScoped<IRequestHandler<CreateJobGrade.Request, Result<JobGradeResponse>>, CreateJobGrade.Handler>();
        services.AddScoped<IRequestHandler<GetJobGradeById.Query, Result<JobGradeResponse>>, GetJobGradeById.Handler>();
        services.AddScoped<IRequestHandler<GetJobGradePage.Query, Result<PagedResult<JobGradeResponse>>>, GetJobGradePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateJobGrade.Request, Result<JobGradeResponse>>, UpdateJobGrade.Handler>();
        services.AddScoped<IRequestHandler<DeleteJobGrade.Command, Result<DeleteJobGrade.Response>>, DeleteJobGrade.Handler>();
        services.AddScoped<IRequestHandler<CreateLeaveRequest.Request, Result<LeaveRequestResponse>>, CreateLeaveRequest.Handler>();
        services.AddScoped<IRequestHandler<GetLeaveRequestById.Query, Result<LeaveRequestResponse>>, GetLeaveRequestById.Handler>();
        services.AddScoped<IRequestHandler<GetLeaveRequestPage.Query, Result<PagedResult<LeaveRequestResponse>>>, GetLeaveRequestPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateLeaveRequest.Request, Result<LeaveRequestResponse>>, UpdateLeaveRequest.Handler>();
        services.AddScoped<IRequestHandler<DeleteLeaveRequest.Command, Result<DeleteLeaveRequest.Response>>, DeleteLeaveRequest.Handler>();
        services.AddScoped<IRequestHandler<CreatePosition.Request, Result<PositionResponse>>, CreatePosition.Handler>();
        services.AddScoped<IRequestHandler<GetPositionById.Query, Result<PositionResponse>>, GetPositionById.Handler>();
        services.AddScoped<IRequestHandler<GetPositionPage.Query, Result<PagedResult<PositionResponse>>>, GetPositionPage.Handler>();
        services.AddScoped<IRequestHandler<UpdatePosition.Request, Result<PositionResponse>>, UpdatePosition.Handler>();
        services.AddScoped<IRequestHandler<DeletePosition.Command, Result<DeletePosition.Response>>, DeletePosition.Handler>();
        services.AddScoped<IRequestHandler<CreateResume.Request, Result<ResumeResponse>>, CreateResume.Handler>();
        services.AddScoped<IRequestHandler<GetResumeById.Query, Result<ResumeResponse>>, GetResumeById.Handler>();
        services.AddScoped<IRequestHandler<GetResumePage.Query, Result<PagedResult<ResumeResponse>>>, GetResumePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateResume.Request, Result<ResumeResponse>>, UpdateResume.Handler>();
        services.AddScoped<IRequestHandler<DeleteResume.Command, Result<DeleteResume.Response>>, DeleteResume.Handler>();

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
