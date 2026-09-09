BEGIN;

CREATE SCHEMA IF NOT EXISTS admission;
CREATE SCHEMA IF NOT EXISTS activity;
CREATE SCHEMA IF NOT EXISTS finance;
CREATE SCHEMA IF NOT EXISTS exam;

CREATE TABLE IF NOT EXISTS admission.applicant (
    applicant_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL,
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    metadata_json jsonb NULL,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz NULL,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8),
    CONSTRAINT uq_applicant_tenant_code UNIQUE (tenant_id, code)
);

CREATE TABLE IF NOT EXISTS finance.discount (
    discount_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL,
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    metadata_json jsonb NULL,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz NULL,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8),
    CONSTRAINT uq_discount_tenant_code UNIQUE (tenant_id, code)
);

CREATE TABLE IF NOT EXISTS activity.student_of_month (
    student_of_month_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL,
    department_id uuid NOT NULL,
    student_id uuid NULL,
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    award_month smallint NULL CHECK (award_month BETWEEN 1 AND 12),
    award_year smallint NULL,
    metadata_json jsonb NULL,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz NULL,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8),
    CONSTRAINT fk_student_of_month_department FOREIGN KEY (department_id) REFERENCES org.department(department_id),
    CONSTRAINT fk_student_of_month_student FOREIGN KEY (student_id) REFERENCES student.student(student_id),
    CONSTRAINT uq_student_of_month_department_code UNIQUE (tenant_id, department_id, code)
);
CREATE INDEX IF NOT EXISTS ix_student_of_month_department ON activity.student_of_month(tenant_id, department_id);

ALTER TABLE finance.fee_type ADD COLUMN IF NOT EXISTS department_id uuid;
ALTER TABLE finance.fee_structure ADD COLUMN IF NOT EXISTS department_id uuid;
CREATE INDEX IF NOT EXISTS ix_fee_type_department ON finance.fee_type(tenant_id, department_id);
CREATE INDEX IF NOT EXISTS ix_fee_structure_department ON finance.fee_structure(tenant_id, department_id);
DO $$ BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_fee_type_department') THEN
        ALTER TABLE finance.fee_type ADD CONSTRAINT fk_fee_type_department FOREIGN KEY (department_id) REFERENCES org.department(department_id);
    END IF;
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_fee_structure_department') THEN
        ALTER TABLE finance.fee_structure ADD CONSTRAINT fk_fee_structure_department FOREIGN KEY (department_id) REFERENCES org.department(department_id);
    END IF;
END $$;

CREATE TABLE IF NOT EXISTS exam.grade_scale (
    grade_scale_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL,
    campus_id uuid NOT NULL,
    code varchar(100) NOT NULL,
    name varchar(50) NOT NULL,
    minimum_percentage numeric(5,2) NOT NULL,
    maximum_percentage numeric(5,2) NOT NULL,
    grade_point numeric(4,2) NULL,
    description varchar(500) NULL,
    metadata_json jsonb NULL,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz NULL,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8),
    CONSTRAINT fk_grade_scale_campus FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id),
    CONSTRAINT ck_grade_scale_percentage CHECK (minimum_percentage >= 0 AND maximum_percentage <= 100 AND minimum_percentage <= maximum_percentage),
    CONSTRAINT uq_grade_scale_campus_name UNIQUE (tenant_id, campus_id, name)
);
CREATE INDEX IF NOT EXISTS ix_grade_scale_campus ON exam.grade_scale(tenant_id, campus_id);

COMMIT;
