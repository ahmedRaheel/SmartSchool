BEGIN;
CREATE SCHEMA IF NOT EXISTS workflow;

CREATE TABLE IF NOT EXISTS workflow.workflowdefinition (
    workflow_definition_id uuid PRIMARY KEY,
    tenant_id uuid NOT NULL,
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    description varchar(2000),
    trigger_type varchar(50) NOT NULL DEFAULT 'MANUAL',
    entity_type varchar(100) NOT NULL DEFAULT 'GENERIC',
    status varchar(30) NOT NULL DEFAULT 'ACTIVE',
    version integer NOT NULL DEFAULT 1,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT '\\x00'
);
ALTER TABLE workflow.workflowdefinition ADD COLUMN IF NOT EXISTS description varchar(2000);
ALTER TABLE workflow.workflowdefinition ADD COLUMN IF NOT EXISTS trigger_type varchar(50) NOT NULL DEFAULT 'MANUAL';
ALTER TABLE workflow.workflowdefinition ADD COLUMN IF NOT EXISTS entity_type varchar(100) NOT NULL DEFAULT 'GENERIC';
ALTER TABLE workflow.workflowdefinition ADD COLUMN IF NOT EXISTS status varchar(30) NOT NULL DEFAULT 'ACTIVE';
ALTER TABLE workflow.workflowdefinition ADD COLUMN IF NOT EXISTS version integer NOT NULL DEFAULT 1;
CREATE UNIQUE INDEX IF NOT EXISTS ux_workflow_definition_tenant_code ON workflow.workflowdefinition(tenant_id, code);

CREATE TABLE IF NOT EXISTS workflow.workflowstep (
    workflow_step_id uuid PRIMARY KEY,
    workflow_definition_id uuid,
    tenant_id uuid NOT NULL,
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    step_order integer NOT NULL DEFAULT 1,
    step_type varchar(30) NOT NULL DEFAULT 'APPROVAL',
    approver_role varchar(100),
    action_code varchar(100),
    is_required boolean NOT NULL DEFAULT true,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT '\\x00'
);
ALTER TABLE workflow.workflowstep ADD COLUMN IF NOT EXISTS workflow_definition_id uuid;
ALTER TABLE workflow.workflowstep ADD COLUMN IF NOT EXISTS step_order integer NOT NULL DEFAULT 1;
ALTER TABLE workflow.workflowstep ADD COLUMN IF NOT EXISTS step_type varchar(30) NOT NULL DEFAULT 'APPROVAL';
ALTER TABLE workflow.workflowstep ADD COLUMN IF NOT EXISTS approver_role varchar(100);
ALTER TABLE workflow.workflowstep ADD COLUMN IF NOT EXISTS action_code varchar(100);
ALTER TABLE workflow.workflowstep ADD COLUMN IF NOT EXISTS is_required boolean NOT NULL DEFAULT true;
CREATE UNIQUE INDEX IF NOT EXISTS ux_workflow_step_order ON workflow.workflowstep(tenant_id, workflow_definition_id, step_order) WHERE workflow_definition_id IS NOT NULL;

CREATE TABLE IF NOT EXISTS workflow.workflowinstance (
    workflow_instance_id uuid PRIMARY KEY,
    workflow_definition_id uuid,
    tenant_id uuid NOT NULL,
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    entity_type varchar(100) NOT NULL DEFAULT 'GENERIC',
    entity_id uuid,
    status varchar(30) NOT NULL DEFAULT 'IN_PROGRESS',
    current_step_order integer NOT NULL DEFAULT 0,
    started_by_user_id uuid,
    started_at timestamptz NOT NULL DEFAULT now(),
    completed_at timestamptz,
    context_json jsonb,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT '\\x00'
);
ALTER TABLE workflow.workflowinstance ADD COLUMN IF NOT EXISTS workflow_definition_id uuid;
ALTER TABLE workflow.workflowinstance ADD COLUMN IF NOT EXISTS entity_type varchar(100) NOT NULL DEFAULT 'GENERIC';
ALTER TABLE workflow.workflowinstance ADD COLUMN IF NOT EXISTS entity_id uuid;
ALTER TABLE workflow.workflowinstance ADD COLUMN IF NOT EXISTS status varchar(30) NOT NULL DEFAULT 'IN_PROGRESS';
ALTER TABLE workflow.workflowinstance ADD COLUMN IF NOT EXISTS current_step_order integer NOT NULL DEFAULT 0;
ALTER TABLE workflow.workflowinstance ADD COLUMN IF NOT EXISTS started_by_user_id uuid;
ALTER TABLE workflow.workflowinstance ADD COLUMN IF NOT EXISTS started_at timestamptz NOT NULL DEFAULT now();
ALTER TABLE workflow.workflowinstance ADD COLUMN IF NOT EXISTS completed_at timestamptz;
ALTER TABLE workflow.workflowinstance ADD COLUMN IF NOT EXISTS context_json jsonb;
CREATE UNIQUE INDEX IF NOT EXISTS ux_workflow_instance_tenant_code ON workflow.workflowinstance(tenant_id, code);

