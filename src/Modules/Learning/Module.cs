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

        services.AddScoped<CreateAssignment.Handler>();
        services.AddScoped<GetAssignmentById.Handler>();
        services.AddScoped<GetAssignmentPage.Handler>();
        services.AddScoped<UpdateAssignment.Handler>();
        services.AddScoped<DeleteAssignment.Handler>();
        services.AddScoped<IValidator<CreateAssignment.Request>, CreateAssignment.Validator>();
        services.AddScoped<IValidator<UpdateAssignment.Request>, UpdateAssignment.Validator>();
        services.AddScoped<CreateAssignmentSubmission.Handler>();
        services.AddScoped<GetAssignmentSubmissionById.Handler>();
        services.AddScoped<GetAssignmentSubmissionPage.Handler>();
        services.AddScoped<UpdateAssignmentSubmission.Handler>();
        services.AddScoped<DeleteAssignmentSubmission.Handler>();
        services.AddScoped<IValidator<CreateAssignmentSubmission.Request>, CreateAssignmentSubmission.Validator>();
        services.AddScoped<IValidator<UpdateAssignmentSubmission.Request>, UpdateAssignmentSubmission.Validator>();
        services.AddScoped<CreateLearningResource.Handler>();
        services.AddScoped<GetLearningResourceById.Handler>();
        services.AddScoped<GetLearningResourcePage.Handler>();
        services.AddScoped<UpdateLearningResource.Handler>();
        services.AddScoped<DeleteLearningResource.Handler>();
        services.AddScoped<IValidator<CreateLearningResource.Request>, CreateLearningResource.Validator>();
        services.AddScoped<IValidator<UpdateLearningResource.Request>, UpdateLearningResource.Validator>();
        services.AddScoped<CreateLesson.Handler>();
        services.AddScoped<GetLessonById.Handler>();
        services.AddScoped<GetLessonPage.Handler>();
        services.AddScoped<UpdateLesson.Handler>();
        services.AddScoped<DeleteLesson.Handler>();
        services.AddScoped<IValidator<CreateLesson.Request>, CreateLesson.Validator>();
        services.AddScoped<IValidator<UpdateLesson.Request>, UpdateLesson.Validator>();

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
