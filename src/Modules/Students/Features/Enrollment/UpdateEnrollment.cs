using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;
using SmartSchool.Modules.Students.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Enrollment;

public static class UpdateEnrollment
{
	public sealed record Response(Guid TenantId, Guid Id, Guid StudentId, Guid AcademicYearId, Guid ClassSectionId, DateOnly EnrollmentDate, string Status);
	public sealed record Request(Guid TenantId, Guid Id, Guid ClassSectionId, string Status) : IRequest<Result<Response>>;
	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator(){ RuleFor(x=>x.TenantId).NotEmpty(); RuleFor(x=>x.Id).NotEmpty(); RuleFor(x=>x.ClassSectionId).NotEmpty(); RuleFor(x=>x.Status).NotEmpty().MaximumLength(30); }
	}
	public sealed class Handler(IEnrollmentQuery query, IEnrollmentCommand command) : IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
		{
			var entity=await query.GetByIdAsync(request.TenantId,request.Id,cancellationToken);
			if(entity is null) return Result<Response>.Failure(Error.NotFound("Enrollment was not found."));
			entity.ChangePlacement(request.ClassSectionId,request.Status);
			await command.UpdateAsync(entity,cancellationToken);
			return Result<Response>.Success(new(entity.TenantId,entity.StudentEnrollmentId,entity.StudentId,entity.AcademicYearId,entity.ClassSectionId,entity.EnrollmentDate,entity.Status));
		}
	}
	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPut(ApiRoutes.EntityById(ModuleConstants.RouteSegment,"enrollment"), async(Guid id,Request request,IMediator mediator,CancellationToken cancellationToken)=>
		{
			var command=request with { Id=id }; var result=await mediator.SendAsync<Request,Result<Response>>(command,cancellationToken); return result.ToHttpResult();
		}).WithName("UpdateEnrollment").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantStudent);
		return endpoints;
	}
}
