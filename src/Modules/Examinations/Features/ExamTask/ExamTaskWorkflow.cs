using Dapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Features.StudentExamResult;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.Modules.Examinations.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Examinations.Features.ExamTask;

public static class ExamTaskWorkflow
{
    public const string PaperTask = "EXAM_PAPER";
    public const string ResultTask = "RESULT_ENTRY";

    public sealed record TaskOption(
        Guid ExamSubjectId,
        string Subject,
        Guid CourseOfferingId,
        Guid TeacherCourseAssignmentId,
        Guid TeacherEmployeeId,
        Guid TeacherUserId,
        string TeacherName);

    public sealed record TaskItem(
        Guid TaskId,
        Guid ExamId,
        string Exam,
        Guid ExamSubjectId,
        string Subject,
        string ClassSection,
        Guid TeacherEmployeeId,
        Guid TeacherUserId,
        string TeacherName,
        string TaskType,
        string Title,
        string? Instructions,
        DateTimeOffset AssignedAt,
        DateTimeOffset DueAt,
        string Status,
        DateTimeOffset? SubmittedAt,
        DateTimeOffset? CompletedAt,
        string? SubmissionNotes,
        string? SubmissionFileName,
        bool IsOverdue);

    public interface IExamTaskReadQuery
    {
        Task<IReadOnlyList<TaskOption>> GetOptionsAsync(Guid tenantId, Guid examId, CancellationToken cancellationToken);
        Task<TaskOption?> GetOptionAsync(Guid tenantId, Guid examId, Guid examSubjectId, Guid teacherCourseAssignmentId, CancellationToken cancellationToken);
        Task<IReadOnlyList<TaskItem>> GetExamTasksAsync(Guid tenantId, Guid examId, Guid? campusId, CancellationToken cancellationToken);
        Task<IReadOnlyList<TaskItem>> GetTeacherTasksAsync(Guid tenantId, Guid teacherUserId, CancellationToken cancellationToken);
    }

    internal sealed class ExamTaskReadQuery(IDbConnectionFactory connectionFactory) : IExamTaskReadQuery
    {
        public async Task<IReadOnlyList<TaskOption>> GetOptionsAsync(Guid tenantId, Guid examId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    es.exam_subject_id AS "ExamSubjectId",
                    es.name AS "Subject",
                    es.course_offering_id AS "CourseOfferingId",
                    ta.teacher_course_assignment_id AS "TeacherCourseAssignmentId",
                    ta.employee_id AS "TeacherEmployeeId",
                    employee.user_id AS "TeacherUserId",
                    trim(employee.first_name || ' ' || coalesce(employee.last_name, '')) AS "TeacherName"
                FROM exam.exam e
                JOIN exam.exam_subject es
                  ON es.exam_id = e.exam_id
                 AND es.tenant_id = e.tenant_id
                 AND es.is_active
                JOIN academic.teacher_course_assignment ta
                  ON ta.tenant_id = e.tenant_id
                 AND ta.course_offering_id = es.course_offering_id
                 AND ta.class_section_id = e.class_section_id
                 AND ta.is_active
                 AND (ta.effective_from IS NULL OR ta.effective_from <= CURRENT_DATE)
                 AND (ta.effective_to IS NULL OR ta.effective_to >= CURRENT_DATE)
                JOIN hr.employee employee
                  ON employee.employee_id = ta.employee_id
                 AND employee.tenant_id = ta.tenant_id
                 AND employee.is_active
                 AND upper(employee.status) = 'ACTIVE'
                 AND employee.user_id IS NOT NULL
                WHERE e.tenant_id = @TenantId
                  AND e.exam_id = @ExamId
                  AND e.is_active
                ORDER BY es.name, "TeacherName";
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<TaskOption>(new CommandDefinition(
                sql,
                new { TenantId = tenantId, ExamId = examId },
                cancellationToken: cancellationToken))).AsList();
        }

        public async Task<TaskOption?> GetOptionAsync(
            Guid tenantId,
            Guid examId,
            Guid examSubjectId,
            Guid teacherCourseAssignmentId,
            CancellationToken cancellationToken)
        {
            var options = await GetOptionsAsync(tenantId, examId, cancellationToken);
            return options.SingleOrDefault(option =>
                option.ExamSubjectId == examSubjectId &&
                option.TeacherCourseAssignmentId == teacherCourseAssignmentId);
        }

