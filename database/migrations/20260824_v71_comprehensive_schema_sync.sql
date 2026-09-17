-- SmartSchool v71 comprehensive model/schema synchronization
-- Generated 2026-08-24. Idempotent PostgreSQL migration.


CREATE SCHEMA IF NOT EXISTS ai;

CREATE TABLE IF NOT EXISTS ai.ml_prediction_result (
    ml_prediction_result_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    prediction_type text NOT NULL,
    student_id uuid,
    subject_id uuid,
    related_entity_id uuid,
    score numeric(18,6) NOT NULL,
    probability numeric(18,6) NOT NULL,
    risk_level text NOT NULL,
    outcome text NOT NULL,
    confidence_score numeric(18,6) NOT NULL,
    model_version text NOT NULL,
    used_machine_learning boolean NOT NULL,
    factors_json jsonb,
    generated_at timestamp with time zone NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    CONSTRAINT pk_ml_prediction_result PRIMARY KEY (ml_prediction_result_id)
);

CREATE INDEX IF NOT EXISTS ix_ml_prediction_result_tenant_id ON ai.ml_prediction_result(tenant_id);

ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai.prediction_model ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai.class_performance_insight ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai.class_performance_insight ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai.class_performance_insight ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai.teaching_recommendation ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai.teaching_recommendation ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai.teaching_recommendation ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai.student_performance_prediction ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai.student_performance_prediction ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai.student_performance_prediction ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai.student_intervention ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai.student_intervention ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai.student_intervention ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS subject_id uuid;

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS target_exam_id uuid;

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS target_exam_subject_id uuid;

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS target_exam_type_code text;

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS predicted_marks numeric(18,6);

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS predicted_percentage numeric(18,6);

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS predicted_grade text;

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS lower_bound_percentage numeric(18,6);

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS upper_bound_percentage numeric(18,6);

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS confidence_score numeric(18,6);

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS pass_probability numeric(18,6);

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS trend text;

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS model_version text;

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS historical_result_count integer;

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS used_machine_learning boolean;

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS generated_at timestamp with time zone;

ALTER TABLE academic.subject ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.timetable ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE academic.timetable ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE student.student_course_enrollment ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE student.student_course_enrollment ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE student.student_course_enrollment ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.course_offering ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE academic.course_offering ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE academic.course_offering ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.program ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.timetable_entry ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE academic.timetable_entry ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE academic.timetable_entry ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.timetable_entry ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE academic.timetable_entry ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE academic.timetable_entry ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE academic.timetable_entry ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE academic.timetable_entry ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE academic.academic_system ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.term ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.teacher_course_assignment ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE academic.teacher_course_assignment ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE academic.teacher_course_assignment ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.academic_year ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE academic.academic_year ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.grade_level ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.class_section ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE academic.class_section ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE academic.class_section ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_tutor.student_topic_mastery ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_tutor.student_topic_mastery ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_tutor.student_topic_mastery ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai_tutor.tutor_conversation ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_tutor.tutor_conversation ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_tutor.tutor_conversation ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai_tutor.generated_quiz ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_tutor.generated_quiz ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_tutor.generated_quiz ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai_parent.parent_message ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_parent.parent_message ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_parent.parent_message ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_parent.parent_message ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_parent.parent_message ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_parent.parent_message ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_parent.parent_message ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai_parent.parent_conversation ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_parent.parent_conversation ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_parent.parent_conversation ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_parent.parent_tool_execution ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_parent.parent_tool_execution ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_parent.parent_tool_execution ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_parent.parent_tool_execution ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_parent.parent_tool_execution ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_parent.parent_tool_execution ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai_parent.parent_tool_execution ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_parent.parent_tool_execution ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

CREATE SCHEMA IF NOT EXISTS admission;

CREATE TABLE IF NOT EXISTS admission.admissiondecision (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    admission_decision_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_admissiondecision PRIMARY KEY (admission_decision_id)
);

CREATE INDEX IF NOT EXISTS ix_admissiondecision_tenant_id ON admission.admissiondecision(tenant_id);

CREATE SCHEMA IF NOT EXISTS admission;

CREATE TABLE IF NOT EXISTS admission.application (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    application_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_application PRIMARY KEY (application_id)
);

CREATE INDEX IF NOT EXISTS ix_application_tenant_id ON admission.application(tenant_id);

CREATE SCHEMA IF NOT EXISTS admission;

CREATE TABLE IF NOT EXISTS admission.applicant (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    applicant_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_applicant PRIMARY KEY (applicant_id)
);

