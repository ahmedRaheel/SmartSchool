/*
 SmartSchool - PostgreSQL 16+ Foundation Database
 Modular-monolith friendly, schema-per-bounded-context.
 Includes core tables and seeded lookup values discussed in the SmartSchool design.
 Identity authentication is external (IdentityServer); identity.user_profile stores the IdentityServer subject.
*/

BEGIN;

CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE SCHEMA IF NOT EXISTS saas;
CREATE SCHEMA IF NOT EXISTS org;
CREATE SCHEMA IF NOT EXISTS identity_ref;
CREATE SCHEMA IF NOT EXISTS academic;
CREATE SCHEMA IF NOT EXISTS student;
CREATE SCHEMA IF NOT EXISTS admission;
CREATE SCHEMA IF NOT EXISTS lms;
CREATE SCHEMA IF NOT EXISTS exam;
CREATE SCHEMA IF NOT EXISTS finance;
CREATE SCHEMA IF NOT EXISTS hr;
CREATE SCHEMA IF NOT EXISTS payroll;
CREATE SCHEMA IF NOT EXISTS document;
CREATE SCHEMA IF NOT EXISTS communication;
CREATE SCHEMA IF NOT EXISTS workflow;
CREATE SCHEMA IF NOT EXISTS activity;
CREATE SCHEMA IF NOT EXISTS transport;
CREATE SCHEMA IF NOT EXISTS library;
CREATE SCHEMA IF NOT EXISTS inventory;
CREATE SCHEMA IF NOT EXISTS ai;
CREATE SCHEMA IF NOT EXISTS ai_core;
CREATE SCHEMA IF NOT EXISTS ai_tutor;
CREATE SCHEMA IF NOT EXISTS ai_inquiry;
CREATE SCHEMA IF NOT EXISTS ai_parent;
CREATE SCHEMA IF NOT EXISTS audit;

-- =========================================================
-- COMMON LOOKUPS
-- =========================================================
CREATE TABLE saas.lookup_type (
    lookup_type_id bigint GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    code varchar(80) NOT NULL UNIQUE,
    name varchar(150) NOT NULL
);

CREATE TABLE saas.lookup_value (
    lookup_value_id bigint GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    lookup_type_id bigint NOT NULL REFERENCES saas.lookup_type(lookup_type_id),
    code varchar(80) NOT NULL,
    name varchar(150) NOT NULL,
    sort_order int NOT NULL DEFAULT 0,
    is_active boolean NOT NULL DEFAULT true,
    metadata jsonb,
    UNIQUE (lookup_type_id, code)
);

INSERT INTO saas.lookup_type(code,name) VALUES
('TENANT_STATUS','Tenant Status'),
('ACADEMIC_SYSTEM_TYPE','Academic System Type'),
('SUBJECT_REQUIREMENT_TYPE','Subject Requirement Type'),
('ENROLLMENT_TYPE','Course Enrollment Type'),
('EXAM_TYPE','Exam / Assessment Type'),
('ATTENDANCE_STATUS','Attendance Status'),
('ASSIGNMENT_TYPE','Academic Assignment Type'),
('WORK_ASSIGNMENT_STATUS','Work Assignment Status'),
('EMPLOYMENT_TYPE','Employment Type'),
('CANDIDATE_STATUS','Candidate Status'),
('APPLICATION_STATUS','Job Application Status'),
('INTERVIEW_TYPE','Interview Type'),
('DOCUMENT_TYPE','Certificate / Letter Type'),
('INCREMENT_REQUEST_TYPE','Increment Request Type'),
('INCREMENT_TYPE','Increment Type'),
('APPROVAL_STATUS','Approval Status'),
('PAYROLL_STATUS','Payroll Status'),
('MESSAGE_TYPE','Message Type'),
('CONVERSATION_TYPE','Conversation Type'),
('AWARD_TYPE','Award Type'),
('NOTIFICATION_CHANNEL','Notification Channel');

INSERT INTO saas.lookup_value(lookup_type_id,code,name,sort_order)
SELECT t.lookup_type_id,v.code,v.name,v.sort_order
FROM saas.lookup_type t
JOIN (VALUES
('TENANT_STATUS','TRIAL','Trial',1),('TENANT_STATUS','ACTIVE','Active',2),('TENANT_STATUS','SUSPENDED','Suspended',3),('TENANT_STATUS','CANCELLED','Cancelled',4),
('ACADEMIC_SYSTEM_TYPE','CAMBRIDGE','Cambridge',1),('ACADEMIC_SYSTEM_TYPE','MATRIC','Matric / SSC',2),('ACADEMIC_SYSTEM_TYPE','INTERMEDIATE','Intermediate / HSSC',3),('ACADEMIC_SYSTEM_TYPE','IB','International Baccalaureate',4),('ACADEMIC_SYSTEM_TYPE','AMERICAN','American',5),('ACADEMIC_SYSTEM_TYPE','CUSTOM','Custom',99),
('SUBJECT_REQUIREMENT_TYPE','MANDATORY','Mandatory',1),('SUBJECT_REQUIREMENT_TYPE','OPTIONAL','Optional',2),('SUBJECT_REQUIREMENT_TYPE','ELECTIVE','Elective',3),
('ENROLLMENT_TYPE','MANDATORY','Mandatory',1),('ENROLLMENT_TYPE','ELECTIVE','Elective',2),('ENROLLMENT_TYPE','OPTIONAL','Optional',3),('ENROLLMENT_TYPE','TRANSFERRED','Transferred',4),
('EXAM_TYPE','QUIZ','Quiz',1),('EXAM_TYPE','CLASS_TEST','Class Test',2),('EXAM_TYPE','WEEKLY_TEST','Weekly Test',3),('EXAM_TYPE','MONTHLY_TEST','Monthly Test',4),('EXAM_TYPE','UNIT_TEST','Unit / Chapter Test',5),('EXAM_TYPE','MIDTERM','Midterm',6),('EXAM_TYPE','TERM','Term Examination',7),('EXAM_TYPE','PREBOARD','Pre-Board',8),('EXAM_TYPE','MOCK','Mock Examination',9),('EXAM_TYPE','ANNUAL','Annual Examination',10),('EXAM_TYPE','FINAL','Final Examination',11),('EXAM_TYPE','PRACTICAL','Practical',12),('EXAM_TYPE','VIVA','Oral / Viva',13),('EXAM_TYPE','PROJECT','Project / Coursework',14),('EXAM_TYPE','SUPPLEMENTARY','Supplementary',15),('EXAM_TYPE','RESIT','Re-sit',16),
('ATTENDANCE_STATUS','PRESENT','Present',1),('ATTENDANCE_STATUS','ABSENT','Absent',2),('ATTENDANCE_STATUS','LATE','Late',3),('ATTENDANCE_STATUS','EXCUSED','Excused',4),('ATTENDANCE_STATUS','LEAVE','Leave',5),('ATTENDANCE_STATUS','HALF_DAY','Half Day',6),
('ASSIGNMENT_TYPE','HOMEWORK','Homework',1),('ASSIGNMENT_TYPE','CLASSWORK','Classwork',2),('ASSIGNMENT_TYPE','PROJECT','Project',3),('ASSIGNMENT_TYPE','RESEARCH','Research',4),('ASSIGNMENT_TYPE','PRESENTATION','Presentation',5),('ASSIGNMENT_TYPE','PRACTICAL','Practical',6),('ASSIGNMENT_TYPE','LAB_WORK','Lab Work',7),('ASSIGNMENT_TYPE','ESSAY','Essay',8),('ASSIGNMENT_TYPE','READING','Reading',9),('ASSIGNMENT_TYPE','GROUP_WORK','Group Work',10),('ASSIGNMENT_TYPE','HOLIDAY_HOMEWORK','Holiday Homework',11),('ASSIGNMENT_TYPE','CUSTOM','Custom',99),
('WORK_ASSIGNMENT_STATUS','DRAFT','Draft',1),('WORK_ASSIGNMENT_STATUS','ASSIGNED','Assigned',2),('WORK_ASSIGNMENT_STATUS','ACCEPTED','Accepted',3),('WORK_ASSIGNMENT_STATUS','IN_PROGRESS','In Progress',4),('WORK_ASSIGNMENT_STATUS','BLOCKED','Blocked',5),('WORK_ASSIGNMENT_STATUS','COMPLETED','Completed',6),('WORK_ASSIGNMENT_STATUS','REJECTED','Rejected',7),('WORK_ASSIGNMENT_STATUS','CANCELLED','Cancelled',8),('WORK_ASSIGNMENT_STATUS','OVERDUE','Overdue',9),
('EMPLOYMENT_TYPE','PERMANENT','Permanent',1),('EMPLOYMENT_TYPE','CONTRACT','Contract',2),('EMPLOYMENT_TYPE','PART_TIME','Part Time',3),('EMPLOYMENT_TYPE','TEMPORARY','Temporary',4),('EMPLOYMENT_TYPE','VISITING','Visiting',5),('EMPLOYMENT_TYPE','INTERN','Intern',6),
('CANDIDATE_STATUS','NEW','New',1),('CANDIDATE_STATUS','SCREENING','Screening',2),('CANDIDATE_STATUS','SHORTLISTED','Shortlisted',3),('CANDIDATE_STATUS','INTERVIEW','Interview',4),('CANDIDATE_STATUS','ASSESSMENT','Assessment',5),('CANDIDATE_STATUS','SELECTED','Selected',6),('CANDIDATE_STATUS','OFFER','Offer',7),('CANDIDATE_STATUS','HIRED','Hired',8),('CANDIDATE_STATUS','REJECTED','Rejected',9),('CANDIDATE_STATUS','WITHDRAWN','Withdrawn',10),('CANDIDATE_STATUS','ON_HOLD','On Hold',11),
('APPLICATION_STATUS','APPLIED','Applied',1),('APPLICATION_STATUS','SCREENING','Screening',2),('APPLICATION_STATUS','SHORTLISTED','Shortlisted',3),('APPLICATION_STATUS','INTERVIEW','Interview',4),('APPLICATION_STATUS','OFFERED','Offered',5),('APPLICATION_STATUS','HIRED','Hired',6),('APPLICATION_STATUS','REJECTED','Rejected',7),('APPLICATION_STATUS','WITHDRAWN','Withdrawn',8),
('INTERVIEW_TYPE','HR_SCREENING','HR Screening',1),('INTERVIEW_TYPE','SUBJECT','Subject / Technical Interview',2),('INTERVIEW_TYPE','TEACHING_DEMO','Teaching Demo',3),('INTERVIEW_TYPE','PANEL','Panel Interview',4),('INTERVIEW_TYPE','PRINCIPAL','Principal Interview',5),('INTERVIEW_TYPE','FINAL','Final Interview',6),
('DOCUMENT_TYPE','SCHOOL_LEAVING','School Leaving Certificate',1),('DOCUMENT_TYPE','TRANSFER','Transfer Certificate',2),('DOCUMENT_TYPE','MIGRATION','Migration Certificate',3),('DOCUMENT_TYPE','CHARACTER','Character / Conduct Certificate',4),('DOCUMENT_TYPE','BONAFIDE','Bonafide / Enrollment Certificate',5),('DOCUMENT_TYPE','APPRECIATION','Appreciation Certificate',6),('DOCUMENT_TYPE','STUDENT_OF_MONTH','Student of the Month Certificate',7),('DOCUMENT_TYPE','ACHIEVEMENT','Achievement Certificate',8),('DOCUMENT_TYPE','SPORTS','Sports Certificate',9),('DOCUMENT_TYPE','ACTIVITY','Co-curricular Activity Certificate',10),('DOCUMENT_TYPE','ADMISSION_OFFER','Admission Offer Letter',11),('DOCUMENT_TYPE','WARNING','Warning Letter',12),('DOCUMENT_TYPE','EMPLOYMENT','Employment Letter',13),('DOCUMENT_TYPE','EXPERIENCE','Experience Letter',14),('DOCUMENT_TYPE','CUSTOM','Custom Document',99),
('INCREMENT_REQUEST_TYPE','AUTO','Automatic Proposal',1),('INCREMENT_REQUEST_TYPE','MANUAL','Manual Proposal',2),
('INCREMENT_TYPE','PERCENTAGE','Percentage',1),('INCREMENT_TYPE','FIXED','Fixed Amount',2),('INCREMENT_TYPE','NEW_SALARY','New Salary',3),('INCREMENT_TYPE','GRADE_STEP','Grade / Step',4),
('APPROVAL_STATUS','DRAFT','Draft',1),('APPROVAL_STATUS','PENDING','Pending',2),('APPROVAL_STATUS','APPROVED','Approved',3),('APPROVAL_STATUS','REJECTED','Rejected',4),('APPROVAL_STATUS','CANCELLED','Cancelled',5),
('PAYROLL_STATUS','DRAFT','Draft',1),('PAYROLL_STATUS','CALCULATED','Calculated',2),('PAYROLL_STATUS','HR_REVIEW','HR Review',3),('PAYROLL_STATUS','FINANCE_REVIEW','Finance Review',4),('PAYROLL_STATUS','APPROVED','Approved',5),('PAYROLL_STATUS','LOCKED','Locked',6),('PAYROLL_STATUS','PAID','Paid',7),
('MESSAGE_TYPE','TEXT','Text',1),('MESSAGE_TYPE','IMAGE','Image',2),('MESSAGE_TYPE','FILE','File',3),('MESSAGE_TYPE','VOICE','Voice Note',4),('MESSAGE_TYPE','SYSTEM','System',5),
('CONVERSATION_TYPE','PARENT_TEACHER','Parent / Teacher',1),('CONVERSATION_TYPE','CLASS','Class Channel',2),('CONVERSATION_TYPE','SUBJECT','Subject Channel',3),('CONVERSATION_TYPE','ADMIN','Administration',4),('CONVERSATION_TYPE','STAFF','Staff',5),
('AWARD_TYPE','STUDENT_OF_MONTH','Student of the Month',1),('AWARD_TYPE','ACADEMIC_EXCELLENCE','Academic Excellence',2),('AWARD_TYPE','BEST_ATTENDANCE','Best Attendance',3),('AWARD_TYPE','MOST_IMPROVED','Most Improved',4),('AWARD_TYPE','LEADERSHIP','Leadership',5),('AWARD_TYPE','SPORTS_EXCELLENCE','Sports Excellence',6),('AWARD_TYPE','COMMUNITY_SERVICE','Community Service',7),('AWARD_TYPE','APPRECIATION','Appreciation',8),
('NOTIFICATION_CHANNEL','IN_APP','In-App',1),('NOTIFICATION_CHANNEL','PUSH','Push',2),('NOTIFICATION_CHANNEL','EMAIL','Email',3),('NOTIFICATION_CHANNEL','SMS','SMS',4),('NOTIFICATION_CHANNEL','WHATSAPP','WhatsApp',5)
) v(type_code,code,name,sort_order) ON t.code=v.type_code;

