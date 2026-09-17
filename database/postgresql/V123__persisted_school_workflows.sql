-- V123: Persisted school workflows and model/schema alignment.
-- Apply after the bundled baseline / V122 on PostgreSQL. No Identity database changes.
BEGIN;
ALTER TABLE "saas"."school_branding" ADD COLUMN IF NOT EXISTS "id" uuid DEFAULT gen_random_uuid() NOT NULL;
ALTER TABLE "saas"."school_branding" ADD COLUMN IF NOT EXISTS "code" character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE "saas"."school_branding" ADD COLUMN IF NOT EXISTS "name" character varying(250) DEFAULT '' NOT NULL;
ALTER TABLE "org"."campus" ADD COLUMN IF NOT EXISTS "academic_system_id" uuid;
ALTER TABLE "academic"."class_section" ADD COLUMN IF NOT EXISTS "room_no" character varying(50);
ALTER TABLE "academic"."course_offering" ADD COLUMN IF NOT EXISTS "code" character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE "academic"."course_offering" ADD COLUMN IF NOT EXISTS "name" character varying(250) DEFAULT '' NOT NULL;
ALTER TABLE "academic"."course_offering" ADD COLUMN IF NOT EXISTS "subject_id" uuid;
ALTER TABLE "student"."student_course_enrollment" ADD COLUMN IF NOT EXISTS "code" character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE "student"."student_course_enrollment" ADD COLUMN IF NOT EXISTS "name" character varying(250) DEFAULT '' NOT NULL;
ALTER TABLE "academic"."grade_level" ADD COLUMN IF NOT EXISTS "education_level_id" uuid;
CREATE TABLE IF NOT EXISTS "org"."subscription" (
    "subscription_id" uuid DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
    "code" character varying(100) DEFAULT '' NOT NULL,
    "created_at" timestamp with time zone DEFAULT now() NOT NULL,
    "is_active" boolean DEFAULT true NOT NULL,
    "metadata_json" jsonb,
    "name" character varying(250) DEFAULT '' NOT NULL,
    "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL,
    "tenant_id" uuid NOT NULL,
    "updated_at" timestamp with time zone
);
ALTER TABLE "academic"."teacher_course_assignment" ADD COLUMN IF NOT EXISTS "code" character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE "academic"."teacher_course_assignment" ADD COLUMN IF NOT EXISTS "name" character varying(250) DEFAULT '' NOT NULL;
ALTER TABLE "saas"."tenant" ADD COLUMN IF NOT EXISTS "first_name" text DEFAULT '' NOT NULL;
ALTER TABLE "saas"."tenant" ADD COLUMN IF NOT EXISTS "last_name" text DEFAULT '' NOT NULL;
CREATE TABLE IF NOT EXISTS "saas"."tenant_settings" (
    "tenant_settings_id" uuid DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
    "academic_year_start_month" smallint DEFAULT 0 NOT NULL,
    "ai_agent" boolean DEFAULT false NOT NULL,
    "ai_parent_chatbot" boolean DEFAULT false NOT NULL,
    "ai_predictions" boolean DEFAULT false NOT NULL,
    "ai_quiz" boolean DEFAULT false NOT NULL,
    "ai_rag_assistant" boolean DEFAULT false NOT NULL,
    "ai_tutor" boolean DEFAULT false NOT NULL,
    "assignments" boolean DEFAULT false NOT NULL,
    "biometric_attendance" boolean DEFAULT false NOT NULL,
    "broadcast" boolean DEFAULT false NOT NULL,
    "created_at" timestamp with time zone DEFAULT now() NOT NULL,
    "date_format" smallint DEFAULT 0 NOT NULL,
    "default_language" smallint DEFAULT 0 NOT NULL,
    "digital_receipts" boolean DEFAULT false NOT NULL,
    "fee_reminders" boolean DEFAULT false NOT NULL,
    "fee_warning_days" smallint DEFAULT 0 NOT NULL,
    "internal_chat" boolean DEFAULT false NOT NULL,
    "ip_restriction" boolean DEFAULT false NOT NULL,
    "is_active" boolean DEFAULT true NOT NULL,
    "library_enabled" boolean DEFAULT false NOT NULL,
    "notifications" boolean DEFAULT false NOT NULL,
    "online_payment" boolean DEFAULT false NOT NULL,
    "parent_portal" boolean DEFAULT false NOT NULL,
    "qr_attendance" boolean DEFAULT false NOT NULL,
    "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL,
    "session_timeout" boolean DEFAULT false NOT NULL,
    "staff_self_leave" boolean DEFAULT false NOT NULL,
    "student_leave_apply" boolean DEFAULT false NOT NULL,
    "tenant_id" uuid NOT NULL,
    "time_zone" character varying(100) DEFAULT '' NOT NULL,
    "two_factor" boolean DEFAULT false NOT NULL,
    "updated_at" timestamp with time zone,
    "week_start" smallint DEFAULT 0 NOT NULL
);
ALTER TABLE "academic"."timetable" ADD COLUMN IF NOT EXISTS "code" character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE "academic"."timetable_entry" ADD COLUMN IF NOT EXISTS "code" character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE "academic"."timetable_entry" ADD COLUMN IF NOT EXISTS "created_at" timestamp with time zone DEFAULT now() NOT NULL;
ALTER TABLE "academic"."timetable_entry" ADD COLUMN IF NOT EXISTS "is_active" boolean DEFAULT true NOT NULL;
ALTER TABLE "academic"."timetable_entry" ADD COLUMN IF NOT EXISTS "name" character varying(250) DEFAULT '' NOT NULL;
ALTER TABLE "academic"."timetable_entry" ADD COLUMN IF NOT EXISTS "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL;
ALTER TABLE "academic"."timetable_entry" ADD COLUMN IF NOT EXISTS "tenant_id" uuid;
ALTER TABLE "academic"."timetable_entry" ADD COLUMN IF NOT EXISTS "updated_at" timestamp with time zone;
CREATE TABLE IF NOT EXISTS "student"."attendance" (
    "attendance_id" uuid DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
    "code" character varying(100) DEFAULT '' NOT NULL,
    "created_at" timestamp with time zone DEFAULT now() NOT NULL,
    "is_active" boolean DEFAULT true NOT NULL,
    "metadata_json" jsonb,
    "name" character varying(250) DEFAULT '' NOT NULL,
    "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL,
    "tenant_id" uuid NOT NULL,
    "updated_at" timestamp with time zone
);
ALTER TABLE "admission"."student_application" ADD COLUMN IF NOT EXISTS "created_at" timestamp with time zone DEFAULT now() NOT NULL;
ALTER TABLE "admission"."student_application" ADD COLUMN IF NOT EXISTS "entrance_test_marks" numeric;
ALTER TABLE "admission"."student_application" ADD COLUMN IF NOT EXISTS "interview_passed" boolean;
ALTER TABLE "admission"."student_application" ADD COLUMN IF NOT EXISTS "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL;
ALTER TABLE "admission"."student_application" ADD COLUMN IF NOT EXISTS "updated_at" timestamp with time zone;
CREATE TABLE IF NOT EXISTS "admission"."admissiondecision" (
    "admission_decision_id" uuid DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
    "code" character varying(100) DEFAULT '' NOT NULL,
    "created_at" timestamp with time zone DEFAULT now() NOT NULL,
    "is_active" boolean DEFAULT true NOT NULL,
    "metadata_json" jsonb,
    "name" character varying(250) DEFAULT '' NOT NULL,
    "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL,
    "tenant_id" uuid NOT NULL,
    "updated_at" timestamp with time zone
);
CREATE TABLE IF NOT EXISTS "admission"."application" (
    "application_id" uuid DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
    "code" character varying(100) DEFAULT '' NOT NULL,
    "created_at" timestamp with time zone DEFAULT now() NOT NULL,
    "is_active" boolean DEFAULT true NOT NULL,
    "metadata_json" jsonb,
    "name" character varying(250) DEFAULT '' NOT NULL,
    "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL,
    "tenant_id" uuid NOT NULL,
    "updated_at" timestamp with time zone
);
CREATE TABLE IF NOT EXISTS "admission"."inquiry" (
    "inquiry_id" uuid DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
    "code" character varying(100) DEFAULT '' NOT NULL,
    "created_at" timestamp with time zone DEFAULT now() NOT NULL,
    "is_active" boolean DEFAULT true NOT NULL,
    "metadata_json" jsonb,
    "name" character varying(250) DEFAULT '' NOT NULL,
    "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL,
    "tenant_id" uuid NOT NULL,
    "updated_at" timestamp with time zone
);
ALTER TABLE "hr"."candidate" ADD COLUMN IF NOT EXISTS "code" character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE "hr"."candidate" ADD COLUMN IF NOT EXISTS "name" character varying(250) DEFAULT '' NOT NULL;
ALTER TABLE "hr"."employee" ADD COLUMN IF NOT EXISTS "date_of_birth" date;
ALTER TABLE "hr"."employee" ADD COLUMN IF NOT EXISTS "department_id" uuid;
ALTER TABLE "hr"."employee" ADD COLUMN IF NOT EXISTS "designation" smallint DEFAULT 0 NOT NULL;
ALTER TABLE "hr"."employee" ADD COLUMN IF NOT EXISTS "gender" character varying(30);
ALTER TABLE "hr"."employee" ADD COLUMN IF NOT EXISTS "job_title" character varying(150);
CREATE TABLE IF NOT EXISTS "hr"."employmenthistory" (
    "employment_history_id" uuid DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
    "code" character varying(100) DEFAULT '' NOT NULL,
    "created_at" timestamp with time zone DEFAULT now() NOT NULL,
    "is_active" boolean DEFAULT true NOT NULL,
    "metadata_json" jsonb,
    "name" character varying(250) DEFAULT '' NOT NULL,
    "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL,
    "tenant_id" uuid NOT NULL,
    "updated_at" timestamp with time zone
);
ALTER TABLE "hr"."interview" ADD COLUMN IF NOT EXISTS "code" character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE "hr"."interview" ADD COLUMN IF NOT EXISTS "created_at" timestamp with time zone DEFAULT now() NOT NULL;
ALTER TABLE "hr"."interview" ADD COLUMN IF NOT EXISTS "is_active" boolean DEFAULT true NOT NULL;
ALTER TABLE "hr"."interview" ADD COLUMN IF NOT EXISTS "name" character varying(250) DEFAULT '' NOT NULL;
ALTER TABLE "hr"."interview" ADD COLUMN IF NOT EXISTS "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL;
ALTER TABLE "hr"."interview" ADD COLUMN IF NOT EXISTS "tenant_id" uuid;
ALTER TABLE "hr"."interview" ADD COLUMN IF NOT EXISTS "updated_at" timestamp with time zone;
ALTER TABLE "hr"."job" ADD COLUMN IF NOT EXISTS "name" character varying(250) DEFAULT '' NOT NULL;
ALTER TABLE "hr"."leave_request" ADD COLUMN IF NOT EXISTS "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL;
ALTER TABLE "hr"."leave_request" ADD COLUMN IF NOT EXISTS "updated_at" timestamp with time zone;
ALTER TABLE "hr"."position" ADD COLUMN IF NOT EXISTS "code" character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE "hr"."position" ADD COLUMN IF NOT EXISTS "name" character varying(250) DEFAULT '' NOT NULL;
CREATE TABLE IF NOT EXISTS "hr"."resume" (
    "resume_id" uuid DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
    "code" character varying(100) DEFAULT '' NOT NULL,
    "created_at" timestamp with time zone DEFAULT now() NOT NULL,
    "is_active" boolean DEFAULT true NOT NULL,
    "metadata_json" jsonb,
    "name" character varying(250) DEFAULT '' NOT NULL,
    "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL,
    "tenant_id" uuid NOT NULL,
    "updated_at" timestamp with time zone
);
ALTER TABLE "lms"."academic_assignment" ADD COLUMN IF NOT EXISTS "code" character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE "lms"."academic_assignment" ADD COLUMN IF NOT EXISTS "name" character varying(250) DEFAULT '' NOT NULL;
CREATE TABLE IF NOT EXISTS "lms"."assignment_student" (
    "assignment_student_id" uuid DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
    "academic_assignment_id" uuid NOT NULL,
    "created_at" timestamp with time zone DEFAULT now() NOT NULL,
    "is_active" boolean DEFAULT true NOT NULL,
    "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL,
    "student_id" uuid NOT NULL,
    "tenant_id" uuid NOT NULL,
    "updated_at" timestamp with time zone
);
ALTER TABLE "lms"."student_assignment_submission" ADD COLUMN IF NOT EXISTS "code" character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE "lms"."student_assignment_submission" ADD COLUMN IF NOT EXISTS "content_type" character varying(200);
ALTER TABLE "lms"."student_assignment_submission" ADD COLUMN IF NOT EXISTS "created_at" timestamp with time zone DEFAULT now() NOT NULL;
ALTER TABLE "lms"."student_assignment_submission" ADD COLUMN IF NOT EXISTS "file_content" bytea;
ALTER TABLE "lms"."student_assignment_submission" ADD COLUMN IF NOT EXISTS "file_name" character varying(250);
ALTER TABLE "lms"."student_assignment_submission" ADD COLUMN IF NOT EXISTS "is_active" boolean DEFAULT true NOT NULL;
ALTER TABLE "lms"."student_assignment_submission" ADD COLUMN IF NOT EXISTS "name" character varying(250) DEFAULT '' NOT NULL;
ALTER TABLE "lms"."student_assignment_submission" ADD COLUMN IF NOT EXISTS "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL;
ALTER TABLE "lms"."student_assignment_submission" ADD COLUMN IF NOT EXISTS "tenant_id" uuid;
ALTER TABLE "lms"."student_assignment_submission" ADD COLUMN IF NOT EXISTS "updated_at" timestamp with time zone;
CREATE TABLE IF NOT EXISTS "lms"."learningresource" (
    "learning_resource_id" uuid DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
    "code" character varying(100) DEFAULT '' NOT NULL,
    "created_at" timestamp with time zone DEFAULT now() NOT NULL,
    "is_active" boolean DEFAULT true NOT NULL,
    "metadata_json" jsonb,
    "name" character varying(250) DEFAULT '' NOT NULL,
    "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL,
    "tenant_id" uuid NOT NULL,
    "updated_at" timestamp with time zone
);
ALTER TABLE "exam"."exam" ADD COLUMN IF NOT EXISTS "class_section_id" uuid;
ALTER TABLE "exam"."exam" ADD COLUMN IF NOT EXISTS "code" character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE "exam"."exam_subject" ADD COLUMN IF NOT EXISTS "code" character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE "exam"."exam_subject" ADD COLUMN IF NOT EXISTS "created_at" timestamp with time zone DEFAULT now() NOT NULL;
ALTER TABLE "exam"."exam_subject" ADD COLUMN IF NOT EXISTS "is_active" boolean DEFAULT true NOT NULL;
ALTER TABLE "exam"."exam_subject" ADD COLUMN IF NOT EXISTS "name" character varying(250) DEFAULT '' NOT NULL;
ALTER TABLE "exam"."exam_subject" ADD COLUMN IF NOT EXISTS "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL;
ALTER TABLE "exam"."exam_subject" ADD COLUMN IF NOT EXISTS "tenant_id" uuid;
ALTER TABLE "exam"."exam_subject" ADD COLUMN IF NOT EXISTS "updated_at" timestamp with time zone;
ALTER TABLE "exam"."student_exam_result" ADD COLUMN IF NOT EXISTS "created_at" timestamp with time zone DEFAULT now() NOT NULL;
ALTER TABLE "exam"."student_exam_result" ADD COLUMN IF NOT EXISTS "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL;
ALTER TABLE "exam"."student_exam_result" ADD COLUMN IF NOT EXISTS "updated_at" timestamp with time zone;
ALTER TABLE "transport"."driver" ADD COLUMN IF NOT EXISTS "assigned_vehicle_id" uuid;
ALTER TABLE "transport"."driver" ADD COLUMN IF NOT EXISTS "cnic" text DEFAULT '' NOT NULL;
ALTER TABLE "transport"."driver" ADD COLUMN IF NOT EXISTS "employee_number" text DEFAULT '' NOT NULL;
ALTER TABLE "transport"."driver" ADD COLUMN IF NOT EXISTS "employment_status_code" text DEFAULT '' NOT NULL;
ALTER TABLE "transport"."driver" ADD COLUMN IF NOT EXISTS "first_name" text DEFAULT '' NOT NULL;
ALTER TABLE "transport"."driver" ADD COLUMN IF NOT EXISTS "joining_date" date;
ALTER TABLE "transport"."driver" ADD COLUMN IF NOT EXISTS "last_name" text DEFAULT '' NOT NULL;
ALTER TABLE "transport"."driver" ADD COLUMN IF NOT EXISTS "license_expiry_date" date;
ALTER TABLE "transport"."driver" ADD COLUMN IF NOT EXISTS "mobile_number" text DEFAULT '' NOT NULL;
ALTER TABLE "transport"."route" ADD COLUMN IF NOT EXISTS "arrival_time" time without time zone;
ALTER TABLE "transport"."route" ADD COLUMN IF NOT EXISTS "dismissal_time" time without time zone;
ALTER TABLE "transport"."route" ADD COLUMN IF NOT EXISTS "driver_id" uuid;
ALTER TABLE "transport"."route" ADD COLUMN IF NOT EXISTS "start_time" time without time zone;
ALTER TABLE "transport"."route" ADD COLUMN IF NOT EXISTS "vehicle_id" uuid;
CREATE TABLE IF NOT EXISTS "transport"."route_notice" (
    "route_notice_id" uuid DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
    "created_at" timestamp with time zone DEFAULT now() NOT NULL,
    "created_by" uuid NOT NULL,
    "delay_minutes" integer DEFAULT 0 NOT NULL,
    "is_active" boolean DEFAULT true NOT NULL,
    "message" text DEFAULT '' NOT NULL,
    "route_id" uuid NOT NULL,
    "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL,
    "service_date" date NOT NULL,
    "tenant_id" uuid NOT NULL,
    "updated_at" timestamp with time zone
);
CREATE TABLE IF NOT EXISTS "transport"."stop" (
    "stop_id" uuid DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
    "code" character varying(100) DEFAULT '' NOT NULL,
    "created_at" timestamp with time zone DEFAULT now() NOT NULL,
    "dropoff_time" time without time zone,
    "is_active" boolean DEFAULT true NOT NULL,
    "metadata_json" jsonb,
    "name" character varying(250) DEFAULT '' NOT NULL,
    "pickup_time" time without time zone,
    "route_id" uuid,
    "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL,
    "sequence" integer DEFAULT 0 NOT NULL,
    "tenant_id" uuid NOT NULL,
    "updated_at" timestamp with time zone
);
CREATE TABLE IF NOT EXISTS "transport"."studenttransport" (
    "student_transport_id" uuid DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
    "code" character varying(100) DEFAULT '' NOT NULL,
    "created_at" timestamp with time zone DEFAULT now() NOT NULL,
    "is_active" boolean DEFAULT true NOT NULL,
    "metadata_json" jsonb,
    "name" character varying(250) DEFAULT '' NOT NULL,
    "route_id" uuid,
    "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL,
    "stop_id" uuid,
    "student_id" uuid,
    "tenant_id" uuid NOT NULL,
    "updated_at" timestamp with time zone
);
CREATE TABLE IF NOT EXISTS "transport"."trip_record" (
    "trip_record_id" uuid DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
    "created_at" timestamp with time zone DEFAULT now() NOT NULL,
    "direction" text DEFAULT '' NOT NULL,
    "is_active" boolean DEFAULT true NOT NULL,
    "recorded_by" uuid NOT NULL,
    "route_id" uuid NOT NULL,
    "row_version" bytea DEFAULT gen_random_bytes(8) NOT NULL,
    "service_date" date NOT NULL,
    "status" text DEFAULT '' NOT NULL,
    "student_id" uuid NOT NULL,
    "tenant_id" uuid NOT NULL,
    "updated_at" timestamp with time zone
);
ALTER TABLE "transport"."vehicle" ADD COLUMN IF NOT EXISTS "code" character varying(100) DEFAULT '' NOT NULL;
ALTER TABLE "transport"."vehicle" ADD COLUMN IF NOT EXISTS "name" character varying(250) DEFAULT '' NOT NULL;

