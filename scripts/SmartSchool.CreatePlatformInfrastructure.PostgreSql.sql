-- SmartSchool shared platform infrastructure (PostgreSQL)
-- Run before business-module migrations or before starting the API.
-- Idempotent: safe to execute more than once.

BEGIN;

CREATE SCHEMA IF NOT EXISTS platform;

CREATE TABLE IF NOT EXISTS platform.business_number_sequence
(
    tenant_id uuid NOT NULL,
    sequence_name character varying(150) NOT NULL,
    last_value bigint NOT NULL DEFAULT 0,

    CONSTRAINT pk_business_number_sequence
        PRIMARY KEY (tenant_id, sequence_name),

    CONSTRAINT ck_business_number_sequence_last_value
        CHECK (last_value >= 0)
);

COMMENT ON TABLE platform.business_number_sequence IS
    'Atomic tenant-scoped counters used by IBusinessNumberGenerator.';

COMMENT ON COLUMN platform.business_number_sequence.tenant_id IS
    'Tenant identifier. Guid.Empty is used for platform/global sequences.';

COMMENT ON COLUMN platform.business_number_sequence.sequence_name IS
    'Logical sequence name normalized to upper-case by BusinessNumberGenerator.';

COMMENT ON COLUMN platform.business_number_sequence.last_value IS
    'Last allocated numeric value for the tenant and sequence name.';

COMMIT;
