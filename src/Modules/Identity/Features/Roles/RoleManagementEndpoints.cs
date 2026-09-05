using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Modules.Identity.Persistence.Identity;

namespace SmartSchool.Modules.Identity.Features.Roles;

public static class RoleManagementEndpoints
{
    public sealed record CreateRoleRequest(string Name, string? Description, Guid? TenantId);
    public sealed record RoleResponse(Guid Id, string Name, string? Description, Guid? TenantId, bool IsSystemRole);

    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group=endpoints.MapGroup("/api/identity/roles").WithTags("Identity - Roles").RequireAuthorization("AdminOnly");
        group.MapGet("", async (RoleManager<SmartSchoolRole> manager, CancellationToken ct) =>
            Results.Ok(await manager.Roles.AsNoTracking().OrderBy(x=>x.Name)
                .Select(x=>new RoleResponse(x.Id,x.Name!,x.Description,x.TenantId,x.IsSystemRole)).ToListAsync(ct)));
        group.MapPost("", CreateAsync);
        group.MapDelete("/{id:guid}", DeleteAsync);
    }

    private static async Task<IResult> CreateAsync(CreateRoleRequest request, RoleManager<SmartSchoolRole> manager)
    {
        var role=new SmartSchoolRole { Id=Guid.NewGuid(), Name=request.Name, Description=request.Description, TenantId=request.TenantId };
        var result=await manager.CreateAsync(role);
        return result.Succeeded ? Results.Created($"/api/identity/roles/{role.Id}",new RoleResponse(role.Id,role.Name!,role.Description,role.TenantId,role.IsSystemRole))
            : Results.ValidationProblem(result.Errors.GroupBy(x=>x.Code).ToDictionary(x=>x.Key,x=>x.Select(e=>e.Description).ToArray()));
    }
    private static async Task<IResult> DeleteAsync(Guid id, RoleManager<SmartSchoolRole> manager)
    {
        var role=await manager.FindByIdAsync(id.ToString());
        if(role is null) return Results.NotFound();
        if(role.IsSystemRole) return Results.BadRequest(new { error="System roles cannot be deleted." });
        var result=await manager.DeleteAsync(role);
        return result.Succeeded ? Results.NoContent() : Results.BadRequest(result.Errors);
    }
}
