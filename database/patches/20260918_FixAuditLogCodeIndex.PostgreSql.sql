-- SmartSchool PostgreSQL forward patch
-- Fixes repeated audit events failing with SQLSTATE 23505.
-- Safe to run repeatedly.

BEGIN;

CREATE SCHEMA IF NOT EXISTS audit;

DO $fix$
BEGIN
    IF to_regclass('audit.audit_log') IS NOT NULL THEN
        DROP INDEX IF EXISTS audit."IX_audit_log_tenant_id_code";

        CREATE INDEX IF NOT EXISTS "IX_audit_log_tenant_id_code"
            ON audit.audit_log (tenant_id, code);
    END IF;
END
$fix$;

COMMIT;
