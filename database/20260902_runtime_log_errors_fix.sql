-- SmartSchool runtime fixes derived from 2026-09-02 API error log.
-- Idempotent and safe to re-run.
BEGIN;

CREATE EXTENSION IF NOT EXISTS pgcrypto;
CREATE SCHEMA IF NOT EXISTS reference;
CREATE SCHEMA IF NOT EXISTS workflow;

-- Organization branch-policy lookups used by BranchPolicyQuery.
CREATE TABLE IF NOT EXISTS reference.branch_gender_type (
    branch_gender_type_id uuid PRIMARY KEY,
    code varchar(40) NOT NULL UNIQUE,
    name varchar(100) NOT NULL,
    sort_order integer NOT NULL DEFAULT 0,
    is_active boolean NOT NULL DEFAULT TRUE
);
CREATE TABLE IF NOT EXISTS reference.education_level (
    education_level_id uuid PRIMARY KEY,
    code varchar(40) NOT NULL UNIQUE,
    name varchar(100) NOT NULL,
    sort_order integer NOT NULL DEFAULT 0,
    is_active boolean NOT NULL DEFAULT TRUE
);
INSERT INTO reference.branch_gender_type(branch_gender_type_id,code,name,sort_order,is_active) VALUES
('10000000-0000-0000-0000-000000000001','BOYS_ONLY','Boys Only',1,TRUE),
('10000000-0000-0000-0000-000000000002','GIRLS_ONLY','Girls Only',2,TRUE),
('10000000-0000-0000-0000-000000000003','CO_EDUCATION','Co-Education',3,TRUE)
ON CONFLICT (code) DO UPDATE SET name=EXCLUDED.name,sort_order=EXCLUDED.sort_order,is_active=TRUE;
INSERT INTO reference.education_level(education_level_id,code,name,sort_order,is_active) VALUES
('20000000-0000-0000-0000-000000000001','PRE_PRIMARY','Pre-Primary',1,TRUE),
('20000000-0000-0000-0000-000000000002','PRIMARY','Primary',2,TRUE),
('20000000-0000-0000-0000-000000000003','MIDDLE','Middle',3,TRUE),
('20000000-0000-0000-0000-000000000004','SECONDARY','Secondary',4,TRUE),
('20000000-0000-0000-0000-000000000005','HIGHER_SECONDARY','Higher Secondary',5,TRUE)
ON CONFLICT (code) DO UPDATE SET name=EXCLUDED.name,sort_order=EXCLUDED.sort_order,is_active=TRUE;

-- Workflow Vertical Slice queries use these canonical lowercase PostgreSQL names.
CREATE TABLE IF NOT EXISTS workflow.workflowdefinition (
    workflow_definition_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    metadata_json text,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8),
    UNIQUE(tenant_id, code)
);
CREATE TABLE IF NOT EXISTS workflow.workflowinstance (
    workflow_instance_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    metadata_json text,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8),
    UNIQUE(tenant_id, code)
);
CREATE TABLE IF NOT EXISTS workflow.workflowstep (
    workflow_step_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    metadata_json text,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8),
    UNIQUE(tenant_id, code)
);
CREATE TABLE IF NOT EXISTS workflow.approval (
    approval_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    metadata_json text,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8),
    UNIQUE(tenant_id, code)
);
CREATE INDEX IF NOT EXISTS ix_workflowdefinition_tenant_active ON workflow.workflowdefinition(tenant_id,is_active);
CREATE INDEX IF NOT EXISTS ix_workflowinstance_tenant_active ON workflow.workflowinstance(tenant_id,is_active);
CREATE INDEX IF NOT EXISTS ix_workflowstep_tenant_active ON workflow.workflowstep(tenant_id,is_active);
CREATE INDEX IF NOT EXISTS ix_approval_tenant_active ON workflow.approval(tenant_id,is_active);

COMMIT;
