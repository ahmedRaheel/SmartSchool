-- SmartSchool campus-scope schema synchronization.
-- Canonical campus column remains branch_id in PostgreSQL; C# exposes BranchId/CampusId according to module contract.

CREATE SCHEMA IF NOT EXISTS hr;
CREATE SCHEMA IF NOT EXISTS finance;

-- Candidate is an HR resource and must exist in the canonical database.
CREATE TABLE IF NOT EXISTS hr.candidate (
    candidate_id uuid PRIMARY KEY,
    tenant_id uuid NOT NULL,
    branch_id uuid,
    first_name text NOT NULL DEFAULT '',
    last_name text,
    email text,
    phone text,
    current_job_title text,
    current_employer text,
    total_experience_years numeric,
    highest_qualification text,
    expected_salary numeric,
    notice_period_days integer,
    status_code text NOT NULL DEFAULT 'Active',
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    metadata_json jsonb,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8)
);

-- Leave belongs to HR, not the teacher schema.
DO $$
BEGIN
    IF to_regclass('teacher.leave_request') IS NOT NULL
       AND to_regclass('hr.leave_request') IS NULL THEN
        ALTER TABLE teacher.leave_request SET SCHEMA hr;
    END IF;
END $$;

CREATE TABLE IF NOT EXISTS hr.leave_request (
    leave_request_id uuid PRIMARY KEY,
    tenant_id uuid NOT NULL,
    branch_id uuid,
    employee_id uuid NOT NULL,
    leave_type text NOT NULL,
    from_date date NOT NULL,
    to_date date NOT NULL,
    reason text NOT NULL,
    status text NOT NULL,
    approved_by uuid,
    decision_at timestamptz,
    decision_note text,
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    metadata_json jsonb,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8)
);

CREATE TABLE IF NOT EXISTS finance.discount (
    discount_id uuid PRIMARY KEY,
    tenant_id uuid NOT NULL,
    branch_id uuid,
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    metadata_json jsonb,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8)
);

CREATE TABLE IF NOT EXISTS finance.scholarship (
    scholarship_id uuid PRIMARY KEY,
    tenant_id uuid NOT NULL,
    branch_id uuid,
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    metadata_json jsonb,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8)
);

CREATE TABLE IF NOT EXISTS finance.studentfee (
    student_fee_id uuid PRIMARY KEY,
    tenant_id uuid NOT NULL,
    branch_id uuid,
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    metadata_json jsonb,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8)
);

-- Add campus scope to existing installations before enforcing NOT NULL.
ALTER TABLE hr.candidate ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE hr.leave_request ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE finance.discount ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE finance.scholarship ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE finance.studentfee ADD COLUMN IF NOT EXISTS branch_id uuid;

-- Other operational resources discovered by the audit that also need explicit campus scope.
ALTER TABLE IF EXISTS hr.job ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE IF EXISTS hr.interview ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE IF EXISTS finance.student_invoice ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE IF EXISTS finance.student_payment ADD COLUMN IF NOT EXISTS branch_id uuid;

CREATE INDEX IF NOT EXISTS ix_candidate_tenant_branch ON hr.candidate(tenant_id, branch_id);
CREATE INDEX IF NOT EXISTS ix_leave_request_tenant_branch ON hr.leave_request(tenant_id, branch_id);
CREATE INDEX IF NOT EXISTS ix_discount_tenant_branch ON finance.discount(tenant_id, branch_id);
CREATE INDEX IF NOT EXISTS ix_scholarship_tenant_branch ON finance.scholarship(tenant_id, branch_id);
CREATE INDEX IF NOT EXISTS ix_studentfee_tenant_branch ON finance.studentfee(tenant_id, branch_id);

-- FKs are additive and safe for databases where org.campus exists.
DO $$
BEGIN
    IF to_regclass('org.campus') IS NOT NULL THEN
        IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'candidate_branch_id_fkey') THEN
            ALTER TABLE hr.candidate ADD CONSTRAINT candidate_branch_id_fkey FOREIGN KEY (branch_id) REFERENCES org.campus(campus_id);
        END IF;
        IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'leave_request_branch_id_fkey') THEN
            ALTER TABLE hr.leave_request ADD CONSTRAINT leave_request_branch_id_fkey FOREIGN KEY (branch_id) REFERENCES org.campus(campus_id);
        END IF;
        IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'discount_branch_id_fkey') THEN
            ALTER TABLE finance.discount ADD CONSTRAINT discount_branch_id_fkey FOREIGN KEY (branch_id) REFERENCES org.campus(campus_id);
        END IF;
        IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'scholarship_branch_id_fkey') THEN
            ALTER TABLE finance.scholarship ADD CONSTRAINT scholarship_branch_id_fkey FOREIGN KEY (branch_id) REFERENCES org.campus(campus_id);
        END IF;
        IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'studentfee_branch_id_fkey') THEN
            ALTER TABLE finance.studentfee ADD CONSTRAINT studentfee_branch_id_fkey FOREIGN KEY (branch_id) REFERENCES org.campus(campus_id);
        END IF;
    END IF;
END $$;
