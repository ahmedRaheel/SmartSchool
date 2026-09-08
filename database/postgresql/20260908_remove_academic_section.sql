BEGIN;

ALTER TABLE academic.class_section
    DROP CONSTRAINT IF EXISTS class_section_section_id_fkey,
    DROP CONSTRAINT IF EXISTS class_section_academic_year_id_program_grade_id_section_id_key;

ALTER TABLE academic.class_section
    DROP COLUMN IF EXISTS section_id;

DROP TABLE IF EXISTS academic.section CASCADE;

COMMIT;
