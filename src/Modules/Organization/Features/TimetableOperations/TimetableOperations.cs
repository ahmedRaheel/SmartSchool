using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.TimetableOperations;

/// <summary>
/// Timetable authoring workflow with reusable periods and collision checks for
/// class sections, teachers and rooms.
/// </summary>
public static class TimetableOperations
{
    public sealed record PeriodItem(
        Guid PeriodId,
        Guid CampusId,
        int? PeriodNumber,
        string Name,
        TimeOnly StartTime,
        TimeOnly EndTime,
        string PeriodType);

    public sealed record TimetableItem(
        Guid TimetableId,
        Guid CampusId,
        Guid AcademicYearId,
        Guid? TermId,
        string Name,
        DateOnly? EffectiveFrom,
        DateOnly? EffectiveTo,
        string Status);

    public sealed record EntryItem(
        Guid EntryId,
        Guid TimetableId,
        int DayOfWeek,
        Guid PeriodId,
        string PeriodName,
        Guid? ClassSectionId,
        string? ClassSectionName,
        Guid? CourseOfferingId,
        string? CourseName,
        Guid? TeacherCourseAssignmentId,
        Guid? TeacherEmployeeId,
        string? TeacherName,
        Guid? RoomId,
        string? RoomName,
        string EntryType);

    public sealed record DashboardQuery(Guid TenantId, Guid? TimetableId)
        : IRequest<Result<DashboardResponse>>;

    public sealed record DashboardResponse(
        IReadOnlyList<PeriodItem> Periods,
        IReadOnlyList<TimetableItem> Timetables,
        IReadOnlyList<EntryItem> Entries);

    public interface ITimetableOperationsDashboardQuery
    {
        Task<DashboardResponse> GetAsync(
            Guid tenantId,
            Guid? campusId,
            Guid? timetableId,
            CancellationToken cancellationToken);
    }

    internal sealed class TimetableOperationsDashboardQuery(IDbConnectionFactory connectionFactory)
        : ITimetableOperationsDashboardQuery
    {
        public async Task<DashboardResponse> GetAsync(
            Guid tenantId,
            Guid? campusId,
            Guid? timetableId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    p.timetable_period_id AS "PeriodId",
                    p.campus_id AS "CampusId",
                    p.period_number AS "PeriodNumber",
                    p.name AS "Name",
                    p.start_time AS "StartTime",
                    p.end_time AS "EndTime",
                    p.period_type AS "PeriodType"
                FROM academic.timetable_period p
                WHERE p.tenant_id = @TenantId
                  AND p.is_active
                  AND (@CampusId IS NULL OR p.campus_id = @CampusId)
                ORDER BY p.campus_id, p.period_number NULLS LAST, p.start_time;

                SELECT
                    t.timetable_id AS "TimetableId",
                    t.campus_id AS "CampusId",
                    t.academic_year_id AS "AcademicYearId",
                    t.term_id AS "TermId",
                    t.name AS "Name",
                    t.effective_from AS "EffectiveFrom",
                    t.effective_to AS "EffectiveTo",
                    t.status AS "Status"
                FROM academic.timetable t
                WHERE t.tenant_id = @TenantId
                  AND t.is_active
                  AND (@CampusId IS NULL OR t.campus_id = @CampusId)
                ORDER BY t.created_at DESC;

                SELECT
                    e.timetable_entry_id AS "EntryId",
                    e.timetable_id AS "TimetableId",
                    e.day_of_week AS "DayOfWeek",
                    e.timetable_period_id AS "PeriodId",
                    p.name AS "PeriodName",
                    e.class_section_id AS "ClassSectionId",
                    cs.name AS "ClassSectionName",
                    e.course_offering_id AS "CourseOfferingId",
                    coalesce(co.display_name, co.name) AS "CourseName",
                    e.teacher_course_assignment_id AS "TeacherCourseAssignmentId",
                    a.employee_id AS "TeacherEmployeeId",
                    trim(emp.first_name || ' ' || coalesce(emp.last_name, '')) AS "TeacherName",
                    e.room_id AS "RoomId",
                    r.name AS "RoomName",
                    e.entry_type AS "EntryType"
                FROM academic.timetable_entry e
                JOIN academic.timetable t
                  ON t.timetable_id = e.timetable_id
                 AND t.tenant_id = e.tenant_id
                JOIN academic.timetable_period p
                  ON p.timetable_period_id = e.timetable_period_id
                 AND p.tenant_id = e.tenant_id
                LEFT JOIN academic.class_section cs
                  ON cs.class_section_id = e.class_section_id
                 AND cs.tenant_id = e.tenant_id
                LEFT JOIN academic.course_offering co
                  ON co.course_offering_id = e.course_offering_id
                 AND co.tenant_id = e.tenant_id
                LEFT JOIN academic.teacher_course_assignment a
                  ON a.teacher_course_assignment_id = e.teacher_course_assignment_id
                 AND a.tenant_id = e.tenant_id
                LEFT JOIN hr.employee emp
                  ON emp.employee_id = a.employee_id
                 AND emp.tenant_id = e.tenant_id
                LEFT JOIN org.room r
                  ON r.room_id = e.room_id
                 AND r.tenant_id = e.tenant_id
                WHERE e.tenant_id = @TenantId
                  AND e.is_active
                  AND t.is_active
                  AND (@CampusId IS NULL OR t.campus_id = @CampusId)
                  AND (@TimetableId IS NULL OR e.timetable_id = @TimetableId)
                ORDER BY e.timetable_id, e.day_of_week, p.start_time;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            using var grid = await connection.QueryMultipleAsync(
                new CommandDefinition(
                    sql,
                    new { TenantId = tenantId, CampusId = campusId, TimetableId = timetableId },
                    cancellationToken: cancellationToken));

            return new DashboardResponse(
                (await grid.ReadAsync<PeriodItem>()).AsList(),
                (await grid.ReadAsync<TimetableItem>()).AsList(),
                (await grid.ReadAsync<EntryItem>()).AsList());
        }
    }

