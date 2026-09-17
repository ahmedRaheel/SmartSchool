BEGIN;
CREATE SCHEMA IF NOT EXISTS org;
CREATE TABLE IF NOT EXISTS org.campus_education_level (
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    campus_id uuid NOT NULL REFERENCES org.campus(campus_id) ON DELETE CASCADE,
    education_level_id uuid NOT NULL REFERENCES reference.education_level(education_level_id),
    created_at timestamptz NOT NULL DEFAULT now(),
    CONSTRAINT pk_campus_education_level PRIMARY KEY (tenant_id, campus_id, education_level_id)
);
CREATE INDEX IF NOT EXISTS ix_campus_education_level_tenant_campus ON org.campus_education_level(tenant_id, campus_id);
CREATE INDEX IF NOT EXISTS ix_campus_education_level_tenant_level ON org.campus_education_level(tenant_id, education_level_id);
COMMIT;
