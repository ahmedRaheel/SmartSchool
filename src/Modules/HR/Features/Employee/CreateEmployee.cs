using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Models;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Employee;

public static class CreateEmployee
{
	public sealed record Response(
		Guid TenantId,
		Guid Id,
		Guid? UserId,
		string EmployeeNumber,
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
		Guid? UserId,
		string EmployeeNumber,
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
		Guid? SourceCandidateId) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.EmployeeNumber).NotEmpty();
			RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
			RuleFor(x => x.EmploymentTypeCode).NotEmpty().MaximumLength(30);
		}
	}

	public sealed class Handler(IEmployeeQuery entityQuery, IEmployeeCommand entityCommand)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
		{
			var exists = await entityQuery.ExistsByEmployeeNumberAsync(
				request.TenantId, request.EmployeeNumber, null, cancellationToken);
			if (exists)
			{
				return Result<Response>.Failure(
					Error.Conflict("Employee with the supplied EmployeeNumber already exists."));
			}

			var entity = EmployeeEntity.Create(
				request.TenantId,
				request.UserId,
				request.EmployeeNumber,
				request.FirstName,
				request.LastName,
				request.CnicNumber,
				request.Photo,
				request.PhotoContentType,
				request.PhotoFileName,
				request.Email,
				request.Phone,
				request.HireDate,
				request.EmploymentTypeCode,
				request.Status,
				request.SourceCandidateId);

			await entityCommand.AddAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "employee"),
				async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("CreateEmployee").WithTags(ModuleConstants.Name).RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(EmployeeEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.Id,
			entity.UserId,
			entity.EmployeeNumber,
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
