using Microsoft.Extensions.Logging;
using SmartSchool.Application.Persistence;
using SmartSchool.BackgroundJobs.Abstractions;

namespace SmartSchool.BackgroundJobs.Jobs;

/// <summary>
/// Creates idempotent in-app notifications for examiner-assigned teacher exam tasks.
/// </summary>
public sealed class ExamNotificationJob(
    IDbConnectionFactory connectionFactory,
    ILogger<ExamNotificationJob> logger) : IWorkflowJob
{
    /// <inheritdoc />
    public async Task ExecuteAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        const string sql = """
            WITH candidates AS (
                SELECT
                    task.exam_task_id,
                    task.tenant_id,
                    task.teacher_user_id,
                    task.title,
                    task.due_at,
                    subject.name AS subject_name,
                    exam.name AS exam_name,
                    CASE
                        WHEN task.assignment_notified_at IS NULL THEN 'ASSIGNMENT'
                        WHEN task.due_at <= now() AND task.overdue_reminder_sent_at IS NULL THEN 'OVERDUE'
                        WHEN task.due_at > now()
                             AND task.due_at <= now() + interval '2 hours'
                             AND task.reminder_2_hours_sent_at IS NULL THEN 'DUE_2H'
                        WHEN task.due_at > now() + interval '2 hours'
                             AND task.due_at <= now() + interval '24 hours'
                             AND task.reminder_24_hours_sent_at IS NULL THEN 'DUE_24H'
                        ELSE NULL
                    END AS reminder_kind
                FROM exam.exam_task task
                JOIN exam.exam exam
                  ON exam.exam_id = task.exam_id
                 AND exam.tenant_id = task.tenant_id
                JOIN exam.exam_subject subject
                  ON subject.exam_subject_id = task.exam_subject_id
                 AND subject.tenant_id = task.tenant_id
                WHERE task.is_active
                  AND task.status IN ('ASSIGNED','IN_PROGRESS','OVERDUE')
                  AND (@TenantId = '00000000-0000-0000-0000-000000000000'::uuid OR task.tenant_id = @TenantId)
            ),
            inserted AS (
                INSERT INTO communication.notification (
                    notification_id,
                    tenant_id,
                    recipient_user_id,
                    type,
                    title,
                    message,
                    related_entity_id,
                    related_entity_type,
                    action_url,
                    priority,
                    is_read,
                    occurred_at,
                    is_active,
                    created_at,
                    row_version)
                SELECT
                    gen_random_uuid(),
                    candidate.tenant_id,
                    candidate.teacher_user_id,
                    'General',
                    CASE candidate.reminder_kind
                        WHEN 'ASSIGNMENT' THEN 'New exam task assigned'
                        WHEN 'DUE_24H' THEN 'Exam task due within 24 hours'
                        WHEN 'DUE_2H' THEN 'Exam task due within 2 hours'
                        WHEN 'OVERDUE' THEN 'Exam task overdue'
                    END,
                    CASE candidate.reminder_kind
                        WHEN 'ASSIGNMENT' THEN candidate.exam_name || ' · ' || candidate.subject_name || ': ' || candidate.title || '. Due ' || to_char(candidate.due_at AT TIME ZONE 'UTC', 'YYYY-MM-DD HH24:MI') || ' UTC.'
                        WHEN 'DUE_24H' THEN candidate.exam_name || ' · ' || candidate.subject_name || ': ' || candidate.title || ' is due within 24 hours.'
                        WHEN 'DUE_2H' THEN candidate.exam_name || ' · ' || candidate.subject_name || ': ' || candidate.title || ' is due within 2 hours.'
                        WHEN 'OVERDUE' THEN candidate.exam_name || ' · ' || candidate.subject_name || ': ' || candidate.title || ' is overdue. Contact the examiner if the deadline must be extended.'
                    END,
                    candidate.exam_task_id,
                    'ExamTask',
                    '/examinations?task=' || candidate.exam_task_id::text,
                    CASE candidate.reminder_kind WHEN 'OVERDUE' THEN 'Urgent' WHEN 'DUE_2H' THEN 'High' WHEN 'DUE_24H' THEN 'High' ELSE 'Normal' END,
                    false,
                    now(),
                    true,
                    now(),
                    public.gen_random_bytes(8)
                FROM candidates candidate
                WHERE candidate.reminder_kind IS NOT NULL
                RETURNING related_entity_id
            )
            UPDATE exam.exam_task task
            SET assignment_notified_at = CASE WHEN candidate.reminder_kind = 'ASSIGNMENT' THEN now() ELSE task.assignment_notified_at END,
                reminder_24_hours_sent_at = CASE WHEN candidate.reminder_kind = 'DUE_24H' THEN now() ELSE task.reminder_24_hours_sent_at END,
                reminder_2_hours_sent_at = CASE WHEN candidate.reminder_kind = 'DUE_2H' THEN now() ELSE task.reminder_2_hours_sent_at END,
                overdue_reminder_sent_at = CASE WHEN candidate.reminder_kind = 'OVERDUE' THEN now() ELSE task.overdue_reminder_sent_at END,
                status = CASE WHEN candidate.reminder_kind = 'OVERDUE' THEN 'OVERDUE' ELSE task.status END,
                updated_at = now()
            FROM candidates candidate
            JOIN inserted notification ON notification.related_entity_id = candidate.exam_task_id
            WHERE task.exam_task_id = candidate.exam_task_id;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = sql;

        var parameter = command.CreateParameter();
        parameter.ParameterName = "TenantId";
        parameter.Value = tenantId;
        command.Parameters.Add(parameter);

        var affected = await command.ExecuteNonQueryAsync(cancellationToken);
        logger.LogInformation(
            "Exam-task reminder workflow completed for tenant {TenantId}; affected rows {AffectedRows}.",
            tenantId,
            affected);
    }
}
