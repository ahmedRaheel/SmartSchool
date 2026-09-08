BEGIN;
ALTER TABLE academic.subject ADD COLUMN IF NOT EXISTS department_id uuid;
UPDATE academic.subject s SET department_id = d.department_id FROM org.department d WHERE s.department_id IS NULL AND d.tenant_id=s.tenant_id AND d.campus_id=s.branch_id;
ALTER TABLE academic.subject DROP COLUMN IF EXISTS branch_id;
DO $$ BEGIN IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname='subject_department_id_fkey') THEN ALTER TABLE academic.subject ADD CONSTRAINT subject_department_id_fkey FOREIGN KEY (department_id) REFERENCES org.department(department_id); END IF; END $$;
CREATE INDEX IF NOT EXISTS ix_subject_tenant_department ON academic.subject(tenant_id, department_id);

CREATE TABLE IF NOT EXISTS hr.teacher_teaching_assignment (
 teacher_teaching_assignment_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, school_id uuid NOT NULL, campus_id uuid NOT NULL, employee_id uuid NOT NULL, class_section_id uuid NOT NULL, subject_id uuid NOT NULL, code varchar(50) NOT NULL, name varchar(250) NOT NULL, periods_per_week integer, is_class_teacher boolean NOT NULL DEFAULT false, effective_from date, effective_to date, is_active boolean NOT NULL DEFAULT true, created_at timestamptz NOT NULL DEFAULT now(), updated_at timestamptz, row_version bytea NOT NULL DEFAULT public.gen_random_bytes(8), CONSTRAINT uq_teacher_teaching_assignment_code UNIQUE(tenant_id,code));
CREATE INDEX IF NOT EXISTS ix_teacher_teaching_assignment_employee ON hr.teacher_teaching_assignment(tenant_id, employee_id);
CREATE INDEX IF NOT EXISTS ix_teacher_teaching_assignment_campus ON hr.teacher_teaching_assignment(tenant_id, campus_id);
COMMIT;
