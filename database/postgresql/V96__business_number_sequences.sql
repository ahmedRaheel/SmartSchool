CREATE SCHEMA IF NOT EXISTS platform;

CREATE TABLE IF NOT EXISTS platform.business_number_sequence
(
    tenant_id uuid NOT NULL,
    sequence_name varchar(80) NOT NULL,
    last_value bigint NOT NULL,
    CONSTRAINT pk_business_number_sequence PRIMARY KEY (tenant_id, sequence_name)
);

ALTER TABLE student.student ALTER COLUMN student_number DROP NOT NULL;
ALTER TABLE hr.employee ALTER COLUMN employee_number DROP NOT NULL;
