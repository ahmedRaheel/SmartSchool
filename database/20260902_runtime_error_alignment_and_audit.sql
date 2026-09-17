BEGIN;

-- Tables referenced by feature slices but absent from the consolidated schema.
CREATE TABLE IF NOT EXISTS inventory.purchase_order (
    purchase_order_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL,
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    metadata_json jsonb,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT public.gen_random_bytes(8)
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_purchase_order_tenant_code ON inventory.purchase_order(tenant_id, code);

CREATE TABLE IF NOT EXISTS finance.fee_structure (
    fee_structure_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL,
    grade_level_id uuid,
    fee_type_id uuid,
    academic_year_id uuid,
    amount numeric(18,2) NOT NULL DEFAULT 0,
    frequency varchar(50) NOT NULL DEFAULT 'Monthly',
    effective_from date,
    effective_to date,
    code varchar(100) NOT NULL DEFAULT '',
    name varchar(250) NOT NULL DEFAULT '',
    metadata_json jsonb,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT public.gen_random_bytes(8)
);
CREATE INDEX IF NOT EXISTS ix_fee_structure_tenant ON finance.fee_structure(tenant_id);

CREATE TABLE IF NOT EXISTS exam.grade_scale (
    grade_scale_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL,
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    metadata_json jsonb,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT public.gen_random_bytes(8)
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_grade_scale_tenant_code ON exam.grade_scale(tenant_id, code);

CREATE TABLE IF NOT EXISTS lms.lesson (
    lesson_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL,
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    metadata_json jsonb,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT public.gen_random_bytes(8)
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_lesson_tenant_code ON lms.lesson(tenant_id, code);

-- SaaS isolation: child/activity tables also carry tenant_id directly.
ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS tenant_id uuid;
UPDATE library.book_loan bl
SET tenant_id = s.tenant_id
FROM student.student s
WHERE s.student_id = bl.student_id AND bl.tenant_id IS NULL;
UPDATE library.book_loan bl
SET tenant_id = e.tenant_id
FROM hr.employee e
WHERE e.employee_id = bl.employee_id AND bl.tenant_id IS NULL;
ALTER TABLE library.book_loan ALTER COLUMN tenant_id SET NOT NULL;
ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS code varchar(100);
ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS name varchar(250);
ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS is_active boolean NOT NULL DEFAULT true;
UPDATE library.book_loan SET code = COALESCE(code, book_loan_id::text), name = COALESCE(name, 'Book Loan');
CREATE INDEX IF NOT EXISTS ix_book_loan_tenant ON library.book_loan(tenant_id, book_loan_id);

ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS tenant_id uuid;
UPDATE exam.student_exam_result r
SET tenant_id = s.tenant_id
FROM student.student s
WHERE s.student_id = r.student_id AND r.tenant_id IS NULL;
ALTER TABLE exam.student_exam_result ALTER COLUMN tenant_id SET NOT NULL;
ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS code varchar(100);
ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS name varchar(250);
ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS is_active boolean NOT NULL DEFAULT true;
UPDATE exam.student_exam_result SET code = COALESCE(code, student_exam_result_id::text), name = COALESCE(name, 'Student Exam Result');
CREATE INDEX IF NOT EXISTS ix_student_exam_result_tenant ON exam.student_exam_result(tenant_id, student_exam_result_id);

ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS code varchar(100);
ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS name varchar(250);
ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS is_active boolean NOT NULL DEFAULT true;
UPDATE teacher.leave_request SET code = COALESCE(code, leave_request_id::text), name = COALESCE(name, leave_type);
CREATE INDEX IF NOT EXISTS ix_leave_request_tenant ON teacher.leave_request(tenant_id, leave_request_id);

ALTER TABLE org.department ADD COLUMN IF NOT EXISTS telephone varchar(50);
ALTER TABLE org.department ADD COLUMN IF NOT EXISTS email varchar(320);
ALTER TABLE org.department ADD COLUMN IF NOT EXISTS head_of_department_employee_id uuid;
ALTER TABLE org.department ADD COLUMN IF NOT EXISTS metadata_json jsonb;

-- Audit read model + automatic EF write auditing support.
ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS code varchar(200);
ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS name varchar(300);
ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS is_active boolean NOT NULL DEFAULT true;
ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS created_at timestamptz NOT NULL DEFAULT now();
ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS updated_at timestamptz;
ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS row_version bytea NOT NULL DEFAULT decode('', 'hex');
UPDATE audit.audit_log
SET code = COALESCE(code, entity_type || '.' || action),
    name = COALESCE(name, action || ' ' || entity_type),
    metadata_json = COALESCE(metadata_json, new_values);
CREATE INDEX IF NOT EXISTS ix_audit_log_tenant_occurred ON audit.audit_log(tenant_id, occurred_at DESC);

COMMIT;