CREATE INDEX IF NOT EXISTS ix_applicant_tenant_id ON admission.applicant(tenant_id);

CREATE SCHEMA IF NOT EXISTS admission;

CREATE TABLE IF NOT EXISTS admission.inquiry (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    inquiry_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_inquiry PRIMARY KEY (inquiry_id)
);

CREATE INDEX IF NOT EXISTS ix_inquiry_tenant_id ON admission.inquiry(tenant_id);

ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE exam.exam ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE exam.exam ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS exam;

CREATE TABLE IF NOT EXISTS exam.gradescale (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    grade_scale_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_gradescale PRIMARY KEY (grade_scale_id)
);

CREATE INDEX IF NOT EXISTS ix_gradescale_tenant_id ON exam.gradescale(tenant_id);

ALTER TABLE hr.position ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE hr.position ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE hr.position ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE hr.job ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE hr.job ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE hr.job_grade ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE hr.candidate ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE hr.candidate ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE hr.candidate ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE org.department ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS org;

CREATE TABLE IF NOT EXISTS org.school (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    school_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_school PRIMARY KEY (school_id)
);

CREATE INDEX IF NOT EXISTS ix_school_tenant_id ON org.school(tenant_id);

ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_inquiry.inquiry_conversation ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_inquiry.inquiry_conversation ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_inquiry.inquiry_conversation ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_inquiry.lead_capture ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_inquiry.lead_capture ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_inquiry.lead_capture ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_inquiry.lead_capture ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_inquiry.lead_capture ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai_inquiry.lead_capture ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_inquiry.lead_capture ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai_inquiry.human_handoff ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_inquiry.human_handoff ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_inquiry.human_handoff ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_inquiry.human_handoff ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_inquiry.human_handoff ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_inquiry.human_handoff ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai_inquiry.human_handoff ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_inquiry.human_handoff ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai_inquiry.inquiry_message ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_inquiry.inquiry_message ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_inquiry.inquiry_message ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_inquiry.inquiry_message ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_inquiry.inquiry_message ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_inquiry.inquiry_message ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_inquiry.inquiry_message ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

CREATE SCHEMA IF NOT EXISTS payroll;

CREATE TABLE IF NOT EXISTS payroll.payslip (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    payslip_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_payslip PRIMARY KEY (payslip_id)
);

CREATE INDEX IF NOT EXISTS ix_payslip_tenant_id ON payroll.payslip(tenant_id);

CREATE SCHEMA IF NOT EXISTS payroll;

CREATE TABLE IF NOT EXISTS payroll.increment (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    increment_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_increment PRIMARY KEY (increment_id)
);

CREATE INDEX IF NOT EXISTS ix_increment_tenant_id ON payroll.increment(tenant_id);

CREATE SCHEMA IF NOT EXISTS payroll;

CREATE TABLE IF NOT EXISTS payroll.salarystructure (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    salary_structure_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_salarystructure PRIMARY KEY (salary_structure_id)
);

CREATE INDEX IF NOT EXISTS ix_salarystructure_tenant_id ON payroll.salarystructure(tenant_id);

ALTER TABLE hr.employee_compensation ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE hr.employee_compensation ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE hr.employee_compensation ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE payroll.payroll_run ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE payroll.payroll_run ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE payroll.payroll_run ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS student;

CREATE TABLE IF NOT EXISTS student.attendance (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    attendance_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_attendance PRIMARY KEY (attendance_id)
);

CREATE INDEX IF NOT EXISTS ix_attendance_tenant_id ON student.attendance(tenant_id);

CREATE SCHEMA IF NOT EXISTS student;

CREATE TABLE IF NOT EXISTS student.parentprofile (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    parent_profile_id uuid DEFAULT gen_random_uuid() NOT NULL,
    first_name text NOT NULL,
    last_name text NOT NULL,
    cnic text NOT NULL,
    relationship_code text NOT NULL,
    mobile_number text NOT NULL,
    alternate_mobile_number text,
    email_address text,
    occupation text,
    employer_name text,
    work_address text,
    residential_address text,
    is_primary_guardian boolean NOT NULL,
    is_emergency_contact boolean NOT NULL,
    can_collect_student boolean NOT NULL,
    CONSTRAINT pk_parentprofile PRIMARY KEY (parent_profile_id)
);

CREATE INDEX IF NOT EXISTS ix_parentprofile_tenant_id ON student.parentprofile(tenant_id);

ALTER TABLE student.student_enrollment ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE student.student_enrollment ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE student.student_enrollment ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS id uuid;

ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

