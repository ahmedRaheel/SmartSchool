using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Models;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Employee;

public static class UpdateEmployee
{
	public sealed record Response(
		Guid TenantId,
		Guid Id,
		Guid? UserId,	
		string FirstName,
		string? LastName,
		string? CnicNumber,
		byte[]? Photo,
		string? PhotoContentType,
		string? PhotoFileName,
		string? Email,
		string? Phone,
		DateOnly HireDate,
		string EmploymentTypeCode,
		string Status,
		Guid? SourceCandidateId);

	public sealed record Request(
		Guid TenantId,
		Guid Id,
		string FirstName,
		string? LastName,
		string? CnicNumber,
		string? Email,
		string? Phone,
		DateOnly HireDate,
		string EmploymentTypeCode,
		string Status) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.Id).NotEmpty();
		}
	}

	public sealed class Handler(IEmployeeQuery entityQuery, IEmployeeCommand entityCommand)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
		{
			var entity = await entityQuery.GetByIdAsync(request.TenantId, request.Id, cancellationToken);
			if (entity is null)
			{
				return Result<Response>.Failure(
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(EmployeeEntity))));
			}

			entity.UpdateDetails(
				request.FirstName,
				request.LastName,
				request.CnicNumber,
				request.Email,
				request.Phone,
				request.HireDate,
				request.EmploymentTypeCode,
				request.Status);

			await entityCommand.UpdateAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPut(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "employee"),
				async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var command = request with { Id = id };
					var result = await mediator.SendAsync<Request, Result<Response>>(command, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("UpdateEmployee").WithTags(ModuleConstants.Name).RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(EmployeeEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.EmployeeId,
			entity.UserId,			
			entity.FirstName,
			entity.LastName,
			entity.CnicNumber,
			entity.Photo,
			entity.PhotoContentType,
			entity.PhotoFileName,
			entity.Email,
			entity.Phone,
			entity.HireDate,
			entity.EmploymentTypeCode,
			entity.Status,
			entity.SourceCandidateId);
	}
}
