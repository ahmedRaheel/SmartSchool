-- V97: token-scoped organization ownership, business numbering and central document management
ALTER TABLE student.student ADD COLUMN IF NOT EXISTS school_id uuid;
ALTER TABLE student.student ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS school_id uuid;
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE student.student_enrollment ADD COLUMN IF NOT EXISTS enrollment_number varchar(80);

CREATE SCHEMA IF NOT EXISTS document;
CREATE TABLE IF NOT EXISTS document.document (
 document_id uuid PRIMARY KEY, tenant_id uuid NOT NULL, school_id uuid NULL, branch_id uuid NULL,
 document_number varchar(50) NOT NULL, original_file_name varchar(255) NOT NULL, stored_file_name varchar(255) NOT NULL,
 extension varchar(20), mime_type varchar(150) NOT NULL, size_bytes bigint NOT NULL, sha256 varchar(64) NOT NULL,
 storage_provider varchar(30) NOT NULL DEFAULT 'DATABASE', storage_key varchar(1000), blob_data bytea,
 category varchar(60) NOT NULL, document_type varchar(80) NOT NULL, title varchar(250), description varchar(1000),
 version_no int NOT NULL DEFAULT 1, status varchar(30) NOT NULL DEFAULT 'ACTIVE', is_confidential boolean NOT NULL DEFAULT false,
 expires_on date NULL, uploaded_by uuid NOT NULL, created_at timestamptz NOT NULL DEFAULT now(), updated_at timestamptz NULL,
 row_version bigint NOT NULL DEFAULT 1, UNIQUE(tenant_id, document_number)
);
CREATE TABLE IF NOT EXISTS document.document_link (
 document_link_id uuid PRIMARY KEY, tenant_id uuid NOT NULL, document_id uuid NOT NULL REFERENCES document.document(document_id),
 entity_type varchar(80) NOT NULL, entity_id uuid NOT NULL, purpose varchar(80) NOT NULL, is_primary boolean NOT NULL DEFAULT false,
 display_order int NOT NULL DEFAULT 0, created_at timestamptz NOT NULL DEFAULT now()
);
CREATE INDEX IF NOT EXISTS ix_document_link_entity ON document.document_link(tenant_id, entity_type, entity_id);
CREATE INDEX IF NOT EXISTS ix_document_scope ON document.document(tenant_id, school_id, branch_id, category);

ALTER TABLE identity."AspNetUsers" ADD COLUMN IF NOT EXISTS "BranchId" uuid;

ALTER TABLE academic.subject ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE academic.course_offering ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE lms.academic_assignment ADD COLUMN IF NOT EXISTS branch_id uuid;
