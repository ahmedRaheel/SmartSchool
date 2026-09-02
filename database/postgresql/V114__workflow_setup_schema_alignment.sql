BEGIN;

-- Admission workflow columns used by the current Dapper query/command slices.
ALTER TABLE admission.student_application ADD COLUMN IF NOT EXISTS class_id uuid REFERENCES academic.class(class_id);
ALTER TABLE admission.student_application ADD COLUMN IF NOT EXISTS section_id uuid REFERENCES academic.section(section_id);
ALTER TABLE admission.student_application ADD COLUMN IF NOT EXISTS gender varchar(30);
ALTER TABLE admission.student_application ADD COLUMN IF NOT EXISTS decision_notes text;
ALTER TABLE admission.student_application ADD COLUMN IF NOT EXISTS student_id uuid;
ALTER TABLE admission.student_application ADD COLUMN IF NOT EXISTS is_active boolean NOT NULL DEFAULT true;
CREATE INDEX IF NOT EXISTS ix_admission_application_tenant_active_submitted
    ON admission.student_application(tenant_id, is_active, submitted_at DESC);

-- Department and fee type are first-class setup masters. Existing installations are aligned idempotently.
CREATE TABLE IF NOT EXISTS org.department (
    department_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(50) NOT NULL,
    name varchar(200) NOT NULL,
    metadata_json text,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT public.gen_random_bytes(8),
    UNIQUE(tenant_id, code)
);

CREATE TABLE IF NOT EXISTS finance.fee_type (
    fee_type_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(50) NOT NULL,
    name varchar(200) NOT NULL,
    metadata_json text,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT public.gen_random_bytes(8),
    UNIQUE(tenant_id, code)
);

COMMIT;
