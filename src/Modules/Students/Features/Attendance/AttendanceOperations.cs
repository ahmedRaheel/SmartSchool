using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Attendance;

/// <summary>
/// Operational attendance workflow. This is intentionally separate from the
/// legacy generated master-data endpoints so attendance is persisted per
/// student, class section and school day.
/// </summary>
public static class AttendanceOperations
{
    private static readonly HashSet<string> AllowedStatuses =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "PRESENT",
            "ABSENT",
            "LATE",
            "EXCUSED",
            "LEAVE"
        };

    public sealed record RosterQuery(
        Guid TenantId,
        Guid ClassSectionId,
        DateOnly AttendanceDate) : IRequest<Result<RosterResponse>>;

    public sealed record AttendanceRow(
        Guid StudentId,
        string StudentNumber,
        string StudentName,
        string? Status,
        string? Remarks,
        DateTimeOffset? UpdatedAt);

    public sealed record RosterResponse(
        Guid ClassSectionId,
        DateOnly AttendanceDate,
        IReadOnlyList<AttendanceRow> Students);

    public interface IAttendanceRosterQuery
    {
        Task<RosterResponse> GetAsync(
            RosterQuery request,
            Guid? campusId,
            CancellationToken cancellationToken);
    }

    internal sealed class AttendanceRosterQuery(IDbConnectionFactory connectionFactory)
        : IAttendanceRosterQuery
    {
        public async Task<RosterResponse> GetAsync(
            RosterQuery request,
            Guid? campusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    s.student_id AS "StudentId",
                    s.student_number AS "StudentNumber",
                    trim(s.first_name || ' ' || coalesce(s.last_name, '')) AS "StudentName",
                    a.attendance_status AS "Status",
                    a.remarks AS "Remarks",
                    a.updated_at AS "UpdatedAt"
                FROM student.student_enrollment enrollment
                JOIN student.student s
                  ON s.student_id = enrollment.student_id
                 AND s.tenant_id = enrollment.tenant_id
                 AND s.is_active
                JOIN academic.class_section section
                  ON section.class_section_id = enrollment.class_section_id
                 AND section.tenant_id = enrollment.tenant_id
                 AND section.is_active
                LEFT JOIN student.attendance a
                  ON a.tenant_id = enrollment.tenant_id
                 AND a.student_id = enrollment.student_id
                 AND a.class_section_id = enrollment.class_section_id
                 AND a.attendance_date = @AttendanceDate
                 AND a.is_active
                WHERE enrollment.tenant_id = @TenantId
                  AND enrollment.class_section_id = @ClassSectionId
                  AND enrollment.is_active
                  AND upper(enrollment.status) = 'ACTIVE'
                  AND (@CampusId IS NULL OR section.campus_id = @CampusId)
                ORDER BY s.first_name, s.last_name, s.student_number;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var rows = await connection.QueryAsync<AttendanceRow>(
                new CommandDefinition(
                    sql,
                    new
                    {
                        request.TenantId,
                        request.ClassSectionId,
                        request.AttendanceDate,
                        CampusId = campusId
                    },
                    cancellationToken: cancellationToken));

            return new RosterResponse(
                request.ClassSectionId,
                request.AttendanceDate,
                rows.AsList());
        }
    }

    public sealed record HistoryQuery(
        Guid TenantId,
        Guid? ClassSectionId,
        Guid? StudentId,
        DateOnly? FromDate,
        DateOnly? ToDate) : IRequest<Result<IReadOnlyList<HistoryItem>>>;

    public sealed record HistoryItem(
        Guid AttendanceId,
        Guid StudentId,
        string StudentNumber,
        string StudentName,
        Guid ClassSectionId,
        string ClassSectionName,
        DateOnly AttendanceDate,
        string Status,
        string? Remarks,
        DateTimeOffset CreatedAt,
        DateTimeOffset? UpdatedAt);

    public interface IAttendanceHistoryQuery
    {
        Task<IReadOnlyList<HistoryItem>> GetAsync(
            HistoryQuery request,
            Guid? campusId,
            CancellationToken cancellationToken);
    }

    internal sealed class AttendanceHistoryQuery(IDbConnectionFactory connectionFactory)
        : IAttendanceHistoryQuery
    {
        public async Task<IReadOnlyList<HistoryItem>> GetAsync(
            HistoryQuery request,
            Guid? campusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    a.attendance_id AS "AttendanceId",
                    a.student_id AS "StudentId",
                    s.student_number AS "StudentNumber",
                    trim(s.first_name || ' ' || coalesce(s.last_name, '')) AS "StudentName",
                    a.class_section_id AS "ClassSectionId",
                    section.name AS "ClassSectionName",
                    a.attendance_date AS "AttendanceDate",
                    a.attendance_status AS "Status",
                    a.remarks AS "Remarks",
                    a.created_at AS "CreatedAt",
                    a.updated_at AS "UpdatedAt"
                FROM student.attendance a
                JOIN student.student s
                  ON s.student_id = a.student_id
                 AND s.tenant_id = a.tenant_id
                JOIN academic.class_section section
                  ON section.class_section_id = a.class_section_id
                 AND section.tenant_id = a.tenant_id
                WHERE a.tenant_id = @TenantId
                  AND a.is_active
                  AND a.student_id IS NOT NULL
                  AND a.class_section_id IS NOT NULL
                  AND a.attendance_date IS NOT NULL
                  AND a.attendance_status IS NOT NULL
                  AND (@CampusId IS NULL OR section.campus_id = @CampusId)
                  AND (@ClassSectionId IS NULL OR a.class_section_id = @ClassSectionId)
                  AND (@StudentId IS NULL OR a.student_id = @StudentId)
                  AND (@FromDate IS NULL OR a.attendance_date >= @FromDate)
                  AND (@ToDate IS NULL OR a.attendance_date <= @ToDate)
                ORDER BY a.attendance_date DESC, section.name, s.first_name, s.last_name
                LIMIT 2000;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<HistoryItem>(
                new CommandDefinition(
                    sql,
                    new
                    {
                        request.TenantId,
                        request.ClassSectionId,
                        request.StudentId,
                        request.FromDate,
                        request.ToDate,
                        CampusId = campusId
                    },
                    cancellationToken: cancellationToken))).AsList();
        }
    }

    public sealed class HistoryHandler(
        IAttendanceHistoryQuery query,
        ICurrentUser currentUser) : IRequestHandler<HistoryQuery, Result<IReadOnlyList<HistoryItem>>>
    {
        public async Task<Result<IReadOnlyList<HistoryItem>>> HandleAsync(
            HistoryQuery request,
            CancellationToken cancellationToken)
        {
            if (request.FromDate.HasValue && request.ToDate.HasValue && request.FromDate > request.ToDate)
            {
                return Result<IReadOnlyList<HistoryItem>>.Failure(
                    Error.Validation("Attendance from-date cannot be after to-date."));
            }

            return Result<IReadOnlyList<HistoryItem>>.Success(
                await query.GetAsync(request, currentUser.BranchId, cancellationToken));
        }
    }

    public sealed record MarkRow(
        Guid StudentId,
        string Status,
        string? Remarks);

    public sealed record MarkRequest(
        Guid TenantId,
        Guid ClassSectionId,
        DateOnly AttendanceDate,
        IReadOnlyList<MarkRow> Students) : IRequest<Result<MarkResponse>>;

    public sealed record MarkResponse(int SavedCount);

    public sealed class MarkValidator : AbstractValidator<MarkRequest>
    {
        public MarkValidator()
        {
            RuleFor(request => request.TenantId).NotEmpty();
            RuleFor(request => request.ClassSectionId).NotEmpty();
            RuleFor(request => request.AttendanceDate)
                .Must(date => date <= DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))
                .WithMessage("Attendance date cannot be in the future.");
            RuleFor(request => request.Students)
                .NotNull()
                .NotEmpty()
                .Must(rows => rows is not null
                    && rows.Count <= 2000
                    && rows.Select(row => row.StudentId).Distinct().Count() == rows.Count)
                .WithMessage("Attendance must contain unique students and no more than 2000 rows.");

            RuleForEach(request => request.Students).ChildRules(student =>
            {
                student.RuleFor(row => row.StudentId).NotEmpty();
                student.RuleFor(row => row.Status)
                    .NotEmpty()
                    .Must(status => AllowedStatuses.Contains(status))
                    .WithMessage("Attendance status must be PRESENT, ABSENT, LATE, EXCUSED or LEAVE.");
                student.RuleFor(row => row.Remarks).MaximumLength(1000);
            });
        }
    }

    public interface IAttendanceEnrollmentQuery
    {
        Task<HashSet<Guid>> GetEligibleStudentsAsync(
            MarkRequest request,
            Guid? campusId,
            CancellationToken cancellationToken);
    }

    internal sealed class AttendanceEnrollmentQuery(IDbConnectionFactory connectionFactory)
        : IAttendanceEnrollmentQuery
    {
        public async Task<HashSet<Guid>> GetEligibleStudentsAsync(
            MarkRequest request,
            Guid? campusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT enrollment.student_id
                FROM student.student_enrollment enrollment
                JOIN academic.class_section section
                  ON section.class_section_id = enrollment.class_section_id
                 AND section.tenant_id = enrollment.tenant_id
                 AND section.is_active
                WHERE enrollment.tenant_id = @TenantId
                  AND enrollment.class_section_id = @ClassSectionId
                  AND enrollment.is_active
                  AND upper(enrollment.status) = 'ACTIVE'
                  AND (@CampusId IS NULL OR section.campus_id = @CampusId);
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var ids = await connection.QueryAsync<Guid>(
                new CommandDefinition(
                    sql,
                    new
                    {
                        request.TenantId,
                        request.ClassSectionId,
                        CampusId = campusId
                    },
                    cancellationToken: cancellationToken));

            return ids.ToHashSet();
        }
    }

    public interface IMarkAttendanceCommand
    {
        Task<Result<MarkResponse>> SaveAsync(
            MarkRequest request,
            CancellationToken cancellationToken);
    }

    internal sealed class MarkAttendanceCommand(
        IStudentsDbContext dbContext,
        ICurrentUser currentUser) : IMarkAttendanceCommand
    {
        public async Task<Result<MarkResponse>> SaveAsync(
            MarkRequest request,
            CancellationToken cancellationToken)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            foreach (var student in request.Students)
            {
                var normalizedStatus = student.Status.Trim().ToUpperInvariant();
                var normalizedRemarks = string.IsNullOrWhiteSpace(student.Remarks)
                    ? null
                    : student.Remarks.Trim();
                var attendanceId = Guid.NewGuid();
                var code = $"ATT-{request.AttendanceDate:yyyyMMdd}-{student.StudentId:N}";
                var name = $"Attendance {request.AttendanceDate:yyyy-MM-dd}";

                await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                    INSERT INTO student.attendance
                    (
                        attendance_id,
                        tenant_id,
                        code,
                        name,
                        student_id,
                        class_section_id,
                        attendance_date,
                        attendance_status,
                        remarks,
                        marked_by,
                        is_active,
                        created_at,
                        updated_at,
                        row_version
                    )
                    VALUES
                    (
                        {attendanceId},
                        {request.TenantId},
                        {code},
                        {name},
                        {student.StudentId},
                        {request.ClassSectionId},
                        {request.AttendanceDate},
                        {normalizedStatus},
                        {normalizedRemarks},
                        {currentUser.UserId},
                        TRUE,
                        now(),
                        now(),
                        decode(md5(random()::text || clock_timestamp()::text), 'hex')
                    )
                    ON CONFLICT (tenant_id, student_id, class_section_id, attendance_date)
                    DO UPDATE SET
                        attendance_status = EXCLUDED.attendance_status,
                        remarks = EXCLUDED.remarks,
                        marked_by = EXCLUDED.marked_by,
                        is_active = TRUE,
                        updated_at = now(),
                        row_version = decode(md5(random()::text || clock_timestamp()::text), 'hex');
                    """, cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
            return Result<MarkResponse>.Success(new MarkResponse(request.Students.Count));
        }
    }

    public sealed class MarkHandler(
        IAttendanceEnrollmentQuery enrollmentQuery,
        IMarkAttendanceCommand command,
        ICurrentUser currentUser) : IRequestHandler<MarkRequest, Result<MarkResponse>>
    {
        public async Task<Result<MarkResponse>> HandleAsync(
            MarkRequest request,
            CancellationToken cancellationToken)
        {
            var eligibleStudents = await enrollmentQuery.GetEligibleStudentsAsync(
                request,
                currentUser.BranchId,
                cancellationToken);

            if (request.Students.Any(row => !eligibleStudents.Contains(row.StudentId)))
            {
                return Result<MarkResponse>.Failure(
                    Error.Validation("Attendance can only be saved for active students enrolled in the selected class section."));
            }

            return await command.SaveAsync(request, cancellationToken);
        }
    }

    public sealed class RosterHandler(
        IAttendanceRosterQuery query,
        ICurrentUser currentUser) : IRequestHandler<RosterQuery, Result<RosterResponse>>
    {
        public async Task<Result<RosterResponse>> HandleAsync(
            RosterQuery request,
            CancellationToken cancellationToken)
        {
            var response = await query.GetAsync(request, currentUser.BranchId, cancellationToken);
            return Result<RosterResponse>.Success(response);
        }
    }

    public static IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/students/attendance",
                async (
                    Guid? tenantId,
                    Guid? classSectionId,
                    Guid? studentId,
                    DateOnly? fromDate,
                    DateOnly? toDate,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var resolvedTenantId = tenantScope.Resolve(tenantId);
                    if (!resolvedTenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }

                    var result = await mediator.SendAsync<HistoryQuery, Result<IReadOnlyList<HistoryItem>>>(
                        new HistoryQuery(
                            resolvedTenantId.Value,
                            classSectionId,
                            studentId,
                            fromDate,
                            toDate),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetAttendanceHistory")
            .WithTags("Students")
            .RequireAuthorization(SmartSchoolPolicies.AcademicManagement);

        endpoints.MapGet(
                "/api/students/attendance/roster",
                async (
                    Guid? tenantId,
                    Guid classSectionId,
                    DateOnly attendanceDate,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var resolvedTenantId = tenantScope.Resolve(tenantId);
                    if (!resolvedTenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }

                    var result = await mediator.SendAsync<RosterQuery, Result<RosterResponse>>(
                        new RosterQuery(resolvedTenantId.Value, classSectionId, attendanceDate),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetAttendanceRoster")
            .WithTags("Students")
            .RequireAuthorization(SmartSchoolPolicies.AcademicManagement);

        endpoints.MapPut(
                "/api/students/attendance",
                async (
                    MarkRequest request,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var resolvedTenantId = tenantScope.Resolve(request.TenantId);
                    if (!resolvedTenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }

                    var scopedRequest = request with { TenantId = resolvedTenantId.Value };
                    var result = await mediator.SendAsync<MarkRequest, Result<MarkResponse>>(
                        scopedRequest,
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("MarkStudentAttendance")
            .WithTags("Students")
            .RequireAuthorization(SmartSchoolPolicies.AcademicManagement);

        return endpoints;
    }
}
