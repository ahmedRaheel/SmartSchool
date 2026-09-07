CREATE TABLE IF NOT EXISTS lms.assignment_student (
    assignment_student_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL,
    academic_assignment_id uuid NOT NULL,
    student_id uuid NOT NULL,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz NULL,
    row_version bytea NOT NULL DEFAULT decode(md5(random()::text), 'hex'),
    CONSTRAINT fk_assignment_student_assignment
        FOREIGN KEY (academic_assignment_id)
        REFERENCES lms.academic_assignment(academic_assignment_id)
        ON DELETE CASCADE,
    CONSTRAINT uq_assignment_student UNIQUE (tenant_id, academic_assignment_id, student_id)
);

CREATE INDEX IF NOT EXISTS ix_assignment_student_student
    ON lms.assignment_student (tenant_id, student_id, is_active);
