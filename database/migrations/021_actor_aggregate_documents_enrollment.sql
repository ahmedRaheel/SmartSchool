BEGIN;

CREATE SCHEMA IF NOT EXISTS document;
CREATE SCHEMA IF NOT EXISTS student;
CREATE SCHEMA IF NOT EXISTS hr;
CREATE SCHEMA IF NOT EXISTS saas;
CREATE SCHEMA IF NOT EXISTS org;

-- Required-document policy. One policy table drives UI requirements and backend approval gates.
CREATE TABLE IF NOT EXISTS document.required_document (
    required_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NULL,
    actor_type varchar(40) NOT NULL,
    staff_type varchar(40) NULL,
    document_type varchar(60) NOT NULL,
    display_name varchar(120) NOT NULL,
    is_required boolean NOT NULL DEFAULT true,
    condition_code varchar(60) NULL,
    min_count smallint NOT NULL DEFAULT 1 CHECK (min_count > 0),
    allowed_mime_types varchar(500) NULL,
    max_size_bytes bigint NULL,
    sort_order smallint NOT NULL DEFAULT 0,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    CONSTRAINT uq_required_document_policy UNIQUE NULLS NOT DISTINCT
        (tenant_id, actor_type, staff_type, document_type, condition_code)
);

-- Contact data is separated from aggregate roots so actors can have multiple contact methods/addresses.
CREATE TABLE IF NOT EXISTS saas.tenant_contact (
    tenant_contact_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL,
    contact_type varchar(30) NOT NULL, contact_name varchar(150), email varchar(200), phone varchar(30),
    address_line1 varchar(250), address_line2 varchar(250), city varchar(100), province varchar(100), country varchar(100), postal_code varchar(30),
    is_primary boolean NOT NULL DEFAULT false, is_active boolean NOT NULL DEFAULT true, created_at timestamptz NOT NULL DEFAULT now()
);
CREATE TABLE IF NOT EXISTS student.student_contact (
    student_contact_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, student_id uuid NOT NULL,
    contact_type varchar(30) NOT NULL, email varchar(200), phone varchar(30), address_line1 varchar(250), address_line2 varchar(250),
    city varchar(100), province varchar(100), country varchar(100), postal_code varchar(30), is_primary boolean NOT NULL DEFAULT false,
    is_active boolean NOT NULL DEFAULT true, created_at timestamptz NOT NULL DEFAULT now()
);
CREATE TABLE IF NOT EXISTS hr.employee_contact (
    employee_contact_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, employee_id uuid NOT NULL,
    contact_type varchar(30) NOT NULL, email varchar(200), phone varchar(30), address_line1 varchar(250), address_line2 varchar(250),
    city varchar(100), province varchar(100), country varchar(100), postal_code varchar(30), is_primary boolean NOT NULL DEFAULT false,
    is_active boolean NOT NULL DEFAULT true, created_at timestamptz NOT NULL DEFAULT now()
);

-- Typed document links. document.document remains the aggregate root/file metadata + binary store.
CREATE TABLE IF NOT EXISTS document.tenant_document (
    tenant_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, document_id uuid NOT NULL,
    required_document_id uuid NULL, created_at timestamptz NOT NULL DEFAULT now(), UNIQUE(tenant_id, document_id)
);
CREATE TABLE IF NOT EXISTS document.student_document (
    student_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, student_id uuid NOT NULL, document_id uuid NOT NULL,
    required_document_id uuid NULL, created_at timestamptz NOT NULL DEFAULT now(), UNIQUE(student_id, document_id)
);
CREATE TABLE IF NOT EXISTS document.teacher_document (
    teacher_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, teacher_id uuid NOT NULL, document_id uuid NOT NULL,
    required_document_id uuid NULL, created_at timestamptz NOT NULL DEFAULT now(), UNIQUE(teacher_id, document_id)
);
CREATE TABLE IF NOT EXISTS document.admin_officer_document (
    admin_officer_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, employee_id uuid NOT NULL, document_id uuid NOT NULL,
    required_document_id uuid NULL, created_at timestamptz NOT NULL DEFAULT now(), UNIQUE(employee_id, document_id)
);
CREATE TABLE IF NOT EXISTS document.staff_document (
    staff_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, employee_id uuid NOT NULL, document_id uuid NOT NULL,
    required_document_id uuid NULL, created_at timestamptz NOT NULL DEFAULT now(), UNIQUE(employee_id, document_id)
);
CREATE TABLE IF NOT EXISTS document.driver_document (
    driver_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, driver_id uuid NOT NULL, document_id uuid NOT NULL,
    required_document_id uuid NULL, created_at timestamptz NOT NULL DEFAULT now(), UNIQUE(driver_id, document_id)
);
CREATE TABLE IF NOT EXISTS document.guardian_document (
    guardian_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, guardian_id uuid NOT NULL, document_id uuid NOT NULL,
    required_document_id uuid NULL, created_at timestamptz NOT NULL DEFAULT now(), UNIQUE(guardian_id, document_id)
);
CREATE TABLE IF NOT EXISTS document.campus_document (
    campus_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, campus_id uuid NOT NULL, document_id uuid NOT NULL,
    required_document_id uuid NULL, created_at timestamptz NOT NULL DEFAULT now(), UNIQUE(campus_id, document_id)
);

