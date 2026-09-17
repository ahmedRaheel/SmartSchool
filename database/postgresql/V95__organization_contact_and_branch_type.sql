ALTER TABLE org.school
    ADD COLUMN IF NOT EXISTS fax varchar(50),
    ADD COLUMN IF NOT EXISTS province varchar(120);

ALTER TABLE org.campus
    ADD COLUMN IF NOT EXISTS branch_type varchar(40),
    ADD COLUMN IF NOT EXISTS city varchar(120),
    ADD COLUMN IF NOT EXISTS province varchar(120),
    ADD COLUMN IF NOT EXISTS fax varchar(50),
    ADD COLUMN IF NOT EXISTS mobile varchar(50),
    ADD COLUMN IF NOT EXISTS logo_url varchar(500);

UPDATE org.campus
SET branch_type = 'REGIONAL_BRANCH'
WHERE branch_type IS NULL OR btrim(branch_type) = '';

ALTER TABLE org.campus
    ALTER COLUMN branch_type SET DEFAULT 'REGIONAL_BRANCH',
    ALTER COLUMN branch_type SET NOT NULL;

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint WHERE conname = 'ck_campus_branch_type'
    ) THEN
        ALTER TABLE org.campus
            ADD CONSTRAINT ck_campus_branch_type
            CHECK (branch_type IN ('HEAD_OFFICE', 'REGIONAL_HEAD_OFFICE', 'REGIONAL_BRANCH'));
    END IF;
END $$;
