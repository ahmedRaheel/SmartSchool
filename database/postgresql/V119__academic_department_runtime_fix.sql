-- SmartSchool v119 runtime schema alignment.
-- Safe to run repeatedly.
ALTER TABLE academic.academic_year ADD COLUMN IF NOT EXISTS school_id uuid;
ALTER TABLE academic.academic_year ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE academic.academic_year ADD COLUMN IF NOT EXISTS code varchar(30);
UPDATE academic.academic_year SET branch_id = campus_id WHERE branch_id IS NULL;
UPDATE academic.academic_year ay SET school_id = c.school_id FROM org.campus c WHERE ay.campus_id = c.campus_id AND ay.school_id IS NULL;
UPDATE academic.academic_year SET code = replace(name, '/', '-') WHERE code IS NULL;

ALTER TABLE org.department ADD COLUMN IF NOT EXISTS campus_id uuid;
ALTER TABLE org.department ADD COLUMN IF NOT EXISTS head_of_department_employee_id uuid;
CREATE INDEX IF NOT EXISTS ix_department_tenant_campus ON org.department(tenant_id, campus_id);
