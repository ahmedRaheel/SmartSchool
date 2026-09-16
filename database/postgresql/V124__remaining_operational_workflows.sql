-- SmartSchool V124 - remaining operational workflows
-- Idempotent upgrade for attendance, library circulation, finance, payroll,
-- timetable authoring, certificates and 384-dimensional Ollama RAG.

BEGIN;

CREATE EXTENSION IF NOT EXISTS pgcrypto;
CREATE EXTENSION IF NOT EXISTS vector;

-- -----------------------------------------------------------------------------
-- Student attendance: extend the generated master-data shell into operational
-- per-student/per-class/per-day attendance while retaining old rows safely.
-- -----------------------------------------------------------------------------
ALTER TABLE student.attendance ADD COLUMN IF NOT EXISTS student_id uuid;
ALTER TABLE student.attendance ADD COLUMN IF NOT EXISTS class_section_id uuid;
ALTER TABLE student.attendance ADD COLUMN IF NOT EXISTS attendance_date date;
ALTER TABLE student.attendance ADD COLUMN IF NOT EXISTS attendance_status character varying(30);
ALTER TABLE student.attendance ADD COLUMN IF NOT EXISTS remarks character varying(1000);
ALTER TABLE student.attendance ADD COLUMN IF NOT EXISTS marked_by uuid;

CREATE UNIQUE INDEX IF NOT EXISTS ux_student_attendance_daily
    ON student.attendance(tenant_id, student_id, class_section_id, attendance_date);
CREATE INDEX IF NOT EXISTS ix_student_attendance_class_date
    ON student.attendance(tenant_id, class_section_id, attendance_date)
    WHERE is_active = true;
CREATE INDEX IF NOT EXISTS ix_student_attendance_student_date
    ON student.attendance(tenant_id, student_id, attendance_date)
    WHERE is_active = true;

-- -----------------------------------------------------------------------------
-- Library: align the canonical catalogue/copy/loan tables with the Entity base
-- fields already expected by the module and backfill pre-existing data.
-- -----------------------------------------------------------------------------
ALTER TABLE library.book ADD COLUMN IF NOT EXISTS code character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE library.book ADD COLUMN IF NOT EXISTS name character varying(250) DEFAULT '' NOT NULL;
ALTER TABLE library.book ADD COLUMN IF NOT EXISTS metadata_json jsonb;
UPDATE library.book
SET code = 'BOOK-' || left(replace(book_id::text, '-', ''), 13)
WHERE code = '';
UPDATE library.book SET name = title WHERE name = '';
CREATE UNIQUE INDEX IF NOT EXISTS ux_library_book_tenant_code
    ON library.book(tenant_id, code);

ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS tenant_id uuid;
ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS code character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS name character varying(250) DEFAULT '' NOT NULL;
ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT true NOT NULL;
ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now() NOT NULL;
ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;
ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8) NOT NULL;
UPDATE library.book_copy copy
SET tenant_id = book.tenant_id,
    code = CASE WHEN copy.code = '' THEN copy.barcode ELSE copy.code END,
    name = CASE WHEN copy.name = '' THEN book.title ELSE copy.name END
FROM library.book book
WHERE book.book_id = copy.book_id
  AND (copy.tenant_id IS NULL OR copy.code = '' OR copy.name = '');
ALTER TABLE library.book_copy ALTER COLUMN tenant_id SET NOT NULL;
CREATE UNIQUE INDEX IF NOT EXISTS ux_library_book_copy_tenant_code
    ON library.book_copy(tenant_id, code);
CREATE INDEX IF NOT EXISTS ix_library_book_copy_availability
    ON library.book_copy(tenant_id, campus_id, status)
    WHERE is_active = true;

ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS tenant_id uuid;
ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS code character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS name character varying(250) DEFAULT '' NOT NULL;
ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT true NOT NULL;
ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now() NOT NULL;
ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;
ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8) NOT NULL;
UPDATE library.book_loan loan
SET tenant_id = book.tenant_id,
    code = CASE WHEN loan.code = '' THEN 'LOAN-' || left(replace(loan.book_loan_id::text, '-', ''), 13) ELSE loan.code END,
    name = CASE WHEN loan.name = '' THEN 'Library Loan' ELSE loan.name END,
    created_at = coalesce(loan.created_at, loan.issued_at)
FROM library.book_copy copy
JOIN library.book book ON book.book_id = copy.book_id
WHERE copy.book_copy_id = loan.book_copy_id
  AND (loan.tenant_id IS NULL OR loan.code = '' OR loan.name = '');
ALTER TABLE library.book_loan ALTER COLUMN tenant_id SET NOT NULL;
CREATE UNIQUE INDEX IF NOT EXISTS ux_library_book_loan_tenant_code
    ON library.book_loan(tenant_id, code);
CREATE INDEX IF NOT EXISTS ix_library_book_loan_open
    ON library.book_loan(tenant_id, due_at)
    WHERE is_active = true AND returned_at IS NULL;

-- -----------------------------------------------------------------------------
-- Finance: add the Entity base business identifiers used by operational invoice
-- and immutable posted-payment workflows. Existing accounting columns remain the
-- source of truth.
-- -----------------------------------------------------------------------------
ALTER TABLE finance.student_invoice ADD COLUMN IF NOT EXISTS code character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE finance.student_invoice ADD COLUMN IF NOT EXISTS name character varying(250) DEFAULT '' NOT NULL;
ALTER TABLE finance.student_invoice ADD COLUMN IF NOT EXISTS metadata_json jsonb;
UPDATE finance.student_invoice
SET code = invoice_number
WHERE code = '';
UPDATE finance.student_invoice
SET name = 'Invoice ' || invoice_number
WHERE name = '';
CREATE UNIQUE INDEX IF NOT EXISTS ux_finance_invoice_tenant_code
    ON finance.student_invoice(tenant_id, code);