-- Restore canonical academic relationships and legacy data where the mapping is unambiguous.
ALTER TABLE academic.course_offering ALTER COLUMN program_subject_id DROP NOT NULL;
UPDATE academic.course_offering co SET subject_id = ps.subject_id FROM academic.program_subject ps
 WHERE co.program_subject_id = ps.program_subject_id AND co.subject_id IS NULL;
UPDATE academic.course_offering SET branch_id = campus_id WHERE branch_id IS NULL;
UPDATE academic.course_offering SET code = 'CRS-' || course_offering_id::text WHERE code = '';
UPDATE academic.course_offering SET name = coalesce(display_name,code) WHERE name = '';
UPDATE academic.teacher_course_assignment SET code = 'TAS-' || teacher_course_assignment_id::text WHERE code = '';
UPDATE academic.grade_level SET education_level_id = ('20000000-0000-0000-0000-00000000000' ||
 CASE WHEN sort_order <= 5 THEN '2' WHEN sort_order <= 8 THEN '3' WHEN sort_order <= 10 THEN '4' ELSE '5' END)::uuid
 WHERE education_level_id IS NULL AND sort_order BETWEEN 1 AND 12;
UPDATE academic.timetable_entry e SET tenant_id = t.tenant_id FROM academic.timetable t WHERE e.timetable_id = t.timetable_id AND e.tenant_id IS NULL;
UPDATE lms.student_assignment_submission s SET tenant_id = a.tenant_id FROM lms.academic_assignment a WHERE s.academic_assignment_id = a.academic_assignment_id AND s.tenant_id IS NULL;
UPDATE exam.exam_subject s SET tenant_id = e.tenant_id FROM exam.exam e WHERE s.exam_id = e.exam_id AND s.tenant_id IS NULL;
ALTER TABLE academic.timetable_entry ALTER COLUMN tenant_id SET NOT NULL;
ALTER TABLE lms.student_assignment_submission ALTER COLUMN tenant_id SET NOT NULL;
ALTER TABLE exam.exam_subject ALTER COLUMN tenant_id SET NOT NULL;
UPDATE transport.driver SET cnic=cnic_number, employee_number=driver_number, first_name=full_name,
 mobile_number=coalesce(phone,''), employment_status_code=status,
 joining_date=coalesce(hire_date,created_at::date),license_expiry_date=driving_license_expires_on
 WHERE employee_number='';
