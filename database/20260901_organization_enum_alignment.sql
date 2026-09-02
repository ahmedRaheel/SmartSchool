-- Organization categorical fields are persisted as smallint-backed enums.
-- ContactType: Primary=1, Billing=2, Administrative=3, Emergency=4, Other=99
-- TenantStatus: Active=1, Inactive=2, Suspended=3, Pending=4

ALTER TABLE saas.tenant
    ADD COLUMN IF NOT EXISTS first_name varchar(100) NOT NULL DEFAULT '',
    ADD COLUMN IF NOT EXISTS last_name varchar(100) NOT NULL DEFAULT '',
    ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE saas.tenant
    ALTER COLUMN status_code DROP DEFAULT;

ALTER TABLE saas.tenant
    ALTER COLUMN status_code TYPE smallint
    USING CASE UPPER(status_code::text)
        WHEN 'ACTIVE' THEN 1
        WHEN 'INACTIVE' THEN 2
        WHEN 'SUSPENDED' THEN 3
        WHEN 'PENDING' THEN 4
        WHEN '1' THEN 1
        WHEN '2' THEN 2
        WHEN '3' THEN 3
        WHEN '4' THEN 4
        ELSE 1
    END;

ALTER TABLE saas.tenant
    ALTER COLUMN status_code SET DEFAULT 1;

ALTER TABLE saas.tenant_contact
    ALTER COLUMN contact_type DROP DEFAULT;

ALTER TABLE saas.tenant_contact
    ALTER COLUMN contact_type TYPE smallint
    USING CASE UPPER(contact_type::text)
        WHEN 'PRIMARY' THEN 1
        WHEN 'BILLING' THEN 2
        WHEN 'ADMINISTRATIVE' THEN 3
        WHEN 'EMERGENCY' THEN 4
        WHEN 'OTHER' THEN 99
        WHEN '1' THEN 1
        WHEN '2' THEN 2
        WHEN '3' THEN 3
        WHEN '4' THEN 4
        WHEN '99' THEN 99
        ELSE 99
    END;

ALTER TABLE saas.tenant_contact
    ALTER COLUMN contact_type SET DEFAULT 1;