-- Teacher/staff evidence. Certificates are documents; these tables hold the structured facts.
CREATE TABLE IF NOT EXISTS hr.employee_education (
    employee_education_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, employee_id uuid NOT NULL,
    qualification varchar(150) NOT NULL, institute varchar(200), field_of_study varchar(150), start_date date, end_date date,
    grade varchar(50), is_highest boolean NOT NULL DEFAULT false, created_at timestamptz NOT NULL DEFAULT now()
);
CREATE TABLE IF NOT EXISTS hr.employee_experience (
    employee_experience_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, employee_id uuid NOT NULL,
    employer varchar(200) NOT NULL, job_title varchar(150) NOT NULL, start_date date NOT NULL, end_date date,
    responsibilities text, created_at timestamptz NOT NULL DEFAULT now()
);

-- Normalize the student/guardian bridge; older dumps lacked an aggregate link id and tenant audit columns.
ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS student_guardian_id uuid DEFAULT gen_random_uuid();
ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS tenant_id uuid;
ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS is_active boolean NOT NULL DEFAULT true;
ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS created_at timestamptz NOT NULL DEFAULT now();
ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS row_version bigint NOT NULL DEFAULT 0;
UPDATE student.student_guardian sg SET tenant_id=s.tenant_id FROM student.student s WHERE sg.student_id=s.student_id AND sg.tenant_id IS NULL;
CREATE UNIQUE INDEX IF NOT EXISTS ux_student_guardian_pair ON student.student_guardian(student_id,guardian_id);

-- Pending admission placement. Enrollment is created only when admission is approved.
CREATE TABLE IF NOT EXISTS student.admission_placement (
    admission_placement_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, student_id uuid NOT NULL,
    academic_year_id uuid NOT NULL, class_section_id uuid NOT NULL, requested_at timestamptz NOT NULL DEFAULT now(),
    status varchar(20) NOT NULL DEFAULT 'PENDING', approved_at timestamptz NULL,
    CONSTRAINT uq_student_pending_placement UNIQUE(student_id, academic_year_id)
);

-- Ensure enrollment has a business enrollment number and direct class/section traceability.
ALTER TABLE student.student_enrollment ADD COLUMN IF NOT EXISTS enrollment_number varchar(30);
ALTER TABLE student.student_enrollment ADD COLUMN IF NOT EXISTS class_id uuid;
CREATE UNIQUE INDEX IF NOT EXISTS ux_student_enrollment_number ON student.student_enrollment(tenant_id, enrollment_number) WHERE enrollment_number IS NOT NULL;

-- Default global policies. Tenant-specific rows can override/extend these.
INSERT INTO document.required_document(tenant_id,actor_type,staff_type,document_type,display_name,is_required,condition_code,sort_order)
VALUES
(NULL,'STUDENT',NULL,'PHOTO','Student photograph',true,NULL,10),
(NULL,'STUDENT',NULL,'BIRTH_CERTIFICATE','Birth certificate',true,NULL,20),
(NULL,'STUDENT',NULL,'CNIC_BFORM','CNIC / B-Form',true,NULL,30),
(NULL,'GUARDIAN',NULL,'CNIC','Guardian CNIC',true,NULL,10),
(NULL,'EMPLOYEE',NULL,'PHOTO','Photograph',true,NULL,10),
(NULL,'EMPLOYEE',NULL,'CNIC','CNIC / national ID',true,NULL,20),
(NULL,'EMPLOYEE','TEACHER','EDUCATION_CERTIFICATE','Education certificate',true,NULL,30),
(NULL,'EMPLOYEE','TEACHER','EXPERIENCE_CERTIFICATE','Experience certificate',true,'EXPERIENCE_PRESENT',40),
(NULL,'EMPLOYEE','DRIVER','DRIVING_LICENSE','Driving licence',true,NULL,30),
(NULL,'TENANT',NULL,'REGISTRATION_CERTIFICATE','Registration certificate',true,NULL,10),
(NULL,'CAMPUS',NULL,'LOGO','Campus / school logo',true,NULL,10)
ON CONFLICT DO NOTHING;

COMMIT;
