using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
namespace SmartSchool.Modules.Finance.Features.FeeStructure;

public static class CreateFeeStructure
{
	public sealed record Response(Guid TenantId, Guid Id, Guid GradeLevelId, Guid FeeTypeId, Guid? AcademicYearId, decimal Amount, string Frequency, DateOnly? EffectiveFrom, DateOnly? EffectiveTo, bool IsActive);
	public sealed record Request(Guid TenantId, Guid GradeLevelId, Guid FeeTypeId, decimal Amount, string Frequency = "Monthly", Guid? AcademicYearId = null, DateOnly? EffectiveFrom = null, DateOnly? EffectiveTo = null) : IRequest<Result<Response>>;
	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.GradeLevelId).NotEmpty();
			RuleFor(x => x.FeeTypeId).NotEmpty();
			RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
		}
	}
	public interface ICreateFeeStructure
	{
		Task AddAsync(FeeStructureEntity entity, CancellationToken cancellationToken);
	}
	internal sealed class CreateFeeStructurePersistence(IApplicationDbContext db) : ICreateFeeStructure
	{
		public async Task AddAsync(FeeStructureEntity entity, CancellationToken ct)
		{
			await db.Set<FeeStructureEntity>().AddAsync(entity, ct);
			await db.SaveChangesAsync(ct);
		}
	}
	public sealed class Handler(ICreateFeeStructure persistence) : IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Request x, CancellationToken  cancellationToken)
		{
			var feeStructure = FeeStructureEntity.Create(x.TenantId, 
				x.GradeLevelId, 
				x.FeeTypeId,
				x.Amount, 
				x.Frequency, 
				x.AcademicYearId, 
				x.EffectiveFrom, 
				x.EffectiveTo);
			await persistence.AddAsync(feeStructure, cancellationToken);
			return Result<Response>.Success(new(feeStructure.TenantId,
				feeStructure.FeeStructureId,
				feeStructure.GradeLevelId,
				feeStructure.FeeTypeId,
				feeStructure.AcademicYearId, 
				feeStructure.Amount, 
				feeStructure.Frequency, 
				feeStructure.EffectiveFrom,
				feeStructure.EffectiveTo, 
				feeStructure.IsActive));
		}
	}
	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, 
			"fee-structure"), 
			async (Request request, IMediator mediator, CancellationToken ct) 
				=> (await mediator.SendAsync<Request, Result<Response>>(request, ct)).ToHttpResult())
			.WithName("CreateFeeStructure")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}
}
