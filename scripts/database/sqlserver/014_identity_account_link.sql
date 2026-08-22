IF COL_LENGTH('identity.Users','BusinessEntityId') IS NULL ALTER TABLE identity.Users ADD BusinessEntityId uniqueidentifier NULL;
IF COL_LENGTH('identity.Users','AccountType') IS NULL ALTER TABLE identity.Users ADD AccountType nvarchar(50) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Users_Tenant_BusinessEntity_AccountType')
CREATE INDEX IX_Users_Tenant_BusinessEntity_AccountType ON identity.Users(TenantId,BusinessEntityId,AccountType);