CREATE SCHEMA IF NOT EXISTS student;

CREATE TABLE IF NOT EXISTS student.studentprofile (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    student_profile_id uuid DEFAULT gen_random_uuid() NOT NULL,
    student_id uuid NOT NULL,
    admission_number text NOT NULL,
    first_name text NOT NULL,
    middle_name text,
    last_name text NOT NULL,
    date_of_birth date NOT NULL,
    gender_code text NOT NULL,
    b_form_number text,
    passport_number text,
    blood_group_code text,
    primary_language_code text,
    mobile_number text,
    email_address text,
    address_line1 text,
    address_line2 text,
    city text,
    province text,
    postal_code text,
    country_code text,
    emergency_contact_name text,
    emergency_contact_phone text,
    medical_notes text,
    allergies text,
    admission_date date NOT NULL,
    current_program_id uuid,
    current_class_id uuid,
    current_section_id uuid,
    CONSTRAINT pk_studentprofile PRIMARY KEY (student_profile_id)
);

CREATE INDEX IF NOT EXISTS ix_studentprofile_tenant_id ON student.studentprofile(tenant_id);

ALTER TABLE communication.conversation ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE communication.conversation ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE communication.conversation ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS recipient_user_id uuid;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS type text;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS message text;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS related_entity_id uuid;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS related_entity_type text;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS action_url text;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS priority text;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS is_read boolean;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS read_at timestamp with time zone;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS occurred_at timestamp with time zone;

ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS id uuid;

ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS id uuid;

ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

CREATE SCHEMA IF NOT EXISTS workflow;

CREATE TABLE IF NOT EXISTS workflow.workflowinstance (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    workflow_instance_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_workflowinstance PRIMARY KEY (workflow_instance_id)
);

CREATE INDEX IF NOT EXISTS ix_workflowinstance_tenant_id ON workflow.workflowinstance(tenant_id);

CREATE SCHEMA IF NOT EXISTS workflow;

CREATE TABLE IF NOT EXISTS workflow.workflowstep (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    workflow_step_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_workflowstep PRIMARY KEY (workflow_step_id)
);

CREATE INDEX IF NOT EXISTS ix_workflowstep_tenant_id ON workflow.workflowstep(tenant_id);

CREATE SCHEMA IF NOT EXISTS workflow;

CREATE TABLE IF NOT EXISTS workflow.approval (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    approval_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_approval PRIMARY KEY (approval_id)
);

CREATE INDEX IF NOT EXISTS ix_approval_tenant_id ON workflow.approval(tenant_id);

CREATE SCHEMA IF NOT EXISTS workflow;

CREATE TABLE IF NOT EXISTS workflow.workflowdefinition (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    workflow_definition_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_workflowdefinition PRIMARY KEY (workflow_definition_id)
);

CREATE INDEX IF NOT EXISTS ix_workflowdefinition_tenant_id ON workflow.workflowdefinition(tenant_id);

ALTER TABLE document.generated_document ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE document.generated_document ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE document.generated_document ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS document;

CREATE TABLE IF NOT EXISTS document.certificate (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    certificate_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_certificate PRIMARY KEY (certificate_id)
);

CREATE INDEX IF NOT EXISTS ix_certificate_tenant_id ON document.certificate(tenant_id);

CREATE SCHEMA IF NOT EXISTS document;

CREATE TABLE IF NOT EXISTS document.schoollogo (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    school_logo_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_schoollogo PRIMARY KEY (school_logo_id)
);

CREATE INDEX IF NOT EXISTS ix_schoollogo_tenant_id ON document.schoollogo(tenant_id);

ALTER TABLE document.document_template ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE transport.route ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS transport;

CREATE TABLE IF NOT EXISTS transport.studenttransport (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    student_transport_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_studenttransport PRIMARY KEY (student_transport_id)
);

CREATE INDEX IF NOT EXISTS ix_studenttransport_tenant_id ON transport.studenttransport(tenant_id);

ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS employee_number text;

ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS first_name text;

ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS last_name text;

ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS cnic text;

ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS mobile_number text;

ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS license_expiry_date date;

ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS joining_date date;

ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS employment_status_code text;

ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS assigned_vehicle_id uuid;

ALTER TABLE transport.vehicle ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE transport.vehicle ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE transport.vehicle ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS transport;

CREATE TABLE IF NOT EXISTS transport.stop (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    stop_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_stop PRIMARY KEY (stop_id)
);

CREATE INDEX IF NOT EXISTS ix_stop_tenant_id ON transport.stop(tenant_id);

