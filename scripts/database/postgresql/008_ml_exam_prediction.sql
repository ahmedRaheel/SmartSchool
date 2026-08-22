CREATE SCHEMA IF NOT EXISTS ai;

CREATE TABLE IF NOT EXISTS ai.ml_exam_prediction (
    ml_exam_prediction_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    student_id uuid NOT NULL REFERENCES student.student(student_id),
    subject_id uuid NOT NULL REFERENCES academic.subject(subject_id),
    target_exam_id uuid NULL REFERENCES exam.exam(exam_id),
    target_exam_subject_id uuid NULL REFERENCES exam.exam_subject(exam_subject_id),
    target_exam_type_code varchar(40) NOT NULL,
    predicted_marks numeric(8,2) NOT NULL,
    predicted_percentage numeric(7,3) NOT NULL,
    predicted_grade varchar(20) NOT NULL,
    lower_bound_percentage numeric(7,3) NOT NULL,
    upper_bound_percentage numeric(7,3) NOT NULL,
    confidence_score numeric(7,4) NOT NULL CHECK (confidence_score BETWEEN 0 AND 1),
    pass_probability numeric(7,4) NOT NULL CHECK (pass_probability BETWEEN 0 AND 1),
    trend varchar(30) NOT NULL,
    risk_level varchar(30) NOT NULL,
    model_version varchar(80) NOT NULL,
    historical_result_count integer NOT NULL,
    used_machine_learning boolean NOT NULL,
    generated_at timestamptz NOT NULL DEFAULT now(),
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz NULL,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8)
);

CREATE INDEX IF NOT EXISTS ix_ml_exam_prediction_student_subject
ON ai.ml_exam_prediction(tenant_id, student_id, subject_id, generated_at DESC);
