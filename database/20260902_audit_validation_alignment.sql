BEGIN;

ALTER TABLE audit.audit_log
    ADD COLUMN IF NOT EXISTS code varchar(100),
    ADD COLUMN IF NOT EXISTS name varchar(250),
    ADD COLUMN IF NOT EXISTS metadata_json jsonb,
    ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT true,
    ADD COLUMN IF NOT EXISTS created_at timestamptz DEFAULT now(),
    ADD COLUMN IF NOT EXISTS updated_at timestamptz,
    ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

UPDATE audit.audit_log
SET code = COALESCE(NULLIF(code, ''), 'AUD-' || audit_log_id::text),
    name = COALESCE(NULLIF(name, ''), action, 'Audit Log'),
    is_active = COALESCE(is_active, true),
    created_at = COALESCE(created_at, occurred_at, now()),
    row_version = COALESCE(row_version, gen_random_bytes(8));

ALTER TABLE audit.audit_log
    ALTER COLUMN code SET NOT NULL,
    ALTER COLUMN name SET NOT NULL,
    ALTER COLUMN is_active SET NOT NULL,
    ALTER COLUMN created_at SET NOT NULL,
    ALTER COLUMN row_version SET NOT NULL;

COMMIT;
