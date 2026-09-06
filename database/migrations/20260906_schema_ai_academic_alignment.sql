-- 2026-09-06: runtime schema contract alignment. Idempotent and safe on existing databases.
-- Keeps PostgreSQL schema synchronized with EF/Dapper contracts to prevent 42703 failures.
CREATE EXTENSION IF NOT EXISTS pgcrypto;

ALTER TABLE IF EXISTS academic.academic_system ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.academic_year ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.class_section ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.course_offering ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.grade_level ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.program ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.subject ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.teacher_course_assignment ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.term ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.timetable ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.timetable_entry ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS activity.activity ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS activity.student_activity ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS activity.student_award ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai.class_performance_insight ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai.prediction_evaluation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai.prediction_evidence ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai.prediction_model ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai.student_intervention ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai.student_performance_prediction ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai.teaching_recommendation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai.topic_performance_insight ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.ai_execution_log ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.human_handoff ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.inquiry_conversation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.inquiry_message ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.knowledge_collection ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.knowledge_document ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.lead_capture ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.model_configuration ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.parent_conversation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.parent_message ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.parent_tool_execution ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.prompt_template ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.tool_definition ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_tutor.generated_quiz ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_tutor.student_topic_mastery ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_tutor.tutor_conversation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS audit.audit_log ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS communication.conversation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS communication.conversation_participant ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS communication.message ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS communication.message_receipt ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS document.document_template ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS document.generated_document ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS exam.exam ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS exam.exam_subject ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS exam.student_exam_result ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS finance.fee_structure ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS finance.fee_type ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS finance.student_invoice ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS finance.student_payment ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS hr.candidate ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS hr.employee_compensation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS hr.interview ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS hr.job ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS hr.job_grade ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS hr.position ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS inventory.item ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS library.book ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS library.book_copy ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS library.book_loan ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS lms.academic_assignment ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS lms.student_assignment_submission ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS org.department ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS payroll.payroll_run ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS saas.school_branding ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS saas.tenant ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS student.student_course_enrollment ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS teacher.leave_request ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS transport.route ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS transport.vehicle ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS workflow.approval ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS workflow.workflowdefinition ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS workflow.workflowinstance ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS workflow.workflowstep ADD COLUMN IF NOT EXISTS metadata_json jsonb;

-- Finance contract used by FeeTypeEntity.
ALTER TABLE IF EXISTS finance.fee_type ADD COLUMN IF NOT EXISTS frequency varchar(30) NOT NULL DEFAULT 'Monthly';
ALTER TABLE IF EXISTS finance.fee_type ADD COLUMN IF NOT EXISTS description varchar(500);

-- Campus-owned grade hierarchy.
ALTER TABLE IF EXISTS academic.grade_level ADD COLUMN IF NOT EXISTS campus_id uuid;
ALTER TABLE IF EXISTS academic.grade_level ADD COLUMN IF NOT EXISTS academic_system_id uuid;
ALTER TABLE IF EXISTS academic.grade_level ADD COLUMN IF NOT EXISTS metadata_json jsonb;
CREATE INDEX IF NOT EXISTS ix_grade_level_tenant_campus ON academic.grade_level(tenant_id, campus_id);
CREATE UNIQUE INDEX IF NOT EXISTS ux_grade_level_tenant_campus_code ON academic.grade_level(tenant_id, campus_id, code) WHERE campus_id IS NOT NULL;
ALTER TABLE IF EXISTS academic.class_section ADD COLUMN IF NOT EXISTS grade_level_id uuid;
ALTER TABLE IF EXISTS academic.class_section ADD COLUMN IF NOT EXISTS code varchar(100);
ALTER TABLE IF EXISTS academic.class_section ADD COLUMN IF NOT EXISTS name varchar(250);
ALTER TABLE IF EXISTS academic.class_section ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.class_section ALTER COLUMN program_grade_id DROP NOT NULL;
CREATE INDEX IF NOT EXISTS ix_class_section_tenant_grade ON academic.class_section(tenant_id, grade_level_id);

DO $$ BEGIN
  IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname='fk_grade_level_campus') THEN
    ALTER TABLE academic.grade_level ADD CONSTRAINT fk_grade_level_campus FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id) ON DELETE CASCADE;
  END IF;
  IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname='fk_grade_level_academic_system') THEN
    ALTER TABLE academic.grade_level ADD CONSTRAINT fk_grade_level_academic_system FOREIGN KEY (academic_system_id) REFERENCES academic.academic_system(academic_system_id) ON DELETE RESTRICT;
  END IF;
  IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname='fk_class_section_grade_level') THEN
    ALTER TABLE academic.class_section ADD CONSTRAINT fk_class_section_grade_level FOREIGN KEY (grade_level_id) REFERENCES academic.grade_level(grade_level_id) ON DELETE RESTRICT;
  END IF;
END $$;

-- RAG table indexes used by chatbot retrieval.
CREATE INDEX IF NOT EXISTS ix_rag_knowledge_chunk_tenant_collection ON ai_core.rag_knowledge_chunk(tenant_id, collection) WHERE is_active = true;