    public sealed class DashboardHandler(
        ITimetableOperationsDashboardQuery query,
        ICurrentUser currentUser) : IRequestHandler<DashboardQuery, Result<DashboardResponse>>
    {
        public async Task<Result<DashboardResponse>> HandleAsync(
            DashboardQuery request,
            CancellationToken cancellationToken)
        {
            return Result<DashboardResponse>.Success(
                await query.GetAsync(
                    request.TenantId,
                    currentUser.BranchId,
                    request.TimetableId,
                    cancellationToken));
        }
    }

    public sealed record CreatePeriodRequest(
        Guid TenantId,
        Guid CampusId,
        int? PeriodNumber,
        string Name,
        TimeOnly StartTime,
        TimeOnly EndTime,
        string PeriodType = "SUBJECT") : IRequest<Result<CreatePeriodResponse>>;

    public sealed record CreatePeriodResponse(Guid PeriodId);

    public sealed class CreatePeriodValidator : AbstractValidator<CreatePeriodRequest>
    {
        public CreatePeriodValidator()
        {
            RuleFor(request => request.TenantId).NotEmpty();
            RuleFor(request => request.CampusId).NotEmpty();
            RuleFor(request => request.Name).NotEmpty().MaximumLength(80);
            RuleFor(request => request.PeriodNumber).GreaterThan(0).When(request => request.PeriodNumber.HasValue);
            RuleFor(request => request)
                .Must(request => request.EndTime > request.StartTime)
                .WithMessage("Period end time must be after start time.");
            RuleFor(request => request.PeriodType).NotEmpty().MaximumLength(30);
        }
    }

    public interface ICreateTimetablePeriodQuery
    {
        Task<bool> CampusExistsAsync(
            Guid tenantId,
            Guid campusId,
            Guid? scopedCampusId,
            CancellationToken cancellationToken);

        Task<bool> OverlapsAsync(
            CreatePeriodRequest request,
            CancellationToken cancellationToken);
    }

