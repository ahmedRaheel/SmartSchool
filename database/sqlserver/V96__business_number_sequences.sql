IF SCHEMA_ID('platform') IS NULL EXEC('CREATE SCHEMA platform');
IF OBJECT_ID('platform.business_number_sequence', 'U') IS NULL
BEGIN
    CREATE TABLE platform.business_number_sequence
    (
        tenant_id uniqueidentifier NOT NULL,
        sequence_name varchar(80) NOT NULL,
        last_value bigint NOT NULL,
        CONSTRAINT pk_business_number_sequence PRIMARY KEY (tenant_id, sequence_name)
    );
END;
ALTER TABLE student.student ALTER COLUMN student_number nvarchar(60) NULL;
ALTER TABLE hr.employee ALTER COLUMN employee_number nvarchar(60) NULL;