-- =========================================================
-- TENANCY / ORGANIZATION
-- =========================================================
CREATE TABLE saas.tenant (
    tenant_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    code varchar(50) NOT NULL UNIQUE,
    name varchar(200) NOT NULL,
    status_code varchar(30) NOT NULL DEFAULT 'ACTIVE',
    default_language varchar(10) NOT NULL DEFAULT 'en',
    timezone varchar(80) NOT NULL DEFAULT 'Asia/Karachi',
    currency_code char(3) NOT NULL DEFAULT 'PKR',
    created_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE saas.school_branding (
    tenant_id uuid PRIMARY KEY REFERENCES saas.tenant(tenant_id),
    logo_url text,
    small_logo_url text,
    favicon_url text,
    certificate_logo_url text,
    letterhead_url text,
    watermark_url text,
    primary_color varchar(20),
    secondary_color varchar(20),
    accent_color varchar(20),
    footer_text text
);

CREATE TABLE org.campus (
    campus_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(50) NOT NULL,
    name varchar(200) NOT NULL,
    address text,
    phone varchar(50),
    email varchar(200),
    is_active boolean NOT NULL DEFAULT true,
    UNIQUE(tenant_id,code)
);

CREATE TABLE org.department (
    department_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    campus_id uuid REFERENCES org.campus(campus_id),
    code varchar(50) NOT NULL,
    name varchar(150) NOT NULL,
    UNIQUE(tenant_id,code)
);

CREATE TABLE org.room (
    room_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    campus_id uuid NOT NULL REFERENCES org.campus(campus_id),
    code varchar(50) NOT NULL,
    name varchar(120) NOT NULL,
    capacity int,
    room_type varchar(40),
    UNIQUE(campus_id,code)
);

-- =========================================================
-- IDENTITY REFERENCE / AUTHORIZATION
-- =========================================================
CREATE TABLE identity_ref.user_profile (
    user_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    identity_subject_id varchar(200) NOT NULL UNIQUE,
    display_name varchar(200),
    email varchar(250),
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE identity_ref.tenant_membership (
    tenant_membership_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    user_id uuid NOT NULL REFERENCES identity_ref.user_profile(user_id),
    campus_id uuid REFERENCES org.campus(campus_id),
    is_active boolean NOT NULL DEFAULT true,
    UNIQUE(tenant_id,user_id,campus_id)
);

CREATE TABLE identity_ref.role (
    role_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid REFERENCES saas.tenant(tenant_id),
    code varchar(80) NOT NULL,
    name varchar(120) NOT NULL,
    UNIQUE(tenant_id,code)
);

CREATE TABLE identity_ref.permission (
    permission_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    code varchar(150) NOT NULL UNIQUE,
    name varchar(200) NOT NULL
);

CREATE TABLE identity_ref.user_role (
    tenant_membership_id uuid NOT NULL REFERENCES identity_ref.tenant_membership(tenant_membership_id),
    role_id uuid NOT NULL REFERENCES identity_ref.role(role_id),
    PRIMARY KEY(tenant_membership_id,role_id)
);

CREATE TABLE identity_ref.role_permission (
    role_id uuid NOT NULL REFERENCES identity_ref.role(role_id),
    permission_id uuid NOT NULL REFERENCES identity_ref.permission(permission_id),
    PRIMARY KEY(role_id,permission_id)
);

-- =========================================================
-- ACADEMIC SYSTEM / PROGRAM / COURSE / TIMETABLE
-- =========================================================
CREATE TABLE academic.academic_system (
    academic_system_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(50) NOT NULL,
    name varchar(150) NOT NULL,
    system_type_code varchar(40) NOT NULL,
    is_active boolean NOT NULL DEFAULT true,
    UNIQUE(tenant_id,code)
);

CREATE TABLE academic.education_board (
    education_board_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(50) NOT NULL,
    name varchar(200) NOT NULL,
    UNIQUE(tenant_id,code)
);

CREATE TABLE academic.program (
    program_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    academic_system_id uuid NOT NULL REFERENCES academic.academic_system(academic_system_id),
    code varchar(50) NOT NULL,
    name varchar(150) NOT NULL,
    description text,
    is_active boolean NOT NULL DEFAULT true,
    UNIQUE(tenant_id,code)
);

CREATE TABLE academic.campus_program (
    campus_program_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    campus_id uuid NOT NULL REFERENCES org.campus(campus_id),
    program_id uuid NOT NULL REFERENCES academic.program(program_id),
    effective_from date,
    effective_to date,
    is_active boolean NOT NULL DEFAULT true,
    UNIQUE(campus_id,program_id,effective_from)
);

CREATE TABLE academic.grade_level (
    grade_level_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(50) NOT NULL,
    name varchar(120) NOT NULL,
    sort_order int NOT NULL DEFAULT 0,
    UNIQUE(tenant_id,code)
);

CREATE TABLE academic.program_grade (
    program_grade_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    program_id uuid NOT NULL REFERENCES academic.program(program_id),
    grade_level_id uuid NOT NULL REFERENCES academic.grade_level(grade_level_id),
    sort_order int NOT NULL DEFAULT 0,
    UNIQUE(program_id,grade_level_id)
);

CREATE TABLE academic.subject (
    subject_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(50) NOT NULL,
    name varchar(150) NOT NULL,
    short_name varchar(50),
    is_practical boolean NOT NULL DEFAULT false,
    is_active boolean NOT NULL DEFAULT true,
    UNIQUE(tenant_id,code)
);

CREATE TABLE academic.program_subject (
    program_subject_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    program_grade_id uuid NOT NULL REFERENCES academic.program_grade(program_grade_id),
    subject_id uuid NOT NULL REFERENCES academic.subject(subject_id),
    requirement_type_code varchar(30) NOT NULL,
    periods_per_week int,
    minimum_pass_marks numeric(7,2),
    display_order int NOT NULL DEFAULT 0,
    is_active boolean NOT NULL DEFAULT true,
    UNIQUE(program_grade_id,subject_id)
);

CREATE TABLE academic.course_selection_group (
    selection_group_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    program_grade_id uuid NOT NULL REFERENCES academic.program_grade(program_grade_id),
    name varchar(150) NOT NULL,
    min_selections int NOT NULL DEFAULT 0,
    max_selections int NOT NULL,
    requires_approval boolean NOT NULL DEFAULT false
);

CREATE TABLE academic.course_selection_group_course (
    selection_group_id uuid NOT NULL REFERENCES academic.course_selection_group(selection_group_id),
    program_subject_id uuid NOT NULL REFERENCES academic.program_subject(program_subject_id),
    PRIMARY KEY(selection_group_id,program_subject_id)
);

CREATE TABLE academic.academic_year (
    academic_year_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    campus_id uuid NOT NULL REFERENCES org.campus(campus_id),
    name varchar(80) NOT NULL,
    start_date date NOT NULL,
    end_date date NOT NULL,
    is_current boolean NOT NULL DEFAULT false,
    UNIQUE(campus_id,name)
);

CREATE TABLE academic.term (
    term_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    academic_year_id uuid NOT NULL REFERENCES academic.academic_year(academic_year_id),
    code varchar(40) NOT NULL,
    name varchar(100) NOT NULL,
    start_date date NOT NULL,
    end_date date NOT NULL,
    UNIQUE(academic_year_id,code)
);

CREATE TABLE academic.section (
    section_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(30) NOT NULL,
    name varchar(80) NOT NULL,
    UNIQUE(tenant_id,code)
);

CREATE TABLE academic.class_section (
    class_section_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    campus_id uuid NOT NULL REFERENCES org.campus(campus_id),
    academic_year_id uuid NOT NULL REFERENCES academic.academic_year(academic_year_id),
    program_grade_id uuid NOT NULL REFERENCES academic.program_grade(program_grade_id),
    section_id uuid NOT NULL REFERENCES academic.section(section_id),
    class_teacher_employee_id uuid,
    room_id uuid REFERENCES org.room(room_id),
    capacity int,
    status varchar(30) NOT NULL DEFAULT 'ACTIVE',
    UNIQUE(academic_year_id,program_grade_id,section_id)
);

CREATE TABLE academic.course_offering (
    course_offering_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    campus_id uuid NOT NULL REFERENCES org.campus(campus_id),
    academic_year_id uuid NOT NULL REFERENCES academic.academic_year(academic_year_id),
    term_id uuid REFERENCES academic.term(term_id),
    program_subject_id uuid NOT NULL REFERENCES academic.program_subject(program_subject_id),
    display_name varchar(150),
    status varchar(30) NOT NULL DEFAULT 'ACTIVE'
);

-- =========================================================
-- STUDENT / GUARDIAN / COURSE SELECTION
-- =========================================================
CREATE TABLE student.student (
    student_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    user_id uuid REFERENCES identity_ref.user_profile(user_id),
    student_number varchar(60) NOT NULL,
    first_name varchar(100) NOT NULL,
    last_name varchar(100),
    date_of_birth date,
    gender varchar(30),
    admission_date date,
    status varchar(30) NOT NULL DEFAULT 'ACTIVE',
    UNIQUE(tenant_id,student_number)
);

CREATE TABLE student.guardian (
    guardian_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    user_id uuid REFERENCES identity_ref.user_profile(user_id),
    full_name varchar(200) NOT NULL,
    email varchar(250),
    phone varchar(50)
);

CREATE TABLE student.student_guardian (
    student_id uuid NOT NULL REFERENCES student.student(student_id),
    guardian_id uuid NOT NULL REFERENCES student.guardian(guardian_id),
    relationship varchar(60) NOT NULL,
    is_primary boolean NOT NULL DEFAULT false,
    can_view_academics boolean NOT NULL DEFAULT true,
    can_view_finance boolean NOT NULL DEFAULT true,
    can_pickup boolean NOT NULL DEFAULT false,
    PRIMARY KEY(student_id,guardian_id)
);

CREATE TABLE student.student_enrollment (
    student_enrollment_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    student_id uuid NOT NULL REFERENCES student.student(student_id),
    academic_year_id uuid NOT NULL REFERENCES academic.academic_year(academic_year_id),
    class_section_id uuid NOT NULL REFERENCES academic.class_section(class_section_id),
    enrollment_date date NOT NULL DEFAULT CURRENT_DATE,
    status varchar(30) NOT NULL DEFAULT 'ACTIVE',
    UNIQUE(student_id,academic_year_id)
);

CREATE TABLE student.student_course_enrollment (
    student_course_enrollment_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    student_enrollment_id uuid NOT NULL REFERENCES student.student_enrollment(student_enrollment_id),
    course_offering_id uuid NOT NULL REFERENCES academic.course_offering(course_offering_id),
    enrollment_type_code varchar(30) NOT NULL,
    selected_at timestamptz NOT NULL DEFAULT now(),
    approved_by uuid REFERENCES identity_ref.user_profile(user_id),
    approved_at timestamptz,
    status varchar(30) NOT NULL DEFAULT 'ACTIVE',
    UNIQUE(student_enrollment_id,course_offering_id)
);

-- =========================================================
-- HR JOB ARCHITECTURE / RECRUITMENT
-- =========================================================
CREATE TABLE hr.job_family (
    job_family_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(50) NOT NULL,
    name varchar(120) NOT NULL,
    UNIQUE(tenant_id,code)
);

CREATE TABLE hr.job_grade (
    job_grade_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(50) NOT NULL,
    name varchar(120) NOT NULL,
    grade_level int,
    minimum_salary numeric(18,2),
    midpoint_salary numeric(18,2),
    maximum_salary numeric(18,2),
    currency_code char(3) NOT NULL DEFAULT 'PKR',
    is_active boolean NOT NULL DEFAULT true,
    UNIQUE(tenant_id,code)
);

CREATE TABLE hr.job (
    job_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    department_id uuid REFERENCES org.department(department_id),
    job_family_id uuid REFERENCES hr.job_family(job_family_id),
    code varchar(50) NOT NULL,
    title varchar(150) NOT NULL,
    description text,
    responsibilities text,
    minimum_qualification text,
    minimum_experience_years numeric(5,2),
    is_teaching_position boolean NOT NULL DEFAULT false,
    is_active boolean NOT NULL DEFAULT true,
    UNIQUE(tenant_id,code)
);

CREATE TABLE hr.job_grade_mapping (
    job_id uuid NOT NULL REFERENCES hr.job(job_id),
    job_grade_id uuid NOT NULL REFERENCES hr.job_grade(job_grade_id),
    is_default boolean NOT NULL DEFAULT false,
    effective_from date,
    effective_to date,
    PRIMARY KEY(job_id,job_grade_id)
);

CREATE TABLE hr.position (
    position_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    campus_id uuid NOT NULL REFERENCES org.campus(campus_id),
    department_id uuid REFERENCES org.department(department_id),
    job_id uuid NOT NULL REFERENCES hr.job(job_id),
    job_grade_id uuid REFERENCES hr.job_grade(job_grade_id),
    reports_to_position_id uuid REFERENCES hr.position(position_id),
    position_code varchar(60) NOT NULL,
    headcount int NOT NULL DEFAULT 1,
    status varchar(30) NOT NULL DEFAULT 'ACTIVE',
    UNIQUE(tenant_id,position_code)
);

CREATE TABLE hr.employee (
    employee_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    user_id uuid REFERENCES identity_ref.user_profile(user_id),
    employee_number varchar(60) NOT NULL,
    first_name varchar(100) NOT NULL,
    last_name varchar(100),
    email varchar(250),
    phone varchar(50),
    hire_date date NOT NULL,
    employment_type_code varchar(30) NOT NULL,
    status varchar(30) NOT NULL DEFAULT 'ACTIVE',
    source_candidate_id uuid,
    UNIQUE(tenant_id,employee_number)
);

ALTER TABLE academic.class_section
ADD CONSTRAINT fk_class_teacher FOREIGN KEY(class_teacher_employee_id) REFERENCES hr.employee(employee_id);

CREATE TABLE hr.employee_position (
    employee_position_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    employee_id uuid NOT NULL REFERENCES hr.employee(employee_id),
    position_id uuid NOT NULL REFERENCES hr.position(position_id),
    effective_from date NOT NULL,
    effective_to date,
    is_primary boolean NOT NULL DEFAULT true,
    change_reason varchar(150),
    status varchar(30) NOT NULL DEFAULT 'ACTIVE'
);

CREATE UNIQUE INDEX ux_employee_primary_current_position
ON hr.employee_position(employee_id)
WHERE is_primary = true AND effective_to IS NULL;

CREATE TABLE academic.teacher_course_assignment (
    teacher_course_assignment_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    course_offering_id uuid NOT NULL REFERENCES academic.course_offering(course_offering_id),
    employee_id uuid NOT NULL REFERENCES hr.employee(employee_id),
    class_section_id uuid REFERENCES academic.class_section(class_section_id),
    teaching_group_id uuid,
    assignment_role varchar(40) NOT NULL DEFAULT 'PRIMARY',
    periods_per_week int,
    effective_from date,
    effective_to date,
    is_primary boolean NOT NULL DEFAULT true
);

CREATE TABLE hr.candidate (
    candidate_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    first_name varchar(100) NOT NULL,
    last_name varchar(100),
    email varchar(250),
    phone varchar(50),
    current_job_title varchar(150),
    current_employer varchar(200),
    total_experience_years numeric(5,2),
    highest_qualification varchar(250),
    expected_salary numeric(18,2),
    notice_period_days int,
    status_code varchar(30) NOT NULL DEFAULT 'NEW',
    created_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE hr.candidate_document (
    candidate_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    candidate_id uuid NOT NULL REFERENCES hr.candidate(candidate_id),
    document_type varchar(50) NOT NULL,
    file_name varchar(255) NOT NULL,
    file_url text NOT NULL,
    mime_type varchar(120),
    size_bytes bigint,
    uploaded_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE hr.job_vacancy (
    job_vacancy_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    position_id uuid NOT NULL REFERENCES hr.position(position_id),
    number_of_positions int NOT NULL DEFAULT 1,
    opening_date date,
    closing_date date,
    status varchar(30) NOT NULL DEFAULT 'DRAFT'
);

CREATE TABLE hr.job_application (
    job_application_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    candidate_id uuid NOT NULL REFERENCES hr.candidate(candidate_id),
    job_vacancy_id uuid NOT NULL REFERENCES hr.job_vacancy(job_vacancy_id),
    application_date date NOT NULL DEFAULT CURRENT_DATE,
    status_code varchar(30) NOT NULL DEFAULT 'APPLIED',
    screening_score numeric(6,2),
    final_score numeric(6,2),
    rejection_reason text,
    eligible_for_future_opening boolean NOT NULL DEFAULT false,
    UNIQUE(candidate_id,job_vacancy_id)
);

CREATE TABLE hr.interview (
    interview_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    job_application_id uuid NOT NULL REFERENCES hr.job_application(job_application_id),
    interview_type_code varchar(40) NOT NULL,
    round_number int NOT NULL DEFAULT 1,
    scheduled_at timestamptz,
    duration_minutes int,
    location varchar(250),
    meeting_url text,
    status varchar(30) NOT NULL DEFAULT 'SCHEDULED',
    overall_score numeric(6,2),
    recommendation varchar(100),
    notes text
);

CREATE TABLE hr.interview_panel (
    interview_id uuid NOT NULL REFERENCES hr.interview(interview_id),
    employee_id uuid NOT NULL REFERENCES hr.employee(employee_id),
    PRIMARY KEY(interview_id,employee_id)
);

CREATE TABLE hr.interview_evaluation (
    interview_evaluation_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    interview_id uuid NOT NULL REFERENCES hr.interview(interview_id),
    interviewer_employee_id uuid NOT NULL REFERENCES hr.employee(employee_id),
    score numeric(6,2),
    strengths text,
    weaknesses text,
    comments text,
    recommendation varchar(100),
    submitted_at timestamptz
);

-- =========================================================
-- TEACHING GROUPS / TIMETABLE
-- =========================================================
CREATE TABLE academic.teaching_group (
    teaching_group_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    academic_year_id uuid NOT NULL REFERENCES academic.academic_year(academic_year_id),
    term_id uuid REFERENCES academic.term(term_id),
    course_offering_id uuid NOT NULL REFERENCES academic.course_offering(course_offering_id),
    name varchar(150) NOT NULL,
    capacity int,
    room_id uuid REFERENCES org.room(room_id),
    status varchar(30) NOT NULL DEFAULT 'ACTIVE'
);

ALTER TABLE academic.teacher_course_assignment
ADD CONSTRAINT fk_teacher_assignment_group FOREIGN KEY(teaching_group_id) REFERENCES academic.teaching_group(teaching_group_id);

CREATE TABLE academic.teaching_group_student (
    teaching_group_id uuid NOT NULL REFERENCES academic.teaching_group(teaching_group_id),
    student_course_enrollment_id uuid NOT NULL REFERENCES student.student_course_enrollment(student_course_enrollment_id),
    PRIMARY KEY(teaching_group_id,student_course_enrollment_id)
);

CREATE TABLE academic.timetable_period (
    timetable_period_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    campus_id uuid NOT NULL REFERENCES org.campus(campus_id),
    period_number int,
    name varchar(80) NOT NULL,
    start_time time NOT NULL,
    end_time time NOT NULL,
    period_type varchar(30) NOT NULL DEFAULT 'SUBJECT'
);

CREATE TABLE academic.timetable (
    timetable_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    campus_id uuid NOT NULL REFERENCES org.campus(campus_id),
    academic_year_id uuid NOT NULL REFERENCES academic.academic_year(academic_year_id),
    term_id uuid REFERENCES academic.term(term_id),
    name varchar(150) NOT NULL,
    effective_from date,
    effective_to date,
    status varchar(30) NOT NULL DEFAULT 'DRAFT'
);

CREATE TABLE academic.timetable_entry (
    timetable_entry_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    timetable_id uuid NOT NULL REFERENCES academic.timetable(timetable_id),
    day_of_week smallint NOT NULL CHECK(day_of_week BETWEEN 1 AND 7),
    timetable_period_id uuid NOT NULL REFERENCES academic.timetable_period(timetable_period_id),
    class_section_id uuid REFERENCES academic.class_section(class_section_id),
    teaching_group_id uuid REFERENCES academic.teaching_group(teaching_group_id),
    course_offering_id uuid REFERENCES academic.course_offering(course_offering_id),
    teacher_course_assignment_id uuid REFERENCES academic.teacher_course_assignment(teacher_course_assignment_id),
    room_id uuid REFERENCES org.room(room_id),
    entry_type varchar(30) NOT NULL DEFAULT 'SUBJECT',
    CHECK (class_section_id IS NOT NULL OR teaching_group_id IS NOT NULL)
);

-- =========================================================
-- LMS / ASSIGNMENTS
-- =========================================================
CREATE TABLE lms.academic_assignment (
    academic_assignment_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    course_offering_id uuid NOT NULL REFERENCES academic.course_offering(course_offering_id),
    class_section_id uuid REFERENCES academic.class_section(class_section_id),
    teaching_group_id uuid REFERENCES academic.teaching_group(teaching_group_id),
    teacher_employee_id uuid NOT NULL REFERENCES hr.employee(employee_id),
    assignment_type_code varchar(40) NOT NULL,
    title varchar(250) NOT NULL,
    description text,
    instructions text,
    assigned_at timestamptz NOT NULL DEFAULT now(),
    due_at timestamptz,
    total_marks numeric(8,2),
    allow_late_submission boolean NOT NULL DEFAULT false,
    max_attempts int NOT NULL DEFAULT 1,
    status varchar(30) NOT NULL DEFAULT 'DRAFT'
);

CREATE TABLE lms.student_assignment_submission (
    submission_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    academic_assignment_id uuid NOT NULL REFERENCES lms.academic_assignment(academic_assignment_id),
    student_id uuid NOT NULL REFERENCES student.student(student_id),
    attempt_no int NOT NULL DEFAULT 1,
    submitted_at timestamptz,
    submission_text text,
    marks_obtained numeric(8,2),
    teacher_feedback text,
    status varchar(30) NOT NULL DEFAULT 'DRAFT',
    UNIQUE(academic_assignment_id,student_id,attempt_no)
);

-- =========================================================
-- EXAMS / RESULTS
-- =========================================================
CREATE TABLE exam.exam (
    exam_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    campus_id uuid NOT NULL REFERENCES org.campus(campus_id),
    academic_year_id uuid NOT NULL REFERENCES academic.academic_year(academic_year_id),
    term_id uuid REFERENCES academic.term(term_id),
    academic_system_id uuid NOT NULL REFERENCES academic.academic_system(academic_system_id),
    exam_type_code varchar(40) NOT NULL,
    name varchar(180) NOT NULL,
    start_date date,
    end_date date,
    result_publish_date date,
    status varchar(30) NOT NULL DEFAULT 'DRAFT'
);

CREATE TABLE exam.exam_subject (
    exam_subject_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    exam_id uuid NOT NULL REFERENCES exam.exam(exam_id),
    course_offering_id uuid NOT NULL REFERENCES academic.course_offering(course_offering_id),
    exam_date date,
    start_time time,
    duration_minutes int,
    total_marks numeric(8,2) NOT NULL,
    passing_marks numeric(8,2),
    room_id uuid REFERENCES org.room(room_id)
);

CREATE TABLE exam.student_exam_result (
    student_exam_result_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    exam_subject_id uuid NOT NULL REFERENCES exam.exam_subject(exam_subject_id),
    student_id uuid NOT NULL REFERENCES student.student(student_id),
    marks_obtained numeric(8,2),
    percentage numeric(7,3),
    grade varchar(20),
    is_absent boolean NOT NULL DEFAULT false,
    remarks text,
    entered_by uuid REFERENCES identity_ref.user_profile(user_id),
    verified_by uuid REFERENCES identity_ref.user_profile(user_id),
    UNIQUE(exam_subject_id,student_id)
);

-- =========================================================
-- FEES
-- =========================================================
CREATE TABLE finance.fee_type (
    fee_type_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(50) NOT NULL,
    name varchar(120) NOT NULL,
    UNIQUE(tenant_id,code)
);

CREATE TABLE finance.student_invoice (
    student_invoice_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    student_id uuid NOT NULL REFERENCES student.student(student_id),
    academic_year_id uuid REFERENCES academic.academic_year(academic_year_id),
    invoice_number varchar(80) NOT NULL,
    invoice_date date NOT NULL,
    due_date date,
    status varchar(30) NOT NULL DEFAULT 'OPEN',
    total_amount numeric(18,2) NOT NULL DEFAULT 0,
    balance_amount numeric(18,2) NOT NULL DEFAULT 0,
    UNIQUE(tenant_id,invoice_number)
);

CREATE TABLE finance.student_invoice_line (
    student_invoice_line_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    student_invoice_id uuid NOT NULL REFERENCES finance.student_invoice(student_invoice_id),
    fee_type_id uuid NOT NULL REFERENCES finance.fee_type(fee_type_id),
    description varchar(250),
    amount numeric(18,2) NOT NULL
);

CREATE TABLE finance.student_payment (
    student_payment_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    student_id uuid NOT NULL REFERENCES student.student(student_id),
    payment_number varchar(80) NOT NULL,
    payment_date timestamptz NOT NULL DEFAULT now(),
    amount numeric(18,2) NOT NULL,
    payment_method varchar(40) NOT NULL,
    reference_no varchar(150),
    UNIQUE(tenant_id,payment_number)
);

CREATE TABLE finance.payment_allocation (
    payment_allocation_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    student_payment_id uuid NOT NULL REFERENCES finance.student_payment(student_payment_id),
    student_invoice_id uuid NOT NULL REFERENCES finance.student_invoice(student_invoice_id),
    amount numeric(18,2) NOT NULL
);

-- =========================================================
-- COMPENSATION / INCREMENTS / PAYROLL
-- =========================================================
CREATE TABLE hr.salary_component (
    salary_component_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(50) NOT NULL,
    name varchar(120) NOT NULL,
    component_type varchar(40) NOT NULL,
    calculation_type varchar(40) NOT NULL DEFAULT 'FIXED',
    taxable boolean NOT NULL DEFAULT false,
    is_recurring boolean NOT NULL DEFAULT true,
    UNIQUE(tenant_id,code)
);

CREATE TABLE hr.employee_compensation (
    employee_compensation_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    employee_id uuid NOT NULL REFERENCES hr.employee(employee_id),
    job_grade_id uuid REFERENCES hr.job_grade(job_grade_id),
    effective_from date NOT NULL,
    effective_to date,
    basic_salary numeric(18,2) NOT NULL,
    gross_salary numeric(18,2),
    currency_code char(3) NOT NULL DEFAULT 'PKR',
    status varchar(30) NOT NULL DEFAULT 'ACTIVE'
);

CREATE TABLE hr.employee_salary_component (
    employee_compensation_id uuid NOT NULL REFERENCES hr.employee_compensation(employee_compensation_id),
    salary_component_id uuid NOT NULL REFERENCES hr.salary_component(salary_component_id),
    amount numeric(18,2),
    percentage numeric(9,4),
    formula text,
    PRIMARY KEY(employee_compensation_id,salary_component_id)
);

CREATE TABLE hr.increment_policy (
    increment_policy_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    name varchar(150) NOT NULL,
    frequency varchar(30) NOT NULL DEFAULT 'ANNUAL',
    increment_type_code varchar(30) NOT NULL,
    increment_value numeric(18,4),
    minimum_service_months int NOT NULL DEFAULT 12,
    minimum_performance_score numeric(6,2),
    requires_hr_approval boolean NOT NULL DEFAULT true,
    requires_finance_approval boolean NOT NULL DEFAULT false,
    requires_principal_approval boolean NOT NULL DEFAULT true,
    is_automatic boolean NOT NULL DEFAULT false,
    is_active boolean NOT NULL DEFAULT true
);

CREATE TABLE hr.salary_increment_request (
    increment_request_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    employee_id uuid NOT NULL REFERENCES hr.employee(employee_id),
    increment_policy_id uuid REFERENCES hr.increment_policy(increment_policy_id),
    request_type_code varchar(20) NOT NULL,
    increment_type_code varchar(30) NOT NULL,
    current_basic_salary numeric(18,2) NOT NULL,
    percentage numeric(9,4),
    increment_amount numeric(18,2),
    proposed_basic_salary numeric(18,2) NOT NULL,
    effective_date date NOT NULL,
    reason text,
    requested_by uuid REFERENCES identity_ref.user_profile(user_id),
    requested_at timestamptz NOT NULL DEFAULT now(),
    status_code varchar(30) NOT NULL DEFAULT 'PENDING'
);

CREATE TABLE hr.increment_approval (
    increment_approval_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    increment_request_id uuid NOT NULL REFERENCES hr.salary_increment_request(increment_request_id),
    approval_level int NOT NULL,
    approver_user_id uuid NOT NULL REFERENCES identity_ref.user_profile(user_id),
    decision varchar(30),
    comments text,
    decision_at timestamptz
);

CREATE TABLE payroll.payroll_period (
    payroll_period_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    year int NOT NULL,
    month int NOT NULL CHECK(month BETWEEN 1 AND 12),
    start_date date NOT NULL,
    end_date date NOT NULL,
    UNIQUE(tenant_id,year,month)
);

CREATE TABLE payroll.payroll_run (
    payroll_run_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    payroll_period_id uuid NOT NULL REFERENCES payroll.payroll_period(payroll_period_id),
    status_code varchar(30) NOT NULL DEFAULT 'DRAFT',
    created_at timestamptz NOT NULL DEFAULT now(),
    approved_by uuid REFERENCES identity_ref.user_profile(user_id),
    approved_at timestamptz
);

CREATE TABLE payroll.employee_payroll (
    employee_payroll_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    payroll_run_id uuid NOT NULL REFERENCES payroll.payroll_run(payroll_run_id),
    employee_id uuid NOT NULL REFERENCES hr.employee(employee_id),
    gross_amount numeric(18,2) NOT NULL DEFAULT 0,
    deduction_amount numeric(18,2) NOT NULL DEFAULT 0,
    net_amount numeric(18,2) NOT NULL DEFAULT 0,
    UNIQUE(payroll_run_id,employee_id)
);

CREATE TABLE payroll.payroll_line_item (
    payroll_line_item_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    employee_payroll_id uuid NOT NULL REFERENCES payroll.employee_payroll(employee_payroll_id),
    salary_component_id uuid REFERENCES hr.salary_component(salary_component_id),
    description varchar(200),
    amount numeric(18,2) NOT NULL
);

-- =========================================================
-- GENERIC WORK ASSIGNMENTS
-- =========================================================
CREATE TABLE workflow.work_assignment (
    work_assignment_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    campus_id uuid REFERENCES org.campus(campus_id),
    title varchar(250) NOT NULL,
    description text,
    assigned_by_user_id uuid NOT NULL REFERENCES identity_ref.user_profile(user_id),
    assigned_to_user_id uuid REFERENCES identity_ref.user_profile(user_id),
    priority varchar(30) NOT NULL DEFAULT 'NORMAL',
    status_code varchar(30) NOT NULL DEFAULT 'ASSIGNED',
    assigned_at timestamptz NOT NULL DEFAULT now(),
    due_at timestamptz,
    completed_at timestamptz,
    related_entity_type varchar(100),
    related_entity_id uuid
);

-- =========================================================
-- DOCUMENT TEMPLATES / CERTIFICATES / LETTERS
-- =========================================================
CREATE TABLE document.document_template (
    document_template_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    campus_id uuid REFERENCES org.campus(campus_id),
    academic_system_id uuid REFERENCES academic.academic_system(academic_system_id),
    document_type_code varchar(50) NOT NULL,
    code varchar(80) NOT NULL,
    name varchar(180) NOT NULL,
    subject_template text,
    header_html text,
    body_html text NOT NULL,
    footer_html text,
    language_code varchar(10) NOT NULL DEFAULT 'en',
    version int NOT NULL DEFAULT 1,
    requires_approval boolean NOT NULL DEFAULT false,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    UNIQUE(tenant_id,code,version)
);

CREATE TABLE document.generated_document (
    generated_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    document_template_id uuid NOT NULL REFERENCES document.document_template(document_template_id),
    template_version int NOT NULL,
    student_id uuid REFERENCES student.student(student_id),
    employee_id uuid REFERENCES hr.employee(employee_id),
    document_number varchar(100) NOT NULL,
    rendered_content_snapshot text NOT NULL,
    file_url text,
    verification_code varchar(100),
    issued_by uuid REFERENCES identity_ref.user_profile(user_id),
    approved_by uuid REFERENCES identity_ref.user_profile(user_id),
    issued_at timestamptz,
    status varchar(30) NOT NULL DEFAULT 'DRAFT',
    UNIQUE(tenant_id,document_number),
    UNIQUE(verification_code)
);

-- =========================================================
-- ACTIVITIES / AWARDS
-- =========================================================
CREATE TABLE activity.activity (
    activity_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    campus_id uuid REFERENCES org.campus(campus_id),
    name varchar(180) NOT NULL,
    category varchar(100),
    coordinator_employee_id uuid REFERENCES hr.employee(employee_id),
    is_active boolean NOT NULL DEFAULT true
);

CREATE TABLE activity.student_activity (
    activity_id uuid NOT NULL REFERENCES activity.activity(activity_id),
    student_id uuid NOT NULL REFERENCES student.student(student_id),
    role_name varchar(100),
    joined_at date,
    left_at date,
    PRIMARY KEY(activity_id,student_id)
);

CREATE TABLE activity.student_award (
    student_award_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    student_id uuid NOT NULL REFERENCES student.student(student_id),
    award_type_code varchar(50) NOT NULL,
    title varchar(180) NOT NULL,
    description text,
    award_date date NOT NULL,
    approved_by uuid REFERENCES identity_ref.user_profile(user_id),
    generated_document_id uuid REFERENCES document.generated_document(generated_document_id)
);

-- =========================================================
-- REAL-TIME CHAT / NOTIFICATIONS
-- =========================================================
CREATE TABLE communication.conversation (
    conversation_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    campus_id uuid REFERENCES org.campus(campus_id),
    conversation_type_code varchar(40) NOT NULL,
    student_id uuid REFERENCES student.student(student_id),
    class_section_id uuid REFERENCES academic.class_section(class_section_id),
    subject_id uuid REFERENCES academic.subject(subject_id),
    title varchar(200),
    created_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE communication.conversation_participant (
    conversation_id uuid NOT NULL REFERENCES communication.conversation(conversation_id),
    user_id uuid NOT NULL REFERENCES identity_ref.user_profile(user_id),
    joined_at timestamptz NOT NULL DEFAULT now(),
    left_at timestamptz,
    PRIMARY KEY(conversation_id,user_id)
);

CREATE TABLE communication.message (
    message_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    conversation_id uuid NOT NULL REFERENCES communication.conversation(conversation_id),
    sender_user_id uuid NOT NULL REFERENCES identity_ref.user_profile(user_id),
    reply_to_message_id uuid REFERENCES communication.message(message_id),
    message_type_code varchar(30) NOT NULL DEFAULT 'TEXT',
    body text,
    sent_at timestamptz NOT NULL DEFAULT now(),
    edited_at timestamptz,
    deleted_at timestamptz
);

CREATE TABLE communication.message_receipt (
    message_id uuid NOT NULL REFERENCES communication.message(message_id),
    user_id uuid NOT NULL REFERENCES identity_ref.user_profile(user_id),
    delivered_at timestamptz,
    read_at timestamptz,
    PRIMARY KEY(message_id,user_id)
);

CREATE TABLE communication.notification (
    notification_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    user_id uuid NOT NULL REFERENCES identity_ref.user_profile(user_id),
    title varchar(250) NOT NULL,
    body text,
    channel_code varchar(30) NOT NULL,
    status varchar(30) NOT NULL DEFAULT 'QUEUED',
    created_at timestamptz NOT NULL DEFAULT now(),
    sent_at timestamptz
);

-- =========================================================
-- LIBRARY / INVENTORY / TRANSPORT (core)
-- =========================================================
CREATE TABLE library.book (
    book_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    isbn varchar(30),
    title varchar(250) NOT NULL,
    author_text varchar(250),
    publisher_text varchar(250)
);

CREATE TABLE library.book_copy (
    book_copy_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    book_id uuid NOT NULL REFERENCES library.book(book_id),
    campus_id uuid NOT NULL REFERENCES org.campus(campus_id),
    barcode varchar(100) NOT NULL,
    status varchar(30) NOT NULL DEFAULT 'AVAILABLE',
    UNIQUE(campus_id,barcode)
);

CREATE TABLE library.book_loan (
    book_loan_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    book_copy_id uuid NOT NULL REFERENCES library.book_copy(book_copy_id),
    student_id uuid REFERENCES student.student(student_id),
    employee_id uuid REFERENCES hr.employee(employee_id),
    issued_at timestamptz NOT NULL DEFAULT now(),
    due_at timestamptz NOT NULL,
    returned_at timestamptz,
    CHECK ((student_id IS NOT NULL) <> (employee_id IS NOT NULL))
);

CREATE TABLE inventory.item (
    item_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(60) NOT NULL,
    name varchar(180) NOT NULL,
    unit varchar(30),
    reorder_level numeric(18,3),
    UNIQUE(tenant_id,code)
);

CREATE TABLE transport.vehicle (
    vehicle_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    campus_id uuid NOT NULL REFERENCES org.campus(campus_id),
    registration_no varchar(80) NOT NULL,
    capacity int,
    status varchar(30) NOT NULL DEFAULT 'ACTIVE',
    UNIQUE(tenant_id,registration_no)
);

CREATE TABLE transport.route (
    route_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    campus_id uuid NOT NULL REFERENCES org.campus(campus_id),
    code varchar(50) NOT NULL,
    name varchar(150) NOT NULL,
    UNIQUE(campus_id,code)
);

-- =========================================================
-- SHARED AI CORE
-- =========================================================
CREATE TABLE ai_core.model_configuration (
 model_configuration_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 tenant_id uuid REFERENCES saas.tenant(tenant_id),
 code varchar(80) NOT NULL, provider varchar(80) NOT NULL,
 model_name varchar(150) NOT NULL, configuration jsonb,
 is_active boolean NOT NULL DEFAULT true, UNIQUE(tenant_id,code)
);
CREATE TABLE ai_core.prompt_template (
 prompt_template_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 tenant_id uuid REFERENCES saas.tenant(tenant_id),
 assistant_type varchar(50) NOT NULL, prompt_type varchar(30) NOT NULL,
 code varchar(100) NOT NULL, prompt_text text NOT NULL,
 version int NOT NULL DEFAULT 1, is_active boolean NOT NULL DEFAULT true,
 UNIQUE(tenant_id,code,version)
);
CREATE TABLE ai_core.knowledge_collection (
 knowledge_collection_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
 code varchar(80) NOT NULL, name varchar(150) NOT NULL,
 description text, access_scope varchar(50) NOT NULL DEFAULT 'TENANT',
 UNIQUE(tenant_id,code)
);
CREATE TABLE ai_core.knowledge_document (
 knowledge_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 knowledge_collection_id uuid NOT NULL REFERENCES ai_core.knowledge_collection(knowledge_collection_id),
 tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
 campus_id uuid REFERENCES org.campus(campus_id),
 academic_system_id uuid REFERENCES academic.academic_system(academic_system_id),
 title varchar(250) NOT NULL, document_type varchar(80), source_url text,
 metadata jsonb, status varchar(30) NOT NULL DEFAULT 'ACTIVE'
);
CREATE TABLE ai_core.knowledge_chunk (
 knowledge_chunk_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 knowledge_document_id uuid NOT NULL REFERENCES ai_core.knowledge_document(knowledge_document_id),
 chunk_index int NOT NULL, content text NOT NULL, metadata jsonb,
 embedding_reference varchar(250), UNIQUE(knowledge_document_id,chunk_index)
);
CREATE TABLE ai_core.tool_definition (
 tool_definition_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 code varchar(100) NOT NULL UNIQUE, name varchar(150) NOT NULL,
 description text, handler_key varchar(200) NOT NULL,
 requires_user_authorization boolean NOT NULL DEFAULT true,
 requires_human_approval boolean NOT NULL DEFAULT false,
 is_active boolean NOT NULL DEFAULT true
);
CREATE TABLE ai_core.assistant_tool (
 assistant_tool_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 tenant_id uuid REFERENCES saas.tenant(tenant_id),
 assistant_type varchar(50) NOT NULL,
 tool_definition_id uuid NOT NULL REFERENCES ai_core.tool_definition(tool_definition_id),
 is_enabled boolean NOT NULL DEFAULT true,
 UNIQUE(tenant_id,assistant_type,tool_definition_id)
);
CREATE TABLE ai_core.assistant_knowledge_collection (
 assistant_knowledge_collection_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
 assistant_type varchar(50) NOT NULL,
 knowledge_collection_id uuid NOT NULL REFERENCES ai_core.knowledge_collection(knowledge_collection_id),
 UNIQUE(tenant_id,assistant_type,knowledge_collection_id)
);
CREATE TABLE ai_core.ai_execution_log (
 ai_execution_log_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
 assistant_type varchar(50) NOT NULL, conversation_reference_id uuid,
 user_id uuid REFERENCES identity_ref.user_profile(user_id),
 model_configuration_id uuid REFERENCES ai_core.model_configuration(model_configuration_id),
 prompt_tokens int, completion_tokens int, total_tokens int,
 estimated_cost numeric(18,8), latency_ms int, status varchar(30) NOT NULL,
 correlation_id varchar(100), created_at timestamptz NOT NULL DEFAULT now()
);
CREATE TABLE ai_core.tool_execution (
 tool_execution_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 ai_execution_log_id uuid REFERENCES ai_core.ai_execution_log(ai_execution_log_id),
 tool_definition_id uuid NOT NULL REFERENCES ai_core.tool_definition(tool_definition_id),
 input_payload jsonb, output_payload jsonb, status varchar(30) NOT NULL,
 error_message text, started_at timestamptz NOT NULL DEFAULT now(), completed_at timestamptz
);

-- STUDENT TUTOR
CREATE TABLE ai_tutor.tutor_conversation (
 tutor_conversation_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
 student_id uuid NOT NULL REFERENCES student.student(student_id),
 academic_year_id uuid REFERENCES academic.academic_year(academic_year_id),
 course_offering_id uuid REFERENCES academic.course_offering(course_offering_id),
 subject_id uuid REFERENCES academic.subject(subject_id),
 title varchar(250), started_at timestamptz NOT NULL DEFAULT now(),
 ended_at timestamptz, status varchar(30) NOT NULL DEFAULT 'ACTIVE'
);
CREATE TABLE ai_tutor.tutor_message (
 tutor_message_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 tutor_conversation_id uuid NOT NULL REFERENCES ai_tutor.tutor_conversation(tutor_conversation_id),
 role varchar(20) NOT NULL CHECK(role IN ('system','user','assistant','tool')),
 content text, created_at timestamptz NOT NULL DEFAULT now()
);
CREATE TABLE ai_tutor.tutor_message_reference (
 tutor_message_reference_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 tutor_message_id uuid NOT NULL REFERENCES ai_tutor.tutor_message(tutor_message_id),
 knowledge_chunk_id uuid REFERENCES ai_core.knowledge_chunk(knowledge_chunk_id),
 citation_label varchar(150), relevance_score numeric(10,6)
);
CREATE TABLE ai_tutor.tutor_session (
 tutor_session_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 tutor_conversation_id uuid NOT NULL REFERENCES ai_tutor.tutor_conversation(tutor_conversation_id),
 topic varchar(250), learning_objective text,
 started_at timestamptz NOT NULL DEFAULT now(), ended_at timestamptz, session_summary text
);
CREATE TABLE ai_tutor.tutor_feedback (
 tutor_feedback_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 tutor_message_id uuid NOT NULL REFERENCES ai_tutor.tutor_message(tutor_message_id),
 student_id uuid NOT NULL REFERENCES student.student(student_id),
 rating smallint CHECK(rating BETWEEN 1 AND 5), was_helpful boolean,
 comments text, created_at timestamptz NOT NULL DEFAULT now()
);
CREATE TABLE ai_tutor.student_topic_mastery (
 student_topic_mastery_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
 student_id uuid NOT NULL REFERENCES student.student(student_id),
 subject_id uuid NOT NULL REFERENCES academic.subject(subject_id),
 topic varchar(250) NOT NULL,
 mastery_score numeric(7,4) CHECK(mastery_score BETWEEN 0 AND 1),
 confidence_score numeric(7,4) CHECK(confidence_score BETWEEN 0 AND 1),
 evidence_count int NOT NULL DEFAULT 0, last_assessed_at timestamptz,
 UNIQUE(student_id,subject_id,topic)
);
CREATE TABLE ai_tutor.learning_recommendation (
 learning_recommendation_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 student_id uuid NOT NULL REFERENCES student.student(student_id),
 subject_id uuid REFERENCES academic.subject(subject_id), topic varchar(250),
 recommendation_type varchar(50) NOT NULL, recommendation_text text NOT NULL,
 priority int NOT NULL DEFAULT 0, status varchar(30) NOT NULL DEFAULT 'ACTIVE',
 created_at timestamptz NOT NULL DEFAULT now()
);
CREATE TABLE ai_tutor.generated_quiz (
 generated_quiz_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
 student_id uuid NOT NULL REFERENCES student.student(student_id),
 subject_id uuid NOT NULL REFERENCES academic.subject(subject_id),
 tutor_conversation_id uuid REFERENCES ai_tutor.tutor_conversation(tutor_conversation_id),
 topic varchar(250), difficulty varchar(30), created_at timestamptz NOT NULL DEFAULT now()
);
CREATE TABLE ai_tutor.generated_quiz_question (
 generated_quiz_question_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 generated_quiz_id uuid NOT NULL REFERENCES ai_tutor.generated_quiz(generated_quiz_id),
 sequence_no int NOT NULL, question_text text NOT NULL, question_type varchar(30) NOT NULL,
 options jsonb, expected_answer text, explanation text, UNIQUE(generated_quiz_id,sequence_no)
);
CREATE TABLE ai_tutor.student_quiz_attempt (
 student_quiz_attempt_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 generated_quiz_id uuid NOT NULL REFERENCES ai_tutor.generated_quiz(generated_quiz_id),
 student_id uuid NOT NULL REFERENCES student.student(student_id),
 started_at timestamptz NOT NULL DEFAULT now(), completed_at timestamptz,
 score numeric(7,3), answers jsonb
);

-- ADMISSION / ENQUIRY ASSISTANT
CREATE TABLE ai_inquiry.inquiry_conversation (
 inquiry_conversation_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
 campus_id uuid REFERENCES org.campus(campus_id),
 visitor_session_id varchar(150) NOT NULL,
 user_id uuid REFERENCES identity_ref.user_profile(user_id),
 visitor_name varchar(200), phone varchar(50), email varchar(250),
 interested_program_id uuid REFERENCES academic.program(program_id),
 started_at timestamptz NOT NULL DEFAULT now(), ended_at timestamptz,
 status varchar(30) NOT NULL DEFAULT 'ACTIVE'
);
CREATE TABLE ai_inquiry.inquiry_message (
 inquiry_message_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 inquiry_conversation_id uuid NOT NULL REFERENCES ai_inquiry.inquiry_conversation(inquiry_conversation_id),
 role varchar(20) NOT NULL CHECK(role IN ('system','user','assistant','tool')),
 content text, created_at timestamptz NOT NULL DEFAULT now()
);
CREATE TABLE ai_inquiry.lead_capture (
 lead_capture_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 inquiry_conversation_id uuid NOT NULL REFERENCES ai_inquiry.inquiry_conversation(inquiry_conversation_id),
 name varchar(200), phone varchar(50), email varchar(250),
 interested_campus_id uuid REFERENCES org.campus(campus_id),
 interested_program_id uuid REFERENCES academic.program(program_id),
 interested_grade_id uuid REFERENCES academic.grade_level(grade_level_id),
 notes text, captured_at timestamptz NOT NULL DEFAULT now(), converted_inquiry_id uuid
);
CREATE TABLE ai_inquiry.human_handoff (
 human_handoff_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 inquiry_conversation_id uuid NOT NULL REFERENCES ai_inquiry.inquiry_conversation(inquiry_conversation_id),
 requested_at timestamptz NOT NULL DEFAULT now(), reason text,
 assigned_to_user_id uuid REFERENCES identity_ref.user_profile(user_id),
 accepted_at timestamptz, resolved_at timestamptz,
 status varchar(30) NOT NULL DEFAULT 'REQUESTED'
);

-- PARENT ASSISTANT
CREATE TABLE ai_parent.parent_conversation (
 parent_conversation_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
 guardian_id uuid NOT NULL REFERENCES student.guardian(guardian_id),
 selected_student_id uuid REFERENCES student.student(student_id),
 title varchar(250), started_at timestamptz NOT NULL DEFAULT now(),
 ended_at timestamptz, status varchar(30) NOT NULL DEFAULT 'ACTIVE'
);
CREATE TABLE ai_parent.parent_message (
 parent_message_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 parent_conversation_id uuid NOT NULL REFERENCES ai_parent.parent_conversation(parent_conversation_id),
 role varchar(20) NOT NULL CHECK(role IN ('system','user','assistant','tool')),
 content text, created_at timestamptz NOT NULL DEFAULT now()
);
CREATE TABLE ai_parent.parent_tool_execution (
 parent_tool_execution_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 parent_conversation_id uuid NOT NULL REFERENCES ai_parent.parent_conversation(parent_conversation_id),
 tool_definition_id uuid NOT NULL REFERENCES ai_core.tool_definition(tool_definition_id),
 student_id uuid REFERENCES student.student(student_id),
 input_payload jsonb, output_payload jsonb, status varchar(30) NOT NULL,
 executed_at timestamptz NOT NULL DEFAULT now()
);

-- PREDICTIVE ML
CREATE TABLE ai.prediction_model (
 prediction_model_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 tenant_id uuid REFERENCES saas.tenant(tenant_id), code varchar(80) NOT NULL,
 name varchar(180) NOT NULL, prediction_type varchar(80) NOT NULL,
 is_active boolean NOT NULL DEFAULT true
);
CREATE TABLE ai.prediction (
 prediction_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
 prediction_model_id uuid NOT NULL REFERENCES ai.prediction_model(prediction_model_id),
 student_id uuid REFERENCES student.student(student_id),
 prediction_type varchar(80) NOT NULL, score numeric(10,6),
 risk_level varchar(30), explanation jsonb, predicted_at timestamptz NOT NULL DEFAULT now()
);

INSERT INTO ai_core.tool_definition(code,name,handler_key,requires_user_authorization,requires_human_approval) VALUES
('GET_STUDENT_SUBJECTS','Get Student Subjects','Tutor.GetStudentSubjects',true,false),
('SEARCH_COURSE_MATERIAL','Search Course Material','Tutor.SearchCourseMaterial',true,false),
('GENERATE_PRACTICE_QUIZ','Generate Practice Quiz','Tutor.GeneratePracticeQuiz',true,false),
('GET_STUDENT_PROGRESS','Get Student Progress','Tutor.GetStudentProgress',true,false),
('GET_PROGRAMS','Get School Programs','Inquiry.GetPrograms',false,false),
('GET_ADMISSION_INFO','Get Admission Information','Inquiry.GetAdmissionInfo',false,false),
('CREATE_ADMISSION_INQUIRY','Create Admission Inquiry','Inquiry.CreateAdmissionInquiry',false,true),
('REQUEST_HUMAN_HANDOFF','Request Human Handoff','Inquiry.RequestHumanHandoff',false,false),
('GET_CHILD_ATTENDANCE','Get Child Attendance','Parent.GetChildAttendance',true,false),
('GET_CHILD_RESULTS','Get Child Results','Parent.GetChildResults',true,false),
('GET_CHILD_TIMETABLE','Get Child Timetable','Parent.GetChildTimetable',true,false),
('GET_CHILD_FEE_BALANCE','Get Child Fee Balance','Parent.GetChildFeeBalance',true,false)
ON CONFLICT (code) DO NOTHING;

CREATE INDEX ix_tutor_conversation_student ON ai_tutor.tutor_conversation(student_id,started_at DESC);
CREATE INDEX ix_tutor_message_conversation ON ai_tutor.tutor_message(tutor_conversation_id,created_at);
CREATE INDEX ix_inquiry_conversation_session ON ai_inquiry.inquiry_conversation(tenant_id,visitor_session_id,started_at DESC);
CREATE INDEX ix_inquiry_message_conversation ON ai_inquiry.inquiry_message(inquiry_conversation_id,created_at);
CREATE INDEX ix_parent_conversation_guardian ON ai_parent.parent_conversation(guardian_id,started_at DESC);
CREATE INDEX ix_prediction_student_type ON ai.prediction(student_id,prediction_type,predicted_at DESC);


-- =========================================================
-- STUDENT PERFORMANCE INTELLIGENCE / PREDICTION
-- =========================================================

-- A prediction is immutable history: generate a new row instead of overwriting.
CREATE TABLE ai.student_performance_prediction (
    student_performance_prediction_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    student_id uuid NOT NULL REFERENCES student.student(student_id),
    academic_year_id uuid NOT NULL REFERENCES academic.academic_year(academic_year_id),
    term_id uuid REFERENCES academic.term(term_id),
    course_offering_id uuid NOT NULL REFERENCES academic.course_offering(course_offering_id),
    subject_id uuid NOT NULL REFERENCES academic.subject(subject_id),

    target_exam_id uuid REFERENCES exam.exam(exam_id),
    target_exam_subject_id uuid REFERENCES exam.exam_subject(exam_subject_id),
    target_exam_type_code varchar(40),
    target_date date,

    predicted_marks numeric(8,2),
    predicted_percentage numeric(7,3),
    predicted_grade varchar(20),
    lower_bound_percentage numeric(7,3),
    upper_bound_percentage numeric(7,3),
    confidence_score numeric(7,4) CHECK(confidence_score IS NULL OR confidence_score BETWEEN 0 AND 1),

    pass_probability numeric(7,4) CHECK(pass_probability IS NULL OR pass_probability BETWEEN 0 AND 1),
    fail_probability numeric(7,4) CHECK(fail_probability IS NULL OR fail_probability BETWEEN 0 AND 1),
    target_grade varchar(20),
    target_grade_probability numeric(7,4) CHECK(target_grade_probability IS NULL OR target_grade_probability BETWEEN 0 AND 1),

    trend varchar(30),              -- IMPROVING/STABLE/DECLINING
    risk_level varchar(30),         -- LOW/MEDIUM/HIGH/CRITICAL
    explanation_summary text,
    explanation jsonb,

    prediction_model_id uuid REFERENCES ai.prediction_model(prediction_model_id),
    model_version varchar(80),
    generated_at timestamptz NOT NULL DEFAULT now(),
    expires_at timestamptz
);

CREATE TABLE ai.prediction_evidence (
    prediction_evidence_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    student_performance_prediction_id uuid NOT NULL
        REFERENCES ai.student_performance_prediction(student_performance_prediction_id) ON DELETE CASCADE,
    evidence_type varchar(60) NOT NULL, -- EXAM/TEST/ASSIGNMENT/ATTENDANCE/TUTOR_MASTERY/TREND
    source_entity_type varchar(100),
    source_entity_id uuid,
    numeric_value numeric(18,6),
    text_value text,
    normalized_value numeric(10,6),
    weight numeric(10,6),
    occurred_at timestamptz,
    explanation text
);

-- Store grade probabilities when the model produces a full distribution.
CREATE TABLE ai.predicted_grade_probability (
    predicted_grade_probability_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    student_performance_prediction_id uuid NOT NULL
        REFERENCES ai.student_performance_prediction(student_performance_prediction_id) ON DELETE CASCADE,
    grade varchar(20) NOT NULL,
    probability numeric(7,4) NOT NULL CHECK(probability BETWEEN 0 AND 1),
    UNIQUE(student_performance_prediction_id, grade)
);

-- Snapshot of insights for an entire class/course.
CREATE TABLE ai.class_performance_insight (
    class_performance_insight_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    academic_year_id uuid NOT NULL REFERENCES academic.academic_year(academic_year_id),
    term_id uuid REFERENCES academic.term(term_id),
    class_section_id uuid NOT NULL REFERENCES academic.class_section(class_section_id),
    course_offering_id uuid NOT NULL REFERENCES academic.course_offering(course_offering_id),
    teacher_employee_id uuid REFERENCES hr.employee(employee_id),

    students_count int NOT NULL DEFAULT 0,
    on_track_count int NOT NULL DEFAULT 0,
    needs_attention_count int NOT NULL DEFAULT 0,
    high_risk_count int NOT NULL DEFAULT 0,
    predicted_class_average numeric(7,3),
    current_class_average numeric(7,3),
    trend varchar(30),
    summary text,
    generated_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE ai.topic_performance_insight (
    topic_performance_insight_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    class_performance_insight_id uuid NOT NULL
        REFERENCES ai.class_performance_insight(class_performance_insight_id) ON DELETE CASCADE,
    subject_id uuid NOT NULL REFERENCES academic.subject(subject_id),
    topic varchar(250) NOT NULL,
    average_mastery_score numeric(7,4),
    students_struggling_count int NOT NULL DEFAULT 0,
    students_mastered_count int NOT NULL DEFAULT 0,
    risk_level varchar(30),
    recommended_focus text
);

-- AI-generated recommendation to a teacher. Teacher remains decision maker.
CREATE TABLE ai.teaching_recommendation (
    teaching_recommendation_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    class_performance_insight_id uuid REFERENCES ai.class_performance_insight(class_performance_insight_id),
    class_section_id uuid NOT NULL REFERENCES academic.class_section(class_section_id),
    course_offering_id uuid NOT NULL REFERENCES academic.course_offering(course_offering_id),
    teacher_employee_id uuid NOT NULL REFERENCES hr.employee(employee_id),
    subject_id uuid REFERENCES academic.subject(subject_id),
    topic varchar(250),

    recommendation_type varchar(60) NOT NULL, -- REVISION/PRACTICE/GROUPING/QUIZ/ONE_TO_ONE/etc.
    title varchar(250) NOT NULL,
    recommendation_text text NOT NULL,
    rationale text,
    priority varchar(30) NOT NULL DEFAULT 'NORMAL',

    status varchar(30) NOT NULL DEFAULT 'PROPOSED', -- PROPOSED/ACCEPTED/REJECTED/COMPLETED
    generated_at timestamptz NOT NULL DEFAULT now(),
    reviewed_at timestamptz,
    reviewed_by uuid REFERENCES identity_ref.user_profile(user_id),
    teacher_comments text
);

-- Intervention can target one student or a group/class.
CREATE TABLE ai.student_intervention (
    student_intervention_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    student_id uuid NOT NULL REFERENCES student.student(student_id),
    subject_id uuid REFERENCES academic.subject(subject_id),
    course_offering_id uuid REFERENCES academic.course_offering(course_offering_id),
    teacher_employee_id uuid REFERENCES hr.employee(employee_id),
    source_prediction_id uuid REFERENCES ai.student_performance_prediction(student_performance_prediction_id),
    source_recommendation_id uuid REFERENCES ai.teaching_recommendation(teaching_recommendation_id),

    title varchar(250) NOT NULL,
    reason text,
    target_outcome text,
    start_date date,
    target_date date,
    status varchar(30) NOT NULL DEFAULT 'PLANNED',
    created_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE ai.intervention_action (
    intervention_action_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    student_intervention_id uuid NOT NULL
        REFERENCES ai.student_intervention(student_intervention_id) ON DELETE CASCADE,
    sequence_no int NOT NULL,
    action_type varchar(60) NOT NULL, -- LESSON/WORKSHEET/QUIZ/TUTOR_SESSION/TEACHER_SESSION/etc.
    description text NOT NULL,
    related_entity_type varchar(100),
    related_entity_id uuid,
    due_at timestamptz,
    completed_at timestamptz,
    status varchar(30) NOT NULL DEFAULT 'PENDING',
    UNIQUE(student_intervention_id, sequence_no)
);

CREATE TABLE ai.intervention_outcome (
    intervention_outcome_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    student_intervention_id uuid NOT NULL REFERENCES ai.student_intervention(student_intervention_id),
    measured_at timestamptz NOT NULL DEFAULT now(),
    before_score numeric(7,3),
    after_score numeric(7,3),
    improvement numeric(7,3),
    outcome_status varchar(30),
    teacher_notes text
);

-- Compare forecast with actual exam/test result for model monitoring.
CREATE TABLE ai.prediction_evaluation (
    prediction_evaluation_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    student_performance_prediction_id uuid NOT NULL
        REFERENCES ai.student_performance_prediction(student_performance_prediction_id),
    student_exam_result_id uuid NOT NULL REFERENCES exam.student_exam_result(student_exam_result_id),

    predicted_percentage numeric(7,3),
    actual_percentage numeric(7,3),
    absolute_error numeric(7,3),
    predicted_grade varchar(20),
    actual_grade varchar(20),
    grade_correct boolean,
    evaluated_at timestamptz NOT NULL DEFAULT now(),

    UNIQUE(student_performance_prediction_id, student_exam_result_id)
);

-- User-facing recommendations derived from predictions.
CREATE TABLE ai.student_progress_recommendation (
    student_progress_recommendation_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    student_id uuid NOT NULL REFERENCES student.student(student_id),
    prediction_id uuid REFERENCES ai.student_performance_prediction(student_performance_prediction_id),
    audience varchar(20) NOT NULL CHECK(audience IN ('STUDENT','PARENT','TEACHER')),
    title varchar(250) NOT NULL,
    recommendation_text text NOT NULL,
    priority varchar(30) NOT NULL DEFAULT 'NORMAL',
    status varchar(30) NOT NULL DEFAULT 'ACTIVE',
    generated_at timestamptz NOT NULL DEFAULT now(),
    expires_at timestamptz
);

CREATE INDEX ix_perf_prediction_student_subject
ON ai.student_performance_prediction(student_id, subject_id, generated_at DESC);

CREATE INDEX ix_perf_prediction_target_exam
ON ai.student_performance_prediction(target_exam_id, student_id);

CREATE INDEX ix_perf_prediction_risk
ON ai.student_performance_prediction(tenant_id, risk_level, generated_at DESC);

CREATE INDEX ix_prediction_evidence_prediction
ON ai.prediction_evidence(student_performance_prediction_id);

CREATE INDEX ix_class_insight_section_course
ON ai.class_performance_insight(class_section_id, course_offering_id, generated_at DESC);

CREATE INDEX ix_teaching_recommendation_teacher
ON ai.teaching_recommendation(teacher_employee_id, status, generated_at DESC);

CREATE INDEX ix_student_intervention_student
ON ai.student_intervention(student_id, status, created_at DESC);

CREATE INDEX ix_progress_recommendation_student_audience
ON ai.student_progress_recommendation(student_id, audience, status, generated_at DESC);


-- =========================================================
-- AUDIT
-- =========================================================
CREATE TABLE audit.audit_log (
    audit_log_id bigint GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    tenant_id uuid,
    user_id uuid,
    action varchar(100) NOT NULL,
    entity_type varchar(150) NOT NULL,
    entity_id varchar(100),
    old_values jsonb,
    new_values jsonb,
    ip_address inet,
    correlation_id varchar(100),
    occurred_at timestamptz NOT NULL DEFAULT now()
);

-- Useful indexes
CREATE INDEX ix_student_tenant_name ON student.student(tenant_id,last_name,first_name);
CREATE INDEX ix_enrollment_class ON student.student_enrollment(class_section_id,status);
CREATE INDEX ix_course_enrollment_course ON student.student_course_enrollment(course_offering_id,status);
CREATE INDEX ix_teacher_assignment_employee ON academic.teacher_course_assignment(employee_id,effective_to);
CREATE INDEX ix_timetable_section_day ON academic.timetable_entry(class_section_id,day_of_week,timetable_period_id);
CREATE INDEX ix_timetable_teacher ON academic.timetable_entry(teacher_course_assignment_id,day_of_week,timetable_period_id);
CREATE INDEX ix_exam_result_student ON exam.student_exam_result(student_id);
CREATE INDEX ix_message_conversation_time ON communication.message(conversation_id,sent_at DESC);
CREATE INDEX ix_notification_user_status ON communication.notification(user_id,status,created_at DESC);
CREATE INDEX ix_candidate_status ON hr.candidate(tenant_id,status_code);
CREATE INDEX ix_employee_tenant_status ON hr.employee(tenant_id,status);
CREATE INDEX ix_prediction_student_type ON ai.prediction(student_id,prediction_type,predicted_at DESC);

COMMIT;


-- ============================================================
-- SmartSchool normalized document attachments
-- File bytes belong in object/file storage; these tables retain
-- relational metadata, verification, integrity and lifecycle.
-- ============================================================

CREATE TABLE IF NOT EXISTS DocumentType (
	Id uuid PRIMARY KEY,
	TenantId uuid NOT NULL,
	Code varchar(80) NOT NULL,
	Name varchar(150) NOT NULL,
	OwnerCategory varchar(50) NOT NULL,
	IsIdentityDocument boolean NOT NULL DEFAULT false,
	RequiresExpiryDate boolean NOT NULL DEFAULT false,
	RequiresVerification boolean NOT NULL DEFAULT false,
	IsActive boolean NOT NULL DEFAULT true,
	CreatedAt timestamptz NOT NULL,
	UpdatedAt timestamptz NULL,
	RowVersion bytea NOT NULL,
	CONSTRAINT UQ_DocumentType_Tenant_Code UNIQUE (TenantId, Code)
);

-- Owner-specific attachment tables intentionally remain separate:
-- authorization, retention, required-document rules and audit policy differ
-- between students, guardians, employees, candidates and transport staff.

-- Columns shared by all document tables:
-- Id, TenantId, OwnerId, DocumentTypeId, OriginalFileName, ContentType,
-- FileSizeBytes, StorageProvider, StorageKey, Sha256Hash, DocumentNumber,
-- IssuedOn, ExpiresOn, IsVerified, VerifiedByUserId, VerifiedAt, Notes,
-- IsActive, CreatedAt, UpdatedAt, RowVersion.

CREATE TABLE IF NOT EXISTS StudentDocument (
	Id uuid PRIMARY KEY,
	TenantId uuid NOT NULL,
	StudentId uuid NOT NULL,
	DocumentTypeId uuid NOT NULL,
	OriginalFileName varchar(255) NOT NULL,
	ContentType varchar(150) NOT NULL,
	FileSizeBytes bigint NOT NULL CHECK (FileSizeBytes > 0),
	StorageProvider varchar(50) NOT NULL,
	StorageKey varchar(500) NOT NULL,
	Sha256Hash char(64) NOT NULL,
	DocumentNumber varchar(100) NULL,
	IssuedOn date NULL,
	ExpiresOn date NULL,
	IsVerified boolean NOT NULL DEFAULT false,
	VerifiedByUserId uuid NULL,
	VerifiedAt timestamptz NULL,
	Notes varchar(1000) NULL,
	IsActive boolean NOT NULL DEFAULT true,
	CreatedAt timestamptz NOT NULL,
	UpdatedAt timestamptz NULL,
	RowVersion bytea NOT NULL,
	CONSTRAINT FK_StudentDocument_DocumentType FOREIGN KEY (DocumentTypeId) REFERENCES DocumentType(Id),
	CONSTRAINT CK_StudentDocument_Dates CHECK (ExpiresOn IS NULL OR IssuedOn IS NULL OR ExpiresOn >= IssuedOn),
	CONSTRAINT UQ_StudentDocument_Storage UNIQUE (TenantId, StorageProvider, StorageKey)
);
CREATE INDEX IF NOT EXISTS IX_StudentDocument_Owner_Type
	ON StudentDocument(TenantId, StudentId, DocumentTypeId);
CREATE INDEX IF NOT EXISTS IX_StudentDocument_Hash
	ON StudentDocument(TenantId, Sha256Hash);

CREATE TABLE IF NOT EXISTS ParentDocument (
	Id uuid PRIMARY KEY,
	TenantId uuid NOT NULL,
	ParentId uuid NOT NULL,
	DocumentTypeId uuid NOT NULL,
	OriginalFileName varchar(255) NOT NULL,
	ContentType varchar(150) NOT NULL,
	FileSizeBytes bigint NOT NULL CHECK (FileSizeBytes > 0),
	StorageProvider varchar(50) NOT NULL,
	StorageKey varchar(500) NOT NULL,
	Sha256Hash char(64) NOT NULL,
	DocumentNumber varchar(100) NULL,
	IssuedOn date NULL,
	ExpiresOn date NULL,
	IsVerified boolean NOT NULL DEFAULT false,
	VerifiedByUserId uuid NULL,
	VerifiedAt timestamptz NULL,
	Notes varchar(1000) NULL,
	IsActive boolean NOT NULL DEFAULT true,
	CreatedAt timestamptz NOT NULL,
	UpdatedAt timestamptz NULL,
	RowVersion bytea NOT NULL,
	CONSTRAINT FK_ParentDocument_DocumentType FOREIGN KEY (DocumentTypeId) REFERENCES DocumentType(Id),
	CONSTRAINT CK_ParentDocument_Dates CHECK (ExpiresOn IS NULL OR IssuedOn IS NULL OR ExpiresOn >= IssuedOn),
	CONSTRAINT UQ_ParentDocument_Storage UNIQUE (TenantId, StorageProvider, StorageKey)
);
CREATE INDEX IF NOT EXISTS IX_ParentDocument_Owner_Type
	ON ParentDocument(TenantId, ParentId, DocumentTypeId);
CREATE INDEX IF NOT EXISTS IX_ParentDocument_Hash
	ON ParentDocument(TenantId, Sha256Hash);

CREATE TABLE IF NOT EXISTS TeacherDocument (
	Id uuid PRIMARY KEY,
	TenantId uuid NOT NULL,
	TeacherId uuid NOT NULL,
	DocumentTypeId uuid NOT NULL,
	OriginalFileName varchar(255) NOT NULL,
	ContentType varchar(150) NOT NULL,
	FileSizeBytes bigint NOT NULL CHECK (FileSizeBytes > 0),
	StorageProvider varchar(50) NOT NULL,
	StorageKey varchar(500) NOT NULL,
	Sha256Hash char(64) NOT NULL,
	DocumentNumber varchar(100) NULL,
	IssuedOn date NULL,
	ExpiresOn date NULL,
	IsVerified boolean NOT NULL DEFAULT false,
	VerifiedByUserId uuid NULL,
	VerifiedAt timestamptz NULL,
	Notes varchar(1000) NULL,
	IsActive boolean NOT NULL DEFAULT true,
	CreatedAt timestamptz NOT NULL,
	UpdatedAt timestamptz NULL,
	RowVersion bytea NOT NULL,
	CONSTRAINT FK_TeacherDocument_DocumentType FOREIGN KEY (DocumentTypeId) REFERENCES DocumentType(Id),
	CONSTRAINT CK_TeacherDocument_Dates CHECK (ExpiresOn IS NULL OR IssuedOn IS NULL OR ExpiresOn >= IssuedOn),
	CONSTRAINT UQ_TeacherDocument_Storage UNIQUE (TenantId, StorageProvider, StorageKey)
);
CREATE INDEX IF NOT EXISTS IX_TeacherDocument_Owner_Type
	ON TeacherDocument(TenantId, TeacherId, DocumentTypeId);
CREATE INDEX IF NOT EXISTS IX_TeacherDocument_Hash
	ON TeacherDocument(TenantId, Sha256Hash);

CREATE TABLE IF NOT EXISTS EmployeeDocument (
	Id uuid PRIMARY KEY,
	TenantId uuid NOT NULL,
	EmployeeId uuid NOT NULL,
	DocumentTypeId uuid NOT NULL,
	OriginalFileName varchar(255) NOT NULL,
	ContentType varchar(150) NOT NULL,
	FileSizeBytes bigint NOT NULL CHECK (FileSizeBytes > 0),
	StorageProvider varchar(50) NOT NULL,
	StorageKey varchar(500) NOT NULL,
	Sha256Hash char(64) NOT NULL,
	DocumentNumber varchar(100) NULL,
	IssuedOn date NULL,
	ExpiresOn date NULL,
	IsVerified boolean NOT NULL DEFAULT false,
	VerifiedByUserId uuid NULL,
	VerifiedAt timestamptz NULL,
	Notes varchar(1000) NULL,
	IsActive boolean NOT NULL DEFAULT true,
	CreatedAt timestamptz NOT NULL,
	UpdatedAt timestamptz NULL,
	RowVersion bytea NOT NULL,
	CONSTRAINT FK_EmployeeDocument_DocumentType FOREIGN KEY (DocumentTypeId) REFERENCES DocumentType(Id),
	CONSTRAINT CK_EmployeeDocument_Dates CHECK (ExpiresOn IS NULL OR IssuedOn IS NULL OR ExpiresOn >= IssuedOn),
	CONSTRAINT UQ_EmployeeDocument_Storage UNIQUE (TenantId, StorageProvider, StorageKey)
);
CREATE INDEX IF NOT EXISTS IX_EmployeeDocument_Owner_Type
	ON EmployeeDocument(TenantId, EmployeeId, DocumentTypeId);
CREATE INDEX IF NOT EXISTS IX_EmployeeDocument_Hash
	ON EmployeeDocument(TenantId, Sha256Hash);

CREATE TABLE IF NOT EXISTS CandidateDocument (
	Id uuid PRIMARY KEY,
	TenantId uuid NOT NULL,
	CandidateId uuid NOT NULL,
	DocumentTypeId uuid NOT NULL,
	OriginalFileName varchar(255) NOT NULL,
	ContentType varchar(150) NOT NULL,
	FileSizeBytes bigint NOT NULL CHECK (FileSizeBytes > 0),
	StorageProvider varchar(50) NOT NULL,
	StorageKey varchar(500) NOT NULL,
	Sha256Hash char(64) NOT NULL,
	DocumentNumber varchar(100) NULL,
	IssuedOn date NULL,
	ExpiresOn date NULL,
	IsVerified boolean NOT NULL DEFAULT false,
	VerifiedByUserId uuid NULL,
	VerifiedAt timestamptz NULL,
	Notes varchar(1000) NULL,
	IsActive boolean NOT NULL DEFAULT true,
	CreatedAt timestamptz NOT NULL,
	UpdatedAt timestamptz NULL,
	RowVersion bytea NOT NULL,
	CONSTRAINT FK_CandidateDocument_DocumentType FOREIGN KEY (DocumentTypeId) REFERENCES DocumentType(Id),
	CONSTRAINT CK_CandidateDocument_Dates CHECK (ExpiresOn IS NULL OR IssuedOn IS NULL OR ExpiresOn >= IssuedOn),
	CONSTRAINT UQ_CandidateDocument_Storage UNIQUE (TenantId, StorageProvider, StorageKey)
);
CREATE INDEX IF NOT EXISTS IX_CandidateDocument_Owner_Type
	ON CandidateDocument(TenantId, CandidateId, DocumentTypeId);
CREATE INDEX IF NOT EXISTS IX_CandidateDocument_Hash
	ON CandidateDocument(TenantId, Sha256Hash);

CREATE TABLE IF NOT EXISTS DriverDocument (
	Id uuid PRIMARY KEY,
	TenantId uuid NOT NULL,
	DriverId uuid NOT NULL,
	DocumentTypeId uuid NOT NULL,
	OriginalFileName varchar(255) NOT NULL,
	ContentType varchar(150) NOT NULL,
	FileSizeBytes bigint NOT NULL CHECK (FileSizeBytes > 0),
	StorageProvider varchar(50) NOT NULL,
	StorageKey varchar(500) NOT NULL,
	Sha256Hash char(64) NOT NULL,
	DocumentNumber varchar(100) NULL,
	IssuedOn date NULL,
	ExpiresOn date NULL,
	IsVerified boolean NOT NULL DEFAULT false,
	VerifiedByUserId uuid NULL,
	VerifiedAt timestamptz NULL,
	Notes varchar(1000) NULL,
	IsActive boolean NOT NULL DEFAULT true,
	CreatedAt timestamptz NOT NULL,
	UpdatedAt timestamptz NULL,
	RowVersion bytea NOT NULL,
	CONSTRAINT FK_DriverDocument_DocumentType FOREIGN KEY (DocumentTypeId) REFERENCES DocumentType(Id),
	CONSTRAINT CK_DriverDocument_Dates CHECK (ExpiresOn IS NULL OR IssuedOn IS NULL OR ExpiresOn >= IssuedOn),
	CONSTRAINT UQ_DriverDocument_Storage UNIQUE (TenantId, StorageProvider, StorageKey)
);
CREATE INDEX IF NOT EXISTS IX_DriverDocument_Owner_Type
	ON DriverDocument(TenantId, DriverId, DocumentTypeId);
CREATE INDEX IF NOT EXISTS IX_DriverDocument_Hash
	ON DriverDocument(TenantId, Sha256Hash);

CREATE TABLE IF NOT EXISTS SchoolDocument (
	Id uuid PRIMARY KEY,
	TenantId uuid NOT NULL,
	SchoolId uuid NOT NULL,
	DocumentTypeId uuid NOT NULL,
	OriginalFileName varchar(255) NOT NULL,
	ContentType varchar(150) NOT NULL,
	FileSizeBytes bigint NOT NULL CHECK (FileSizeBytes > 0),
	StorageProvider varchar(50) NOT NULL,
	StorageKey varchar(500) NOT NULL,
	Sha256Hash char(64) NOT NULL,
	DocumentNumber varchar(100) NULL,
	IssuedOn date NULL,
	ExpiresOn date NULL,
	IsVerified boolean NOT NULL DEFAULT false,
	VerifiedByUserId uuid NULL,
	VerifiedAt timestamptz NULL,
	Notes varchar(1000) NULL,
	IsActive boolean NOT NULL DEFAULT true,
	CreatedAt timestamptz NOT NULL,
	UpdatedAt timestamptz NULL,
	RowVersion bytea NOT NULL,
	CONSTRAINT FK_SchoolDocument_DocumentType FOREIGN KEY (DocumentTypeId) REFERENCES DocumentType(Id),
	CONSTRAINT CK_SchoolDocument_Dates CHECK (ExpiresOn IS NULL OR IssuedOn IS NULL OR ExpiresOn >= IssuedOn),
	CONSTRAINT UQ_SchoolDocument_Storage UNIQUE (TenantId, StorageProvider, StorageKey)
);
CREATE INDEX IF NOT EXISTS IX_SchoolDocument_Owner_Type
	ON SchoolDocument(TenantId, SchoolId, DocumentTypeId);
CREATE INDEX IF NOT EXISTS IX_SchoolDocument_Hash
	ON SchoolDocument(TenantId, Sha256Hash);

-- Materialized read tables. These are application-managed projections,
-- portable across PostgreSQL and SQL Server, unlike vendor-specific
-- materialized views.
CREATE TABLE IF NOT EXISTS StudentDirectoryRead (
	Id uuid PRIMARY KEY,
	TenantId uuid NOT NULL,
	StudentId uuid NOT NULL,
	AdmissionNumber varchar(100) NOT NULL,
	StudentName varchar(250) NOT NULL,
	ProgramName varchar(250) NULL,
	ClassName varchar(150) NULL,
	SectionName varchar(100) NULL,
	PrimaryGuardianName varchar(250) NULL,
	PrimaryGuardianMobile varchar(50) NULL,
	AttendancePercentage numeric(5,2) NULL,
	LatestExamPercentage numeric(5,2) NULL,
	OutstandingBalance numeric(18,2) NOT NULL DEFAULT 0,
	DocumentCount integer NOT NULL DEFAULT 0,
	VerifiedDocumentCount integer NOT NULL DEFAULT 0,
	IsActive boolean NOT NULL DEFAULT true,
	CreatedAt timestamptz NOT NULL,
	UpdatedAt timestamptz NULL,
	RowVersion bytea NOT NULL,
	CONSTRAINT UQ_StudentDirectoryRead UNIQUE (TenantId, StudentId)
);

CREATE TABLE IF NOT EXISTS TeacherDirectoryRead (
	Id uuid PRIMARY KEY,
	TenantId uuid NOT NULL,
	TeacherId uuid NOT NULL,
	EmployeeNumber varchar(100) NOT NULL,
	TeacherName varchar(250) NOT NULL,
	JobTitle varchar(150) NULL,
	JobGrade varchar(100) NULL,
	DepartmentName varchar(150) NULL,
	MobileNumber varchar(50) NULL,
	ActiveClassAssignments integer NOT NULL DEFAULT 0,
	DocumentCount integer NOT NULL DEFAULT 0,
	VerifiedDocumentCount integer NOT NULL DEFAULT 0,
	IsActive boolean NOT NULL DEFAULT true,
	CreatedAt timestamptz NOT NULL,
	UpdatedAt timestamptz NULL,
	RowVersion bytea NOT NULL,
	CONSTRAINT UQ_TeacherDirectoryRead UNIQUE (TenantId, TeacherId)
);

CREATE TABLE IF NOT EXISTS DriverDirectoryRead (
	Id uuid PRIMARY KEY,
	TenantId uuid NOT NULL,
	DriverId uuid NOT NULL,
	EmployeeNumber varchar(100) NOT NULL,
	DriverName varchar(250) NOT NULL,
	MobileNumber varchar(50) NULL,
	LicenseNumber varchar(100) NOT NULL,
	LicenseExpiryDate date NULL,
	VehicleRegistrationNumber varchar(100) NULL,
	RouteName varchar(250) NULL,
	DocumentCount integer NOT NULL DEFAULT 0,
	VerifiedDocumentCount integer NOT NULL DEFAULT 0,
	IsActive boolean NOT NULL DEFAULT true,
	CreatedAt timestamptz NOT NULL,
	UpdatedAt timestamptz NULL,
	RowVersion bytea NOT NULL,
	CONSTRAINT UQ_DriverDirectoryRead UNIQUE (TenantId, DriverId)
);
