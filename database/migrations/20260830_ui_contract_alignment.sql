BEGIN;

-- UI contract alignment: Organization / Campus.
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS school_id uuid;
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS branch_type varchar(40);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS branch_gender_type_id uuid;
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS academic_system_id uuid;
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS city varchar(120);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS province varchar(120);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS country varchar(120);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS fax varchar(50);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS mobile varchar(50);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS logo_url varchar(500);
CREATE INDEX IF NOT EXISTS ix_campus_tenant_school ON org.campus(tenant_id, school_id);
CREATE INDEX IF NOT EXISTS ix_campus_academic_system ON org.campus(tenant_id, academic_system_id);

-- UI contract alignment: HR / Employee. Existing normalized education and experience
-- tables remain canonical; qualification/experience are not duplicated on employee.
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS school_id uuid;
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS department_id uuid;
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS staff_type varchar(30) DEFAULT 'OTHER';
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS date_of_birth date;
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS gender varchar(30);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS job_title varchar(150);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS alternate_phone varchar(50);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS address varchar(500);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS emergency_contact_name varchar(200);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS emergency_contact_phone varchar(50);
CREATE INDEX IF NOT EXISTS ix_employee_tenant_branch ON hr.employee(tenant_id, branch_id);
CREATE INDEX IF NOT EXISTS ix_employee_tenant_department ON hr.employee(tenant_id, department_id);

-- UI contract alignment: Student. These columns already exist in the domain model in newer
-- source revisions but are missing from older consolidated database dumps.
ALTER TABLE student.student ADD COLUMN IF NOT EXISTS school_id uuid;
ALTER TABLE student.student ADD COLUMN IF NOT EXISTS branch_id uuid;
CREATE INDEX IF NOT EXISTS ix_student_tenant_branch ON student.student(tenant_id, branch_id);

COMMIT;
