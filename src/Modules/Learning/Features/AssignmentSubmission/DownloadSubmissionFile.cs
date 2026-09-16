using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Learning.Authorization;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.Modules.Learning.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Learning.Features.AssignmentSubmission;

public static class DownloadSubmissionFile
{
    public sealed record Query(Guid TenantId, Guid Id) : IRequest<Result<Response>>;
    public sealed record Response(string FileName, string ContentType, byte[] Content);
    public interface IDownloadSubmissionFileQuery { Task<Response?> GetAsync(Query request, CancellationToken cancellationToken); }
    internal sealed class DownloadSubmissionFileQuery(IDbConnectionFactory factory, ICurrentUser user) : IDownloadSubmissionFileQuery
    {
        public async Task<Response?> GetAsync(Query request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT s.file_name AS "FileName", coalesce(s.content_type, 'application/octet-stream') AS "ContentType",
                    s.file_content AS "Content" FROM lms.student_assignment_submission s
                JOIN lms.academic_assignment a ON a.academic_assignment_id = s.academic_assignment_id AND a.tenant_id = s.tenant_id
                WHERE s.tenant_id = @TenantId AND s.submission_id = @Id AND s.is_active AND a.is_active AND s.file_content IS NOT NULL
                    AND (@CampusId IS NULL OR a.branch_id = @CampusId)
                    AND (@ManageAll OR a.teacher_employee_id = @EmployeeId OR s.student_id = @StudentId);
                """;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<Response>(new CommandDefinition(sql, new { request.TenantId, request.Id,
                CampusId = user.BranchId, ManageAll = LearningPermissions.CanManageAll(user),
                EmployeeId = user.IsInRole(SmartSchoolRoles.Teacher) ? user.EmployeeId ?? user.TeacherId : null,
                user.StudentId }, cancellationToken: cancellationToken));
        }
    }
    public sealed class Handler(IDownloadSubmissionFileQuery query) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var file = await query.GetAsync(request, cancellationToken);
            return file is null ? Result<Response>.Failure(Error.NotFound("File not found.")) : Result<Response>.Success(file);
        }
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/learning/assignment-submission/{id:guid}/file", async (Guid id, Guid? tenantId,
            ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(tenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            var result = await mediator.SendAsync<Query, Result<Response>>(new(tenant.Value, id), cancellationToken);
            return result.IsSuccess && result.Value is { } file
                ? Results.File(file.Content, file.ContentType, file.FileName) : result.ToHttpResult();
        }).WithName("DownloadSubmissionFile").WithTags("Learning").RequireAuthorization();
        return endpoints;
    }
}
