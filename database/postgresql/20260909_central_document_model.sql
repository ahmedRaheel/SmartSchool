-- Centralize actor documents in the Documents module.
BEGIN;

CREATE SCHEMA IF NOT EXISTS document;

DROP TABLE IF EXISTS student.student_document CASCADE;
DROP TABLE IF EXISTS student.parent_document CASCADE;
DROP TABLE IF EXISTS hr.teacher_document CASCADE;
DROP TABLE IF EXISTS hr.employee_document CASCADE;
DROP TABLE IF EXISTS hr.candidate_document CASCADE;
DROP TABLE IF EXISTS transport.driver_document CASCADE;
DROP TABLE IF EXISTS org.school_document CASCADE;
DROP TABLE IF EXISTS document.document_link CASCADE;

DROP TABLE IF EXISTS document.required_document CASCADE;
DROP TABLE IF EXISTS document.document_type CASCADE;
DROP TABLE IF EXISTS document.required_document_type CASCADE;
DROP TABLE IF EXISTS document.document CASCADE;

CREATE TABLE document.required_document_type (
    required_document_type_id uuid PRIMARY KEY,
    tenant_id uuid NOT NULL,
    campus_id uuid NULL,
    code varchar(80) NOT NULL,
    name varchar(150) NOT NULL,
    description varchar(500) NULL,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz NULL,
    row_version bytea NOT NULL DEFAULT '\\x00'::bytea,
    CONSTRAINT uq_required_document_type UNIQUE NULLS NOT DISTINCT (tenant_id, campus_id, code)
);

CREATE TABLE document.required_document (
    required_document_id uuid PRIMARY KEY,
    tenant_id uuid NOT NULL,
    campus_id uuid NULL,
    user_role varchar(50) NOT NULL,
    is_mandatory boolean NOT NULL DEFAULT TRUE,
    required_document_type_id uuid NOT NULL REFERENCES document.required_document_type(required_document_type_id),
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz NULL,
    row_version bytea NOT NULL DEFAULT '\\x00'::bytea,
    CONSTRAINT uq_required_document UNIQUE NULLS NOT DISTINCT (tenant_id, campus_id, user_role, required_document_type_id)
);

CREATE TABLE document.document_type (
    document_type_id uuid PRIMARY KEY,
    tenant_id uuid NOT NULL,
    campus_id uuid NULL,
    owner_type varchar(40) NOT NULL CHECK (owner_type IN ('StudentDocument','TeacherDocument','ParentDocument','CampusDocument','ExaminerDocument','EmployeeDocument','DriverDocument','Certificate')),
    code varchar(80) NOT NULL,
    name varchar(150) NOT NULL,
    description varchar(500) NULL,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz NULL,
    row_version bytea NOT NULL DEFAULT '\\x00'::bytea,
    CONSTRAINT uq_document_type UNIQUE NULLS NOT DISTINCT (tenant_id, campus_id, code)
);

CREATE TABLE document.document (
    document_id uuid PRIMARY KEY,
    tenant_id uuid NOT NULL,
    campus_id uuid NULL,
    document_type_id uuid NOT NULL REFERENCES document.document_type(document_type_id),
    required_document_type_id uuid NULL REFERENCES document.required_document_type(required_document_type_id),
    owner_type varchar(40) NOT NULL CHECK (owner_type IN ('StudentDocument','TeacherDocument','ParentDocument','CampusDocument','ExaminerDocument','EmployeeDocument','DriverDocument','Certificate')),
    owner_id uuid NOT NULL,
    document_number varchar(50) NOT NULL,
    original_file_name varchar(255) NOT NULL,
    stored_file_name varchar(255) NOT NULL,
    extension varchar(20) NULL,
    mime_type varchar(150) NOT NULL,
    size_bytes bigint NOT NULL,
    sha256 varchar(64) NOT NULL,
    blob_data bytea NULL,
    title varchar(250) NULL,
    status varchar(30) NOT NULL DEFAULT 'ACTIVE',
    is_confidential boolean NOT NULL DEFAULT FALSE,
    uploaded_by uuid NOT NULL,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz NULL,
    row_version bytea NOT NULL DEFAULT '\\x00'::bytea,
    CONSTRAINT uq_document_number UNIQUE (tenant_id, document_number)
);
CREATE INDEX ix_document_owner ON document.document(tenant_id, campus_id, owner_type, owner_id);
CREATE INDEX ix_document_required_type ON document.document(tenant_id, required_document_type_id);

-- Global seed catalog. Tenant/campus-specific rows can be copied/configured by setup APIs.
-- Required-document types are intentionally data, not an application enum.

COMMIT;
