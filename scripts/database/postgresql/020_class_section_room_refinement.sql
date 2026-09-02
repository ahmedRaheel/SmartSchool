BEGIN;
ALTER TABLE academic.class_section ADD COLUMN IF NOT EXISTS room_no varchar(50);
ALTER TABLE academic.class_section ADD COLUMN IF NOT EXISTS grade_level_id uuid REFERENCES academic.grade_level(grade_level_id);
ALTER TABLE academic.class_section ALTER COLUMN program_grade_id DROP NOT NULL;
CREATE INDEX IF NOT EXISTS ix_class_section_grade_level ON academic.class_section(grade_level_id);
COMMIT;