    internal sealed class CreateTimetablePeriodQuery(IDbConnectionFactory connectionFactory)
        : ICreateTimetablePeriodQuery
    {
        public async Task<bool> CampusExistsAsync(
            Guid tenantId,
            Guid campusId,
            Guid? scopedCampusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT EXISTS (
                    SELECT 1 FROM org.campus
                    WHERE tenant_id = @TenantId
                      AND campus_id = @CampusId
                      AND is_active
                      AND (@ScopedCampusId IS NULL OR campus_id = @ScopedCampusId)
                );
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(
                new CommandDefinition(
                    sql,
                    new { TenantId = tenantId, CampusId = campusId, ScopedCampusId = scopedCampusId },
                    cancellationToken: cancellationToken));
        }

        public async Task<bool> OverlapsAsync(
            CreatePeriodRequest request,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT EXISTS (
                    SELECT 1 FROM academic.timetable_period
                    WHERE tenant_id = @TenantId
                      AND campus_id = @CampusId
                      AND is_active
                      AND start_time < @EndTime
                      AND end_time > @StartTime
                );
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(
                new CommandDefinition(sql, request, cancellationToken: cancellationToken));
        }
    }

    public interface ICreateTimetablePeriodCommand
    {
        Task<CreatePeriodResponse> CreateAsync(
            CreatePeriodRequest request,
            CancellationToken cancellationToken);
    }

