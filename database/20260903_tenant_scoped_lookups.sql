BEGIN;
ALTER TABLE saas.lookup_type ADD COLUMN IF NOT EXISTS is_tenant_scoped boolean NOT NULL DEFAULT false;
ALTER TABLE saas.lookup_value ADD COLUMN IF NOT EXISTS tenant_id uuid NULL;

-- Universal platform-owned lookup types. Add other truly universal types here.
UPDATE saas.lookup_type SET is_tenant_scoped = false WHERE code IN
('GENDER','BLOOD_GROUP','NATIONALITY','RELATIONSHIP','MARITAL_STATUS');

-- School configurable examples. Existing values become templates only; copy them per tenant when required.
UPDATE saas.lookup_type SET is_tenant_scoped = true WHERE code IN
('LEAVE_TYPE','EMPLOYMENT_TYPE','FEE_FREQUENCY','PAYMENT_METHOD');

DROP INDEX IF EXISTS saas.lookup_value_lookup_type_id_code_key;
ALTER TABLE saas.lookup_value DROP CONSTRAINT IF EXISTS lookup_value_lookup_type_id_code_key;
CREATE UNIQUE INDEX IF NOT EXISTS ux_lookup_value_universal ON saas.lookup_value(lookup_type_id,code) WHERE tenant_id IS NULL;
CREATE UNIQUE INDEX IF NOT EXISTS ux_lookup_value_tenant ON saas.lookup_value(tenant_id,lookup_type_id,code) WHERE tenant_id IS NOT NULL;
CREATE INDEX IF NOT EXISTS ix_lookup_value_tenant_type ON saas.lookup_value(tenant_id,lookup_type_id,sort_order) WHERE tenant_id IS NOT NULL;
COMMIT;
