BEGIN;

CREATE SCHEMA IF NOT EXISTS ai_core;
CREATE SCHEMA IF NOT EXISTS ai;
CREATE SCHEMA IF NOT EXISTS ai_tutor;

DO $$
DECLARE
    item record;
BEGIN
    FOR item IN
        SELECT schemaname, tablename
        FROM pg_tables
        WHERE schemaname IN ('ai_parent', 'ai_inquiry')
    LOOP
        EXECUTE format('ALTER TABLE %I.%I SET SCHEMA ai_core', item.schemaname, item.tablename);
    END LOOP;

    FOR item IN
        SELECT schemaname, tablename
        FROM pg_tables
        WHERE schemaname = 'ai_prediction'
    LOOP
        EXECUTE format('ALTER TABLE %I.%I SET SCHEMA ai', item.schemaname, item.tablename);
    END LOOP;
END $$;

DROP SCHEMA IF EXISTS ai_parent;
DROP SCHEMA IF EXISTS ai_inquiry;
DROP SCHEMA IF EXISTS ai_prediction;

COMMIT;