        public async Task<IReadOnlyList<TaskItem>> GetExamTasksAsync(
            Guid tenantId,
            Guid examId,
            Guid? campusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    task.exam_task_id AS "TaskId",
                    task.exam_id AS "ExamId",
                    exam.name AS "Exam",
                    task.exam_subject_id AS "ExamSubjectId",
                    subject.name AS "Subject",
                    section.name AS "ClassSection",
                    task.teacher_employee_id AS "TeacherEmployeeId",
                    task.teacher_user_id AS "TeacherUserId",
                    trim(employee.first_name || ' ' || coalesce(employee.last_name, '')) AS "TeacherName",
                    task.task_type AS "TaskType",
                    task.title AS "Title",
                    task.instructions AS "Instructions",
                    task.assigned_at AS "AssignedAt",
                    task.due_at AS "DueAt",
                    CASE
                        WHEN task.status NOT IN ('COMPLETED','SUBMITTED') AND task.due_at < now() THEN 'OVERDUE'
                        ELSE task.status
                    END AS "Status",
                    task.submitted_at AS "SubmittedAt",
                    task.completed_at AS "CompletedAt",
                    task.submission_notes AS "SubmissionNotes",
                    task.submission_file_name AS "SubmissionFileName",
                    (task.status NOT IN ('COMPLETED','SUBMITTED') AND task.due_at < now()) AS "IsOverdue"
                FROM exam.exam_task task
                JOIN exam.exam exam
                  ON exam.exam_id = task.exam_id
                 AND exam.tenant_id = task.tenant_id
                JOIN exam.exam_subject subject
                  ON subject.exam_subject_id = task.exam_subject_id
                 AND subject.tenant_id = task.tenant_id
                JOIN academic.class_section section
                  ON section.class_section_id = exam.class_section_id
                 AND section.tenant_id = exam.tenant_id
                JOIN hr.employee employee
                  ON employee.employee_id = task.teacher_employee_id
                 AND employee.tenant_id = task.tenant_id
                WHERE task.tenant_id = @TenantId
                  AND task.exam_id = @ExamId
                  AND task.is_active
                  AND (@CampusId IS NULL OR exam.campus_id = @CampusId)
                ORDER BY task.due_at, subject.name;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<TaskItem>(new CommandDefinition(
                sql,
                new { TenantId = tenantId, ExamId = examId, CampusId = campusId },
                cancellationToken: cancellationToken))).AsList();
        }

        public async Task<IReadOnlyList<TaskItem>> GetTeacherTasksAsync(
            Guid tenantId,
            Guid teacherUserId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    task.exam_task_id AS "TaskId",
                    task.exam_id AS "ExamId",
                    exam.name AS "Exam",
                    task.exam_subject_id AS "ExamSubjectId",
                    subject.name AS "Subject",
                    section.name AS "ClassSection",
                    task.teacher_employee_id AS "TeacherEmployeeId",
                    task.teacher_user_id AS "TeacherUserId",
                    trim(employee.first_name || ' ' || coalesce(employee.last_name, '')) AS "TeacherName",
                    task.task_type AS "TaskType",
                    task.title AS "Title",
                    task.instructions AS "Instructions",
                    task.assigned_at AS "AssignedAt",
                    task.due_at AS "DueAt",
                    CASE
                        WHEN task.status NOT IN ('COMPLETED','SUBMITTED') AND task.due_at < now() THEN 'OVERDUE'
                        ELSE task.status
                    END AS "Status",
                    task.submitted_at AS "SubmittedAt",
                    task.completed_at AS "CompletedAt",
                    task.submission_notes AS "SubmissionNotes",
                    task.submission_file_name AS "SubmissionFileName",
                    (task.status NOT IN ('COMPLETED','SUBMITTED') AND task.due_at < now()) AS "IsOverdue"
                FROM exam.exam_task task
                JOIN exam.exam exam
                  ON exam.exam_id = task.exam_id
                 AND exam.tenant_id = task.tenant_id
                JOIN exam.exam_subject subject
                  ON subject.exam_subject_id = task.exam_subject_id
                 AND subject.tenant_id = task.tenant_id
                JOIN academic.class_section section
                  ON section.class_section_id = exam.class_section_id
                 AND section.tenant_id = exam.tenant_id
                JOIN hr.employee employee
                  ON employee.employee_id = task.teacher_employee_id
                 AND employee.tenant_id = task.tenant_id
                WHERE task.tenant_id = @TenantId
                  AND task.teacher_user_id = @TeacherUserId
                  AND task.is_active
                ORDER BY
                    CASE WHEN task.status IN ('ASSIGNED','IN_PROGRESS') THEN 0 ELSE 1 END,
                    task.due_at,
                    exam.name;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<TaskItem>(new CommandDefinition(
                sql,
                new { TenantId = tenantId, TeacherUserId = teacherUserId },
                cancellationToken: cancellationToken))).AsList();
        }
    }

    public sealed record AssignTaskRequest(
        Guid TenantId,
        Guid ExamId,
        Guid ExamSubjectId,
        Guid TeacherCourseAssignmentId,
        string TaskType,
        string Title,
        string? Instructions,
        DateTimeOffset DueAt) : IRequest<Result<AssignTaskResponse>>;

    public sealed record AssignTaskResponse(Guid TaskId, string Status, DateTimeOffset DueAt);

    public sealed class AssignTaskValidator : AbstractValidator<AssignTaskRequest>
    {
        public AssignTaskValidator()
        {
            RuleFor(request => request.TenantId).NotEmpty();
            RuleFor(request => request.ExamId).NotEmpty();
            RuleFor(request => request.ExamSubjectId).NotEmpty();
            RuleFor(request => request.TeacherCourseAssignmentId).NotEmpty();
            //RuleFor(request => request.TaskType)
            //    .Must(value => value == PaperTask || value == ResultTask)
            //    .WithMessage("Task type must be EXAM_PAPER or RESULT_ENTRY.")/*;*/
            RuleFor(request => request.Title).NotEmpty().MaximumLength(250);
            RuleFor(request => request.Instructions).MaximumLength(4000);
            RuleFor(request => request.DueAt)
                .Must(value => value > DateTimeOffset.UtcNow)
                .WithMessage("Due time must be in the future.");
        }
    }

    public interface IExamTaskCommand
    {
        Task<bool> ExistsAsync(Guid tenantId, Guid examSubjectId, Guid teacherEmployeeId, string taskType, CancellationToken cancellationToken);
        Task<ExamTaskEntity?> GetAsync(Guid tenantId, Guid taskId, CancellationToken cancellationToken);
        Task AddAsync(ExamTaskEntity task, CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);
    }

    internal sealed class ExamTaskCommand(IExaminationsDbContext dbContext) : IExamTaskCommand
    {
        public Task<bool> ExistsAsync(
            Guid tenantId,
            Guid examSubjectId,
            Guid teacherEmployeeId,
            string taskType,
            CancellationToken cancellationToken) =>
            dbContext.ExamTasks.AnyAsync(task =>
                task.TenantId == tenantId &&
                task.ExamSubjectId == examSubjectId &&
                task.TeacherEmployeeId == teacherEmployeeId &&
                task.TaskType == taskType &&
                task.IsActive,
                cancellationToken);

        public Task<ExamTaskEntity?> GetAsync(Guid tenantId, Guid taskId, CancellationToken cancellationToken) =>
            dbContext.ExamTasks.SingleOrDefaultAsync(task =>
                task.TenantId == tenantId && task.ExamTaskId == taskId && task.IsActive,
                cancellationToken);

        public async Task AddAsync(ExamTaskEntity task, CancellationToken cancellationToken)
        {
            await dbContext.ExamTasks.AddAsync(task, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task SaveAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
    }

    public sealed class AssignTaskHandler(
        IExamTaskReadQuery query,
        IExamTaskCommand command,
        ICurrentUser currentUser)
        : IRequestHandler<AssignTaskRequest, Result<AssignTaskResponse>>
    {
        public async Task<Result<AssignTaskResponse>> HandleAsync(
            AssignTaskRequest request,
            CancellationToken cancellationToken)
        {
            var option = await query.GetOptionAsync(
                request.TenantId,
                request.ExamId,
                request.ExamSubjectId,
                request.TeacherCourseAssignmentId,
                cancellationToken);

            if (option is null)
            {
                return Result<AssignTaskResponse>.Failure(
                    Error.Validation("Select the teacher allocated to this exam subject and class."));
            }

            if (await command.ExistsAsync(
                request.TenantId,
                request.ExamSubjectId,
                option.TeacherEmployeeId,
                request.TaskType,
                cancellationToken))
            {
                return Result<AssignTaskResponse>.Failure(
                    Error.Conflict("This teacher already has an active task of this type for the subject."));
            }

            var task = ExamTaskEntity.Assign(
                request.TenantId,
                request.ExamId,
                request.ExamSubjectId,
                option.CourseOfferingId,
                option.TeacherCourseAssignmentId,
                option.TeacherEmployeeId,
                option.TeacherUserId,
                request.TaskType,
                request.Title,
                request.Instructions,
                currentUser.UserId,
                request.DueAt);

            await command.AddAsync(task, cancellationToken);
            return Result<AssignTaskResponse>.Success(new(task.ExamTaskId, task.Status, task.DueAt));
        }
    }

    public sealed record UpdateTaskRequest(
        Guid TenantId,
        Guid TaskId,
        DateTimeOffset DueAt,
        string? Instructions) : IRequest<Result>;

    public sealed class UpdateTaskValidator : AbstractValidator<UpdateTaskRequest>
    {
        public UpdateTaskValidator()
        {
            RuleFor(request => request.TenantId).NotEmpty();
            RuleFor(request => request.TaskId).NotEmpty();
            RuleFor(request => request.DueAt).Must(value => value > DateTimeOffset.UtcNow);
            RuleFor(request => request.Instructions).MaximumLength(4000);
        }
    }

    public sealed class UpdateTaskHandler(IExamTaskCommand command)
        : IRequestHandler<UpdateTaskRequest, Result>
    {
        public async Task<Result> HandleAsync(UpdateTaskRequest request, CancellationToken cancellationToken)
        {
            var task = await command.GetAsync(request.TenantId, request.TaskId, cancellationToken);
            if (task is null)
            {
                return Result.Failure(Error.NotFound("Exam task not found."));
            }

            if (task.Status == "COMPLETED")
            {
                return Result.Failure(Error.Conflict("Completed tasks must be reopened before editing."));
            }

            task.UpdateAssignment(request.DueAt, request.Instructions);
            await command.SaveAsync(cancellationToken);
            return Result.Success();
        }
    }

    public sealed record CompleteTaskRequest(Guid TenantId, Guid TaskId) : IRequest<Result>;

    public sealed class CompleteTaskHandler(IExamTaskCommand command, ICurrentUser currentUser)
        : IRequestHandler<CompleteTaskRequest, Result>
    {
        public async Task<Result> HandleAsync(CompleteTaskRequest request, CancellationToken cancellationToken)
        {
            var task = await command.GetAsync(request.TenantId, request.TaskId, cancellationToken);
            if (task is null)
            {
                return Result.Failure(Error.NotFound("Exam task not found."));
            }

            if (task.Status != "SUBMITTED")
            {
                return Result.Failure(Error.Validation("The teacher must submit the task before it can be completed."));
            }

            task.Complete(currentUser.UserId);
            await command.SaveAsync(cancellationToken);
            return Result.Success();
        }
    }

    public sealed record ReopenTaskRequest(Guid TenantId, Guid TaskId, DateTimeOffset DueAt) : IRequest<Result>;

    public sealed class ReopenTaskValidator : AbstractValidator<ReopenTaskRequest>
    {
        public ReopenTaskValidator()
        {
            RuleFor(request => request.DueAt).Must(value => value > DateTimeOffset.UtcNow);
        }
    }

    public sealed class ReopenTaskHandler(IExamTaskCommand command)
        : IRequestHandler<ReopenTaskRequest, Result>
    {
        public async Task<Result> HandleAsync(ReopenTaskRequest request, CancellationToken cancellationToken)
        {
            var task = await command.GetAsync(request.TenantId, request.TaskId, cancellationToken);
            if (task is null)
            {
                return Result.Failure(Error.NotFound("Exam task not found."));
            }

            task.Reopen(request.DueAt);
            await command.SaveAsync(cancellationToken);
            return Result.Success();
        }
    }

    public sealed record SavePaperRequest(
        Guid TenantId,
        Guid TaskId,
        string FileName,
        string ContentType,
        byte[] Data,
        string? Notes) : IRequest<Result>;

    public sealed class SavePaperHandler(IExamTaskCommand command, ICurrentUser currentUser)
        : IRequestHandler<SavePaperRequest, Result>
    {
        public async Task<Result> HandleAsync(SavePaperRequest request, CancellationToken cancellationToken)
        {
            var task = await command.GetAsync(request.TenantId, request.TaskId, cancellationToken);
            if (task is null || task.TeacherUserId != currentUser.UserId)
            {
                return Result.Failure(Error.NotFound("Assigned exam-paper task not found."));
            }

            if (task.TaskType != PaperTask)
            {
                return Result.Failure(Error.Validation("This task is not an exam-paper upload task."));
            }

            if (task.Status == "COMPLETED")
            {
                return Result.Failure(Error.Conflict("The examiner has already completed this task."));
            }

            if (DateTimeOffset.UtcNow > task.DueAt)
            {
                return Result.Failure(Error.Conflict("The task deadline has passed. Ask the examiner to extend it."));
            }

            task.SubmitPaper(request.FileName, request.ContentType, request.Data, request.Notes);
            await command.SaveAsync(cancellationToken);
            return Result.Success();
        }
    }

    public sealed record PaperDownload(string FileName, string ContentType, byte[] Data);

    public sealed record GetResultRosterQuery(Guid TenantId, Guid TaskId) : IRequest<Result<ResultRosterResponse>>;
    public sealed record ResultRosterRow(
        Guid StudentId,
        string StudentName,
        string StudentNumber,
        decimal? MarksObtained,
        bool IsAbsent,
        string? Remarks);
    public sealed record ResultRosterHeader(
        Guid TaskId,
        Guid ExamId,
        Guid ExamSubjectId,
        string Subject,
        decimal TotalMarks,
        decimal? PassingMarks,
        DateTimeOffset DueAt,
        string Status);

    public sealed record ResultRosterResponse(
        Guid TaskId,
        Guid ExamId,
        Guid ExamSubjectId,
        string Subject,
        decimal TotalMarks,
        decimal? PassingMarks,
        DateTimeOffset DueAt,
        string Status,
        IReadOnlyList<ResultRosterRow> Rows);

    public interface IResultTaskQuery
    {
        Task<ResultRosterResponse?> GetRosterAsync(Guid tenantId, Guid taskId, Guid currentUserId, bool examiner, CancellationToken cancellationToken);
    }

    internal sealed class ResultTaskQuery(IDbConnectionFactory connectionFactory) : IResultTaskQuery
    {
        public async Task<ResultRosterResponse?> GetRosterAsync(
            Guid tenantId,
            Guid taskId,
            Guid currentUserId,
            bool examiner,
            CancellationToken cancellationToken)
        {
            const string headerSql = """
                SELECT
                    task.exam_task_id AS "TaskId",
                    task.exam_id AS "ExamId",
                    task.exam_subject_id AS "ExamSubjectId",
                    subject.name AS "Subject",
                    subject.total_marks AS "TotalMarks",
                    subject.passing_marks AS "PassingMarks",
                    task.due_at AS "DueAt",
                    task.status AS "Status"
                FROM exam.exam_task task
                JOIN exam.exam_subject subject
                  ON subject.exam_subject_id = task.exam_subject_id
                 AND subject.tenant_id = task.tenant_id
                WHERE task.tenant_id = @TenantId
                  AND task.exam_task_id = @TaskId
                  AND task.task_type = 'RESULT_ENTRY'
                  AND task.is_active
                  AND (@Examiner OR task.teacher_user_id = @CurrentUserId);
                """;

            const string rowsSql = """
                SELECT
                    student.student_id AS "StudentId",
                    trim(student.first_name || ' ' || coalesce(student.last_name, '')) AS "StudentName",
                    student.student_number AS "StudentNumber",
                    result.marks_obtained AS "MarksObtained",
                    coalesce(result.is_absent, false) AS "IsAbsent",
                    result.remarks AS "Remarks"
                FROM exam.exam_task task
                JOIN exam.exam exam
                  ON exam.exam_id = task.exam_id
                 AND exam.tenant_id = task.tenant_id
                JOIN student.student_enrollment enrollment
                  ON enrollment.tenant_id = exam.tenant_id
                 AND enrollment.class_section_id = exam.class_section_id
                 AND enrollment.is_active
                 AND enrollment.status = 'ACTIVE'
                JOIN student.student student
                  ON student.student_id = enrollment.student_id
                 AND student.tenant_id = enrollment.tenant_id
                 AND student.is_active
                LEFT JOIN exam.student_exam_result result
                  ON result.tenant_id = task.tenant_id
                 AND result.exam_subject_id = task.exam_subject_id
                 AND result.student_id = student.student_id
                 AND result.is_active
                WHERE task.tenant_id = @TenantId
                  AND task.exam_task_id = @TaskId
                  AND task.task_type = 'RESULT_ENTRY'
                  AND task.is_active
                  AND (@Examiner OR task.teacher_user_id = @CurrentUserId)
                ORDER BY student.student_number, "StudentName";
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var parameters = new
            {
                TenantId = tenantId,
                TaskId = taskId,
                CurrentUserId = currentUserId,
                Examiner = examiner
            };

            var header = await connection.QuerySingleOrDefaultAsync<ResultRosterHeader>(new CommandDefinition(
                headerSql,
                parameters,
                cancellationToken: cancellationToken));
            if (header is null)
            {
                return null;
            }

            var rows = (await connection.QueryAsync<ResultRosterRow>(new CommandDefinition(
                rowsSql,
                parameters,
                cancellationToken: cancellationToken))).AsList();

            return new ResultRosterResponse(
                header.TaskId,
                header.ExamId,
                header.ExamSubjectId,
                header.Subject,
                header.TotalMarks,
                header.PassingMarks,
                header.DueAt,
                header.Status,
                rows);
        }
    }

    public sealed class GetResultRosterHandler(IResultTaskQuery query, ICurrentUser currentUser)
        : IRequestHandler<GetResultRosterQuery, Result<ResultRosterResponse>>
    {
        public async Task<Result<ResultRosterResponse>> HandleAsync(GetResultRosterQuery request, CancellationToken cancellationToken)
        {
            var response = await query.GetRosterAsync(
                request.TenantId,
                request.TaskId,
                currentUser.UserId,
                currentUser.IsInRole(SmartSchoolRoles.Examiner),
                cancellationToken);

            return response is null
                ? Result<ResultRosterResponse>.Failure(Error.NotFound("Assigned result-entry task not found."))
                : Result<ResultRosterResponse>.Success(response);
        }
    }

    public sealed record ResultEntry(Guid StudentId, decimal? MarksObtained, bool IsAbsent, string? Remarks);
    public sealed record SaveTaskResultsRequest(
        Guid TenantId,
        Guid TaskId,
        IReadOnlyList<ResultEntry> Rows,
        string? Notes) : IRequest<Result<SaveExamResults.Response>>;

    public sealed class SaveTaskResultsValidator : AbstractValidator<SaveTaskResultsRequest>
    {
        public SaveTaskResultsValidator()
        {
            RuleFor(request => request.Rows).NotEmpty();
            RuleForEach(request => request.Rows).ChildRules(row =>
            {
                row.RuleFor(item => item.StudentId).NotEmpty();
                row.RuleFor(item => item.MarksObtained).GreaterThanOrEqualTo(0);
                row.RuleFor(item => item).Must(item => item.IsAbsent || item.MarksObtained.HasValue)
                    .WithMessage("Enter marks or mark the student absent.");
                row.RuleFor(item => item.Remarks).MaximumLength(2000);
            });
            RuleFor(request => request.Notes).MaximumLength(4000);
        }
    }

    public sealed class SaveTaskResultsHandler(
        IExamTaskCommand command,
        ICurrentUser currentUser,
        IMediator mediator)
        : IRequestHandler<SaveTaskResultsRequest, Result<SaveExamResults.Response>>
    {
        public async Task<Result<SaveExamResults.Response>> HandleAsync(
            SaveTaskResultsRequest request,
            CancellationToken cancellationToken)
        {
            var task = await command.GetAsync(request.TenantId, request.TaskId, cancellationToken);
            if (task is null || task.TeacherUserId != currentUser.UserId)
            {
                return Result<SaveExamResults.Response>.Failure(Error.NotFound("Assigned result-entry task not found."));
            }

            if (task.TaskType != ResultTask)
            {
                return Result<SaveExamResults.Response>.Failure(Error.Validation("This task is not a result-entry task."));
            }

            if (task.Status == "COMPLETED")
            {
                return Result<SaveExamResults.Response>.Failure(Error.Conflict("The examiner has already completed this task."));
            }

            if (DateTimeOffset.UtcNow > task.DueAt)
            {
                return Result<SaveExamResults.Response>.Failure(Error.Conflict("The task deadline has passed. Ask the examiner to extend it."));
            }

            var saveRequest = new SaveExamResults.Request(
                request.TenantId,
                task.ExamId,
                request.Rows.Select(row => new SaveExamResults.Entry(
                    row.StudentId,
                    task.ExamSubjectId,
                    row.MarksObtained,
                    row.IsAbsent,
                    row.Remarks)).ToArray());

            var result = await mediator.SendAsync<SaveExamResults.Request, Result<SaveExamResults.Response>>(
                saveRequest,
                cancellationToken);
            if (result.IsFailure)
            {
                return result;
            }

            task.MarkResultsSubmitted(request.Notes);
            await command.SaveAsync(cancellationToken);
            return result;
        }
    }

    public static IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/examinations/exam/{examId:guid}/task-options", async (
                Guid examId,
                Guid? tenantId,
                ITenantScope tenantScope,
                IExamTaskReadQuery query,
                CancellationToken cancellationToken) =>
            {
                var resolvedTenantId = tenantScope.Resolve(tenantId);
                if (!resolvedTenantId.HasValue)
                {
                    return Results.BadRequest(new { message = "Select a tenant." });
                }

                return Results.Ok(new
                {
                    items = await query.GetOptionsAsync(resolvedTenantId.Value, examId, cancellationToken)
                });
            })
            .WithName("GetExamTaskOptions")
            .WithTags("Examinations")
            .RequireAuthorization(SmartSchoolPolicies.ExaminerOnly);

        endpoints.MapPost("/api/examinations/exam/{examId:guid}/tasks", async (
                Guid examId,
                AssignTaskRequest request,
                ITenantScope tenantScope,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var resolvedTenantId = tenantScope.Resolve(request.TenantId);
                if (!resolvedTenantId.HasValue)
                {
                    return Results.BadRequest(new { message = "Select a tenant." });
                }

                var result = await mediator.SendAsync<AssignTaskRequest, Result<AssignTaskResponse>>(
                    request with { TenantId = resolvedTenantId.Value, ExamId = examId },
                    cancellationToken);
                return result.ToHttpResult();
            })
            .WithName("AssignExamTask")
            .WithTags("Examinations")
            .RequireAuthorization(SmartSchoolPolicies.ExaminerOnly);

        endpoints.MapGet("/api/examinations/exam/{examId:guid}/tasks", async (
                Guid examId,
                Guid? tenantId,
                ITenantScope tenantScope,
                ICurrentUser currentUser,
                IExamTaskReadQuery query,
                CancellationToken cancellationToken) =>
            {
                var resolvedTenantId = tenantScope.Resolve(tenantId);
                if (!resolvedTenantId.HasValue)
                {
                    return Results.BadRequest(new { message = "Select a tenant." });
                }

                return Results.Ok(new
                {
                    items = await query.GetExamTasksAsync(
                        resolvedTenantId.Value,
                        examId,
                        currentUser.BranchId,
                        cancellationToken)
                });
            })
            .WithName("GetExamTasks")
            .WithTags("Examinations")
            .RequireAuthorization(SmartSchoolPolicies.ExaminerOnly);

        endpoints.MapGet("/api/examinations/tasks/my", async (
                Guid? tenantId,
                ITenantScope tenantScope,
                ICurrentUser currentUser,
                IExamTaskReadQuery query,
                CancellationToken cancellationToken) =>
            {
                var resolvedTenantId = tenantScope.Resolve(tenantId);
                if (!resolvedTenantId.HasValue)
                {
                    return Results.BadRequest(new { message = "Select a tenant." });
                }

                return Results.Ok(new
                {
                    items = await query.GetTeacherTasksAsync(
                        resolvedTenantId.Value,
                        currentUser.UserId,
                        cancellationToken)
                });
            })
            .WithName("GetMyExamTasks")
            .WithTags("Examinations")
            .RequireAuthorization(SmartSchoolPolicies.TeacherOnly);

        endpoints.MapPut("/api/examinations/tasks/{taskId:guid}", async (
                Guid taskId,
                UpdateTaskRequest request,
                ITenantScope tenantScope,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var resolvedTenantId = tenantScope.Resolve(request.TenantId);
                if (!resolvedTenantId.HasValue)
                {
                    return Results.BadRequest(new { message = "Select a tenant." });
                }

                var result = await mediator.SendAsync<UpdateTaskRequest, Result>(
                    request with { TenantId = resolvedTenantId.Value, TaskId = taskId },
                    cancellationToken);
                return result.ToHttpResult();
            })
            .WithName("UpdateExamTask")
            .WithTags("Examinations")
            .RequireAuthorization(SmartSchoolPolicies.ExaminerOnly);

        endpoints.MapPost("/api/examinations/tasks/{taskId:guid}/complete", async (
                Guid taskId,
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

                var result = await mediator.SendAsync<CompleteTaskRequest, Result>(
                    new CompleteTaskRequest(resolvedTenantId.Value, taskId),
                    cancellationToken);
                return result.ToHttpResult();
            })
            .WithName("CompleteExamTask")
            .WithTags("Examinations")
            .RequireAuthorization(SmartSchoolPolicies.ExaminerOnly);

        endpoints.MapPost("/api/examinations/tasks/{taskId:guid}/reopen", async (
                Guid taskId,
                ReopenTaskRequest request,
                ITenantScope tenantScope,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var resolvedTenantId = tenantScope.Resolve(request.TenantId);
                if (!resolvedTenantId.HasValue)
                {
                    return Results.BadRequest(new { message = "Select a tenant." });
                }

                var result = await mediator.SendAsync<ReopenTaskRequest, Result>(
                    request with { TenantId = resolvedTenantId.Value, TaskId = taskId },
                    cancellationToken);
                return result.ToHttpResult();
            })
            .WithName("ReopenExamTask")
            .WithTags("Examinations")
            .RequireAuthorization(SmartSchoolPolicies.ExaminerOnly);

        endpoints.MapPost("/api/examinations/tasks/{taskId:guid}/paper", async (
                Guid taskId,
                Guid? tenantId,
                HttpRequest httpRequest,
                ITenantScope tenantScope,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var resolvedTenantId = tenantScope.Resolve(tenantId);
                if (!resolvedTenantId.HasValue)
                {
                    return Results.BadRequest(new { message = "Select a tenant." });
                }

                if (!httpRequest.HasFormContentType)
                {
                    return Results.BadRequest(new { message = "Upload the exam paper using multipart/form-data." });
                }

                var form = await httpRequest.ReadFormAsync(cancellationToken);
                var file = form.Files.GetFile("file");
                if (file is null || file.Length == 0)
                {
                    return Results.BadRequest(new { message = "Select an exam-paper file." });
                }

                const long maxBytes = 15 * 1024 * 1024;
                if (file.Length > maxBytes)
                {
                    return Results.BadRequest(new { message = "Exam paper must be 15 MB or smaller." });
                }

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (extension is not ".pdf" and not ".doc" and not ".docx")
                {
                    return Results.BadRequest(new { message = "Exam paper must be PDF, DOC or DOCX." });
                }

                await using var stream = new MemoryStream();
                await file.CopyToAsync(stream, cancellationToken);
                var request = new SavePaperRequest(
                    resolvedTenantId.Value,
                    taskId,
                    Path.GetFileName(file.FileName),
                    string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
                    stream.ToArray(),
                    form["notes"].FirstOrDefault());

                return (await mediator.SendAsync<SavePaperRequest, Result>(request, cancellationToken)).ToHttpResult();
            })
            .DisableAntiforgery()
            .WithName("UploadAssignedExamPaper")
            .WithTags("Examinations")
            .RequireAuthorization(SmartSchoolPolicies.TeacherOnly);

        endpoints.MapGet("/api/examinations/tasks/{taskId:guid}/paper", async (
                Guid taskId,
                Guid? tenantId,
                ITenantScope tenantScope,
                ICurrentUser currentUser,
                IExamTaskCommand command,
                CancellationToken cancellationToken) =>
            {
                var resolvedTenantId = tenantScope.Resolve(tenantId);
                if (!resolvedTenantId.HasValue)
                {
                    return Results.BadRequest(new { message = "Select a tenant." });
                }

                var task = await command.GetAsync(resolvedTenantId.Value, taskId, cancellationToken);
                if (task is null ||
                    (!currentUser.IsInRole(SmartSchoolRoles.Examiner) && task.TeacherUserId != currentUser.UserId))
                {
                    return Results.NotFound();
                }

                if (task.SubmissionFileData is null || string.IsNullOrWhiteSpace(task.SubmissionFileName))
                {
                    return Results.NotFound();
                }

                return Results.File(
                    task.SubmissionFileData,
                    task.SubmissionContentType ?? "application/octet-stream",
                    task.SubmissionFileName);
            })
            .WithName("DownloadAssignedExamPaper")
            .WithTags("Examinations")
            .RequireAuthorization(SmartSchoolPolicies.ExamTaskAccess);

        endpoints.MapGet("/api/examinations/tasks/{taskId:guid}/results", async (
                Guid taskId,
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

                return (await mediator.SendAsync<GetResultRosterQuery, Result<ResultRosterResponse>>(
                    new GetResultRosterQuery(resolvedTenantId.Value, taskId),
                    cancellationToken)).ToHttpResult();
            })
            .WithName("GetAssignedExamResultRoster")
            .WithTags("Examinations")
            .RequireAuthorization(SmartSchoolPolicies.ExamTaskAccess);

        endpoints.MapPut("/api/examinations/tasks/{taskId:guid}/results", async (
                Guid taskId,
                SaveTaskResultsRequest request,
                ITenantScope tenantScope,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var resolvedTenantId = tenantScope.Resolve(request.TenantId);
                if (!resolvedTenantId.HasValue)
                {
                    return Results.BadRequest(new { message = "Select a tenant." });
                }

                var result = await mediator.SendAsync<SaveTaskResultsRequest, Result<SaveExamResults.Response>>(
                    request with { TenantId = resolvedTenantId.Value, TaskId = taskId },
                    cancellationToken);
                return result.ToHttpResult();
            })
            .WithName("SaveAssignedExamResults")
            .WithTags("Examinations")
            .RequireAuthorization(SmartSchoolPolicies.TeacherOnly);

        return endpoints;
    }
}