UPDATE transport.vehicle SET code=registration_no, name=registration_no WHERE code='';
UPDATE lms.academic_assignment SET code='ASG-' || academic_assignment_id::text,name=title WHERE code='';
UPDATE exam.exam SET code='EXM-' || exam_id::text WHERE code='';
UPDATE exam.exam_subject SET code='EXS-' || exam_subject_id::text WHERE code='';

-- Admission uploads use a distinct owner until enrollment is committed.
ALTER TABLE document.document DROP CONSTRAINT IF EXISTS document_owner_type_check;
ALTER TABLE document.document ADD CONSTRAINT document_owner_type_check CHECK (owner_type IN
 ('StudentDocument','TeacherDocument','ParentDocument','CampusDocument','ExaminerDocument','EmployeeDocument','DriverDocument','Certificate','AdmissionDocument'));
ALTER TABLE document.document_type DROP CONSTRAINT IF EXISTS document_type_owner_type_check;
ALTER TABLE document.document_type ADD CONSTRAINT document_type_owner_type_check CHECK (owner_type IN
 ('StudentDocument','TeacherDocument','ParentDocument','CampusDocument','ExaminerDocument','EmployeeDocument','DriverDocument','Certificate','AdmissionDocument'));

CREATE UNIQUE INDEX IF NOT EXISTS ux_tenant_settings_tenant ON saas.tenant_settings(tenant_id);
CREATE UNIQUE INDEX IF NOT EXISTS ux_trip_record_service ON transport.trip_record(tenant_id,route_id,student_id,service_date,direction);
CREATE UNIQUE INDEX IF NOT EXISTS ux_studenttransport_active_student ON transport.studenttransport(tenant_id,student_id) WHERE is_active;
DROP INDEX IF EXISTS transport.ux_stop_route_sequence;
CREATE INDEX IF NOT EXISTS ix_stop_route_sequence ON transport.stop(tenant_id,route_id,sequence) WHERE is_active;
CREATE INDEX IF NOT EXISTS ix_route_notice_date ON transport.route_notice(tenant_id,route_id,service_date);
CREATE INDEX IF NOT EXISTS ix_assignment_submission_tenant ON lms.student_assignment_submission(tenant_id,academic_assignment_id,student_id);
CREATE INDEX IF NOT EXISTS ix_class_section_grade_year ON academic.class_section(tenant_id,grade_level_id,academic_year_id);
ALTER TABLE admission.admission_criteria ALTER COLUMN class_section_id DROP NOT NULL;
-- Preserve legacy class identifiers while moving admissions to the canonical grade-level table.
INSERT INTO academic.grade_level(grade_level_id,tenant_id,campus_id,code,name,sort_order,is_active,education_level_id)
SELECT c.class_id,c.tenant_id,c.branch_id,
 CASE WHEN EXISTS (SELECT 1 FROM academic.grade_level g WHERE g.tenant_id=c.tenant_id AND g.code=c.code)
 THEN left(c.code,60) || '-LEGACY-' || left(c.class_id::text,8) ELSE c.code END,
 c.name,c.sort_order,c.is_active,c.education_level_id
