using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Features;

public static class AcademicSetup
{
    public enum AcademicSetupType
    {
        AcademicYear,
        Class,
        Section
    }

    public sealed record Response(
        Guid Id,
        string Name,
        string? Code,
        Guid BranchId,
        Guid? ParentId = null,
        DateOnly? StartDate = null,
        DateOnly? EndDate = null,
        bool? IsCurrent = null,
        Guid? EducationLevelId = null,
        string? EducationLevelName = null);

    public sealed record ListRequest(
        Guid? TenantId,
        Guid BranchId,
        AcademicSetupType Type)
        : IRequest<Result<IReadOnlyCollection<Response>>>;

    public sealed record CreateRequest(
        Guid? TenantId,
        Guid SchoolId,
        Guid BranchId,
        AcademicSetupType Type,
        string Name,
        Guid? ParentId,
        Guid? EducationLevelId,
        DateOnly? StartDate,
        DateOnly? EndDate,
        bool IsCurrent = false)
        : IRequest<Result<Response>>;

    public sealed record CreateApiRequest(
        Guid? TenantId,
        Guid SchoolId,
        Guid BranchId,
        string Kind,
        string Name,
        Guid? ParentId,
        Guid? EducationLevelId,
        DateOnly? StartDate,
        DateOnly? EndDate,
        bool IsCurrent = false);

