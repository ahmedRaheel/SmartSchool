BEGIN;

-- Student awards now reference the centralized document.document table.
ALTER TABLE IF EXISTS activity.student_award
    DROP CONSTRAINT IF EXISTS student_award_generated_document_id_fkey;

DO $$
BEGIN
    IF EXISTS (
        SELECT 1
        FROM information_schema.columns
        WHERE table_schema = 'activity'
          AND table_name = 'student_award'
          AND column_name = 'generated_document_id'
    ) AND NOT EXISTS (
        SELECT 1
        FROM information_schema.columns
        WHERE table_schema = 'activity'
          AND table_name = 'student_award'
          AND column_name = 'document_id'
    ) THEN
        ALTER TABLE activity.student_award
            RENAME COLUMN generated_document_id TO document_id;
    END IF;
END
$$;

ALTER TABLE IF EXISTS activity.student_award
    DROP CONSTRAINT IF EXISTS student_award_document_id_fkey;

ALTER TABLE IF EXISTS activity.student_award
    ADD CONSTRAINT student_award_document_id_fkey
        FOREIGN KEY (document_id)
        REFERENCES document.document(document_id)
        ON DELETE SET NULL;

CREATE INDEX IF NOT EXISTS ix_student_award_document_id
    ON activity.student_award(document_id);

-- These legacy document resources are replaced by document.document + document.document_type.
DROP TABLE IF EXISTS document.generated_document CASCADE;
DROP TABLE IF EXISTS document.document_template CASCADE;
DROP TABLE IF EXISTS document.certificate CASCADE;
DROP TABLE IF EXISTS document.school_logo CASCADE;

COMMIT;
