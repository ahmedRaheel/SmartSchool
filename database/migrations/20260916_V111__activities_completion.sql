BEGIN;

ALTER TABLE activity.activity
    ADD COLUMN IF NOT EXISTS code varchar(50),
    ADD COLUMN IF NOT EXISTS activity_date date,
    ADD COLUMN IF NOT EXISTS start_time time,
    ADD COLUMN IF NOT EXISTS end_time time,
    ADD COLUMN IF NOT EXISTS venue varchar(250),
    ADD COLUMN IF NOT EXISTS description text,
    ADD COLUMN IF NOT EXISTS max_participants integer,
    ADD COLUMN IF NOT EXISTS status varchar(30) NOT NULL DEFAULT 'UPCOMING';

UPDATE activity.activity
SET code = COALESCE(NULLIF(code, ''), 'ACT-' || upper(substr(replace(activity_id::text, '-', ''), 1, 8))),
    activity_date = COALESCE(activity_date, created_at::date),
    category = COALESCE(NULLIF(category, ''), 'OTHER')
WHERE code IS NULL
   OR code = ''
   OR activity_date IS NULL
   OR category IS NULL
   OR category = '';

ALTER TABLE activity.activity
    ALTER COLUMN code SET NOT NULL,
    ALTER COLUMN activity_date SET NOT NULL,
    ALTER COLUMN category SET NOT NULL;

CREATE UNIQUE INDEX IF NOT EXISTS ux_activity_tenant_code
    ON activity.activity(tenant_id, code);
CREATE INDEX IF NOT EXISTS ix_activity_tenant_date_status
    ON activity.activity(tenant_id, activity_date, status);

ALTER TABLE activity.student_activity
    ADD COLUMN IF NOT EXISTS student_activity_id uuid DEFAULT gen_random_uuid(),
    ADD COLUMN IF NOT EXISTS tenant_id uuid,
    ADD COLUMN IF NOT EXISTS is_active boolean NOT NULL DEFAULT true,
    ADD COLUMN IF NOT EXISTS created_at timestamptz NOT NULL DEFAULT now(),
    ADD COLUMN IF NOT EXISTS updated_at timestamptz,
    ADD COLUMN IF NOT EXISTS row_version bytea NOT NULL DEFAULT public.gen_random_bytes(8);

UPDATE activity.student_activity sa
SET tenant_id = a.tenant_id,
    student_activity_id = COALESCE(sa.student_activity_id, gen_random_uuid()),
    joined_at = COALESCE(sa.joined_at, CURRENT_DATE)
FROM activity.activity a
WHERE a.activity_id = sa.activity_id
  AND (sa.tenant_id IS NULL OR sa.student_activity_id IS NULL OR sa.joined_at IS NULL);

ALTER TABLE activity.student_activity
    ALTER COLUMN student_activity_id SET NOT NULL,
    ALTER COLUMN tenant_id SET NOT NULL,
    ALTER COLUMN joined_at SET NOT NULL;

DO $$
DECLARE
    constraint_name text;
BEGIN
    SELECT conname
    INTO constraint_name
    FROM pg_constraint
    WHERE conrelid = 'activity.student_activity'::regclass
      AND contype = 'p'
    LIMIT 1;

    IF constraint_name IS NOT NULL THEN
        EXECUTE format('ALTER TABLE activity.student_activity DROP CONSTRAINT %I', constraint_name);
    END IF;
END $$;

ALTER TABLE activity.student_activity
    ADD CONSTRAINT pk_student_activity PRIMARY KEY (student_activity_id);

CREATE UNIQUE INDEX IF NOT EXISTS ux_student_activity_active
    ON activity.student_activity(tenant_id, activity_id, student_id)
    WHERE is_active = true;
CREATE INDEX IF NOT EXISTS ix_student_activity_student
    ON activity.student_activity(tenant_id, student_id);

COMMIT;