    public sealed class ListHandler(
        ITenantScope tenantScope,
        IAcademicSetupQuery academicSetupQuery)
        : IRequestHandler<ListRequest, Result<IReadOnlyCollection<Response>>>
    {
        public async Task<Result<IReadOnlyCollection<Response>>> HandleAsync(
            ListRequest request,
            CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);

            if (!tenantId.HasValue)
            {
                return Result<IReadOnlyCollection<Response>>.Failure(
                    Error.Validation("Tenant context is required."));
            }

            var items = request.Type switch
            {
                AcademicSetupType.AcademicYear =>
                    await academicSetupQuery.GetAcademicYearsAsync(
                        tenantId.Value,
                        request.BranchId,
                        cancellationToken),

                AcademicSetupType.Class =>
                    await academicSetupQuery.GetClassesAsync(
                        tenantId.Value,
                        request.BranchId,
                        cancellationToken),

                AcademicSetupType.Section =>
                    await academicSetupQuery.GetSectionsAsync(
                        tenantId.Value,
                        request.BranchId,
                        cancellationToken),

                _ => throw new ArgumentOutOfRangeException(
                    nameof(request.Type),
                    request.Type,
                    "Unsupported academic setup type.")
            };

            var response = items
                .Select(MapResponse)
                .ToArray();

            return Result<IReadOnlyCollection<Response>>.Success(response);
        }
    }

    public sealed class CreateHandler(
        ITenantScope tenantScope,
        IAcademicSetupCommand academicSetupCommand,
        IAcademicSetupQuery academicSetupQuery,
        IBusinessNumberGenerator businessNumberGenerator)
        : IRequestHandler<CreateRequest, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            CreateRequest request,
            CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);

            if (!tenantId.HasValue)
            {
                return Result<Response>.Failure(
                    Error.Validation("Tenant context is required."));
            }

            var branchIsValid =
                await academicSetupCommand.BranchBelongsToSchoolAsync(
                    tenantId.Value,
                    request.SchoolId,
                    request.BranchId,
                    cancellationToken);

            if (!branchIsValid)
            {
                return Result<Response>.Failure(
                    Error.Validation(
                        "Branch does not belong to the selected school."));
            }

            var result = request.Type switch
            {
                AcademicSetupType.AcademicYear =>
                    await CreateAcademicYearAsync(
                        tenantId.Value,
                        request,
                        cancellationToken),

                AcademicSetupType.Class =>
                    await CreateClassAsync(
                        tenantId.Value,
                        request,
                        cancellationToken),

                AcademicSetupType.Section =>
                    await CreateSectionAsync(
                        tenantId.Value,
                        request,
                        cancellationToken),

                _ => throw new ArgumentOutOfRangeException(
                    nameof(request.Type),
                    request.Type,
                    "Unsupported academic setup type.")
            };

			if (result.IsFailure || result.Value == null)
			{
				return Result<Response>.Failure(result.Error);
			}		

			var response = MapResponse(result.Value);

			return Result<Response>.Success(response);
		}

        private async Task<Result<AcademicSetupItem>> CreateAcademicYearAsync(
            Guid tenantId,
            CreateRequest request,
            CancellationToken cancellationToken)
        {
            if (!request.StartDate.HasValue || !request.EndDate.HasValue)
            {
                return Result<AcademicSetupItem>.Failure(
                    Error.Validation(
                        "Academic year start and end dates are required."));
            }

            if (request.EndDate.Value <= request.StartDate.Value)
            {
                return Result<AcademicSetupItem>.Failure(
                    Error.Validation(
                        "Academic year end date must be after the start date."));
            }

            var code = request.Name.Replace('/', '-');

            var item = await academicSetupCommand.CreateAcademicYearAsync(
                tenantId,
                request.SchoolId,
                request.BranchId,
                request.Name,
                code,
                request.StartDate.Value,
                request.EndDate.Value,
                request.IsCurrent,
                cancellationToken);

            return Result<AcademicSetupItem>.Success(item);
        }

        private async Task<Result<AcademicSetupItem>> CreateClassAsync(
            Guid tenantId,
            CreateRequest request,
            CancellationToken cancellationToken)
        {
            if (!request.EducationLevelId.HasValue)
            {
                return Result<AcademicSetupItem>.Failure(
                    Error.Validation("Education level is required for a class."));
            }

            var levelIsAllowed = await academicSetupQuery.BranchAllowsEducationLevelAsync(
                tenantId,
                request.BranchId,
                request.EducationLevelId.Value,
                cancellationToken);

            if (!levelIsAllowed)
            {
                return Result<AcademicSetupItem>.Failure(
                    Error.Validation("The selected education level is not enabled for this branch."));
            }

            var code = await businessNumberGenerator.NextAsync(
                $"CLASS:{request.BranchId}",
                "CLS-",
                tenantId,
                5,
                cancellationToken);

            var item = await academicSetupCommand.CreateClassAsync(
                tenantId,
                request.SchoolId,
                request.BranchId,
                request.Name,
                code,
                request.EducationLevelId.Value,
                cancellationToken);

            return Result<AcademicSetupItem>.Success(item);
        }

        private async Task<Result<AcademicSetupItem>> CreateSectionAsync(
            Guid tenantId,
            CreateRequest request,
            CancellationToken cancellationToken)
        {
            if (!request.ParentId.HasValue)
            {
                return Result<AcademicSetupItem>.Failure(
                    Error.Validation("A class is required for a section."));
            }

            var code = await businessNumberGenerator.NextAsync(
                $"SECTION:{request.BranchId}:{request.ParentId.Value}",
                "SEC-",
                tenantId,
                3,
                cancellationToken);

            var item = await academicSetupCommand.CreateSectionAsync(
                tenantId,
                request.BranchId,
                request.ParentId.Value,
                request.Name,
                code,
                cancellationToken);

            return Result<AcademicSetupItem>.Success(item);
        }
    }

    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet(
                "/api/academics/setup/{kind}",
                async (
                    string kind,
                    Guid branchId,
                    Guid? tenantId,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    if (!TryParseType(kind, out var type))
                    {
                        return Results.BadRequest(
                            new { message = "Unknown academic setup type." });
                    }

                    var request = new ListRequest(
                        tenantId,
                        branchId,
                        type);

                    var result = await mediator.SendAsync<
                        ListRequest,
                        Result<IReadOnlyCollection<Response>>>(
                        request,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .RequireAuthorization();

        endpoints
            .MapPost(
                "/api/academics/setup",
                async (
                    CreateApiRequest apiRequest,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    if (!TryParseType(apiRequest.Kind, out var type))
                    {
                        return Results.BadRequest(
                            new { message = "Unknown academic setup type." });
                    }

                    var request = new CreateRequest(
                        apiRequest.TenantId,
                        apiRequest.SchoolId,
                        apiRequest.BranchId,
                        type,
                        apiRequest.Name,
                        apiRequest.ParentId,
                        apiRequest.EducationLevelId,
                        apiRequest.StartDate,
                        apiRequest.EndDate,
                        apiRequest.IsCurrent);

                    var result = await mediator.SendAsync<
                        CreateRequest,
                        Result<Response>>(
                        request,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .RequireAuthorization();
    }

    private static bool TryParseType(
        string value,
        out AcademicSetupType type)
    {
        type = value.Trim().ToLowerInvariant() switch
        {
            "years" => AcademicSetupType.AcademicYear,
            "classes" => AcademicSetupType.Class,
            "sections" => AcademicSetupType.Section,
            _ => default
        };

        return value.Trim().ToLowerInvariant() is
            "years" or "classes" or "sections";
    }

    private static Response MapResponse(AcademicSetupItem item)
    {
		
        return new Response(
            item.Id,
            item.Name,
            item.Code,
            item.BranchId,
            item.ParentId,
            item.StartDate,
            item.EndDate,
            item.IsCurrent,
            item.EducationLevelId,
            item.EducationLevelName);
    }
}
