-- SmartSchool V125
-- Examiner-owned exam creation/task workflow, teacher subject tasks and notification reminders.

CREATE TABLE IF NOT EXISTS exam.exam_task (
    exam_task_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    exam_id uuid NOT NULL,
    exam_subject_id uuid NOT NULL,
    course_offering_id uuid NOT NULL,
    teacher_course_assignment_id uuid NOT NULL,
    teacher_employee_id uuid NOT NULL,
    teacher_user_id uuid NOT NULL,
    task_type character varying(30) NOT NULL,
    title character varying(250) NOT NULL,
    instructions character varying(4000),
    assigned_by_user_id uuid NOT NULL,
    assigned_at timestamp with time zone DEFAULT now() NOT NULL,
    due_at timestamp with time zone NOT NULL,
    status character varying(30) DEFAULT 'ASSIGNED' NOT NULL,
    submitted_at timestamp with time zone,
    completed_at timestamp with time zone,
    completed_by_user_id uuid,
    submission_notes character varying(4000),
    submission_file_name character varying(255),
    submission_content_type character varying(150),
    submission_file_data bytea,
    assignment_notified_at timestamp with time zone,
    reminder_24_hours_sent_at timestamp with time zone,
    reminder_2_hours_sent_at timestamp with time zone,
    overdue_reminder_sent_at timestamp with time zone,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL,
    CONSTRAINT exam_task_pkey PRIMARY KEY (exam_task_id),
    CONSTRAINT ck_exam_task_type CHECK (task_type IN ('EXAM_PAPER','RESULT_ENTRY')),
    CONSTRAINT ck_exam_task_status CHECK (status IN ('ASSIGNED','IN_PROGRESS','SUBMITTED','COMPLETED','OVERDUE')),
    CONSTRAINT ck_exam_task_due CHECK (due_at >= assigned_at)
);

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_exam_task_exam') THEN
        ALTER TABLE exam.exam_task
    ADD CONSTRAINT fk_exam_task_exam
    FOREIGN KEY (exam_id) REFERENCES exam.exam(exam_id) ON DELETE RESTRICT;
    END IF;
END $$;
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_exam_task_subject') THEN
        ALTER TABLE exam.exam_task
    ADD CONSTRAINT fk_exam_task_subject
    FOREIGN KEY (exam_subject_id) REFERENCES exam.exam_subject(exam_subject_id) ON DELETE RESTRICT;
    END IF;
END $$;
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_exam_task_course') THEN
        ALTER TABLE exam.exam_task
    ADD CONSTRAINT fk_exam_task_course
    FOREIGN KEY (course_offering_id) REFERENCES academic.course_offering(course_offering_id) ON DELETE RESTRICT;
    END IF;
END $$;
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_exam_task_teacher_assignment') THEN
        ALTER TABLE exam.exam_task
    ADD CONSTRAINT fk_exam_task_teacher_assignment
    FOREIGN KEY (teacher_course_assignment_id) REFERENCES academic.teacher_course_assignment(teacher_course_assignment_id) ON DELETE RESTRICT;
    END IF;
END $$;
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_exam_task_teacher_employee') THEN
        ALTER TABLE exam.exam_task
    ADD CONSTRAINT fk_exam_task_teacher_employee
    FOREIGN KEY (teacher_employee_id) REFERENCES hr.employee(employee_id) ON DELETE RESTRICT;
    END IF;
END $$;

CREATE UNIQUE INDEX IF NOT EXISTS ux_exam_task_active_teacher_subject_type
    ON exam.exam_task(tenant_id, exam_subject_id, teacher_employee_id, task_type, is_active);
CREATE INDEX IF NOT EXISTS ix_exam_task_teacher_status
    ON exam.exam_task(tenant_id, teacher_user_id, status);
CREATE INDEX IF NOT EXISTS ix_exam_task_exam_subject
    ON exam.exam_task(tenant_id, exam_id, exam_subject_id);
CREATE INDEX IF NOT EXISTS ix_exam_task_due
    ON exam.exam_task(tenant_id, due_at, status)
    WHERE is_active;

-- Reconcile the canonical notification table with the application notification model.
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS recipient_user_id uuid;
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS type character varying(80);
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS message character varying(2000);
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS related_entity_id uuid;
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS related_entity_type character varying(100);
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS action_url character varying(500);
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS priority character varying(50);
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS is_read boolean DEFAULT false;
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS read_at timestamp with time zone;
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS occurred_at timestamp with time zone;

UPDATE communication.notification
SET recipient_user_id = COALESCE(recipient_user_id, user_id),
    type = COALESCE(type, 'General'),
    message = COALESCE(message, left(body, 2000), left(title, 2000)),
    priority = COALESCE(priority, 'Normal'),
    is_read = COALESCE(is_read, false),
    occurred_at = COALESCE(occurred_at, created_at, now())
WHERE recipient_user_id IS NULL
   OR type IS NULL
   OR message IS NULL
   OR priority IS NULL
   OR occurred_at IS NULL;

ALTER TABLE communication.notification ALTER COLUMN user_id DROP NOT NULL;
ALTER TABLE communication.notification ALTER COLUMN channel_code DROP NOT NULL;
ALTER TABLE communication.notification ALTER COLUMN recipient_user_id SET NOT NULL;
ALTER TABLE communication.notification ALTER COLUMN type SET NOT NULL;
ALTER TABLE communication.notification ALTER COLUMN message SET NOT NULL;
ALTER TABLE communication.notification ALTER COLUMN priority SET DEFAULT 'Normal';
ALTER TABLE communication.notification ALTER COLUMN priority SET NOT NULL;
ALTER TABLE communication.notification ALTER COLUMN is_read SET DEFAULT false;
ALTER TABLE communication.notification ALTER COLUMN is_read SET NOT NULL;
ALTER TABLE communication.notification ALTER COLUMN occurred_at SET DEFAULT now();
ALTER TABLE communication.notification ALTER COLUMN occurred_at SET NOT NULL;

CREATE INDEX IF NOT EXISTS ix_notification_recipient_unread
    ON communication.notification(tenant_id, recipient_user_id, is_read, occurred_at DESC);
CREATE INDEX IF NOT EXISTS ix_notification_related_entity
    ON communication.notification(tenant_id, related_entity_type, related_entity_id);
