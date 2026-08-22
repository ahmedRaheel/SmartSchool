CREATE EXTENSION IF NOT EXISTS pgcrypto;
CREATE SCHEMA IF NOT EXISTS infrastructure;


-- =========================================================
-- SMARTSCHOOL ENTITY BASE-FIELD SYNCHRONIZATION
-- Applies lifecycle/audit/concurrency columns to entity tables
-- that have a UUID primary key and a tenant_id column.
-- Junction tables without an Entity aggregate are intentionally excluded.
-- =========================================================
DO $$
DECLARE
    r record;
BEGIN
    FOR r IN
        SELECT DISTINCT
            n.nspname AS schema_name,
            c.relname AS table_name
        FROM pg_class c
        JOIN pg_namespace n ON n.oid = c.relnamespace
        JOIN pg_attribute tenant_column
          ON tenant_column.attrelid = c.oid
         AND tenant_column.attname = 'tenant_id'
         AND tenant_column.attnum > 0
         AND NOT tenant_column.attisdropped
        WHERE c.relkind = 'r'
          AND n.nspname IN (
              'saas','org','academic','student','admission','lms','exam',
              'finance','hr','payroll','document','communication','workflow',
              'activity','transport','library','inventory','ai','ai_core',
              'ai_tutor','ai_inquiry','ai_parent','audit')
          AND EXISTS (
              SELECT 1
              FROM pg_index i
              JOIN pg_attribute a
                ON a.attrelid = i.indrelid
               AND a.attnum = ANY(i.indkey)
              WHERE i.indrelid = c.oid
                AND i.indisprimary
                AND a.atttypid = 'uuid'::regtype)
    LOOP
        EXECUTE format(
            'ALTER TABLE %I.%I ADD COLUMN IF NOT EXISTS is_active boolean NOT NULL DEFAULT true',
            r.schema_name, r.table_name);

        EXECUTE format(
            'ALTER TABLE %I.%I ADD COLUMN IF NOT EXISTS created_at timestamptz NOT NULL DEFAULT now()',
            r.schema_name, r.table_name);

        EXECUTE format(
            'ALTER TABLE %I.%I ADD COLUMN IF NOT EXISTS updated_at timestamptz NULL',
            r.schema_name, r.table_name);

        EXECUTE format(
            'ALTER TABLE %I.%I ADD COLUMN IF NOT EXISTS row_version bytea NOT NULL DEFAULT gen_random_bytes(8)',
            r.schema_name, r.table_name);
    END LOOP;
END $$;

CREATE OR REPLACE FUNCTION infrastructure.smartschool_set_entity_update_fields()
RETURNS trigger
LANGUAGE plpgsql
AS $$
BEGIN
    NEW.updated_at = now();
    NEW.row_version = gen_random_bytes(8);
    RETURN NEW;
END;
$$;

DO $$
DECLARE
    r record;
    trigger_name text;
BEGIN
    FOR r IN
        SELECT n.nspname AS schema_name, c.relname AS table_name
        FROM pg_class c
        JOIN pg_namespace n ON n.oid = c.relnamespace
        JOIN pg_attribute a
          ON a.attrelid = c.oid
         AND a.attname = 'row_version'
         AND a.attnum > 0
         AND NOT a.attisdropped
        WHERE c.relkind = 'r'
          AND n.nspname IN (
              'saas','org','academic','student','admission','lms','exam',
              'finance','hr','payroll','document','communication','workflow',
              'activity','transport','library','inventory','ai','ai_core',
              'ai_tutor','ai_inquiry','ai_parent','audit')
    LOOP
        trigger_name := 'trg_' || r.table_name || '_entity_update';

        IF NOT EXISTS (
            SELECT 1
            FROM pg_trigger t
            JOIN pg_class tc ON tc.oid = t.tgrelid
            JOIN pg_namespace tn ON tn.oid = tc.relnamespace
            WHERE t.tgname = trigger_name
              AND tn.nspname = r.schema_name
              AND tc.relname = r.table_name
              AND NOT t.tgisinternal)
        THEN
            EXECUTE format(
                'CREATE TRIGGER %I BEFORE UPDATE ON %I.%I FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields()',
                trigger_name, r.schema_name, r.table_name);
        END IF;
    END LOOP;
END $$;
