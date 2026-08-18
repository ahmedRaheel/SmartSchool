
using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Learning.Features.Assignment;
using SmartSchool.Modules.Learning.Features.AssignmentSubmission;
using SmartSchool.Modules.Learning.Features.LearningResource;
using SmartSchool.Modules.Learning.Features.Lesson;
using SmartSchool.Modules.Learning.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Learning;

public static class Module
{
    public static IServiceCollection AddLearningModule(
        this IServiceCollection services)
    {
        services.AddSmartSchoolMediator(typeof(Module).Assembly);
        services.AddScoped<IAssignmentQuery, AssignmentQuery>();
        services.AddScoped<IAssignmentCommand, AssignmentCommand>();
        services.AddScoped<IAssignmentSubmissionQuery, AssignmentSubmissionQuery>();
        services.AddScoped<IAssignmentSubmissionCommand, AssignmentSubmissionCommand>();
        services.AddScoped<ILearningResourceQuery, LearningResourceQuery>();
        services.AddScoped<ILearningResourceCommand, LearningResourceCommand>();
        services.AddScoped<ILessonQuery, LessonQuery>();
        services.AddScoped<ILessonCommand, LessonCommand>();

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
