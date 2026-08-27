CREATE TABLE IF NOT EXISTS org.school (
    school_id uuid DEFAULT gen_random_uuid() PRIMARY KEY,
    tenant_id uuid NOT NULL,
    code varchar(50) NOT NULL,
    name varchar(200) NOT NULL,
    registration_number varchar(100),
    email varchar(200),
    phone varchar(50),
    website varchar(300),
    address text,
    city varchar(120),
    country varchar(120),
    logo_url varchar(500),
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamptz DEFAULT now() NOT NULL,
    updated_at timestamptz,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL,
    CONSTRAINT uq_school_tenant_code UNIQUE (tenant_id, code)
);

ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS school_id uuid;

-- Existing campuses are intentionally left nullable during migration. Assign them to a school,
-- then make the column NOT NULL in environments containing legacy data.
CREATE INDEX IF NOT EXISTS ix_campus_tenant_school ON org.campus (tenant_id, school_id);