    internal sealed class CreateTimetablePeriodCommand(IOrganizationDbContext dbContext)
        : ICreateTimetablePeriodCommand
    {
        public async Task<CreatePeriodResponse> CreateAsync(
            CreatePeriodRequest request,
            CancellationToken cancellationToken)
        {
            var periodId = Guid.NewGuid();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                INSERT INTO academic.timetable_period
                (
                    timetable_period_id,
                    tenant_id,
                    campus_id,
                    period_number,
                    name,
                    start_time,
                    end_time,
                    period_type,
                    is_active,
                    created_at,
                    row_version
                )
                VALUES
                (
                    {periodId},
                    {request.TenantId},
                    {request.CampusId},
                    {request.PeriodNumber},
                    {request.Name.Trim()},
                    {request.StartTime},
                    {request.EndTime},
                    {request.PeriodType.Trim().ToUpperInvariant()},
                    TRUE,
                    now(),
                    decode(md5(random()::text || clock_timestamp()::text), 'hex')
                );
                """, cancellationToken);
            return new CreatePeriodResponse(periodId);
        }
    }

    public sealed class CreatePeriodHandler(
        ICreateTimetablePeriodQuery query,
        ICreateTimetablePeriodCommand command,
        ICurrentUser currentUser) : IRequestHandler<CreatePeriodRequest, Result<CreatePeriodResponse>>
    {
        public async Task<Result<CreatePeriodResponse>> HandleAsync(
            CreatePeriodRequest request,
            CancellationToken cancellationToken)
        {
            if (!await query.CampusExistsAsync(
                    request.TenantId,
                    request.CampusId,
                    currentUser.BranchId,
                    cancellationToken))
            {
                return Result<CreatePeriodResponse>.Failure(Error.NotFound("Campus not found."));
            }

            if (await query.OverlapsAsync(request, cancellationToken))
            {
                return Result<CreatePeriodResponse>.Failure(
                    Error.Conflict("The timetable period overlaps an existing active period for this campus."));
            }

            return Result<CreatePeriodResponse>.Success(
                await command.CreateAsync(request, cancellationToken));
        }
    }

    public sealed record CreateTimetableRequest(
        Guid TenantId,
        Guid CampusId,
        Guid AcademicYearId,
        Guid? TermId,
        string Name,
        DateOnly? EffectiveFrom,
        DateOnly? EffectiveTo) : IRequest<Result<CreateTimetableResponse>>;

    public sealed record CreateTimetableResponse(Guid TimetableId, string Status);

    public sealed class CreateTimetableValidator : AbstractValidator<CreateTimetableRequest>
    {
        public CreateTimetableValidator()
        {
            RuleFor(request => request.TenantId).NotEmpty();
            RuleFor(request => request.CampusId).NotEmpty();
            RuleFor(request => request.AcademicYearId).NotEmpty();
            RuleFor(request => request.Name).NotEmpty().MaximumLength(150);
            RuleFor(request => request)
                .Must(request => !request.EffectiveFrom.HasValue
                    || !request.EffectiveTo.HasValue
                    || request.EffectiveTo.Value >= request.EffectiveFrom.Value)
                .WithMessage("Effective-to date cannot be before effective-from date.");
        }
    }

    public interface ICreateOperationalTimetableQuery
    {
        Task<bool> ScopeExistsAsync(
            CreateTimetableRequest request,
            Guid? scopedCampusId,
            CancellationToken cancellationToken);
    }

    internal sealed class CreateOperationalTimetableQuery(IDbConnectionFactory connectionFactory)
        : ICreateOperationalTimetableQuery
    {
        public async Task<bool> ScopeExistsAsync(
            CreateTimetableRequest request,
            Guid? scopedCampusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT EXISTS (
                    SELECT 1
                    FROM academic.academic_year y
                    JOIN org.campus c
                      ON c.campus_id = y.campus_id
                     AND c.tenant_id = y.tenant_id
                    WHERE y.tenant_id = @TenantId
                      AND y.academic_year_id = @AcademicYearId
                      AND y.campus_id = @CampusId
                      AND y.is_active
                      AND c.is_active
                      AND (@ScopedCampusId IS NULL OR y.campus_id = @ScopedCampusId)
                      AND (
                          @TermId IS NULL
                          OR EXISTS (
                              SELECT 1 FROM academic.term term
                              WHERE term.tenant_id = y.tenant_id
                                AND term.term_id = @TermId
                                AND term.academic_year_id = y.academic_year_id
                                AND term.is_active
                          )
                      )
                );
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(
                new CommandDefinition(
                    sql,
                    new
                    {
                        request.TenantId,
                        request.CampusId,
                        request.AcademicYearId,
                        request.TermId,
                        ScopedCampusId = scopedCampusId
                    },
                    cancellationToken: cancellationToken));
        }
    }

    public interface ICreateOperationalTimetableCommand
    {
        Task<CreateTimetableResponse> CreateAsync(
            CreateTimetableRequest request,
            CancellationToken cancellationToken);
    }

    internal sealed class CreateOperationalTimetableCommand(IOrganizationDbContext dbContext)
        : ICreateOperationalTimetableCommand
    {
        public async Task<CreateTimetableResponse> CreateAsync(
            CreateTimetableRequest request,
            CancellationToken cancellationToken)
        {
            var timetableId = Guid.NewGuid();
            var code = $"TIM-{timetableId:N}"[..18].ToUpperInvariant();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                INSERT INTO academic.timetable
                (
                    timetable_id,
                    tenant_id,
                    campus_id,
                    academic_year_id,
                    term_id,
                    name,
                    effective_from,
                    effective_to,
                    status,
                    code,
                    is_active,
                    created_at,
                    row_version
                )
                VALUES
                (
                    {timetableId},
                    {request.TenantId},
                    {request.CampusId},
                    {request.AcademicYearId},
                    {request.TermId},
                    {request.Name.Trim()},
                    {request.EffectiveFrom},
                    {request.EffectiveTo},
                    {"DRAFT"},
                    {code},
                    TRUE,
                    now(),
                    decode(md5(random()::text || clock_timestamp()::text), 'hex')
                );
                """, cancellationToken);
            return new CreateTimetableResponse(timetableId, "DRAFT");
        }
    }

    public sealed class CreateTimetableHandler(
        ICreateOperationalTimetableQuery query,
        ICreateOperationalTimetableCommand command,
        ICurrentUser currentUser) : IRequestHandler<CreateTimetableRequest, Result<CreateTimetableResponse>>
    {
        public async Task<Result<CreateTimetableResponse>> HandleAsync(
            CreateTimetableRequest request,
            CancellationToken cancellationToken)
        {
            if (!await query.ScopeExistsAsync(request, currentUser.BranchId, cancellationToken))
            {
                return Result<CreateTimetableResponse>.Failure(
                    Error.Validation("Campus, academic year or term is outside the current scope."));
            }

            return Result<CreateTimetableResponse>.Success(
                await command.CreateAsync(request, cancellationToken));
        }
    }

    public sealed record AddEntryRequest(
        Guid TenantId,
        Guid TimetableId,
        int DayOfWeek,
        Guid PeriodId,
        Guid ClassSectionId,
        Guid CourseOfferingId,
        Guid TeacherCourseAssignmentId,
        Guid? RoomId,
        string EntryType = "SUBJECT") : IRequest<Result<AddEntryResponse>>;

    public sealed record AddEntryResponse(Guid EntryId);

    public sealed class AddEntryValidator : AbstractValidator<AddEntryRequest>
    {
        public AddEntryValidator()
        {
            RuleFor(request => request.TenantId).NotEmpty();
            RuleFor(request => request.TimetableId).NotEmpty();
            RuleFor(request => request.DayOfWeek).InclusiveBetween(1, 7);
            RuleFor(request => request.PeriodId).NotEmpty();
            RuleFor(request => request.ClassSectionId).NotEmpty();
            RuleFor(request => request.CourseOfferingId).NotEmpty();
            RuleFor(request => request.TeacherCourseAssignmentId).NotEmpty();
            RuleFor(request => request.EntryType).NotEmpty().MaximumLength(30);
        }
    }

    public sealed record EntryContext(
        Guid CampusId,
        Guid AcademicYearId,
        Guid TeacherEmployeeId,
        string ClassSectionName,
        string CourseName,
        string TeacherName,
        string? RoomName);

    public interface IAddTimetableEntryQuery
    {
        Task<EntryContext?> ValidateContextAsync(
            AddEntryRequest request,
            Guid? scopedCampusId,
            CancellationToken cancellationToken);

        Task<bool> HasConflictAsync(
            AddEntryRequest request,
            Guid teacherEmployeeId,
            CancellationToken cancellationToken);
    }

    internal sealed class AddTimetableEntryQuery(IDbConnectionFactory connectionFactory)
        : IAddTimetableEntryQuery
    {
        public async Task<EntryContext?> ValidateContextAsync(
            AddEntryRequest request,
            Guid? scopedCampusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    t.campus_id AS "CampusId",
                    t.academic_year_id AS "AcademicYearId",
                    assignment.employee_id AS "TeacherEmployeeId",
                    section.name AS "ClassSectionName",
                    coalesce(offering.display_name, offering.name) AS "CourseName",
                    trim(employee.first_name || ' ' || coalesce(employee.last_name, '')) AS "TeacherName",
                    room.name AS "RoomName"
                FROM academic.timetable t
                JOIN academic.timetable_period period
                  ON period.timetable_period_id = @PeriodId
                 AND period.tenant_id = t.tenant_id
                 AND period.campus_id = t.campus_id
                 AND period.is_active
                JOIN academic.class_section section
                  ON section.class_section_id = @ClassSectionId
                 AND section.tenant_id = t.tenant_id
                 AND section.campus_id = t.campus_id
                 AND section.academic_year_id = t.academic_year_id
                 AND section.is_active
                JOIN academic.course_offering offering
                  ON offering.course_offering_id = @CourseOfferingId
                 AND offering.tenant_id = t.tenant_id
                 AND offering.campus_id = t.campus_id
                 AND offering.academic_year_id = t.academic_year_id
                 AND offering.is_active
                JOIN academic.teacher_course_assignment assignment
                  ON assignment.teacher_course_assignment_id = @TeacherCourseAssignmentId
                 AND assignment.tenant_id = t.tenant_id
                 AND assignment.course_offering_id = offering.course_offering_id
                 AND assignment.class_section_id = section.class_section_id
                 AND assignment.is_active
                JOIN hr.employee employee
                  ON employee.employee_id = assignment.employee_id
                 AND employee.tenant_id = t.tenant_id
                 AND employee.is_active
                LEFT JOIN org.room room
                  ON room.room_id = @RoomId
                 AND room.tenant_id = t.tenant_id
                 AND room.campus_id = t.campus_id
                 AND room.is_active
                WHERE t.tenant_id = @TenantId
                  AND t.timetable_id = @TimetableId
                  AND t.is_active
                  AND (@ScopedCampusId IS NULL OR t.campus_id = @ScopedCampusId)
                  AND (@RoomId IS NULL OR room.room_id IS NOT NULL);
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<EntryContext>(
                new CommandDefinition(
                    sql,
                    new
                    {
                        request.TenantId,
                        request.TimetableId,
                        request.PeriodId,
                        request.ClassSectionId,
                        request.CourseOfferingId,
                        request.TeacherCourseAssignmentId,
                        request.RoomId,
                        ScopedCampusId = scopedCampusId
                    },
                    cancellationToken: cancellationToken));
        }

        public async Task<bool> HasConflictAsync(
            AddEntryRequest request,
            Guid teacherEmployeeId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT EXISTS (
                    SELECT 1
                    FROM academic.timetable_entry existing
                    LEFT JOIN academic.teacher_course_assignment assignment
                      ON assignment.teacher_course_assignment_id = existing.teacher_course_assignment_id
                     AND assignment.tenant_id = existing.tenant_id
                    WHERE existing.tenant_id = @TenantId
                      AND existing.timetable_id = @TimetableId
                      AND existing.day_of_week = @DayOfWeek
                      AND existing.timetable_period_id = @PeriodId
                      AND existing.is_active
                      AND (
                          existing.class_section_id = @ClassSectionId
                          OR (@RoomId IS NOT NULL AND existing.room_id = @RoomId)
                          OR assignment.employee_id = @TeacherEmployeeId
                      )
                );
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(
                new CommandDefinition(
                    sql,
                    new
                    {
                        request.TenantId,
                        request.TimetableId,
                        request.DayOfWeek,
                        request.PeriodId,
                        request.ClassSectionId,
                        request.RoomId,
                        TeacherEmployeeId = teacherEmployeeId
                    },
                    cancellationToken: cancellationToken));
        }
    }

    public interface IAddTimetableEntryCommand
    {
        Task<AddEntryResponse> AddAsync(
            AddEntryRequest request,
            EntryContext context,
            CancellationToken cancellationToken);
    }

    internal sealed class AddTimetableEntryCommand(IOrganizationDbContext dbContext)
        : IAddTimetableEntryCommand
    {
        public async Task<AddEntryResponse> AddAsync(
            AddEntryRequest request,
            EntryContext context,
            CancellationToken cancellationToken)
        {
            var entryId = Guid.NewGuid();
            var code = $"TENT-{entryId:N}"[..18].ToUpperInvariant();
            var name = $"{context.ClassSectionName} - {context.CourseName}";

            await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                INSERT INTO academic.timetable_entry
                (
                    timetable_entry_id,
                    tenant_id,
                    timetable_id,
                    day_of_week,
                    timetable_period_id,
                    class_section_id,
                    course_offering_id,
                    teacher_course_assignment_id,
                    room_id,
                    entry_type,
                    code,
                    name,
                    is_active,
                    created_at,
                    row_version
                )
                VALUES
                (
                    {entryId},
                    {request.TenantId},
                    {request.TimetableId},
                    {request.DayOfWeek},
                    {request.PeriodId},
                    {request.ClassSectionId},
                    {request.CourseOfferingId},
                    {request.TeacherCourseAssignmentId},
                    {request.RoomId},
                    {request.EntryType.Trim().ToUpperInvariant()},
                    {code},
                    {name},
                    TRUE,
                    now(),
                    decode(md5(random()::text || clock_timestamp()::text), 'hex')
                );
                """, cancellationToken);

            return new AddEntryResponse(entryId);
        }
    }

    public sealed class AddEntryHandler(
        IAddTimetableEntryQuery query,
        IAddTimetableEntryCommand command,
        ICurrentUser currentUser) : IRequestHandler<AddEntryRequest, Result<AddEntryResponse>>
    {
        public async Task<Result<AddEntryResponse>> HandleAsync(
            AddEntryRequest request,
            CancellationToken cancellationToken)
        {
            var context = await query.ValidateContextAsync(
                request,
                currentUser.BranchId,
                cancellationToken);
            if (context is null)
            {
                return Result<AddEntryResponse>.Failure(
                    Error.Validation("Timetable entry references an invalid campus/year/class/course/teacher/room combination."));
            }

            if (await query.HasConflictAsync(request, context.TeacherEmployeeId, cancellationToken))
            {
                return Result<AddEntryResponse>.Failure(
                    Error.Conflict("The class, teacher or room is already booked for this day and period."));
            }

            return Result<AddEntryResponse>.Success(
                await command.AddAsync(request, context, cancellationToken));
        }
    }

    public sealed record DeleteEntryRequest(Guid TenantId, Guid EntryId)
        : IRequest<Result>;

    public interface IDeleteTimetableEntryCommand
    {
        Task<Result> DeleteAsync(
            DeleteEntryRequest request,
            Guid? campusId,
            CancellationToken cancellationToken);
    }

    internal sealed class DeleteTimetableEntryCommand(IOrganizationDbContext dbContext)
        : IDeleteTimetableEntryCommand
    {
        public async Task<Result> DeleteAsync(
            DeleteEntryRequest request,
            Guid? campusId,
            CancellationToken cancellationToken)
        {
            var updated = await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE academic.timetable_entry entry
                SET is_active = FALSE,
                    updated_at = now(),
                    row_version = decode(md5(random()::text || clock_timestamp()::text), 'hex')
                FROM academic.timetable timetable
                WHERE entry.timetable_id = timetable.timetable_id
                  AND entry.tenant_id = timetable.tenant_id
                  AND entry.tenant_id = {request.TenantId}
                  AND entry.timetable_entry_id = {request.EntryId}
                  AND entry.is_active
                  AND ({campusId} IS NULL OR timetable.campus_id = {campusId});
                """, cancellationToken);

            return updated == 1
                ? Result.Success()
                : Result.Failure(Error.NotFound("Timetable entry not found."));
        }
    }

    public sealed class DeleteEntryHandler(
        IDeleteTimetableEntryCommand command,
        ICurrentUser currentUser) : IRequestHandler<DeleteEntryRequest, Result>
    {
        public Task<Result> HandleAsync(
            DeleteEntryRequest request,
            CancellationToken cancellationToken) =>
            command.DeleteAsync(request, currentUser.BranchId, cancellationToken);
    }

    public static IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/academics/timetable-operations",
                async (
                    Guid? tenantId,
                    Guid? timetableId,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var resolvedTenantId = tenantScope.Resolve(tenantId);
                    if (!resolvedTenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }

                    var result = await mediator.SendAsync<DashboardQuery, Result<DashboardResponse>>(
                        new DashboardQuery(resolvedTenantId.Value, timetableId),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetTimetableOperations")
            .WithTags("Academics")
            .RequireAuthorization(SmartSchoolPolicies.AcademicManagement);

        endpoints.MapPost(
                "/api/academics/timetable-period",
                async (
                    CreatePeriodRequest request,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var tenantId = tenantScope.Resolve(request.TenantId);
                    if (!tenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }

                    var result = await mediator.SendAsync<CreatePeriodRequest, Result<CreatePeriodResponse>>(
                        request with { TenantId = tenantId.Value },
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateTimetablePeriod")
            .WithTags("Academics")
            .RequireAuthorization(SmartSchoolPolicies.AcademicManagement);

        endpoints.MapPost(
                "/api/academics/timetable-operations",
                async (
                    CreateTimetableRequest request,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var tenantId = tenantScope.Resolve(request.TenantId);
                    if (!tenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }

                    var result = await mediator.SendAsync<CreateTimetableRequest, Result<CreateTimetableResponse>>(
                        request with { TenantId = tenantId.Value },
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateOperationalTimetable")
            .WithTags("Academics")
            .RequireAuthorization(SmartSchoolPolicies.AcademicManagement);

        endpoints.MapPost(
                "/api/academics/timetable-operations/{timetableId:guid}/entries",
                async (
                    Guid timetableId,
                    AddEntryRequest request,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var tenantId = tenantScope.Resolve(request.TenantId);
                    if (!tenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }

                    var result = await mediator.SendAsync<AddEntryRequest, Result<AddEntryResponse>>(
                        request with { TenantId = tenantId.Value, TimetableId = timetableId },
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("AddOperationalTimetableEntry")
            .WithTags("Academics")
            .RequireAuthorization(SmartSchoolPolicies.AcademicManagement);

        endpoints.MapDelete(
                "/api/academics/timetable-operations/entries/{entryId:guid}",
                async (
                    Guid entryId,
                    Guid? tenantId,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var resolvedTenantId = tenantScope.Resolve(tenantId);
                    if (!resolvedTenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }

                    var result = await mediator.SendAsync<DeleteEntryRequest, Result>(
                        new DeleteEntryRequest(resolvedTenantId.Value, entryId),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("DeleteOperationalTimetableEntry")
            .WithTags("Academics")
            .RequireAuthorization(SmartSchoolPolicies.AcademicManagement);

        return endpoints;
    }
}
