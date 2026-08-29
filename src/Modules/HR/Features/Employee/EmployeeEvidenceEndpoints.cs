using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Models;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Features.Employee;

public static class EmployeeEvidenceEndpoints
{
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        CreateEmployeeEducation.MapEndpoint(endpoints);
        CreateEmployeeExperience.MapEndpoint(endpoints);
        return endpoints;
    }
}

public static class CreateEmployeeEducation
{
    public sealed record Request(Guid? TenantId, Guid EmployeeId, string Qualification, string? Institute, string? FieldOfStudy, DateOnly? StartDate, DateOnly? EndDate, string? Grade, bool IsHighest) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id);
    public sealed class Validator : AbstractValidator<Request> { public Validator() { RuleFor(x=>x.EmployeeId).NotEmpty(); RuleFor(x=>x.Qualification).NotEmpty().MaximumLength(150); } }
    public sealed class Handler(IEmployeeEvidenceCommand command) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var entity = EmployeeEducationEntity.Create(request.TenantId!.Value, request.EmployeeId, request.Qualification, request.Institute, request.FieldOfStudy, request.StartDate, request.EndDate, request.Grade, request.IsHighest);
            await command.AddEducationAsync(entity, cancellationToken);
            return Result<Response>.Success(new Response(entity.EmployeeEducationId));
        }
    }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapPost("/api/hr/employee/{employeeId:guid}/education", async (Guid employeeId, Request request, ITenantScope scope, IMediator mediator, CancellationToken ct) => { var tenantId=scope.Resolve(request.TenantId); if(!tenantId.HasValue)return Results.BadRequest(new{message="Tenant is required."}); return (await mediator.SendAsync<Request,Result<Response>>(request with { TenantId=tenantId.Value, EmployeeId=employeeId },ct)).ToHttpResult(); }).WithTags("HR").RequireAuthorization();
}

public static class CreateEmployeeExperience
{
    public sealed record Request(Guid? TenantId, Guid EmployeeId, string Employer, string JobTitle, DateOnly StartDate, DateOnly? EndDate, string? Responsibilities) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id);
    public sealed class Validator : AbstractValidator<Request> { public Validator() { RuleFor(x=>x.EmployeeId).NotEmpty(); RuleFor(x=>x.Employer).NotEmpty().MaximumLength(200); RuleFor(x=>x.JobTitle).NotEmpty().MaximumLength(150); } }
    public sealed class Handler(IEmployeeEvidenceCommand command) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var entity = EmployeeExperienceEntity.Create(request.TenantId!.Value, request.EmployeeId, request.Employer, request.JobTitle, request.StartDate, request.EndDate, request.Responsibilities);
            await command.AddExperienceAsync(entity, cancellationToken);
            return Result<Response>.Success(new Response(entity.EmployeeExperienceId));
        }
    }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapPost("/api/hr/employee/{employeeId:guid}/experience", async (Guid employeeId, Request request, ITenantScope scope, IMediator mediator, CancellationToken ct) => { var tenantId=scope.Resolve(request.TenantId); if(!tenantId.HasValue)return Results.BadRequest(new{message="Tenant is required."}); return (await mediator.SendAsync<Request,Result<Response>>(request with { TenantId=tenantId.Value, EmployeeId=employeeId },ct)).ToHttpResult(); }).WithTags("HR").RequireAuthorization();
}
