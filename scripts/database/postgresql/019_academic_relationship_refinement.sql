-- SmartSchool academic relationship refinement
-- Canonicalizes department/subject/teacher and student/teacher/subject relationships.

ALTER TABLE org.department
    ADD COLUMN IF NOT EXISTS telephone varchar(50),
    ADD COLUMN IF NOT EXISTS email varchar(250);

CREATE TABLE IF NOT EXISTS academic.department_subject_teacher (
    department_subject_teacher_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    department_id uuid NOT NULL REFERENCES org.department(department_id),
    subject_id uuid NOT NULL REFERENCES academic.subject(subject_id),
    teacher_id uuid NOT NULL REFERENCES hr.employee(employee_id),
    is_primary boolean NOT NULL DEFAULT false,
    effective_from date,
    effective_to date,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bigint NOT NULL DEFAULT 1,
    CONSTRAINT ck_department_subject_teacher_dates
        CHECK (effective_to IS NULL OR effective_from IS NULL OR effective_to >= effective_from),
    UNIQUE (tenant_id, department_id, subject_id, teacher_id, effective_from)
);

CREATE INDEX IF NOT EXISTS ix_department_subject_teacher_department_subject
    ON academic.department_subject_teacher(tenant_id, department_id, subject_id)
    WHERE is_active = true;

CREATE INDEX IF NOT EXISTS ix_department_subject_teacher_teacher
    ON academic.department_subject_teacher(tenant_id, teacher_id)
    WHERE is_active = true;

CREATE TABLE IF NOT EXISTS academic.student_teacher (
    student_teacher_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    student_id uuid NOT NULL REFERENCES student.student(student_id),
    teacher_id uuid NOT NULL REFERENCES hr.employee(employee_id),
    subject_id uuid NOT NULL REFERENCES academic.subject(subject_id),
    student_enrollment_id uuid NOT NULL REFERENCES student.student_enrollment(student_enrollment_id),
    class_section_id uuid NOT NULL REFERENCES academic.class_section(class_section_id),
    academic_year_id uuid NOT NULL REFERENCES academic.academic_year(academic_year_id),
    effective_from date,
    effective_to date,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bigint NOT NULL DEFAULT 1,
    CONSTRAINT ck_student_teacher_dates
        CHECK (effective_to IS NULL OR effective_from IS NULL OR effective_to >= effective_from),
    UNIQUE (tenant_id, student_enrollment_id, teacher_id, subject_id)
);

CREATE INDEX IF NOT EXISTS ix_student_teacher_student_subject
    ON academic.student_teacher(tenant_id, student_id, subject_id)
    WHERE is_active = true;

CREATE INDEX IF NOT EXISTS ix_student_teacher_teacher_subject
    ON academic.student_teacher(tenant_id, teacher_id, subject_id)
    WHERE is_active = true;

-- teacher_course_assignment remains the timetable/course-offering assignment.
-- student_teacher is the explicit student-to-teacher relationship requested by the domain.
-- Do not duplicate class/subject names or teacher names in relationship tables.
