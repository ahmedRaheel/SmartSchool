IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('identity.Users') AND name = 'TenantId' AND is_nullable = 0)
    ALTER TABLE identity.Users ALTER COLUMN TenantId uniqueidentifier NULL;