ALTER TABLE finance.fee_type ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS finance;

CREATE TABLE IF NOT EXISTS finance.scholarship (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    scholarship_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_scholarship PRIMARY KEY (scholarship_id)
);

CREATE INDEX IF NOT EXISTS ix_scholarship_tenant_id ON finance.scholarship(tenant_id);

ALTER TABLE finance.student_invoice ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE finance.student_invoice ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE finance.student_invoice ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS finance;

CREATE TABLE IF NOT EXISTS finance.feestructure (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    fee_structure_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_feestructure PRIMARY KEY (fee_structure_id)
);

CREATE INDEX IF NOT EXISTS ix_feestructure_tenant_id ON finance.feestructure(tenant_id);

ALTER TABLE finance.student_payment ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE finance.student_payment ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE finance.student_payment ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS finance;

CREATE TABLE IF NOT EXISTS finance.discount (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    discount_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_discount PRIMARY KEY (discount_id)
);

CREATE INDEX IF NOT EXISTS ix_discount_tenant_id ON finance.discount(tenant_id);

CREATE SCHEMA IF NOT EXISTS finance;

CREATE TABLE IF NOT EXISTS finance.studentfee (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    student_fee_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_studentfee PRIMARY KEY (student_fee_id)
);

CREATE INDEX IF NOT EXISTS ix_studentfee_tenant_id ON finance.studentfee(tenant_id);

ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE library.book ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE library.book ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE library.book ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

CREATE SCHEMA IF NOT EXISTS library;

CREATE TABLE IF NOT EXISTS library.reservation (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    reservation_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_reservation PRIMARY KEY (reservation_id)
);

CREATE INDEX IF NOT EXISTS ix_reservation_tenant_id ON library.reservation(tenant_id);

ALTER TABLE ai_core.tool_definition ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_core.tool_definition ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_core.tool_definition ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai_core.tool_definition ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_core.tool_definition ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai_core.prompt_template ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_core.prompt_template ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_core.knowledge_collection ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_core.ai_execution_log ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_core.ai_execution_log ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_core.ai_execution_log ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_core.model_configuration ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_core.model_configuration ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_core.knowledge_document ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_core.knowledge_document ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_core.knowledge_document ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

CREATE SCHEMA IF NOT EXISTS lms;

CREATE TABLE IF NOT EXISTS lms.learningresource (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    learning_resource_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_learningresource PRIMARY KEY (learning_resource_id)
);

CREATE INDEX IF NOT EXISTS ix_learningresource_tenant_id ON lms.learningresource(tenant_id);

CREATE SCHEMA IF NOT EXISTS lms;

CREATE TABLE IF NOT EXISTS lms.lesson (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    lesson_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_lesson PRIMARY KEY (lesson_id)
);

CREATE INDEX IF NOT EXISTS ix_lesson_tenant_id ON lms.lesson(tenant_id);

ALTER TABLE lms.academic_assignment ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE lms.academic_assignment ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE lms.academic_assignment ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS id uuid;

ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE activity.student_award ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE activity.student_award ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE activity.student_award ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE activity.activity ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE activity.activity ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS id uuid;

ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

CREATE SCHEMA IF NOT EXISTS activity;

CREATE TABLE IF NOT EXISTS activity.studentofmonth (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    student_of_month_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_studentofmonth PRIMARY KEY (student_of_month_id)
);

CREATE INDEX IF NOT EXISTS ix_studentofmonth_tenant_id ON activity.studentofmonth(tenant_id);

ALTER TABLE inventory.item ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS inventory;

CREATE TABLE IF NOT EXISTS inventory.stocktransaction (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    stock_transaction_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_stocktransaction PRIMARY KEY (stock_transaction_id)
);

CREATE INDEX IF NOT EXISTS ix_stocktransaction_tenant_id ON inventory.stocktransaction(tenant_id);

CREATE SCHEMA IF NOT EXISTS inventory;

CREATE TABLE IF NOT EXISTS inventory.purchaseorder (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    purchase_order_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_purchaseorder PRIMARY KEY (purchase_order_id)
);

CREATE INDEX IF NOT EXISTS ix_purchaseorder_tenant_id ON inventory.purchaseorder(tenant_id);

ALTER TABLE saas.tenant ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE saas.tenant ADD COLUMN IF NOT EXISTS id uuid;

ALTER TABLE saas.school_branding ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE saas.school_branding ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE saas.school_branding ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE saas.school_branding ADD COLUMN IF NOT EXISTS id uuid;
