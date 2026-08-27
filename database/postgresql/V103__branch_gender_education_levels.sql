BEGIN;

CREATE SCHEMA IF NOT EXISTS reference;

CREATE TABLE IF NOT EXISTS reference.branch_gender_type (
    branch_gender_type_id uuid PRIMARY KEY,
    code varchar(40) NOT NULL UNIQUE,
    name varchar(100) NOT NULL,
    sort_order integer NOT NULL DEFAULT 0,
    is_active boolean NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS reference.education_level (
    education_level_id uuid PRIMARY KEY,
    code varchar(40) NOT NULL UNIQUE,
    name varchar(100) NOT NULL,
    sort_order integer NOT NULL DEFAULT 0,
    is_active boolean NOT NULL DEFAULT TRUE
);

INSERT INTO reference.branch_gender_type (branch_gender_type_id, code, name, sort_order) VALUES
('10000000-0000-0000-0000-000000000001','BOYS_ONLY','Boys Only',1),
('10000000-0000-0000-0000-000000000002','GIRLS_ONLY','Girls Only',2),
('10000000-0000-0000-0000-000000000003','CO_EDUCATION','Co-Education',3)
ON CONFLICT (code) DO UPDATE SET name=EXCLUDED.name, sort_order=EXCLUDED.sort_order, is_active=TRUE;

INSERT INTO reference.education_level (education_level_id, code, name, sort_order) VALUES
('20000000-0000-0000-0000-000000000001','PRE_PRIMARY','Pre-Primary',1),
('20000000-0000-0000-0000-000000000002','PRIMARY','Primary',2),
('20000000-0000-0000-0000-000000000003','MIDDLE','Middle',3),
('20000000-0000-0000-0000-000000000004','SECONDARY','Secondary',4),
('20000000-0000-0000-0000-000000000005','HIGHER_SECONDARY','Higher Secondary',5)
ON CONFLICT (code) DO UPDATE SET name=EXCLUDED.name, sort_order=EXCLUDED.sort_order, is_active=TRUE;

ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS branch_gender_type_id uuid;
UPDATE org.campus SET branch_gender_type_id='10000000-0000-0000-0000-000000000003' WHERE branch_gender_type_id IS NULL;
ALTER TABLE org.campus ALTER COLUMN branch_gender_type_id SET NOT NULL;
ALTER TABLE org.campus DROP CONSTRAINT IF EXISTS fk_campus_branch_gender_type;
ALTER TABLE org.campus ADD CONSTRAINT fk_campus_branch_gender_type FOREIGN KEY (branch_gender_type_id) REFERENCES reference.branch_gender_type(branch_gender_type_id);

CREATE TABLE IF NOT EXISTS org.branch_education_level (
    branch_id uuid NOT NULL REFERENCES org.campus(campus_id) ON DELETE CASCADE,
    education_level_id uuid NOT NULL REFERENCES reference.education_level(education_level_id),
    created_at timestamptz NOT NULL DEFAULT now(),
    PRIMARY KEY (branch_id, education_level_id)
);

ALTER TABLE academic.class ADD COLUMN IF NOT EXISTS education_level_id uuid;
ALTER TABLE academic.class DROP CONSTRAINT IF EXISTS fk_class_education_level;
ALTER TABLE academic.class ADD CONSTRAINT fk_class_education_level FOREIGN KEY (education_level_id) REFERENCES reference.education_level(education_level_id);
CREATE INDEX IF NOT EXISTS ix_class_branch_education_level ON academic.class(branch_id, education_level_id);

ALTER TABLE admission.student_application ADD COLUMN IF NOT EXISTS gender varchar(20);
ALTER TABLE student.student ADD COLUMN IF NOT EXISTS gender varchar(20);

COMMIT;
