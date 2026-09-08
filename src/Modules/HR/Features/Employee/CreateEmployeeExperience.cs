using SmartSchool.Modules.HR.Persistence;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Features.Employee;

public static class CreateEmployeeExperience
{
    public sealed record Request(Guid? TenantId, Guid EmployeeId, string Employer, string JobTitle, DateOnly StartDate, DateOnly? EndDate, string? Responsibilities) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id);
    public sealed class Validator : AbstractValidator<Request> { public Validator() { RuleFor(x=>x.EmployeeId).NotEmpty(); RuleFor(x=>x.Employer).NotEmpty().MaximumLength(200); RuleFor(x=>x.JobTitle).NotEmpty().MaximumLength(150); } }
    public interface ICreateEmployeeExperienceCommand
    {
        Task<EmployeeExperienceEntity> AddAsync(EmployeeExperienceEntity entity, CancellationToken cancellationToken);
    }

    internal sealed class CreateEmployeeExperienceCommand(IHRDbContext dbContext) : ICreateEmployeeExperienceCommand
    {
        public async Task<EmployeeExperienceEntity> AddAsync(EmployeeExperienceEntity entity, CancellationToken cancellationToken)
        {
            await dbContext.EmployeeExperiences.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }

    public sealed class Handler(ICreateEmployeeExperienceCommand command) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var entity = EmployeeExperienceEntity.Create(request.TenantId!.Value, request.EmployeeId, request.Employer, request.JobTitle, request.StartDate, request.EndDate, request.Responsibilities);
            await command.AddAsync(entity, cancellationToken);
            return Result<Response>.Success(new Response(entity.EmployeeExperienceId));
        }
    }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapPost("/api/hr/employee/{employeeId:guid}/experience", async (Guid employeeId, Request request, ITenantScope scope, IMediator mediator, CancellationToken ct) => { var tenantId=scope.Resolve(request.TenantId); if(!tenantId.HasValue)return Results.BadRequest(new{message="Tenant is required."}); return (await mediator.SendAsync<Request,Result<Response>>(request with { TenantId=tenantId.Value, EmployeeId=employeeId },ct)).ToHttpResult(); }).WithTags("HR").RequireAuthorization();
}
