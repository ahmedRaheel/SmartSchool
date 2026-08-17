using SmartSchool.Modules.Learning.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Learning.Persistence;
using FluentValidation;
using SmartSchool.Modules.Learning.Features.Assignment;
using SmartSchool.Modules.Learning.Features.AssignmentSubmission;
using SmartSchool.Modules.Learning.Features.LearningResource;
using SmartSchool.Modules.Learning.Features.Lesson;

namespace SmartSchool.Modules.Learning;

public static class Module
{
    public static IServiceCollection AddLearningModule(
        this IServiceCollection services)
    {
        services.AddScoped<IAssignmentQuery, AssignmentQuery>();
        services.AddScoped<IAssignmentCommand, AssignmentCommand>();
        services.AddScoped<IAssignmentSubmissionQuery, AssignmentSubmissionQuery>();
        services.AddScoped<IAssignmentSubmissionCommand, AssignmentSubmissionCommand>();
        services.AddScoped<ILearningResourceQuery, LearningResourceQuery>();
        services.AddScoped<ILearningResourceCommand, LearningResourceCommand>();
        services.AddScoped<ILessonQuery, LessonQuery>();
        services.AddScoped<ILessonCommand, LessonCommand>();
        services.AddScoped<IValidator<CreateAssignment.Request>, CreateAssignment.Validator>();
        services.AddScoped<IValidator<UpdateAssignment.Request>, UpdateAssignment.Validator>();
        services.AddScoped<IValidator<CreateAssignmentSubmission.Request>, CreateAssignmentSubmission.Validator>();
        services.AddScoped<IValidator<UpdateAssignmentSubmission.Request>, UpdateAssignmentSubmission.Validator>();
        services.AddScoped<IValidator<CreateLearningResource.Request>, CreateLearningResource.Validator>();
        services.AddScoped<IValidator<UpdateLearningResource.Request>, UpdateLearningResource.Validator>();
        services.AddScoped<IValidator<CreateLesson.Request>, CreateLesson.Validator>();
        services.AddScoped<IValidator<UpdateLesson.Request>, UpdateLesson.Validator>();


        services.AddScoped<IRequestHandler<CreateAssignment.Request, Result<AssignmentResponse>>, CreateAssignment.Handler>();
        services.AddScoped<IRequestHandler<GetAssignmentById.Query, Result<AssignmentResponse>>, GetAssignmentById.Handler>();
        services.AddScoped<IRequestHandler<GetAssignmentPage.Query, Result<PagedResult<AssignmentResponse>>>, GetAssignmentPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateAssignment.Request, Result<AssignmentResponse>>, UpdateAssignment.Handler>();
        services.AddScoped<IRequestHandler<DeleteAssignment.Command, Result<DeleteAssignment.Response>>, DeleteAssignment.Handler>();
        services.AddScoped<IRequestHandler<CreateAssignmentSubmission.Request, Result<AssignmentSubmissionResponse>>, CreateAssignmentSubmission.Handler>();
        services.AddScoped<IRequestHandler<GetAssignmentSubmissionById.Query, Result<AssignmentSubmissionResponse>>, GetAssignmentSubmissionById.Handler>();
        services.AddScoped<IRequestHandler<GetAssignmentSubmissionPage.Query, Result<PagedResult<AssignmentSubmissionResponse>>>, GetAssignmentSubmissionPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateAssignmentSubmission.Request, Result<AssignmentSubmissionResponse>>, UpdateAssignmentSubmission.Handler>();
        services.AddScoped<IRequestHandler<DeleteAssignmentSubmission.Command, Result<DeleteAssignmentSubmission.Response>>, DeleteAssignmentSubmission.Handler>();
        services.AddScoped<IRequestHandler<CreateLearningResource.Request, Result<LearningResourceResponse>>, CreateLearningResource.Handler>();
        services.AddScoped<IRequestHandler<GetLearningResourceById.Query, Result<LearningResourceResponse>>, GetLearningResourceById.Handler>();
        services.AddScoped<IRequestHandler<GetLearningResourcePage.Query, Result<PagedResult<LearningResourceResponse>>>, GetLearningResourcePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateLearningResource.Request, Result<LearningResourceResponse>>, UpdateLearningResource.Handler>();
        services.AddScoped<IRequestHandler<DeleteLearningResource.Command, Result<DeleteLearningResource.Response>>, DeleteLearningResource.Handler>();
        services.AddScoped<IRequestHandler<CreateLesson.Request, Result<LessonResponse>>, CreateLesson.Handler>();
        services.AddScoped<IRequestHandler<GetLessonById.Query, Result<LessonResponse>>, GetLessonById.Handler>();
        services.AddScoped<IRequestHandler<GetLessonPage.Query, Result<PagedResult<LessonResponse>>>, GetLessonPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateLesson.Request, Result<LessonResponse>>, UpdateLesson.Handler>();
        services.AddScoped<IRequestHandler<DeleteLesson.Command, Result<DeleteLesson.Response>>, DeleteLesson.Handler>();

        return services;
    }

    public static IEndpointRouteBuilder MapLearningEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateAssignment.MapEndpoint(endpoints);
        GetAssignmentById.MapEndpoint(endpoints);
        GetAssignmentPage.MapEndpoint(endpoints);
        UpdateAssignment.MapEndpoint(endpoints);
        DeleteAssignment.MapEndpoint(endpoints);
        CreateAssignmentSubmission.MapEndpoint(endpoints);
        GetAssignmentSubmissionById.MapEndpoint(endpoints);
        GetAssignmentSubmissionPage.MapEndpoint(endpoints);
        UpdateAssignmentSubmission.MapEndpoint(endpoints);
        DeleteAssignmentSubmission.MapEndpoint(endpoints);
        CreateLearningResource.MapEndpoint(endpoints);
        GetLearningResourceById.MapEndpoint(endpoints);
        GetLearningResourcePage.MapEndpoint(endpoints);
        UpdateLearningResource.MapEndpoint(endpoints);
        DeleteLearningResource.MapEndpoint(endpoints);
        CreateLesson.MapEndpoint(endpoints);
        GetLessonById.MapEndpoint(endpoints);
        GetLessonPage.MapEndpoint(endpoints);
        UpdateLesson.MapEndpoint(endpoints);
        DeleteLesson.MapEndpoint(endpoints);

        return endpoints;
    }
}