CREATE TABLE IF NOT EXISTS workflow.approval (
    approval_id uuid PRIMARY KEY,
    workflow_instance_id uuid,
    workflow_step_id uuid,
    tenant_id uuid NOT NULL,
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    assigned_role varchar(100) NOT NULL DEFAULT 'Admin',
    status varchar(30) NOT NULL DEFAULT 'PENDING',
    requested_at timestamptz NOT NULL DEFAULT now(),
    decision_at timestamptz,
    decided_by_user_id uuid,
    comments varchar(2000),
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT '\\x00'
);
ALTER TABLE workflow.approval ADD COLUMN IF NOT EXISTS workflow_instance_id uuid;
ALTER TABLE workflow.approval ADD COLUMN IF NOT EXISTS workflow_step_id uuid;
ALTER TABLE workflow.approval ADD COLUMN IF NOT EXISTS assigned_role varchar(100) NOT NULL DEFAULT 'Admin';
ALTER TABLE workflow.approval ADD COLUMN IF NOT EXISTS status varchar(30) NOT NULL DEFAULT 'PENDING';
ALTER TABLE workflow.approval ADD COLUMN IF NOT EXISTS requested_at timestamptz NOT NULL DEFAULT now();
ALTER TABLE workflow.approval ADD COLUMN IF NOT EXISTS decision_at timestamptz;
ALTER TABLE workflow.approval ADD COLUMN IF NOT EXISTS decided_by_user_id uuid;
ALTER TABLE workflow.approval ADD COLUMN IF NOT EXISTS comments varchar(2000);
CREATE UNIQUE INDEX IF NOT EXISTS ux_workflow_approval_instance_step ON workflow.approval(tenant_id, workflow_instance_id, workflow_step_id) WHERE workflow_instance_id IS NOT NULL AND workflow_step_id IS NOT NULL;
CREATE INDEX IF NOT EXISTS ix_workflow_approval_queue ON workflow.approval(tenant_id,status,assigned_role);

DO $$ BEGIN
 IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname='fk_workflow_step_definition') THEN
   ALTER TABLE workflow.workflowstep ADD CONSTRAINT fk_workflow_step_definition FOREIGN KEY(workflow_definition_id) REFERENCES workflow.workflowdefinition(workflow_definition_id) ON DELETE CASCADE;
 END IF;
 IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname='fk_workflow_instance_definition') THEN
   ALTER TABLE workflow.workflowinstance ADD CONSTRAINT fk_workflow_instance_definition FOREIGN KEY(workflow_definition_id) REFERENCES workflow.workflowdefinition(workflow_definition_id) ON DELETE RESTRICT;
 END IF;
 IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname='fk_workflow_approval_instance') THEN
   ALTER TABLE workflow.approval ADD CONSTRAINT fk_workflow_approval_instance FOREIGN KEY(workflow_instance_id) REFERENCES workflow.workflowinstance(workflow_instance_id) ON DELETE CASCADE;
 END IF;
 IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname='fk_workflow_approval_step') THEN
   ALTER TABLE workflow.approval ADD CONSTRAINT fk_workflow_approval_step FOREIGN KEY(workflow_step_id) REFERENCES workflow.workflowstep(workflow_step_id) ON DELETE RESTRICT;
 END IF;
END $$;
COMMIT;