CREATE INDEX IF NOT EXISTS ix_finance_invoice_student_balance
    ON finance.student_invoice(tenant_id, student_id, balance_amount)
    WHERE is_active = true;

ALTER TABLE finance.student_payment ADD COLUMN IF NOT EXISTS code character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE finance.student_payment ADD COLUMN IF NOT EXISTS name character varying(250) DEFAULT '' NOT NULL;
ALTER TABLE finance.student_payment ADD COLUMN IF NOT EXISTS metadata_json jsonb;
UPDATE finance.student_payment
SET code = payment_number
WHERE code = '';
UPDATE finance.student_payment
SET name = 'Payment ' || payment_number
WHERE name = '';
CREATE UNIQUE INDEX IF NOT EXISTS ux_finance_payment_tenant_code
    ON finance.student_payment(tenant_id, code);

-- -----------------------------------------------------------------------------
-- Payroll: effective-dated compensation and canonical payroll runs/snapshots.
-- -----------------------------------------------------------------------------
ALTER TABLE hr.employee_compensation ADD COLUMN IF NOT EXISTS code character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE hr.employee_compensation ADD COLUMN IF NOT EXISTS name character varying(250) DEFAULT '' NOT NULL;
ALTER TABLE hr.employee_compensation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
UPDATE hr.employee_compensation
SET code = 'COMP-' || left(replace(employee_compensation_id::text, '-', ''), 13)
WHERE code = '';
UPDATE hr.employee_compensation
SET name = 'Employee compensation'
WHERE name = '';
CREATE UNIQUE INDEX IF NOT EXISTS ux_employee_compensation_tenant_code
    ON hr.employee_compensation(tenant_id, code);
CREATE INDEX IF NOT EXISTS ix_employee_compensation_effective
    ON hr.employee_compensation(tenant_id, employee_id, effective_from DESC)
    WHERE is_active = true;

ALTER TABLE payroll.payroll_run ADD COLUMN IF NOT EXISTS code character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE payroll.payroll_run ADD COLUMN IF NOT EXISTS name character varying(250) DEFAULT '' NOT NULL;
ALTER TABLE payroll.payroll_run ADD COLUMN IF NOT EXISTS metadata_json jsonb;
UPDATE payroll.payroll_run run
SET code = 'PAYRUN-' || left(replace(run.payroll_run_id::text, '-', ''), 11),
    name = 'Payroll ' || period.year::text || '-' || lpad(period.month::text, 2, '0')
FROM payroll.payroll_period period
WHERE period.payroll_period_id = run.payroll_period_id
  AND (run.code = '' OR run.name = '');
CREATE UNIQUE INDEX IF NOT EXISTS ux_payroll_run_tenant_code
    ON payroll.payroll_run(tenant_id, code);

-- -----------------------------------------------------------------------------
-- Timetable: V123 supplied the base timetable-entry fields. Complete metadata and
-- unique business identifiers expected by the existing configurations.
-- -----------------------------------------------------------------------------
ALTER TABLE academic.timetable ADD COLUMN IF NOT EXISTS metadata_json jsonb;
UPDATE academic.timetable
SET code = 'TIM-' || left(replace(timetable_id::text, '-', ''), 13)
WHERE code = '';
CREATE UNIQUE INDEX IF NOT EXISTS ux_academic_timetable_tenant_code
    ON academic.timetable(tenant_id, code);

ALTER TABLE academic.timetable_entry ADD COLUMN IF NOT EXISTS metadata_json jsonb;
UPDATE academic.timetable_entry
SET code = 'TENT-' || left(replace(timetable_entry_id::text, '-', ''), 12)
WHERE code = '';
UPDATE academic.timetable_entry
SET name = 'Timetable entry'
WHERE name = '';
CREATE UNIQUE INDEX IF NOT EXISTS ux_academic_timetable_entry_tenant_code
    ON academic.timetable_entry(tenant_id, code);
CREATE INDEX IF NOT EXISTS ix_timetable_entry_collision
    ON academic.timetable_entry(tenant_id, timetable_id, day_of_week, timetable_period_id)
    WHERE is_active = true;

-- Certificate tables are canonical already; reinforce the read paths used by
-- issuance/verification without changing their existing unique constraints.
CREATE INDEX IF NOT EXISTS ix_document_template_scope
    ON document.document_template(tenant_id, campus_id, document_type_code, is_active);
CREATE INDEX IF NOT EXISTS ix_generated_document_owner
    ON document.generated_document(tenant_id, student_id, employee_id, created_at DESC)
    WHERE is_active = true;

-- -----------------------------------------------------------------------------
-- AICore RAG: preserve any historical 768-dimensional embedding in the legacy
-- column while switching active all-minilm retrieval/writes to 384 dimensions.
-- -----------------------------------------------------------------------------
ALTER TABLE ai_core.rag_knowledge_chunk ALTER COLUMN embedding DROP NOT NULL;
ALTER TABLE ai_core.rag_knowledge_chunk ADD COLUMN IF NOT EXISTS embedding_v384 vector(384);
CREATE INDEX IF NOT EXISTS ix_rag_knowledge_chunk_active_384
    ON ai_core.rag_knowledge_chunk(tenant_id, collection, is_active)
    WHERE embedding_v384 IS NOT NULL;

COMMIT;
