ALTER TABLE identity."Users" ADD COLUMN IF NOT EXISTS "BusinessEntityId" uuid NULL;
ALTER TABLE identity."Users" ADD COLUMN IF NOT EXISTS "AccountType" varchar(50) NULL;
CREATE INDEX IF NOT EXISTS "IX_Users_Tenant_BusinessEntity_AccountType"
ON identity."Users"("TenantId","BusinessEntityId","AccountType");