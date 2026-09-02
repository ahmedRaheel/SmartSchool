-- SmartSchool production UI/backend alignment - 2026-09-02
-- Safe additive alignment for the HR employee aggregate.
ALTER TABLE hr.employee
    ADD COLUMN IF NOT EXISTS school_id uuid,
    ADD COLUMN IF NOT EXISTS branch_id uuid,
    ADD COLUMN IF NOT EXISTS department_id uuid,
    ADD COLUMN IF NOT EXISTS staff_type varchar(30) NOT NULL DEFAULT 'OTHER',
    ADD COLUMN IF NOT EXISTS designation smallint NOT NULL DEFAULT 0,
    ADD COLUMN IF NOT EXISTS date_of_birth date,
    ADD COLUMN IF NOT EXISTS gender varchar(30),
    ADD COLUMN IF NOT EXISTS job_title varchar(150),
    ADD COLUMN IF NOT EXISTS alternate_phone varchar(50),
    ADD COLUMN IF NOT EXISTS address varchar(500),
    ADD COLUMN IF NOT EXISTS emergency_contact_name varchar(200),
    ADD COLUMN IF NOT EXISTS emergency_contact_phone varchar(50);

-- Backfill typed designation from the legacy staff_type value.
UPDATE hr.employee
SET designation = CASE UPPER(REPLACE(REPLACE(COALESCE(staff_type, 'OTHER'), '_', ''), '-', ''))
    WHEN 'TEACHER' THEN 1
    WHEN 'ACCOUNTANT' THEN 2
    WHEN 'EXAMINER' THEN 3
    WHEN 'PRINCIPAL' THEN 4
    WHEN 'HR' THEN 5
    WHEN 'HRMANAGER' THEN 5
    WHEN 'ADMINOFFICER' THEN 6
    WHEN 'DRIVER' THEN 7
    WHEN 'LIBRARIAN' THEN 8
    WHEN 'RECEPTIONIST' THEN 9
    WHEN 'COORDINATOR' THEN 10
    ELSE 0
END
WHERE designation = 0;

CREATE INDEX IF NOT EXISTS ix_employee_tenant_designation
    ON hr.employee (tenant_id, designation)
    WHERE is_active = TRUE;