FROM academic.class c WHERE NOT EXISTS (SELECT 1 FROM academic.grade_level g WHERE g.grade_level_id=c.class_id);
UPDATE academic.class_section SET grade_level_id=class_id WHERE grade_level_id IS NULL AND class_id IS NOT NULL;
ALTER TABLE admission.student_application DROP CONSTRAINT IF EXISTS student_application_class_id_fkey;
ALTER TABLE admission.student_application ADD CONSTRAINT student_application_class_id_fkey FOREIGN KEY (class_id) REFERENCES academic.grade_level(grade_level_id);
ALTER TABLE admission.admission_criteria DROP CONSTRAINT IF EXISTS admission_criteria_class_id_fkey;
ALTER TABLE admission.admission_criteria ADD CONSTRAINT admission_criteria_class_id_fkey FOREIGN KEY (class_id) REFERENCES academic.grade_level(grade_level_id);
-- Import existing HR allocations into the same academic records used by the portal.
INSERT INTO academic.course_offering(tenant_id,campus_id,branch_id,academic_year_id,subject_id,code,name,display_name)
SELECT DISTINCT h.tenant_id,h.campus_id,h.campus_id,cs.academic_year_id,h.subject_id,'LEGACY-' || h.subject_id::text,s.name,s.name
FROM hr.teacher_teaching_assignment h
JOIN academic.class_section cs ON cs.class_section_id=h.class_section_id AND cs.tenant_id=h.tenant_id AND cs.campus_id=h.campus_id
JOIN academic.subject s ON s.subject_id=h.subject_id AND s.tenant_id=h.tenant_id
WHERE h.is_active AND NOT EXISTS (SELECT 1 FROM academic.course_offering co WHERE co.tenant_id=h.tenant_id AND co.campus_id=h.campus_id AND co.academic_year_id=cs.academic_year_id AND co.subject_id=h.subject_id AND co.program_subject_id IS NULL);
INSERT INTO academic.teacher_course_assignment(teacher_course_assignment_id,tenant_id,course_offering_id,employee_id,class_section_id,code,name,periods_per_week,effective_from,effective_to,assignment_role,is_primary,is_active)
SELECT h.teacher_teaching_assignment_id,h.tenant_id,co.course_offering_id,h.employee_id,h.class_section_id,h.code,h.name,h.periods_per_week,h.effective_from,h.effective_to,'PRIMARY',true,h.is_active
FROM hr.teacher_teaching_assignment h
JOIN academic.class_section cs ON cs.class_section_id=h.class_section_id AND cs.tenant_id=h.tenant_id
JOIN LATERAL (SELECT c.course_offering_id FROM academic.course_offering c WHERE c.tenant_id=h.tenant_id AND c.campus_id=h.campus_id AND c.academic_year_id=cs.academic_year_id AND c.subject_id=h.subject_id AND c.program_subject_id IS NULL ORDER BY c.created_at,c.course_offering_id LIMIT 1) co ON true
JOIN hr.employee e ON e.employee_id=h.employee_id AND e.tenant_id=h.tenant_id
WHERE NOT EXISTS (SELECT 1 FROM academic.teacher_course_assignment a WHERE a.teacher_course_assignment_id=h.teacher_teaching_assignment_id);
UPDATE academic.class_section cs SET class_teacher_employee_id=h.employee_id FROM hr.teacher_teaching_assignment h
 WHERE h.class_section_id=cs.class_section_id AND h.tenant_id=cs.tenant_id AND h.is_active AND h.is_class_teacher AND cs.class_teacher_employee_id IS NULL;
COMMIT;

