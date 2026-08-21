-- v46 API/dashboard schema sync. Safe to rerun.
-- Adds lifecycle/concurrency columns to core actor tables so the database can carry the same base lifecycle contract.
ALTER TABLE student.student ADD COLUMN IF NOT EXISTS is_active boolean NOT NULL DEFAULT true;
ALTER TABLE student.student ADD COLUMN IF NOT EXISTS created_at timestamptz NOT NULL DEFAULT now();
ALTER TABLE student.student ADD COLUMN IF NOT EXISTS updated_at timestamptz;
ALTER TABLE student.student ADD COLUMN IF NOT EXISTS row_version bytea NOT NULL DEFAULT decode(md5(random()::text || clock_timestamp()::text),'hex');
ALTER TABLE student.guardian ADD COLUMN IF NOT EXISTS is_active boolean NOT NULL DEFAULT true;
ALTER TABLE student.guardian ADD COLUMN IF NOT EXISTS created_at timestamptz NOT NULL DEFAULT now();
ALTER TABLE student.guardian ADD COLUMN IF NOT EXISTS updated_at timestamptz;
ALTER TABLE student.guardian ADD COLUMN IF NOT EXISTS row_version bytea NOT NULL DEFAULT decode(md5(random()::text || clock_timestamp()::text),'hex');
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS is_active boolean NOT NULL DEFAULT true;
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS created_at timestamptz NOT NULL DEFAULT now();
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS updated_at timestamptz;
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS row_version bytea NOT NULL DEFAULT decode(md5(random()::text || clock_timestamp()::text),'hex');
ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS row_version bytea NOT NULL DEFAULT decode(md5(random()::text || clock_timestamp()::text),'hex');
CREATE INDEX IF NOT EXISTS ix_student_tenant_active ON student.student(tenant_id,is_active);
CREATE INDEX IF NOT EXISTS ix_guardian_tenant_active ON student.guardian(tenant_id,is_active);
CREATE INDEX IF NOT EXISTS ix_employee_tenant_active ON hr.employee(tenant_id,is_active);
