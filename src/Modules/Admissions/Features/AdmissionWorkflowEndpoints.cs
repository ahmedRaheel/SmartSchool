using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Admissions.Features;

/// <summary>
/// Maps HTTP routes for the admission workflow vertical slices.
/// </summary>
public static class AdmissionWorkflowEndpoints
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/admissions/workflow/applications",
                GetApplicationsAsync)
            .WithName("GetAdmissionApplications")
            .WithTags("Admissions")
            .RequireAuthorization();

        endpoints.MapPost(
                "/api/admissions/workflow/applications",
                CreateApplicationAsync)
            .WithName("CreateAdmissionApplication")
            .WithTags("Admissions")
            .RequireAuthorization();

        endpoints.MapPut(
                "/api/admissions/workflow/applications/{id:guid}/status",
                ChangeStatusAsync)
            .WithName("ChangeAdmissionStatus")
            .WithTags("Admissions")
            .RequireAuthorization();

        endpoints.MapGet(
                "/api/admissions/criteria",
                GetCriteriaAsync)
            .WithName("GetAdmissionCriteria")
            .WithTags("Admission Criteria")
            .RequireAuthorization();

        endpoints.MapPost(
                "/api/admissions/criteria",
                CreateCriteriaAsync)
            .WithName("CreateAdmissionCriteria")
            .WithTags("Admission Criteria")
            .RequireAuthorization();
    }

    private static async Task<IResult> GetApplicationsAsync(
        Guid? tenantId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new GetAdmissionApplications.Request(tenantId);
        var result = await mediator.SendAsync<
            GetAdmissionApplications.Request,
            Result<IReadOnlyList<AdmissionApplicationDto>>>(
            request,
            cancellationToken);

        return result.ToHttpResult();
    }

    private static async Task<IResult> CreateApplicationAsync(
        CreateAdmissionApplication.Request request,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.SendAsync<
            CreateAdmissionApplication.Request,
            Result<CreateAdmissionApplication.Response>>(
            request,
            cancellationToken);

        return result.ToHttpResult();
    }

    private static async Task<IResult> ChangeStatusAsync(
        Guid id,
        ChangeAdmissionStatus.Body body,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        if (!AdmissionApplicationStatusExtensions.TryParseDatabaseValue(
                body.Status,
                out var status))
        {
            return Results.BadRequest(new
            {
                message = "Invalid admission status."
            });
        }

        var request = new ChangeAdmissionStatus.Request(
            id,
            body.TenantId,
            status,
            body.Notes);

        var result = await mediator.SendAsync<
            ChangeAdmissionStatus.Request,
            Result<ChangeAdmissionStatus.Response>>(
            request,
            cancellationToken);

        return result.ToHttpResult();
    }

    private static async Task<IResult> GetCriteriaAsync(
        Guid? tenantId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new GetAdmissionCriteria.Request(tenantId);
        var result = await mediator.SendAsync<
            GetAdmissionCriteria.Request,
            Result<IReadOnlyList<AdmissionCriteriaDto>>>(
            request,
            cancellationToken);

        return result.ToHttpResult();
    }

    private static async Task<IResult> CreateCriteriaAsync(
        CreateAdmissionCriteria.Request request,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.SendAsync<
            CreateAdmissionCriteria.Request,
            Result<CreateAdmissionCriteria.Response>>(
            request,
            cancellationToken);

        return result.ToHttpResult();
    }
}
